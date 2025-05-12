using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private PlayerMove3 playerMove;
    //[SerializeField]
    //private GameObject obj;

    private Vector3 playerPos;
    //[SerializeField]
    public Vector3 objPos;

    private Vector3 playerScale;
    [SerializeField]
    public Vector3 objScale;

    //[SerializeField]
    //private GameObject enemy;
    [SerializeField]
    private EnemyMove2 enemyMove;
    private Vector3 enemyPos;
    private Vector3 enemyScale;

    //[SerializeField]
    //private PlayerMove playerMove;
    private float blockPos;

    [SerializeField]
    private GameObject stage;

    private Vector3 stagePos;
    private Vector3 stageScale;

    /////// 前提条件///////
    // プレイヤーがオブジェの下面に頭ゴンするのか
    public bool candownHit;
    // プレイヤーが側面に当たることが出来るオブジェか
    public bool canSideHit;
    // 敵がオブジェの下面に頭ゴンするのか
    public bool candownHitEnemy;
    // 敵が側面に当たることが出来るオブジェか
    public bool canSideHitEnemy;
    /////////
    
    // プレイヤーが上にのっているか
    public bool onBlock;
    // プレイヤーが側面に当たっているか
    public bool sideHitBlock;
    // 敵が上にのっているか
    public bool onBlockEnemy;
    // 敵が側面に当たっているか
    public bool sideHitBlockEnemy;

    [SerializeField]
    private float range;


    private bool maeFrame;
    

    //[SerializeField]
    //private Vector3 obj2dModePos;
    //[SerializeField]
    public float obj3dModePosZ;
    
    /*
    [SerializeField]
    private Vector3 Wpos;
    [SerializeField]
    private Vector3 Lpos;
    */

    // Start is called before the first frame update
    void Start()
    {
        stage = GameObject.Find("Stage");
        stagePos = stage.transform.position;
        stageScale = stage.transform.localScale;

        Application.targetFrameRate = 60;
    }
    private void Awake()
    {
        player = GameObject.Find("player");
        playerMove = player.GetComponent<PlayerMove3>();

        stage = GameObject.Find("Stage");
        stagePos = stage.transform.position;
        stageScale = stage.transform.localScale;

        maeFrame = player.GetComponent<PlayerMove3>().mode2d3d;

        Application.targetFrameRate = 60;

        range = 0.1f;
    }

    private void FixedUpdate()
    {
        sideHitBlock = false;
        sideHitBlockEnemy = false;

        SetPos();
        if (maeFrame != player.GetComponent<PlayerMove3>().mode2d3d)
        {
            if (player.GetComponent<PlayerMove3>().mode2d3d)
            {
                Mode2d();
            }
            else
            {
                Mode3d();
            }
            maeFrame = player.GetComponent<PlayerMove3>().mode2d3d;
        }

        //Wpos = transform.position;
        //Lpos = transform.localPosition;
    }

    /*
    // Update is called once per frame
    void Update()
    {
        SetPos();
        if(maeFrame != player.GetComponent<PlayerMove3>().mode2d3d)
        {
            if (player.GetComponent<PlayerMove3>().mode2d3d)
            {
                Mode2d();
            }
            else
            {
                Mode3d();
            }
            maeFrame = player.GetComponent<PlayerMove3>().mode2d3d;
        }
        
        //Wpos = transform.position;
        //Lpos = transform.localPosition;
    }
    */
    // 3d　から　2D　モードになったとき
    public void Mode2d()
    {
        playerPos = player.transform.position;
        // ｘとｙは変わらない　奥行きだけplayerの座標に出てくる
        transform.position = new Vector3(objPos.x, objPos.y, playerPos.z);
        /*
        // 3D　モードでplayerが乗っていたら
        if(onBlock)
        {
            playerPos = player.transform.position;
            //playerPos.z += obj2dModePos.z;
            //player.transform.position = playerPos; 
        }
        */
    }

    // 2d　から　3D　モードになったとき
    public void Mode3d()
    {
        // ｘとｙは変わらない　奥行きだけ変化する
        transform.position = new Vector3(objPos.x, objPos.y, obj3dModePosZ);
        // ２D　モードでplayerが乗っていたら
        if (onBlock)
        {
            playerPos = player.transform.position;
            playerPos.z = obj3dModePosZ;
            player.transform.position = playerPos;
        }
    }


    //  changeMode = 1 : Onplayer   changeMode = 2 : Block  changeMode = 3 ; 下からのすり抜け防止
    public float OnEnemy(float changeMode, GameObject e)
    {
        SetPosEnemy(e);
        // stageのxz範囲にいるか
        if ((enemyPos.x + (enemyScale.x / 2) >= objPos.x - (objScale.x / 2))
            && (enemyPos.x - (enemyScale.x / 2) <= objPos.x + (objScale.x / 2))
            && (enemyPos.z + (enemyScale.z / 2) >= objPos.z - (objScale.z / 2))
            && (enemyPos.z - (enemyScale.z / 2) <= objPos.z + (objScale.z / 2)))
        {
            if (changeMode == 1)
            {
                // ステージより上
                if (enemyPos.y - (enemyScale.y / 2) > objPos.y + (objScale.y / 2))
                {
                    onBlockEnemy = false;
                    return 1.0f;
                }
                // ステージ上
                else if (enemyPos.y - (enemyScale.y / 2) >= objPos.y + (objScale.y / 2))
                {
                    onBlockEnemy = true;
                    return 2.0f;
                }
                // ステージより下
                else
                {
                    onBlockEnemy = false;
                    return 0;
                }
            }
            //
            else if (changeMode == 2)
            {
                // xz範囲で浮いてるブロック発見　敵の下にある
                if (enemyPos.y - (enemyScale.y / 2) >= objPos.y + (objScale.y / 2))
                {
                    return objPos.y + (objScale.y / 2);
                }
                //  敵の上にある
                else
                {
                    return objPos.y + (objScale.y / 2) - 1;
                }
            }
            // 頭がブロックにぶつかった
            else if (changeMode == 3)
            {
                // 敵の頭よりオブジェの底が上　　（0.05は遊び値）
                if (enemyPos.y + (enemyScale.y / 2) < objPos.y - (objScale.y / 2) + range
                    && (enemyPos.y + (enemyScale.y / 2) > objPos.y - (objScale.y / 2) - range))
                {
                    Debug.Log("あったった");
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //
            else if (changeMode == 4)
            {
                //  敵の頭よりうえにある   敵の足より下にある
                if (enemyPos.y + (enemyScale.y / 2) < objPos.y - (objScale.y / 2)
                    || enemyPos.y - (enemyScale.y / 2) > objPos.y + (objScale.y / 2))
                {
                    // 竜巻では何もしない
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //
            else
            {
                Debug.Log("エラー1");
                return 100;
            }
        }
        else
        {
            //  ステージ外
            if (changeMode == 1)
            {
                return -1.0f;
            }
            //
            else if (changeMode == 2)
            {
                //Debug.Log("エラー？");
                // 浮いてるブロックからステージに着地する時　　配列にステージいれればいい？
                if (this.gameObject != stage)
                {
                    //Debug.Log("これは言ってる？");
                    //AllCollision allCollision = stage.GetComponent<AllCollision>();
                    //return allCollision.OnEnemy(2, e);

                    return objPos.y + (objScale.y / 2) - 1;
                }
                else
                {
                    Debug.Log("エラー？");
                    return -10;
                }
            }
            //
            else if (changeMode == 3)
            {
                return 1;
            }
            //
            else if (changeMode == 4)
            {
                return 2;
            }
            //
            else
            {
                Debug.Log("エラー2");
                return 100;
            }
        }
    }
    private void SetPosEnemy(GameObject e)
    {
        enemyPos = e.transform.position;
        enemyMove = e.GetComponent<EnemyMove2>();
        enemyScale = enemyMove.enemyScale;

        objPos = transform.position;
    }

    public Vector3 SideCollisionEnemy(Vector3 move, GameObject e)
    {
        Vector3 ans = new Vector3(1, 1, 1);
        SetPosEnemy(e);
        bool notHit = false;
        if((enemyPos.y - (enemyScale.y / 2) >= objPos.y + (objScale.y / 2)
            || (enemyPos.y + (enemyScale.y / 2) <= objPos.y - (objScale.y / 2))))
        {
            notHit = true; // 高さが違うときtrue
            Debug.Log("haitta");
        }
        //  y範囲が同じとき
        if(!notHit)
        {
            // z範囲が同じとき
            if ((enemyPos.z - (enemyScale.z / 2) < objPos.z + (objScale.z / 2))
            && (enemyPos.z + (enemyScale.z / 2) > objPos.z - (objScale.z / 2)))
            {
                if ((enemyPos.x - (enemyScale.x / 2) < objPos.x + (objScale.x / 2))
                    && (enemyPos.x + (enemyScale.x / 2) > objPos.x - (objScale.x / 2)))
                {
                    //右面があたってるとき && ＋移動してるとき || 左面が当たってるとき && ー移動してるとき
                    if ((enemyPos.x < objPos.x && move.x > 0)
                        || (enemyPos.x > objPos.x && move.x < 0))
                    {
                        // x方向への移動を静止
                        ans.x = 0;

                        Debug.Log("aaaaaaa");

                    }
                }
            }
            if ((enemyPos.x - (enemyScale.x / 2) < objPos.x + (objScale.x / 2))
            && (enemyPos.x + (enemyScale.x / 2) > objPos.x - (objScale.x / 2)))
            {
                if ((enemyPos.z - (enemyScale.z / 2) < objPos.z + (objScale.z / 2))
                    && (enemyPos.z + (enemyScale.z / 2) > objPos.z - (objScale.z / 2)))
                {
                    //前面があたってるとき && ＋移動してるとき || 後面が当たってるとき && ー移動してるとき
                    if ((enemyPos.z < objPos.z && move.z > 0)
                        || (enemyPos.z > objPos.z && move.z < 0))
                    {
                        // z方向への移動を静止
                        ans.z = 0;

                    }
                }
            }
        }
        return ans;
    }




    //  changeMode = 1 : Onplayer   changeMode = 2 : Block  changeMode = 3 ; 下からのすり抜け防止  changeMode = 4 ; 竜巻と敵専用？（オブジェに触れたら反応）
    public float OnPlayer(float changeMode)
    {
        SetPos();
        // stageのxz範囲にいるか
        if ( (playerPos.x + (playerScale.x / 2) >= objPos.x - (objScale.x / 2))
            &&(playerPos.x - (playerScale.x / 2) <= objPos.x + (objScale.x / 2))
            &&(playerPos.z + (playerScale.z / 2) >= objPos.z - (objScale.z / 2))
            &&(playerPos.z - (playerScale.z / 2) <= objPos.z + (objScale.z / 2)) )
        {
            if(changeMode == 1)
            {
                // ステージより上
                if(playerPos.y - (playerScale.y / 2) > objPos.y + (objScale.y / 2))
                {
                    onBlock = false;
                    return 1.0f;
                }
                // ステージ上
                else if(playerPos.y - (playerScale.y / 2) == objPos.y + (objScale.y / 2))
                        //&& playerPos.y - (playerScale.y / 2) > objPos.y + (objScale.y / 2) - 0.05) //これいらないかも
                {
                    onBlock = true;
                    return 2.0f;
                }
                // ステージより下
                else
                {
                    onBlock = false;
                    return 0;
                }
            }
            //
            else if(changeMode == 2)
            {
                // xz範囲で浮いてるブロック発見　プレイヤーの下にある
                if(playerPos.y - (playerScale.y / 2) >= objPos.y + (objScale.y / 2))
                {
                    return objPos.y + (objScale.y / 2);
                }
                //  プレイヤーの上にある
                else
                {
                    //Debug.Log("kokoo1");
                    return objPos.y + (objScale.y / 2) - 1;
                }
            }
            // 頭がブロックにぶつかった
            else if(changeMode == 3)
            {
                // プレイヤーの頭よりオブジェの底が上　　（0.05は遊び値）
                if(playerPos.y + (playerScale.y / 2) < objPos.y - (objScale.y / 2) + range
                    && (playerPos.y + (playerScale.y / 2) > objPos.y - (objScale.y / 2) - range ))
                {
                    Debug.Log("あったった");
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //
            else if(changeMode == 4)
            {
                //  プレイヤーの頭よりうえにある   プレイヤーの足より下にある
                if(playerPos.y + (playerScale.y / 2) < objPos.y - (objScale.y / 2)
                    || playerPos.y - (playerScale.y / 2) > objPos.y + (objScale.y / 2))
                {
                    // 竜巻では何もしない
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //
            else
            {
                Debug.Log("エラー1");
                return 100;
            }
        }
        else
        {
            //  ステージ外
            if(changeMode == 1)
            {
                return -1.0f;
            }
            //
            else if(changeMode == 2)
            {
                Debug.Log("エラー？");
                // 浮いてるブロックからステージに着地する時　　配列にステージいれればいい？
                if(this.gameObject != stage)
                {
                    //Debug.Log("これは言ってる？");
                    //AllCollision allCollision = stage.GetComponent<AllCollision>();
                    //return allCollision.OnPlayer(2);

                    return objPos.y + (objScale.y / 2) - 1;
                }
                else
                {
                    Debug.Log("エラー？");
                    return -10;
                }
            }
            //
            else if(changeMode == 3)
            {
                return 1;
            }
            //
            else if(changeMode == 4)
            {
                return 2;
            }
            //
            else
            {
                Debug.Log("エラー2");
                return 100;
            }
        }
    }
    void SetPos()
    {
        playerPos = player.transform.position;
        playerScale = playerMove.playerScale;

        objPos = transform.position;

        //blockPos = player.GetComponent<PlayerMove>().ground;
    }
    public Vector3 SideCollision(Vector3 move)
    {
        Vector3 ans = new Vector3(1, 1, 1);
        SetPos();
        //  y範囲が同じとき
        if((playerPos.y - (playerScale.y / 2) < objPos.y + (objScale.y / 2))
           && (playerPos.y + (playerScale.y / 2) > objPos.y - (objScale.y / 2)))
        {
            // z範囲が同じとき
            if ((playerPos.z - (playerScale.z / 2) < objPos.z + (objScale.z / 2))
            && (playerPos.z + (playerScale.z / 2) > objPos.z - (objScale.z / 2)))
            {
                if( (playerPos.x - (playerScale.x / 2) < objPos.x + (objScale.x / 2))
                    && (playerPos.x + (playerScale.x / 2) > objPos.x - (objScale.x / 2)))
                {
                    //右面があたってるとき && ＋移動してるとき || 左面が当たってるとき && ー移動してるとき
                    if( ( playerPos.x < objPos.x && move.x > 0 )
                        || ( playerPos.x > objPos.x && move.x < 0))
                    {
                        // x方向への移動を静止
                        ans.x = 0;
                    }
                }
            }
            if ((playerPos.x - (playerScale.x / 2) < objPos.x + (objScale.x / 2))
            && (playerPos.x + (playerScale.x / 2) > objPos.x - (objScale.x / 2)))
            {
                if ((playerPos.z - (playerScale.z / 2) < objPos.z + (objScale.z / 2))
                    && (playerPos.z + (playerScale.z / 2) > objPos.z - (objScale.z / 2)))
                {
                    //前面があたってるとき && ＋移動してるとき || 後面が当たってるとき && ー移動してるとき
                    if ((playerPos.z < objPos.z && move.z > 0)
                        || (playerPos.z > objPos.z && move.z < 0))
                    {
                        // z方向への移動を静止
                        ans.z = 0;
                    }
                }
            }
        }
        return ans;
    }

    //  大きさ計測
    public float Range(Vector3 a)
    {
        a.x = a.x * a.x;
        a.y = a.y * a.y;
        a.z = a.z * a.z;
        return Mathf.Sqrt(a.x + a.y + a.z);
    }



    /*
    // 地面の位置を返す
    public float Block()
    {
        SetPos();
        // stageのxz範囲にいるか
        if ((playerPos.x - (playerScale.x / 2) >= objPos.x - (objScale.x / 2)
            && playerPos.x + (playerScale.x / 2) <= objPos.x + (objScale.x / 2))
            && (playerPos.z - (playerScale.z / 2) >= objPos.z - (objScale.z / 2)
            && playerPos.z + (playerScale.z / 2) <= objPos.z + (objScale.z / 2)))
        {
            if(playerPos.y - (playerScale.y / 2) < objPos.y + (objScale.y / 2) )
            {
                //　地面の位置
                return stagePos.y + (stageScale.y / 2);
            }
            else
            {
                //Debug.Log(objPos);

                //  ブロックの位置
                return objPos.y + (objScale.y / 2);
            }

        }
        else
        {
            //  地面の位置
            return stagePos.y + (stageScale.y / 2);
        }
    }
    */
}