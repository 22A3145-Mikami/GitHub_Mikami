using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    private GameObject obj_A; //プレイヤーとエネミー
    private ObjInfo allInfo_A;//obj_AのObjInfoコード
    private Vector3 obj_A_Position; //プレイヤーとエネミー
    private Vector3 obj_A_Scale; //プレイヤーとエネミー

    private GameObject obj_B; // ブロックとエネミー
    private ObjInfo allInfo_B;//obj_AのObjInfoコード
    private Vector3 obj_B_Position; // ブロックとエネミー
    private Vector3 obj_B_Scale; // ブロックとエネミー

    public GameObject player;//プレイヤー

    public List<GameObject> enemys;//敵の配列

    public List<GameObject> kis;//木の配列

    public List<GameObject> blocks;//ブロックの配列

    public List<GameObject> tatumakis;//竜巻の配列

    public GameObject goal;//家オブジェ

    //モード切り替えのコードと変数
    [SerializeField]
    private Change2D3D change2D3DCode;
    private int change2D3D;


    public float justBlockRange; // ブロックの　ー１　を取りやすくするための値  yの頭と足だけ  ←　全部に着けた
    [SerializeField]
    private float keepJustBlockRange;//保存用

    //ゲーム全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    private GameObject dead_press_moji; //死因文字　圧死

    private GameObject dead_enemy_moji;//死因文字　敵


    private void Awake()
    {
        //モード切り替えの取得
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //フレームレート設定
        Application.targetFrameRate = 60;

        //現在位置のブロック
        keepJustBlockRange = justBlockRange;

        //ゲーム全体の流れ取得
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
        
        //死因文字を入れる
        dead_press_moji = gameMakerCode.dead_press_moji;
        dead_enemy_moji = gameMakerCode.dead_enemy_moji;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //一時中断していない　かつ　コースプレイ中
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1 && !gameMakerCode.pushR)
        {
            //プレイヤー取得
            player = GameObject.FindGameObjectWithTag("Player");



            //エネミーの検索
            enemys = new List<GameObject>(GameObject.FindGameObjectsWithTag("enemy"));

            // ブロックの検索
            blocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("block"));

            
            //１Dモードのときはプレイヤーもブロックにする
            if(change2D3D == 1)
            {
                player.GetComponent<ObjInfo>().block = true;
                blocks.Add(player);
            }
            else
            {
                player.GetComponent<ObjInfo>().block = false;
            }
            //

            // 竜巻の検索
            tatumakis = new List<GameObject>(GameObject.FindGameObjectsWithTag("tatumaki"));

            // 木の検索
            kis = new List<GameObject>(GameObject.FindGameObjectsWithTag("tree"));

            //ゴール取得
            goal = GameObject.FindGameObjectWithTag("goal"); 

            //モード値更新
            change2D3D = change2D3DCode.change2D3D;


            // プレイヤーと敵の当たり判定
            for (int i = 0; i < enemys.Count; i++)
            {
                allInfo_A = player.GetComponent<ObjInfo>();
                allInfo_A.ResetHitWall();

                if (change2D3D == 3)
                {
                    AtariHantei3D(player, enemys[i], 2);
                }
                else if (change2D3D == 2)
                {
                    AtariHantei2D(player, enemys[i], 2);
                }
            }

            //家に着いたか
            if (change2D3D == 3)
            {
                AtariHantei3D(player, goal, 3);
            }
            else if (change2D3D != 3)
            {
                AtariHantei2D(player, goal, 3);
            }
        }
        

    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public void AtariHantei3D(GameObject playerORenemy, GameObject blockORenemy, int typeAns) // typeAns == 0 今まで通りの　typeAns == 1 浮き床とプレイヤーが接しているか   typeAns == 2 プレイヤーと敵の当たり判定  typeAns == 3 プレイヤーとゴール
    {
        SetPositionScale_object_A(playerORenemy); // object_A  playerORenemy

        SetPositionScale_object_B(blockORenemy); // object_B  blockORenemy

        if(!allInfo_B.toreruyuka) //通れる床の場合これしない
        {
            // x軸当たり判定
            if (obj_A_Position.x + (obj_A_Scale.x / 2) + justBlockRange > obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.x + (obj_A_Scale.x / 2) - justBlockRange < obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2)
                    && obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//オブジェ衝突
                {
                    allInfo_A.migiWall = true;
                }
                else if(typeAns == 2)//プレイヤーと敵衝突
                {
                    gameMakerCode.gameOver = true;

                    //敵に当たった
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//プレイヤーと家衝突
                {
                    gameMakerCode.gameClear = true;
                }
            }

            if (obj_A_Position.x - (obj_A_Scale.x / 2) - justBlockRange < obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.x - (obj_A_Scale.x / 2) + justBlockRange > obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2)
                    && obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//オブジェ衝突
                {
                    allInfo_A.hidariWall = true;
                }
                else if (typeAns == 2)//プレイヤーと敵衝突
                {
                    gameMakerCode.gameOver = true;

                    //敵に当たった
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//プレイヤーと家衝突
                {
                    gameMakerCode.gameClear = true;
                }

            }

            // z軸当たり判定
            if (obj_A_Position.z + (obj_A_Scale.z / 2) + justBlockRange > obj_B_Position.z - (obj_B_Scale.z / 2)
                    && obj_A_Position.z + (obj_A_Scale.z / 2) - justBlockRange < obj_B_Position.z - (obj_B_Scale.z / 2)
                    && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//オブジェ衝突
                {
                    allInfo_A.okuWall = true;
                }
                else if (typeAns == 2)//プレイヤーと敵衝突
                {
                    gameMakerCode.gameOver = true;

                    //敵に当たった
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//プレイヤーと家衝突
                {
                    gameMakerCode.gameClear = true;
                }

            }

            if (obj_A_Position.z - (obj_A_Scale.z / 2) - justBlockRange < obj_B_Position.z + (obj_B_Scale.z / 2)
                    && obj_A_Position.z - (obj_A_Scale.z / 2) + justBlockRange > obj_B_Position.z + (obj_B_Scale.z / 2)
                    && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//オブジェ衝突
                {
                    allInfo_A.temaeWall = true;
                }
                else if (typeAns == 2)//プレイヤーと敵衝突
                {
                    gameMakerCode.gameOver = true;

                    //敵に当たった
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//プレイヤーと家衝突
                {
                    gameMakerCode.gameClear = true;
                }
            }


        }

        // y軸当たり判定
        ChangeJustBlockRange();
        if(!allInfo_B.toreruyuka) //通れる床の場合これしない
        {
            if (obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2) - justBlockRange
            && obj_A_Position.y + (obj_A_Scale.y / 2) < obj_B_Position.y - (obj_B_Scale.y / 2) + justBlockRange
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
            && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2)
            && obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2))
            {
                if(typeAns == 0)//オブジェ衝突
                {
                    allInfo_A.ueWall = true;
                }
                else if (typeAns == 2)//プレイヤーと敵衝突
                {
                    gameMakerCode.gameOver = true;

                    //敵に当たった
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//プレイヤーと家衝突
                {
                    gameMakerCode.gameClear = true;
                }

            }
        }
        if (obj_A_Position.y - (obj_A_Scale.y / 2) <= obj_B_Position.y + (obj_B_Scale.y / 2) + 0.01f
            && obj_A_Position.y - (obj_A_Scale.y / 2) > obj_B_Position.y + (obj_B_Scale.y / 2) - justBlockRange
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
            && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2)
            && obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2))
        {
            //オブジェ衝突
            if (typeAns == 0
                && (obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y)
                && !(allInfo_A.hidariWall || allInfo_A.migiWall || allInfo_A.temaeWall || allInfo_A.okuWall))
            {
                allInfo_A.sitaWall = true;
            }

            // 浮き床用コード
            else if (typeAns == 1)
            {
                UkiyukaMove ukiyukaMove = obj_B.GetComponent<UkiyukaMove>();
                ukiyukaMove.onPlayer = true;
            }
            else if (typeAns == 2)//プレイヤーと敵衝突
            {
                gameMakerCode.gameOver = true;

                //敵に当たった
                dead_enemy_moji.SetActive(true);
                //
            }
            else if (typeAns == 3)//プレイヤーと家衝突
            {
                gameMakerCode.gameClear = true;
            }

        }
        ResetJustBlockRane();

    }

    [SerializeField]
    private float addJustBlockRange;//増幅用
    private void ChangeJustBlockRange()
    {
        //プレイヤー用
        if(obj_A == player)
        {
            PlayerMove4 playerMove = obj_A.GetComponent<PlayerMove4>();

            if(!playerMove.onukiyuka) //浮き床にいるときは実行しない
            {
                //　上昇から下降になった時の加算をリセットする
                if (playerMove.canJump != playerMove.keepCanJump)
                {
                    playerMove.addJustBlockRange = 0;
                }

                float distance = playerMove.keepMove_y - obj_A_Position.y;
                if (distance < 0)
                {
                    distance *= -1;
                }
                playerMove.addJustBlockRange += distance;
                //上下方向に移動中は当たり判定を大きくする
                if (playerMove.canJump == 1)
                {
                    justBlockRange += playerMove.addJustBlockRange / 2;
                }
                else if ((playerMove.canJump == 2) || (playerMove.canJump == 0 && distance != 0))
                {
                    justBlockRange += playerMove.addJustBlockRange;
                }
                else
                {
                    playerMove.addJustBlockRange = 0;
                }
                playerMove.keepCanJump = playerMove.canJump;
                playerMove.keepMove_y = obj_A_Position.y;
            }
            
        }
        //敵用
        else if(allInfo_A.enemy)
        {
            EnemyMove3 enemyMove = obj_A.GetComponent<EnemyMove3>();


            //　上昇から下降になった時の加算をリセットする
            if (enemyMove.canJump != enemyMove.keepCanJump)
            {
                enemyMove.addJustBlockRange = 0;
            }

            float distance = enemyMove.keepMove_y - obj_A_Position.y;
            if (distance < 0)
            {
                distance *= -1;
            }
            enemyMove.addJustBlockRange += distance;

            //上下方向に移動中は当たり判定を大きくする
            if (enemyMove.canJump == 1)
            {
                justBlockRange += enemyMove.addJustBlockRange / 2;
            }
            else if (enemyMove.canJump == 2)
            {
                justBlockRange += enemyMove.addJustBlockRange;
            }
            else
            {
                enemyMove.addJustBlockRange = 0;
            }
            enemyMove.keepCanJump = enemyMove.canJump;
            enemyMove.keepMove_y = obj_A_Position.y;
        }
    }
    //初期値に戻す
    private void ResetJustBlockRane()
    {
        justBlockRange = keepJustBlockRange;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public void AtariHantei_tatumaki(GameObject playerORenemy, GameObject blockORenemy)
    {
        SetPositionScale_object_A(playerORenemy); // object_A  playerORenemy

        SetPositionScale_object_B(blockORenemy); // object_B  blockORenemy
        //XY軸当たり判定
        if (obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2)
            && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2))
        {
            if (change2D3D == 1)//１Dモード
            {
                //何もしない
            }
            else if(change2D3D == 2)//２Dモード
            {
                allInfo_A.tatumakiHit = true;
            }
            //Z軸当たり判定　３Dモード用
            else if(obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2)
                && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2))
            {
                if (change2D3D == 3)
                {
                    allInfo_A.tatumakiHit = true;
                }
            }
        }
    }
    //圧死コード
    public void Death_Press(GameObject playerORenemy, GameObject block)
    {
        SetPositionScale_object_A(playerORenemy); // object_A  playerORenemy

        SetPositionScale_object_B(block); // object_B  blockORenemy
        if(!allInfo_B.toreruyuka)
        {
            //XY軸当たり判定
            if (obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2) - justBlockRange
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2) + justBlockRange
            && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2) - justBlockRange
            && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2) + justBlockRange)
            {
                //プレイヤー用
                if(obj_A == player)
                {
                    gameMakerCode.gameOver = true;

                    Destroy(obj_A);
                    //圧死した
                    dead_press_moji.SetActive(true);
                    //
                }
                //敵用
                else if (allInfo_A.enemy)
                {
                    Destroy(obj_A);
                }
            }
        }

    }

    public void AtariHantei2D(GameObject playerORenemy, GameObject blockORenemy, int typeAns) // typeAns == 0 今まで通りの　typeAns == 1 浮き床とプレイヤーが接しているか　typeAns == 2 敵に当たったか
    {
        SetPositionScale_object_A(playerORenemy); // object_A  playerORenemy

        SetPositionScale_object_B(blockORenemy); // object_B  blockORenemy
        if(!allInfo_B.toreruyuka)
        {
            // x軸当たり判定
            if (obj_A_Position.x + (obj_A_Scale.x / 2) + justBlockRange > obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.x + (obj_A_Scale.x / 2) - justBlockRange < obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//オブジェ衝突
                {
                    allInfo_A.migiWall = true;
                }
                else if (typeAns == 2)//プレイヤーと敵衝突
                {
                    if(change2D3D == 2)
                    {
                        gameMakerCode.gameOver = true;
                        //敵に当たった
                        dead_enemy_moji.SetActive(true);
                        //
                        //Debug.Log("死因：敵");
                    }
                }
                else if (typeAns == 3)//プレイヤーと家衝突
                {
                    gameMakerCode.gameClear = true;
                }


            }
            
            if (obj_A_Position.x - (obj_A_Scale.x / 2) - justBlockRange < obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.x - (obj_A_Scale.x / 2) + justBlockRange > obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//オブジェ衝突
                {
                    allInfo_A.hidariWall = true;
                }
                else if (typeAns == 2)//プレイヤーと敵衝突
                {
                    if (change2D3D == 2)
                    {
                        gameMakerCode.gameOver = true;
                        //敵に当たった
                        dead_enemy_moji.SetActive(true);
                        //
                        //Debug.Log("死因：敵");
                    }
                }
                else if (typeAns == 3)//プレイヤーと家衝突
                {
                    gameMakerCode.gameClear = true;
                }

            }
        }

        
        // y軸当たり判定
        ChangeJustBlockRange();
        if(!allInfo_B.toreruyuka)
        {
            if (obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2) - justBlockRange
            && obj_A_Position.y + (obj_A_Scale.y / 2) < obj_B_Position.y - (obj_B_Scale.y / 2) + justBlockRange
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2))
            {
                if (typeAns == 0)//オブジェ衝突
                {
                    allInfo_A.ueWall = true;
                }
                else if (typeAns == 2)//プレイヤーと敵衝突
                {
                    if (change2D3D == 2)
                    {
                        gameMakerCode.gameOver = true;
                        //敵に当たった
                        dead_enemy_moji.SetActive(true);
                        //
                        //Debug.Log("死因：敵");
                    }
                }
                else if (typeAns == 3)//プレイヤーと家衝突
                {
                    gameMakerCode.gameClear = true;
                }


            }
        }
        if (obj_A_Position.y - (obj_A_Scale.y / 2) <= obj_B_Position.y + (obj_B_Scale.y / 2) + (justBlockRange / 5) //　0.01ｆだと芋虫と落下で落ちる
            && obj_A_Position.y - (obj_A_Scale.y / 2) > obj_B_Position.y + (obj_B_Scale.y / 2) -  justBlockRange 
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2))
        {
            //オブジェ衝突
            if (typeAns == 0
                && (obj_A_Position.y + ( obj_A_Scale.y / 2 ) > obj_B_Position.y)
                && !(allInfo_A.hidariWall || allInfo_A.migiWall || allInfo_A.temaeWall || allInfo_A.okuWall))
            {
                allInfo_A.sitaWall = true;
            }

            // 浮き床用コード
            else if(typeAns == 1)
            {
                UkiyukaMove ukiyukaMove = obj_B.GetComponent<UkiyukaMove>();
                ukiyukaMove.onPlayer = true;

                allInfo_A.sitaWall = true;

                if(allInfo_A.player)
                {
                    PlayerMove4 pmove = obj_A.GetComponent<PlayerMove4>();
                    pmove.onukiyuka = true;
                }
                //Debug.Log("はいた" + obj_B);
            }
            else if (typeAns == 2)//プレイヤーと敵衝突
            {
                if (change2D3D == 2)
                {
                    gameMakerCode.gameOver = true;
                    //敵に当たった
                    dead_enemy_moji.SetActive(true);
                    //
                    //Debug.Log("死因：敵");
                }
            }
            else if (typeAns == 3)//プレイヤーと家衝突
            {
                gameMakerCode.gameClear = true;
            }


        }
        ResetJustBlockRane();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    //衝突判定を取りたいオブジェを入れる
    private void SetPositionScale_object_A(GameObject allObject)
    {
        obj_A = allObject;

        allInfo_A = obj_A.GetComponent<ObjInfo>();
        obj_A_Position = allInfo_A.objPosition;
        obj_A_Scale = allInfo_A.objScale;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    //衝突判定を取りたいオブジェを入れる
    private void SetPositionScale_object_B(GameObject allObject)
    {
        obj_B = allObject;

        allInfo_B = obj_B.GetComponent<ObjInfo>();
        obj_B_Position = allInfo_B.objPosition;
        obj_B_Scale = allInfo_B.objScale;
    }
}
