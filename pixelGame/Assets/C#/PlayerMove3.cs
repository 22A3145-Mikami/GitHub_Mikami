using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove3 : MonoBehaviour
{
    //全体
    private Vector3 move;
    //2Dの移動
    private Vector2 move2d;
    //3Dの移動
    private Vector3 move3d;

    //2Dの移動の単位ベクトル
    private Vector2 houkou2d;
    //3Dの移動の単位ベクトル
    private Vector3 houkou3d;

    //[SerializeField]
    public float speed;
    //playerの回転
    private float rotate;

    //[SerializeField]
    public bool mode2d3d; //  true のとき３Dになる  falseのとき２Dになる

    //[SerializeField] // 確認用  0 = ジャンプ可　　１　＝　ジャンプ中↑　２　＝　降下中
    public int canJump;
    //　ジャンプの経過時間
    public float timeJump;
    private float gravity;
    //[SerializeField]
    public float startVec;

    //[SerializeField]
    public Vector3 ground; //　ｙの高さ
    //[SerializeField]
    public float hitBlock; // ジャンプしてあたったブロックの下面　ー　プレイヤーのlocalScale / 2 - 重複したground


    //[SerializeField]
    //private float zHozon;
    [SerializeField]
    private Quaternion playerRot;

    [SerializeField]
    private GameObject stage;
    [SerializeField]
    private AllCollision allColl;
    //[SerializeField]
    public float stageColl;

    public Vector3 playerScale;
    [SerializeField]
    private PlayerANIM playerANIM;

    [SerializeField]
    private TikuwaList tList;

    private Environment environment;// ブロックタグのlist呼び出し
    private AllCollision coll;// ブロックタグlistのAllColl
    [SerializeField]//確認
    private int iHozon;
    [SerializeField]//  ジャンプした際のブロックすり抜け防止
    private float jumpBlockRange;


    //[SerializeField]
    //private GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        houkou2d = new Vector2(1, 0);
        houkou3d = new Vector3(0, 0, 1);

        rotate = 0.1f;

        mode2d3d = true;

        canJump = 0;
        gravity = 9.8f;
        startVec = 7f;

        stage = GameObject.Find("Stage");
        allColl = stage.GetComponent<AllCollision>();
        //allColl.sideHitBlock = true;

        ground.y = stage.transform.position.y + (stage.transform.localScale.y / 2);

        tList = GameObject.Find("TikuwaList").GetComponent<TikuwaList>();

        environment = GameObject.Find("SetStage").GetComponent<Environment>();
        iHozon = -1;

        Application.targetFrameRate = 60;
    }

    private void FixedUpdate()
    {
        //Debug.Log(Time.deltaTime);
        //Debug.Log(Time.deltaTime);

        //stageColl -1 = ステージ上にない　0 = ｙ軸がステージより下　1 = ｙ軸がステージより上　2 = ステージ上にいる
        /*
        for(int i = 0; i < environment.blocks.Length; i++)
        {
            if (iHozon != -1)
            {
                coll = environment.blocks[iHozon].GetComponent<AllCollision>();
                stageColl = coll.OnPlayer(1);
            }
            else
            {
                stageColl = allColl.OnPlayer(1);
            }
        }
        */
        if (iHozon != -1)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            stageColl = coll.OnPlayer(1);
        }
        else
        {
            stageColl = allColl.OnPlayer(1);
        }
        /*
        for(int i = 0; i < environment.blocks.Length; i++)
        {
            coll = environment.blocks[i].GetComponent<AllCollision>();
            if(coll.OnPlayer(1) == 2
                && i != iHozon)
            {
                if(transform.position)
            }
        }
        */

        //stageColl = allColl.OnPlayer(1);

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (mode2d3d == true)  // 　３Dになる
            {
                mode2d3d = false;

                transform.localRotation = playerRot;
                // transform.position = new Vector3(transform.position.x, transform.position.y, zHozon); ;
                //　trueからtrue問題は　こことかに　アニメーションでどうにかなる？


            }
            else   //  ２Dになる
            {
                mode2d3d = true;

                //zHozon = transform.position.z;
                //playerRot = transform.localEulerAngles;
                playerRot = transform.localRotation;
                transform.localRotation = new Quaternion(0, 0, 0, 0);
                //transform.position = new Vector3(transform.position.x, transform.position.y, zHozon);
                //　こことかに　アニメーションでどうにかなる？

            }
        }

        if (mode2d3d == true)
        {
            Move2d();
            Jump2d3d();
        }
        else if (mode2d3d == false)
        {
            Move3d();
            Jump2d3d();
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.deltaTime);

        //stageColl -1 = ステージ上にない　0 = ｙ軸がステージより下　1 = ｙ軸がステージより上　2 = ステージ上にいる
        
        for(int i = 0; i < environment.blocks.Length; i++)
        {
            if (iHozon != -1)
            {
                coll = environment.blocks[iHozon].GetComponent<AllCollision>();
                stageColl = coll.OnPlayer(1);
            }
            else
            {
                stageColl = allColl.OnPlayer(1);
            }
        }
        
        if (iHozon != -1)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            stageColl = coll.OnPlayer(1);
        }
        else
        {
            stageColl = allColl.OnPlayer(1);
        }
        
        for(int i = 0; i < environment.blocks.Length; i++)
        {
            coll = environment.blocks[i].GetComponent<AllCollision>();
            if(coll.OnPlayer(1) == 2
                && i != iHozon)
            {
                if(transform.position)
            }
        }
        

        //stageColl = allColl.OnPlayer(1);

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (mode2d3d == true)  // 　３Dになる
            {
                mode2d3d = false;

                transform.localRotation = playerRot;
                // transform.position = new Vector3(transform.position.x, transform.position.y, zHozon); ;
                //　trueからtrue問題は　こことかに　アニメーションでどうにかなる？

                
            }
            else   //  ２Dになる
            {
                mode2d3d = true;

                //zHozon = transform.position.z;
                //playerRot = transform.localEulerAngles;
                playerRot = transform.localRotation;
                transform.localRotation = new Quaternion(0, 0, 0, 0);
                //transform.position = new Vector3(transform.position.x, transform.position.y, zHozon);
                //　こことかに　アニメーションでどうにかなる？

            }
        }

        if (mode2d3d == true)
        {
            Move2d();
            Jump2d3d();
        }
        else if (mode2d3d == false)
        {
            Move3d();
            Jump2d3d();
        }

    }
    */
    void Move2d()
    {
        if (Input.GetKey(KeyCode.D))
        {
            houkou2d.x = speed;

            playerANIM.Walk();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            houkou2d.x = -speed;

            playerANIM.Walk();
        }
        else
        {
            houkou2d.x = 0;

            playerANIM.StopWalk();
        }
        houkou2d = transform.rotation * houkou2d;

        Vector3 ans = Vector3.one;
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            /*
            // ステージ判定
            if(i == environment.blocks.Length)
            {
                ans = allColl.SideCollision(new Vector3(houkou2d.x, houkou2d.y, 0));
                if (allColl.Range(ans) < Mathf.Sqrt(3))
                {
                    Debug.Log("はいった");
                    allColl.sideHitBlock = true;
                    //Debug.Log(ans);
                    break;
                }
            }
            */
            // 浮いてるブロック判定
            //else
            //{
                coll = environment.blocks[i].GetComponent<AllCollision>();
                if(coll.canSideHit)
                {
                    //  0 を　３ｄでは houkou3d.z
                    ans = coll.SideCollision(new Vector3(houkou2d.x, houkou2d.y, 0));
                    if (coll.Range(ans) < Mathf.Sqrt(3))
                    {
                        Debug.Log("kokoはいった");
                        coll.sideHitBlock = true;
                        Debug.Log(ans);
                        break;
                    }
                }
            //}
            //ans.y = coll.OnPlayer(3);

        }
        houkou2d = new Vector2(houkou2d.x * ans.x, houkou2d.y * ans.y);
        //Debug.Log(houkou2d);
        //  0 を　３ｄでは houkou3d.z
        transform.position += new Vector3(houkou2d.x, houkou2d.y, 0);
    }

    void Jump2d3d()
    {
        if (canJump == 0)
        {
            //  trueの状態で落ちたとき用
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                int iKeep = 0;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    if(coll.OnPlayer(1) == 2)
                    {
                        /*
                        //　２つ以上見つけた場合
                        if(iKeep >= 1)
                        {
                            AllCollision hozonBlock = environment.blocks[iHozon].GetComponent<AllCollision>();
                            //    みぎ　　　　　　　　　ひだり
                            if (coll.objPos.x > hozonBlock.objPos.x)
                            {
                                float x1 = (hozonBlock.objPos.x + (hozonBlock.objScale.x / 2)) - (transform.position.x - (transform.localScale.x / 2));
                                float x2 = (transform.position.x + (transform.localScale.x / 2)) - (coll.objPos.x - (coll.objScale.x / 2));

                                Debug.Log("はいた");
                                Debug.Log("x1" + x1);
                                Debug.Log("x2" + x2);
                                if (x2 >= x1)
                                {
                                    iHozon = iHozon;
                                }
                                else
                                {
                                    iHozon = i;
                                }
                            }
                            else
                            {
                                Debug.Log("はいた２");
                                float x1 = (coll.objPos.x + (coll.objScale.x / 2)) - (transform.position.x - (transform.localScale.x / 2));
                                float x2 = (transform.position.x + (transform.localScale.x / 2)) - (hozonBlock.objPos.x - (hozonBlock.objScale.x / 2));
                                Debug.Log("x1" + x1);
                                Debug.Log("x2" + x2);
                                if (x1 >= x2)
                                {
                                    iHozon = i;
                                }
                                else
                                {
                                    iHozon = iHozon;
                                }
                            }

                            
                        }
                        
                        else
                        {
                            iHozon = i;
                        }
                        */
                        iHozon = i;
                        //Debug.Log("これで決まり！：" + iHozon);
                        iKeep++;
                        
                    }
                    else if(i == environment.blocks.Length - 1)
                    {
                        timeJump = 0;
                        canJump = 2;
                        startVec = 0;
                    }
                }

            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (canJump == 0)
                {
                    timeJump = 0;
                    startVec = 7f;
                    canJump = 1;

                    playerANIM.Jump();
                }
            }


            //  竜巻
            for (int i = 0; i < tList.tatumakiObjList.Length; i++)
            {
                coll = tList.tatumakiObjList[i].GetComponent<AllCollision>();
                if (coll.OnPlayer(4) == 1)
                {
                    Debug.Log("omaeka");
                    timeJump = 0;
                    startVec = 10f;
                    canJump = 1;

                    playerANIM.Jump();

                    break;
                }
            }
            ///

        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            //playerPos (ここだけの)
            Vector3 pPos = transform.position;
            pPos.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (playerScale.y / 2) + ground.y + hitBlock;

            // 頭ゴン
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    if(coll.candownHit)
                    {
                        float ans = coll.OnPlayer(3);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //　　　　当たったブロック　ブロックスケール / 2    プレイヤースケール　/ 2   重複したground
                            hitBlock = coll.objPos.y - (coll.objScale.y / 2) - (playerScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (playerScale.y / 2) + ground.y + hitBlock;
                            break;
                        }
                    }
                }
            }
            //

            //  もし　ｙ座標が前のフレームよりーにいったら
            if (pPos.y <= transform.position.y)
            {
                canJump = 2;
                jumpBlockRange = transform.position.y - pPos.y;
            }

            // ステージ（ブロック）上にある　はずが　計算でステージ（ブロック）より下に行っちゃった場合
            // 更新したい座標 ー スケール/２
            if (pPos.y - (playerScale.y / 2) <= ground.y
                && stageColl != -1)
            {
                //Debug.Log("いるか");
                if(iHozon == -1
                    && ground.y == allColl.objPos.y + (allColl.objScale.y / 2)
                    && !allColl.sideHitBlock)
                {
                    //Debug.Log("いるかいるか");
                    if (pPos.y - (playerScale.y / 2) >= allColl.objPos.y + (allColl.objScale.y / 2) - (jumpBlockRange * 2))
                    {
                        //Debug.Log("いるかいるかいるか");
                        canJump = 0;
                        Debug.Log("手レポ");
                        //   地面（ブロック）      プレイヤーのｙ下
                        pPos.y = ground.y + (playerScale.y / 2);

                        hitBlock = 0;
                        //allColl.sideHitBlock = false;

                        playerANIM.StopJump();

                    }
                    /*
                    else
                    {
                        Debug.Log("次座標がそれより下");
                    }
                    */
                }
                else if (iHozon != -1)
                {
                    //Debug.Log("いるかいるかいるかいるか");
                    coll = environment.blocks[iHozon].GetComponent<AllCollision>();
                    if (ground.y == coll.objPos.y + (coll.objScale.y / 2)
                        && !coll.sideHitBlock)
                    {
                        //Debug.Log("いるかいるかいるかいるかいるか");
                        if (pPos.y - (playerScale.y / 2) >= coll.objPos.y + (coll.objScale.y / 2) - (jumpBlockRange * 2)
                            && pPos.y - (playerScale.y / 2) <= coll.objPos.y + (coll.objScale.y / 2) + jumpBlockRange)
                        {
                            //Debug.Log("いるかいるかいるかいるかいるかいるか");
                            canJump = 0;
                            Debug.Log("手レポ");
                            //   地面（ブロック）      プレイヤーのｙ下
                            pPos.y = ground.y + (playerScale.y / 2);

                            hitBlock = 0;
                            //coll.sideHitBlock = false;

                            playerANIM.StopJump();
                        }
                    }
                }
            }
            for (int i = 0; i < environment.blocks.Length; i++)
            {
                coll = environment.blocks[i].GetComponent<AllCollision>();
                coll.sideHitBlock = false;
            }
            allColl.sideHitBlock = false;
            

            transform.position = pPos;


            // 降下中のときに下にブロックがあるか調べる
            if (canJump == 2)
            {
                //一度ブロックをみつけるとtrue
                bool search = false;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    // 浮いてるブロックのOnplayer(2)用
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    float ans = coll.OnPlayer(2);

                    // ブロックの座標が返された
                    if (ans == coll.objPos.y + (coll.objScale.y / 2))
                    {
                        //Debug.Log(ans);
                        //Debug.Log(transform.position.y - (transform.localScale.y / 2));
                        //  ここ＋ー　ではんいとる
                        if ((transform.position.y - (playerScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (playerScale.y / 2) >= ans - jumpBlockRange))
                        {
                            if(search)
                            {
                                // 多分　i　を保存する必要ある ←　あった
                                // ブロックにする
                                Debug.Log(iHozon);
                                iHozon = i;
                                ground.y = ans;

                                canJump = 0;
                                hitBlock = 0;
                                
                                Debug.Log(i);
                                Debug.Log("何かいた？");

                                // jumpBlockRangeのリセット
                                jumpBlockRange = 0.1f;
                            }
                            else
                            {
                                // 多分　i　を保存する必要ある ←　あった
                                // ブロックにする
                                iHozon = i;
                                ground.y = ans;

                                canJump = 0;
                                hitBlock = 0;
                                search = true;

                                // jumpBlockRangeのリセット
                                jumpBlockRange = 0.1f;
                            }
                        }
                        continue;
                    }
                    
                    // ステージの座標をとろうとしてる　ブロックが見つからなかった　ここ＋ー　ではんいとる
                    else if (ans == coll.objPos.y + (coll.objScale.y / 2) - 1
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {
                        Debug.Log("ここ！");
                        ans = allColl.OnPlayer(2);
                        if( (transform.position.y - (playerScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (playerScale.y / 2) >= ans - jumpBlockRange))
                        {
                            ground.y = stage.transform.position.y + (stage.transform.localScale.y / 2);
                            // ステージにする
                            iHozon = -1;

                            canJump = 0;
                            hitBlock = 0;
                            Debug.Log("ここ！ここ");

                            // jumpBlockRangeのリセット
                            jumpBlockRange = 0.1f;
                        }
                    }
                    
                    // ステージ外（穴落下）
                    else if(ans == -10
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {

                        // プレイヤーがans以下
                        if (transform.position.y - (playerScale.y / 2) <= ans)
                        {
                            if (allColl.OnPlayer(2) == -10)
                            {
                                Debug.Log("GAMEOVER");
                            }
                        }
                    }
                }
            }
        }
    }

    
    void Move3d()
    {
        if (Input.GetKey(KeyCode.W))
        {
            houkou3d.z = speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            houkou3d.z = -speed;
        }
        else
        {
            houkou3d.z = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            houkou3d.x = -speed;
            //transform.localEulerAngles += new Vector3(0, -rotate, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            houkou3d.x = speed;
            //transform.localEulerAngles += new Vector3(0, rotate, 0);
        }
        else //新規
        {
            houkou3d.x = 0;
        }

        if (houkou3d.z == 0 && houkou3d.x == 0)
        {
            playerANIM.StopWalk();
        }
        else
        {
            playerANIM.Walk();
        }

        //transform.position += transform.rotation * houkou3d;
        /*
        if (Input.GetKey(KeyCode.D))
        {
            houkou2d.x = speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            houkou2d.x = -speed;
        }
        else
        {
            houkou3d.x = 0;
        }
        */
        houkou3d = transform.rotation * houkou3d;

        Vector3 ans = Vector3.one;
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            if (i == environment.blocks.Length)
            {
                ans = allColl.SideCollision(new Vector3(houkou3d.x, houkou3d.y, houkou3d.z));
                if (allColl.Range(ans) < Mathf.Sqrt(3))
                {
                    Debug.Log("はいった");
                    allColl.sideHitBlock = true;
                    //Debug.Log(ans);
                    break;
                }
            }
            else
            {
                coll = environment.blocks[i].GetComponent<AllCollision>();
                //  0 を　３ｄでは houkou3d.z
                ans = coll.SideCollision(new Vector3(houkou3d.x, houkou3d.y, houkou3d.z));
                if (coll.Range(ans) < Mathf.Sqrt(3))
                {
                    Debug.Log("はいった");
                    coll.sideHitBlock = true;
                    Debug.Log(ans);
                    break;
                }
            }
            //ans.y = coll.OnPlayer(3);

        }
        houkou3d = new Vector3(houkou3d.x * ans.x, houkou3d.y * ans.y, houkou3d.z * ans.z );
        //Debug.Log(houkou2d);
        //  0 を　３ｄでは houkou3d.z
        transform.position += new Vector3(houkou3d.x, houkou3d.y, houkou3d.z);
        
    }

    /*
    //　使わんかも
    void Jump3d()
    {
        if (canJump == 0)
        {
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                timeJump = 0;
                canJump = 2;
                startVec = 0;
            }
            if (Input.GetKey(KeyCode.Space))
            {

                if (canJump == 0)
                {
                    timeJump = 0;
                    startVec = 7f;
                    canJump = 1;
                }
            }

        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            Vector3 a = transform.position;
            a.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground;

            //  もし　ｙ座標が前のフレームよりーにいったら
            if (a.y <= transform.position.y)
            {
                canJump = 2;
            }

            transform.position = a;

            if (transform.position.y - (transform.localScale.y / 2) <= ground && stageColl != -1)
            {
                canJump = 0;
                a.y = ground + (transform.localScale.y / 2);
                transform.position = a;
            }
        }
    }
    */
}
