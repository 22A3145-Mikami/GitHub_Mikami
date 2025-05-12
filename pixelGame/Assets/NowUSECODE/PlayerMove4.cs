using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove4 : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private ObjInfo objInfo;//プレイヤーのオブジェ情報

    public Vector3 move;//移動方向
    [SerializeField]
    private float speed;//移動速度
    //ジャンプについて
    public int canJump; // 0　ジャンプ可能　1　ジャンプ不可　上昇中　２　ジャンプ不可　下降中

    public float startJump; //最初のジャンプの力
    [SerializeField]
    private float startJump_zero; // ブロックがない時の落下用

    public float startJump_playerself; // プレイヤー自身でジャンプをしたとき用
    public float startJump_tatumaki;

    public float timeJump; // ジャンプの時間

    public float gravity; // 重力

    //モード切り替えのコードと変数
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;
    
    [SerializeField]
    private CubeCollision cubeCollision; // 当たり判定

    [SerializeField]
    private int groundBlock; // 今いる地面のブロックの番号をいれる

    /// CubeCollision用の変数　すり抜け防止用
    public float addJustBlockRange;//増幅用　justBlockRange
    public float keepMove_y; // ひとつ前のｙ座標
    public int keepCanJump; // ひとつ前のジャンプ状態

    public bool onukiyuka;//浮き床に乗っているか

    /// プレイヤーアニメ
    public PlayerANIM playerANIM;

    //ゲーム全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    //死因文字　落下
    private GameObject dead_abs;

    private void Awake()
    {
        //モード切り替えの取得
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //プレイヤー情報取得
        objInfo = GetComponent<ObjInfo>();

        //当たり判定の取得
        cubeCollision = GameObject.Find("JudgeCollision").GetComponent<CubeCollision>();

        startJump_zero = 0;

        //フレームレート設定
        Application.targetFrameRate = 60;

        //ゲーム全体の流れ取得
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
        
        //死因文字を入れる
        dead_abs = gameMakerCode.dead_abs_moji;

    }

    //１D２Dから３Dになった時にプレイヤーのｚ座標を変更する関数
    private void Position3D()
    {
        
        ObjInfo blockInfo; //ブロックの情報取得
        if (type_onPlayerBlock == 0)
        {
            blockInfo = cubeCollision.blocks[groundBlock].GetComponent<ObjInfo>();
        }
        else
        {
            blockInfo = cubeCollision.enemys[groundBlock].GetComponent<ObjInfo>();
        }
        //ｚ座標移動
        Vector3 nowPos = transform.position;
        if( !(nowPos.z - (objInfo.objScale.z / 2) < blockInfo.objPosition.z + (blockInfo.objScale.z / 2)
            && nowPos.z + (objInfo.objScale.z / 2) > blockInfo.objPosition.z - (blockInfo.objScale.z / 2) ))
        {
            nowPos.z = cubeCollision.blocks[groundBlock].transform.position.z;
        }
        
        transform.position = nowPos;
    }

    [SerializeField]
    int type_onPlayerBlock = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //一時中断していない　かつ　コースプレイ中
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            // モード切替
            if (change2D3D != change2D3DCode.change2D3D && change2D3DCode.change2D3D == 3) //2Dから３Dに変わった時
            {
                Position3D();
            }
            else if (change2D3D == 3 && change2D3DCode.change2D3D != 3)//３Dから１Ｄ２Ｄに変わった時
            {
                for (int i = 0; i < cubeCollision.blocks.Count; i++)
                {
                    cubeCollision.Death_Press(this.gameObject, cubeCollision.blocks[i]);//プレイヤーの手前奥にブロックがあれば死
                }
            }

            change2D3D = change2D3DCode.change2D3D;

            //浮き床に乗っていなければfalse
            onukiyuka = false;
            //移動方向のリセット
            move = Vector3.zero;

            //ADキー（左右）移動
            if (Input.GetKey(KeyCode.A))
            {
                Walk_AD(-speed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Walk_AD(speed);

            }

            //3D用　WSキー（手前奥）移動
            if (change2D3D == 3)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Walk_WS(speed);

                }
                if (Input.GetKey(KeyCode.S))
                {
                    Walk_WS(-speed);

                }
            }

            //xz当たり判定
            HitEnemyORBlock_xz(cubeCollision.blocks);
            
            // 1Dの時の敵との当たり判定　　ブロックと敵に挟まれたときに芋虫はプレイヤーを登る
            if(change2D3D == 1)
            {
                HitEnemyORBlock_xz(cubeCollision.enemys);

                // プレイヤーがブロックに埋まらないように
                //xz当たり判定
                objInfo.ResetHitWall();
                for (int i = 0; i < cubeCollision.blocks.Count; i++)
                {
                    cubeCollision.AtariHantei2D(this.gameObject, cubeCollision.blocks[i], 0);
                }

                if (player_position.x < 0 && objInfo.hidariWall)
                {
                    player_position.x = 0;

                    foreach(GameObject obj in pushPlayer_enemys)
                    {
                        if(!obj.GetComponent<EnemyMove3>().climb)
                        {
                            obj.GetComponent<EnemyMove3>().climb_player = true; //プレイヤーを登れるようにする
                        }
                        
                    }
                }

                if (player_position.x > 0 && objInfo.migiWall)
                {
                    player_position.x = 0;

                    foreach (GameObject obj in pushPlayer_enemys)
                    {
                        if (!obj.GetComponent<EnemyMove3>().climb)
                        {
                            obj.GetComponent<EnemyMove3>().climb_player = true;//プレイヤーを登れるようにする
                        }
                    }
                }

                // 芋虫に押されてる
                transform.position += player_position;
                player_position = Vector3.zero;
            }



            // 画像の変更
            if(objInfo.changePlayerPNG != 2)
            {
                if (move.x == 0 && move.z == 0)
                {
                    playerANIM.StopWalk();

                    objInfo.changePlayerPNG = 0;
                }
                else if (move.x != 0 || move.z != 0)
                {
                    playerANIM.Walk();
                    RotatePlayerOBJ();

                    objInfo.changePlayerPNG = 1;
                }
            }
            // 

            //spaceキーでジャンプ出来る
            Jump_Space();
            
            // 床ぬけするからここで書く
            transform.position += move;

            objInfo.ResetHitWall();
            // ｙ当たり判定　床がない時の落下用
            HitEnemyORBlock_y(cubeCollision.blocks);
            
            if(change2D3D == 1) //１Dモードでは芋虫もブロックになる
            {
                HitEnemyORBlock_y(cubeCollision.enemys);
            }
            
            if (canJump == 0) //ジャンプしてないときに
            {
                if (!objInfo.sitaWall) //足が地面についていなかったら
                {
                    //　落下コード
                    canJump = 2;
                    startJump = startJump_zero;
                    timeJump = 0;

                    //Debug.Log("すり抜け");
                    //break;
                }

                //保険
                if( objInfo.sitaWall && keepMove_y > transform.position.y)//fallPos > transform.position.y)
                {
                    //　落下コード
                    canJump = 2;
                    startJump = startJump_zero;
                    timeJump = 0;

                    objInfo.sitaWall = false;

                    //Debug.Log("すり抜け2");
                    //break;
                }
            }
            //////////////


            // y当たり判定　ジャンプからの着地とブロックが頭に当たった時
            HitEnemyORBlock_y_tyakuti_Atama(cubeCollision.blocks, 0);
            
            if (change2D3D == 1) //１Dモードでは芋虫もブロックになる
            {
                HitEnemyORBlock_y_tyakuti_Atama(cubeCollision.enemys, 1);

                //芋虫に乗ってる
                transform.position += player_position;
                player_position = Vector3.zero;
            }



            //落下
            if (canJump == 2 && transform.position.y <= -10)
            {
                //死因文字　落下
                dead_abs.SetActive(true);

                gameMakerCode.gameOver = true;
                //Debug.Log("rakka");
                //奈落落ち
                
                //
            }
            
            //Debug.Log("1周終わり");
        }
        
    }

    Vector3 player_position; //プレイヤーの座標
    void HitEnemyORBlock_y_tyakuti_Atama(List<GameObject> obj, int enemyORblock)
    {
        int co = 0; //プレイヤーが芋虫に乗ってるときに速さが重ね掛け出来ないように

        for (int i = 0; i < obj.Count; i++)
        {

            objInfo.ResetHitWall();

            // 3D用
            if (change2D3D == 3)
            {
                //このブロックに着地しているか　あたまにあたっているか
                cubeCollision.AtariHantei3D(this.gameObject, obj[i], 0);

                if (objInfo.sitaWall && move.y < 0) //着地しているか
                {
                    Jump_Tyakuti(i, enemyORblock);

                    type_onPlayerBlock = enemyORblock;
                }
                else if (objInfo.ueWall && move.y > 0)//あたまにあたっているか
                {
                    move.y = 0;

                    //Debug.Log("あたまあたった");

                    Jump_AtamaHit(i, enemyORblock);

                    //break;
                }

            }
            // 2D用
            else if (change2D3D != 3)
            {
                //このブロックに着地しているか　あたまにあたっているか
                cubeCollision.AtariHantei2D(this.gameObject, obj[i], 0);

                if ( (objInfo.sitaWall && move.y < 0 )) //着地しているか
                {
                    //playerをブロックにしているから
                    if(!obj[i].GetComponent<ObjInfo>().player)
                    {
                        Jump_Tyakuti(i, enemyORblock);
                    }

                    type_onPlayerBlock = enemyORblock;
                }
                else if (objInfo.ueWall && move.y > 0)//あたまにあたっているか
                {
                    move.y = 0;

                    //Debug.Log("あたまあたった");

                    //playerをブロックにしているから
                    if (!obj[i].GetComponent<ObjInfo>().player)
                    {
                        Jump_Tyakuti(i, enemyORblock);
                    }

                }
            }

            
            //敵　接触時のプレイヤー移動
            if (obj[i].GetComponent<ObjInfo>().enemy == true)
            {
                if (change2D3D == 1)
                {
                    Vector3 addMove_player = obj[i].GetComponent<EnemyMove3>().move;
                    if (objInfo.sitaWall == true)
                    {
                        player_position.x += addMove_player.x;
                        player_position.y += addMove_player.y;
                        co++;

                    }
                }
            }

            // 浮き床　接触時のプレイヤーいどう
            if (obj[i].GetComponent<ObjInfo>().ukiyuka)
            {
                UkiyukaMove ukimove = obj[i].GetComponent<UkiyukaMove>();
                if (objInfo.sitaWall)
                {
                    transform.position += ukimove.move * ukimove.speed;
                    if (canJump == 2)
                    {
                        canJump = 0;

                    }
                    if (timeJump == 0)
                    {
                        timeJump = 0;
                    }
                }
                
            }
            

            //地面となっているブロックの記憶
            if (objInfo.sitaWall)
            {
                groundBlock = i;
            }

        }
        //強制移動が重複しないように
        if(co != 0)
        {
            player_position.x /= co;
            if(player_position.y != 0)
            {
                player_position.y /= co;
            }
        }
        
    }

    //プレイヤーと敵・ブロックの当たり判定関数　y軸用
    void HitEnemyORBlock_y(List<GameObject> obj)
    {
        // ｙ当たり判定　床がない時の落下用 (床があるか検知する)
        for (int i = 0; i < obj.Count; i++)
        {
            if (change2D3D == 3)
            {
                cubeCollision.AtariHantei3D(this.gameObject, obj[i], 0);
            }
            else if (change2D3D != 3)
            {
                cubeCollision.AtariHantei2D(this.gameObject, obj[i], 0);

            }

        }
    }

    [SerializeField]
    List<GameObject> pushPlayer_enemys; //プレイヤーを押してる芋虫を探す
    //プレイヤーと敵・ブロックの当たり判定関数　xz軸用
    void HitEnemyORBlock_xz(List<GameObject> obj)
    {
        if(pushPlayer_enemys.Count >= 1)
        {
            pushPlayer_enemys.Clear(); //前フレームで残っているリストの削除
        }

        int co = 0;
        //xz当たり判定
        for (int i = 0; i < obj.Count; i++)
        {

            objInfo.ResetHitWall();

            if (change2D3D == 3)
            {
                cubeCollision.AtariHantei3D(this.gameObject, obj[i], 0);
                //プレイヤーが入力している方向にオブジェがあるとき
                if (objInfo.migiWall && move.x >= 0)
                {
                    move.x = 0;
                }
                if (objInfo.hidariWall && move.x <= 0)
                {
                    move.x = 0;
                }
                if (objInfo.okuWall && move.z >= 0)
                {
                    move.z = 0;
                }
                if (objInfo.temaeWall && move.z <= 0)
                {
                    move.z = 0;
                }

            }
            else if (change2D3D != 3)
            {
                float keep_justBlockRange = cubeCollision.justBlockRange;
                cubeCollision.justBlockRange *= 2; 
                cubeCollision.AtariHantei2D(this.gameObject, obj[i], 0);
                cubeCollision.justBlockRange = keep_justBlockRange;
                //プレイヤーが入力している方向にオブジェがあるとき
                if (objInfo.migiWall && move.x >= 0)
                {
                    move.x = 0;
                }
                if (objInfo.hidariWall && move.x <= 0)
                {
                    move.x = 0;
                }
                //敵との衝突
                if(obj[i].GetComponent<ObjInfo>().enemy == true)
                {
                    if (change2D3D == 1)
                    {
                        EnemyMove3 eMove = obj[i].GetComponent<EnemyMove3>();
                        Vector3 addMove_player = eMove.move;//敵の移動方向
                        if (objInfo.migiWall)
                        {
                            if(move.x > 0) //芋虫に突進してるとき
                            {
                                player_position.x += addMove_player.x;
                                move.x = 0;

                                pushPlayer_enemys.Add(obj[i]);
                            }
                            else if(move.x == 0 && addMove_player.x < 0)
                            {
                                player_position.x += addMove_player.x;

                                pushPlayer_enemys.Add(obj[i]);
                            }

                            co++;
                        }
                        if (objInfo.hidariWall)
                        {
                            if (move.x < 0)//芋虫に突進してるとき
                            {
                                player_position.x += addMove_player.x;
                                move.x = 0;

                                pushPlayer_enemys.Add(obj[i]);
                            }
                            else if(move.x == 0 && addMove_player.x > 0)
                            {
                                player_position.x += addMove_player.x;

                                pushPlayer_enemys.Add(obj[i]);
                            }

                            co++;
                        }
                    }
                }
            }
        }

        if(co != 0)
        {
            player_position.x /= co;
        }
    }

    //主人公が進行方向を向くように
    private void RotatePlayerOBJ()
    {
        //Dキー
        if(move.x > 0)
        {
            if(move.z > 0)//Wキー
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 135, 0);
            }
            else if(move.z < 0)//Sキー
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 225, 0);
            }
            else
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }
        //Aキー
        else if(move.x < 0)
        {
            if (move.z > 0)//Wキー
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 45, 0);
            }
            else if (move.z < 0)//Sキー
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 315, 0);
            }
            else
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        //前後（手前奥）
        else if(move.x == 0)
        {
            if (move.z > 0)//Wキー
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else if (move.z < 0)//Sキー
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 270, 0);
            }
        }
    }

    //移動関数
    private void Walk_AD(float houkou)
    {

        move.x = houkou;
    }
    private void Walk_WS(float houkou)
    {
        move.z = houkou;
    }
    //着地コード
    private void Jump_Tyakuti(int blocks_Number, int enemyORblock)   //ジャンプの着地時の地面のすり抜けを防止するため
    {

        // ジャンプの遷移
        canJump = 0;
        timeJump = 0;
        ObjInfo obj_B_Info;
        if (enemyORblock == 0) // ブロックの時
        {
            obj_B_Info = cubeCollision.blocks[blocks_Number].GetComponent<ObjInfo>();
        }
        else // 敵の時
        {
            obj_B_Info = cubeCollision.enemys[blocks_Number].GetComponent<ObjInfo>();
        }
        //地面になったオブジェB上に移動する
        Vector3 obj_B_Position = obj_B_Info.objPosition;
        Vector3 obj_B_Scale = obj_B_Info.objScale;
        Vector3 nowPos = transform.position;
        nowPos.y = obj_B_Position.y + (obj_B_Scale.y / 2) + (objInfo.objScale.y / 2);
        transform.position = nowPos;

        //Debug.Log("着地");

        playerANIM.StopJump();
        objInfo.changePlayerPNG = 0;
    }
    private void Jump_AtamaHit(int blocks_Number, int enemyORblock)
    {
        //ジャンプの遷移
        canJump = 2;
        startJump = startJump_zero;
        timeJump = 0;

        ObjInfo obj_B_Info;
        if (enemyORblock == 0) // ブロックの時
        {
            obj_B_Info = cubeCollision.blocks[blocks_Number].GetComponent<ObjInfo>();
        }
        else // 敵の時
        {
            obj_B_Info = cubeCollision.enemys[blocks_Number].GetComponent<ObjInfo>();
        }
        //頭が当たったオブジェの下に移動する
        Vector3 obj_B_Position = obj_B_Info.objPosition;
        Vector3 obj_B_Scale = obj_B_Info.objScale;
        Vector3 nowPos = transform.position;
        nowPos.y = obj_B_Position.y - (obj_B_Scale.y / 2) - (objInfo.objScale.y / 2);
        transform.position = nowPos;
    }
    //ジャンプ関数
    private void Jump_Space()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // ジャンプしたとき
            if (canJump == 0 || (onukiyuka && (canJump == 2 || canJump == 1)))
            {
                //ジャンプ開始
                timeJump = 0;

                startJump = startJump_playerself;
                //鉛直投げ上げ
                move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;

                timeJump = timeJump + Time.deltaTime;

                canJump = 1;

                playerANIM.Jump();

                objInfo.changePlayerPNG = 2;
            }
        }
        //ジャンプ中（上昇）
        else if(canJump == 1)
        {
            //鉛直投げ上げ
            move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;

            timeJump = timeJump + Time.deltaTime;

            // もし　次のフレームのｙ座標　が　今のｙ座標　より低くなったら
            if(objInfo.objPosition.y > objInfo.objPosition.y + move.y)
            {
                canJump = 2;
            }
        }
        //ジャンプ中（下降）
        else if(canJump == 2)
        {
            //鉛直投げ上げ
            move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;

            timeJump = timeJump + Time.deltaTime;
        }

    }

}
