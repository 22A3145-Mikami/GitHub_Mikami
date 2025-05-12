using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove2 : MonoBehaviour
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
    //enemyの回転
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
    private Quaternion enemyRot;

    [SerializeField]
    private GameObject stage;
    [SerializeField]
    private AllCollision allColl;
    //[SerializeField]
    public float stageColl;

    public Vector3 enemyScale;

    [SerializeField]
    private TikuwaList tList;

    private Environment environment;// ブロックタグのlist呼び出し
    private AllCollision coll;// ブロックタグlistのAllColl
    [SerializeField]//確認
    private int iHozon;
    [SerializeField]//  ジャンプした際のブロックすり抜け防止
    private float jumpBlockRange;

    [SerializeField]
    bool clim; //現フレーム
    [SerializeField]
    bool maeClim; // 一つ前のフレーム
    [SerializeField]
    int climCo;

    //[SerializeField]
    //private GameObject[] blocks;

    // Start is called before the first frame update
    void Awake()
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

        clim = false;
        maeClim = false;


    }

    private void FixedUpdate()
    {

        //ground.y = environment.blocks[iHozon].transform.position.y + (coll.objScale.y / 2);
        /*
        if(clim)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            ground.y = environment.blocks[iHozon].transform.position.y + (coll.objScale.y / 2);
            stageColl = coll.OnEnemy(1, this.gameObject);
        }
        */
        /*
        else if(climCo > 0)
        {
            climCo++;
            if(climCo > 5)
            {
                clim = false;
                maeClim = false;
                climCo = 0;
            }
        }
        */

        if (iHozon != -1)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            stageColl = coll.OnEnemy(1, this.gameObject);
        }
        else
        {
            stageColl = allColl.OnEnemy(1, this.gameObject);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (mode2d3d == true)  // 　３Dになる
            {
                mode2d3d = false;

                transform.localRotation = enemyRot;
                // transform.position = new Vector3(transform.position.x, transform.position.y, zHozon); ;
                //　trueからtrue問題は　こことかに　アニメーションでどうにかなる？


            }
            else   //  ２Dになる
            {
                mode2d3d = true;

                //zHozon = transform.position.z;
                //playerRot = transform.localEulerAngles;
                enemyRot = transform.localRotation;
                transform.localRotation = new Quaternion(0, 0, 0, 0);
                //transform.position = new Vector3(transform.position.x, transform.position.y, zHozon);
                //　こことかに　アニメーションでどうにかなる？

            }
        }

        if (mode2d3d == true)
        {
            Move2d();
            if(!clim)
            {
                Jump2d3d();
            }
            
        }
        else if (mode2d3d == false)
        {
            Move3d();
            if (!clim)
            {
                Jump2d3d();
            }
        }

        if(!clim && maeClim)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            if ((transform.position.y - (enemyScale.y / 2) >= coll.objPos.y + (coll.objScale.y / 2)
            || (transform.position.y + (enemyScale.y / 2) <= coll.objPos.y - (coll.objScale.y / 2))))
            {
                //maeClim = false;
                ground.y = environment.blocks[iHozon].transform.position.y + (coll.objScale.y / 2);
                Vector3 ePos = transform.position;
                ePos.y = ground.y + (enemyScale.y / 2);
                transform.position = ePos;
                Debug.Log(iHozon + "desu");
                stageColl = coll.OnEnemy(1, this.gameObject);
                if(stageColl == 2)
                {
                    Debug.Log("imaha" + stageColl);
                }
                else if(stageColl == 1)
                {
                    Debug.Log("imaha" + stageColl);
                }
                else
                {
                    Debug.Log("imaha" + stageColl);
                }
                //Debug.Log("imaha" + stageColl);
                
                Jump2d3d();
            }
        }
    }

    void Move2d()
    {
        houkou2d.x = -speed;
        houkou2d.y = 0;

        maeClim = clim;
        clim = false;

        //houkou2d = transform.rotation * houkou2d;

        Vector3 ans = Vector3.one;
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            /*
            // ステージ判定
            if (i == environment.blocks.Length)
            {
                ans = allColl.SideCollisionEnemy(new Vector3(houkou2d.x, houkou2d.y, 0), this.gameObject);
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
                if (coll.canSideHit)
                {
                    //  0 を　３ｄでは houkou3d.z
                    ans = coll.SideCollisionEnemy(new Vector3(houkou2d.x, houkou2d.y, 0), this.gameObject);
                    /*
                    if (coll.Range(ans) < Mathf.Sqrt(3)) // ここ変える必要ある
                    {
                        Debug.Log("はいった2");
                        coll.sideHitBlock = true;

                        //houkou2d.x = 0;
                        //houkou2d.y = speed;

                        
                        clim = true;
                        iHozon = i;
                        //Debug.Log(ans);
                        break;
                    }
                    */
                    if(ans.x == 0)
                    {
                        Debug.Log("はいった2");
                        coll.sideHitBlock = true;

                        //houkou2d.x = 0;
                        //houkou2d.y = speed;


                        //clim = true;
                        iHozon = i;
                        //Debug.Log(ans);
                        break;
                    }
                }
            //}
            //ans.y = coll.OnPlayer(3);

        }

        houkou2d = new Vector2(houkou2d.x * ans.x, houkou2d.y * ans.y);
        if(houkou2d.x == 0)
        {
            clim = true;
            houkou2d.y = speed;
            //clim = true;
        }
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
                    if (coll.OnEnemy(1, this.gameObject) == 2)
                    {
                        iHozon = i;
                        //Debug.Log("これで決まり！：" + iHozon);
                        iKeep++;

                    }
                    else if (i == environment.blocks.Length - 1)
                    {
                        timeJump = 0;
                        canJump = 2;
                        startVec = 0;
                    }
                }

            }


            //  竜巻
            for (int i = 0; i < tList.tatumakiObjList.Length; i++)
            {
                coll = tList.tatumakiObjList[i].GetComponent<AllCollision>();
                if (coll.OnEnemy(4, this.gameObject) == 1)
                {
                    Debug.Log("omaeka");
                    timeJump = 0;
                    startVec = 10f;
                    canJump = 1;

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
            pPos.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (enemyScale.y / 2) + ground.y + hitBlock;

            // 頭ゴン
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    if (coll.candownHit)
                    {
                        float ans = coll.OnEnemy(3, this.gameObject);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //　　　　当たったブロック　ブロックスケール / 2    プレイヤースケール　/ 2   重複したground
                            hitBlock = coll.objPos.y - (coll.objScale.y / 2) - (enemyScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (enemyScale.y / 2) + ground.y + hitBlock;
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
            if (pPos.y - (enemyScale.y / 2) <= ground.y
                && stageColl != -1)
            {
                //Debug.Log("いるか");
                if (iHozon == -1
                    && ground.y == allColl.objPos.y + (allColl.objScale.y / 2)
                    && !allColl.sideHitBlock)
                {
                    //Debug.Log("いるかいるか");
                    if (pPos.y - (enemyScale.y / 2) >= allColl.objPos.y + (allColl.objScale.y / 2) - (jumpBlockRange * 2))
                    {
                        //Debug.Log("いるかいるかいるか");
                        canJump = 0;
                        Debug.Log("手レポ");
                        //   地面（ブロック）      プレイヤーのｙ下
                        pPos.y = ground.y + (enemyScale.y / 2);

                        hitBlock = 0;
                        //allColl.sideHitBlock = false;

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
                        if (pPos.y - (enemyScale.y / 2) >= coll.objPos.y + (coll.objScale.y / 2) - (jumpBlockRange * 2)
                            && pPos.y - (enemyScale.y / 2) <= coll.objPos.y + (coll.objScale.y / 2) + jumpBlockRange)
                        {
                            //Debug.Log("いるかいるかいるかいるかいるかいるか");
                            canJump = 0;
                            Debug.Log("手レポ");
                            //   地面（ブロック）      プレイヤーのｙ下
                            pPos.y = ground.y + (enemyScale.y / 2);

                            hitBlock = 0;
                            //coll.sideHitBlock = false;
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
                    float ans = coll.OnEnemy(2, this.gameObject);

                    // ブロックの座標が返された
                    if (ans == coll.objPos.y + (coll.objScale.y / 2))
                    {
                        //Debug.Log(ans);
                        //Debug.Log(transform.position.y - (transform.localScale.y / 2));
                        //  ここ＋ー　ではんいとる
                        if ((transform.position.y - (enemyScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (enemyScale.y / 2) >= ans - jumpBlockRange))
                        {
                            if (search)
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
                        ans = allColl.OnEnemy(2, this.gameObject);
                        if ((transform.position.y - (enemyScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (enemyScale.y / 2) >= ans - jumpBlockRange))
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
                    else if (ans == -10
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {

                        // プレイヤーがans以下
                        if (transform.position.y - (enemyScale.y / 2) <= ans)
                        {
                            if (allColl.OnEnemy(2, this.gameObject) == -10)
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
        //houkou3d.z = -speed;

        houkou3d.x = -speed;
        houkou3d.y = 0;

        houkou3d = transform.rotation * houkou3d;

        Vector3 ans = Vector3.one;
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            if (i == environment.blocks.Length)
            {
                ans = allColl.SideCollisionEnemy(new Vector3(houkou3d.x, houkou3d.y, houkou3d.z), this.gameObject);
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
                ans = coll.SideCollisionEnemy(new Vector3(houkou3d.x, houkou3d.y, houkou3d.z), this.gameObject);
                if (coll.Range(ans) < Mathf.Sqrt(3))
                {
                    Debug.Log("はいった");
                    coll.sideHitBlock = true;

                    houkou3d.x = 0;
                    houkou3d.y = speed;

                    maeClim = clim;
                    clim = true;

                    Debug.Log(ans);
                    break;
                }
            }

        }
        houkou3d = new Vector3(houkou3d.x * ans.x, houkou3d.y * ans.y, houkou3d.z * ans.z);
        //Debug.Log(houkou2d);
        //  0 を　３ｄでは houkou3d.z
        transform.position += new Vector3(houkou3d.x, houkou3d.y, houkou3d.z);

    }
}
