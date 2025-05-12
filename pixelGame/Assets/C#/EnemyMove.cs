using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //[SerializeField]
    public Vector3 move;

    [SerializeField]
    private int canJump;
    public float timeJump;
    [SerializeField]
    private float gravity;
    public Vector3 ground;

    private GameObject stage;

    private AllCollision allCollision;
    private AllCollision allColl; // ステージ用

    private Environment environment;

    [SerializeField]
    private float stageColl;
    private int iHozon;

    private float jumpBlockRange;

    private float hitBlock;

    private float startVec;

    [SerializeField]
    private TikuwaList tList;

    [SerializeField]
    private int moveChange;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        allCollision = GetComponent<AllCollision>();

        environment = GameObject.Find("SetStage").GetComponent<Environment>();

        tList = GameObject.Find("TikuwaList").GetComponent<TikuwaList>();

        gravity = 9.8f;
        timeJump = 0;

        stage = GameObject.Find("Stage");
        allColl = stage.GetComponent<AllCollision>();

        /*
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            allCollision = environment.blocks[i].GetComponent<AllCollision>();
            float ans = allCollision.OnEnemy(1, this.gameObject);
            if (ans == 2)
            {
                ground.y = environment.blocks[i].transform.position.y;
                stageColl = allCollision.OnEnemy(1, this.gameObject);
                iHozon = i;
                break;
            }
            else if (i == environment.blocks.Length - 1
                && allCollision.OnEnemy(1, this.gameObject) != 2)
            {
                ground.y = stage.transform.position.y;
                stageColl = allColl.OnEnemy(1, this.gameObject);
                iHozon = -1;

                //hitBlock = transform.position.y;
            }
        }
        */
    }

    [SerializeField]
    float speed;
    void Move()
    {
        speed = -0.1f;
        move = new Vector3(transform.position.x + speed, ground.y, 0);
        transform.position = move;
    }

    //  自分からジャンプすることはない
    void Jump()
    {
        if (canJump == 0)
        {
            timeJump = 0;
            startVec = 5;
            canJump = 1;

        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;

            Vector3 pPos = transform.position;
            pPos.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground.y + hitBlock;

            // 頭ゴン
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.candownHit)
                    {
                        float ans = allCollision.OnEnemy(3, this.gameObject);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //　　　　当たったブロック　ブロックスケール / 2    プレイヤースケール　/ 2   重複したground
                            hitBlock = allCollision.objPos.y - (allCollision.objScale.y / 2) - (transform.localScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground.y + hitBlock;
                            break;
                        }
                    }
                }
            }
        }

        /*
        if (canJump == 0)
        {
            //  trueの状態で落ちたとき用
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                int iKeep = 0;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.OnEnemy(1, this.gameObject) == 2)
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
        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            //playerPos (ここだけの)
            Vector3 pPos = transform.position;
            pPos.y = ground.y + hitBlock;

            // 頭ゴン
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.candownHit)
                    {
                        float ans = allCollision.OnEnemy(3, this.gameObject);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //　　　　当たったブロック　ブロックスケール / 2    プレイヤースケール　/ 2   重複したground
                            hitBlock = allCollision.objPos.y - (allCollision.objScale.y / 2) - (transform.localScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground.y + hitBlock;
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
            if (pPos.y - (transform.localScale.y / 2) <= ground.y
                && stageColl != -1)
            {
                //Debug.Log("いるか");
                if (iHozon == -1
                    && ground.y == allColl.objPos.y + (allColl.objScale.y / 2)
                    && !allColl.sideHitBlockEnemy)
                {
                    //Debug.Log("いるかいるか");
                    if (pPos.y - (transform.localScale.y / 2) >= allColl.objPos.y + (allColl.objScale.y / 2) - (jumpBlockRange * 2))
                    {
                        //Debug.Log("いるかいるかいるか");
                        canJump = 0;
                        Debug.Log("手レポ");
                        //   地面（ブロック）      プレイヤーのｙ下
                        pPos.y = ground.y + (transform.localScale.y / 2);

                        hitBlock = 0;
                        //allColl.sideHitBlockEnemy = false;
                    }

                    else
                    {
                        Debug.Log("次座標がそれより下");
                    }

                }
                else if (iHozon != -1)
                {
                    //Debug.Log("いるかいるかいるかいるか");
                    allCollision = environment.blocks[iHozon].GetComponent<AllCollision>();
                    if (ground.y == allCollision.objPos.y + (allCollision.objScale.y / 2)
                        && !allCollision.sideHitBlockEnemy)
                    {
                        //Debug.Log("いるかいるかいるかいるかいるか");
                        if (pPos.y - (transform.localScale.y / 2) >= allCollision.objPos.y + (allCollision.objScale.y / 2) - (jumpBlockRange * 2)
                            && pPos.y - (transform.localScale.y / 2) <= allCollision.objPos.y + (allCollision.objScale.y / 2) + jumpBlockRange)
                        {
                            //Debug.Log("いるかいるかいるかいるかいるかいるか");
                            canJump = 0;
                            Debug.Log("手レポ");
                            //   地面（ブロック）      プレイヤーのｙ下
                            pPos.y = ground.y + (transform.localScale.y / 2);

                            hitBlock = 0;
                            //coll.sideHitBlockEnemy = false;
                        }
                    }
                }
            }
            for (int i = 0; i < environment.blocks.Length; i++)
            {
                allCollision = environment.blocks[i].GetComponent<AllCollision>();
                allCollision.sideHitBlockEnemy = false;
            }
            allColl.sideHitBlockEnemy = false;


            transform.position = pPos;


            // 降下中のときに下にブロックがあるか調べる
            if (canJump == 2)
            {
                //一度ブロックをみつけるとtrue
                bool search = false;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    // 浮いてるブロックのOnplayer(2)用
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    float ans = allCollision.OnEnemy(2, this.gameObject);

                    // ブロックの座標が返された
                    if (ans == allCollision.objPos.y + (allCollision.objScale.y / 2))
                    {
                        //Debug.Log(ans);
                        //Debug.Log(transform.position.y - (transform.localScale.y / 2));
                        //  ここ＋ー　ではんいとる
                        if ((transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
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
                    else if (ans == allCollision.objPos.y + (allCollision.objScale.y / 2) - 1
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {
                        Debug.Log("ここ！");
                        ans = allColl.OnEnemy(2, this.gameObject);
                        if ((transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
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
                        if (transform.position.y - (transform.localScale.y / 2) <= ans)
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




        /*
        //transform.position += move;

        if (allCollision.OnPlayer(4) == 1)
        {
            Debug.Log("敵に当たったよ");
        }
        */
    }

    void DownMove()
    {
        speed = -0.1f;
        move = new Vector3(0, speed, 0);
        transform.position += move;
        Debug.Log("下がった");
    }

    void UpMove()
    {
        speed = 0.1f;
        move = new Vector3(0, speed, 0);
        transform.position += move;

        Debug.Log("上り");
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            allCollision = environment.blocks[i].GetComponent<AllCollision>();
            float ans = allCollision.OnEnemy(1, this.gameObject);
            if (ans == 2)
            {
                ground.y = environment.blocks[i].transform.position.y;
                stageColl = allCollision.OnEnemy(1, this.gameObject);
                iHozon = i;

                moveChange = 0;
                break;
            }
            else if (i == environment.blocks.Length - 1
                && allCollision.OnEnemy(1, this.gameObject) != 2)
            {
                ans = allColl.OnEnemy(1, this.gameObject);
                if (ans == 2)
                {
                    ground.y = stage.transform.position.y;
                }
                else if (ans == 1)
                {
                    // ジャンプしているか
                    if (canJump == 1)
                    {
                        moveChange = 1;
                    }
                    else if (canJump == 0)
                    {
                        moveChange = 2;
                    }
                    else
                    {
                        moveChange = -1;
                    }
                }
            }
        }
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            allCollision = environment.blocks[i].GetComponent<AllCollision>();
            Vector3 ans = allCollision.SideCollisionEnemy(move, this.gameObject);
            if (ans.x == 0 && moveChange == 0)
            {
                /*
                ground.y = environment.blocks[i].transform.position.y;
                stageColl = allCollision.OnEnemy(1, this.gameObject);
                iHozon = i;
                */
                // 上り
                if (moveChange == 0 || moveChange == 1)
                {
                    moveChange = 3;
                    break;
                }
                else
                {
                    Debug.Log("エラー");
                }
            }
        }

        //  横方向の移動
        if (moveChange == 0)
        {
            Move();
        }
        //  竜巻のせい
        else if (moveChange == 1)
        {
            Jump();
        }
        //　木から降りる
        else if (moveChange == 2)
        {
            DownMove();
        }
        //  木に登る
        else if (moveChange == 3)
        {
            UpMove();
        }























        /*
        if (canJump == 0)
        {
            //  trueの状態で落ちたとき用
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                int iKeep = 0;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.OnEnemy(1, this.gameObject) == 2)
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
        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            //playerPos (ここだけの)
            Vector3 pPos = transform.position;
            pPos.y = ground.y + hitBlock;

            // 頭ゴン
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.candownHit)
                    {
                        float ans = allCollision.OnEnemy(3, this.gameObject);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //　　　　当たったブロック　ブロックスケール / 2    プレイヤースケール　/ 2   重複したground
                            hitBlock = allCollision.objPos.y - (allCollision.objScale.y / 2) - (transform.localScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground.y + hitBlock;
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
            if (pPos.y - (transform.localScale.y / 2) <= ground.y
                && stageColl != -1)
            {
                //Debug.Log("いるか");
                if (iHozon == -1
                    && ground.y == allColl.objPos.y + (allColl.objScale.y / 2)
                    && !allColl.sideHitBlockEnemy)
                {
                    //Debug.Log("いるかいるか");
                    if (pPos.y - (transform.localScale.y / 2) >= allColl.objPos.y + (allColl.objScale.y / 2) - (jumpBlockRange * 2))
                    {
                        //Debug.Log("いるかいるかいるか");
                        canJump = 0;
                        Debug.Log("手レポ");
                        //   地面（ブロック）      プレイヤーのｙ下
                        pPos.y = ground.y + (transform.localScale.y / 2);

                        hitBlock = 0;
                        //allColl.sideHitBlockEnemy = false;
                    }

                    else
                    {
                        Debug.Log("次座標がそれより下");
                    }

                }
                else if (iHozon != -1)
                {
                    //Debug.Log("いるかいるかいるかいるか");
                    allCollision = environment.blocks[iHozon].GetComponent<AllCollision>();
                    if (ground.y == allCollision.objPos.y + (allCollision.objScale.y / 2)
                        && !allCollision.sideHitBlockEnemy)
                    {
                        //Debug.Log("いるかいるかいるかいるかいるか");
                        if (pPos.y - (transform.localScale.y / 2) >= allCollision.objPos.y + (allCollision.objScale.y / 2) - (jumpBlockRange * 2)
                            && pPos.y - (transform.localScale.y / 2) <= allCollision.objPos.y + (allCollision.objScale.y / 2) + jumpBlockRange)
                        {
                            //Debug.Log("いるかいるかいるかいるかいるかいるか");
                            canJump = 0;
                            Debug.Log("手レポ");
                            //   地面（ブロック）      プレイヤーのｙ下
                            pPos.y = ground.y + (transform.localScale.y / 2);

                            hitBlock = 0;
                            //coll.sideHitBlockEnemy = false;
                        }
                    }
                }
            }
            for (int i = 0; i < environment.blocks.Length; i++)
            {
                allCollision = environment.blocks[i].GetComponent<AllCollision>();
                allCollision.sideHitBlockEnemy = false;
            }
            allColl.sideHitBlockEnemy = false;


            transform.position = pPos;


            // 降下中のときに下にブロックがあるか調べる
            if (canJump == 2)
            {
                //一度ブロックをみつけるとtrue
                bool search = false;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    // 浮いてるブロックのOnplayer(2)用
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    float ans = allCollision.OnEnemy(2, this.gameObject);

                    // ブロックの座標が返された
                    if (ans == allCollision.objPos.y + (allCollision.objScale.y / 2))
                    {
                        //Debug.Log(ans);
                        //Debug.Log(transform.position.y - (transform.localScale.y / 2));
                        //  ここ＋ー　ではんいとる
                        if ((transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
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
                    else if (ans == allCollision.objPos.y + (allCollision.objScale.y / 2) - 1
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {
                        Debug.Log("ここ！");
                        ans = allColl.OnEnemy(2, this.gameObject);
                        if ((transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
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
                        if (transform.position.y - (transform.localScale.y / 2) <= ans)
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





        //transform.position += move;

        if (allCollision.OnPlayer(4) == 1)
        {
            Debug.Log("敵に当たったよ");
        }

    }

        */
        /*
        // Update is called once per frame
        void Update()
        {
            for(int i = 0; i < environment.blocks.Length; i++)
            {
                allCollision = environment.blocks[i].GetComponent<AllCollision>();
                if (allCollision.OnEnemy(1, this.gameObject) == 1)
                {

                }
            }
            transform.position += move;

            if(allCollision.OnPlayer(4) == 1)
            {
                Debug.Log("敵に当たったよ");
            }
        }
        */
    }
}
