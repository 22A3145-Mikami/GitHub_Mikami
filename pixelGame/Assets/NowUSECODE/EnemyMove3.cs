using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove3 : MonoBehaviour
{
    //[SerializeField]
    //private GameObject player;
    [SerializeField]
    private ObjInfo objInfo; //敵オブジェ情報

    public Vector3 move; //移動方向
    [SerializeField]
    private float speed; //移動速度
    //ジャンプについて
    public int canJump; // 0　ジャンプ可能　1　ジャンプ不可　上昇中　２　ジャンプ不可　下降中
    //[SerializeField]
    public float startJump; //最初のジャンプの力
    [SerializeField]
    private float startJump_zero; // ブロックがない時の落下用
    public float startJump_tatumaki;
    //[SerializeField]
    //private float startJump_playerself; // プレイヤー自身でジャンプをしたとき用
    //[SerializeField]
    public float timeJump; // ジャンプの時間
    //[SerializeField]
    public float gravity; // 重力
    [SerializeField]
    private Vector3 jumpMaePosition; //ジャンプ前の地面にいたときの座標

    //モード切り替えのコードと変数
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;

    //ブロックを登る
    public bool climb;
    //1フレーム前に登っていたか
    private bool maeClimb;
    //登るブロックの情報
    private int climbWall;

    //public bool pushPlayer;
    //public bool climbPlayer;

    [SerializeField]
    private CubeCollision cubeCollision; // 当たり判定

    [SerializeField]
    private int groundBlock; // 今いる地面のブロックの番号をいれる

    /// CubeCollision用の変数　すり抜け防止用
    public float addJustBlockRange;//増幅用　justBlockRange
    public float keepMove_y; // ひとつ前のｙ座標
    public int keepCanJump; // ひとつ前のジャンプ状態

    //ゲーム全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    //プレイヤーを登ることできるか
    public bool climb_player;

    private void Awake()
    {
        //モード切り替えの取得
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //オブジェ情報取得
        objInfo = GetComponent<ObjInfo>();

        //当たり判定の取得
        cubeCollision = GameObject.Find("JudgeCollision").GetComponent<CubeCollision>();

        //ジャンプ値０の保存用
        startJump_zero = 0;

        //フレームレート設定
        Application.targetFrameRate = 60;

        //ゲーム全体の流れ取得
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();

        //プレイヤーを登れるか
        climb_player = false;
    }

    //３D変更時のZ座標移動
    private void Position3D()
    {
        //地面ブロックの情報取得
        ObjInfo blockInfo = cubeCollision.blocks[groundBlock].GetComponent<ObjInfo>();
        Vector3 nowPos = transform.position;
        if (!(nowPos.z - (objInfo.objScale.z / 2) < blockInfo.objPosition.z + (blockInfo.objScale.z / 2)
            && nowPos.z + (objInfo.objScale.z / 2) > blockInfo.objPosition.z - (blockInfo.objScale.z / 2)))
        {
            nowPos.z = cubeCollision.blocks[groundBlock].transform.position.z;
        }
        //　オブジェの移動
        transform.position = nowPos;
    }

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
            else if (change2D3D == 3 && change2D3DCode.change2D3D != 3) //２D１Dになったときの圧死用
            {
                for (int i = 0; i < cubeCollision.blocks.Count; i++)
                {
                    //圧死コード
                    cubeCollision.Death_Press(this.gameObject, cubeCollision.blocks[i]);
                }
            }

            change2D3D = change2D3DCode.change2D3D;

            //移動方向のリセット
            move = Vector3.zero;

            //左移動
            Walk_AD(-speed);


            //xz当たり判定
            for (int i = 0; i < cubeCollision.blocks.Count; i++)
            {
                //衝突判定を外す
                objInfo.ResetHitWall();
                //3Dモード用
                if (change2D3D == 3)
                {
                    //オブジェとブロックでの当たり判定
                    cubeCollision.AtariHantei3D(this.gameObject, cubeCollision.blocks[i], 0);

                    //右に当たったら移動をなくす
                    if (objInfo.migiWall && move.x >= 0)
                    {
                        move.x = 0;
                    }
                    //左に当たったら移動をなくす　ブロックを登る
                    if (objInfo.hidariWall && move.x <= 0)
                    {

                        //プレイヤーがブロックでプレイヤーに登れるとき
                        if (cubeCollision.blocks[i].GetComponent<ObjInfo>().player
                            && climb_player)
                        {
                            move.x = 0;
                            //登る壁オブジェの取得
                            climbWall = i;
                        }
                        //プレイヤーでないオブジェのとき
                        else if (cubeCollision.blocks[i] != cubeCollision.player)
                        {
                            move.x = 0;
                            //登る壁オブジェの取得
                            climbWall = i;
                        }
                    }
                    //奥に当たったら移動をなくす
                    if (objInfo.okuWall && move.z >= 0)
                    {
                        move.z = 0;
                    }
                    //手前に当たったら移動をなくす
                    if (objInfo.temaeWall && move.z <= 0)
                    {
                        move.z = 0;
                    }
                }
                //3Dモード以外
                else if (change2D3D != 3)
                {
                    //ブロックとの当たり判定
                    cubeCollision.AtariHantei2D(this.gameObject, cubeCollision.blocks[i], 0);
                    //右に当たったら移動をなくす
                    if (objInfo.migiWall && move.x >= 0)
                    {
                        move.x = 0;
                    }
                    //左に当たったら移動をなくす　ブロックを登る
                    if (objInfo.hidariWall && move.x <= 0)
                    {
                        //プレイヤーがブロックでプレイヤーに登れるとき
                        if (cubeCollision.blocks[i].GetComponent<ObjInfo>().player
                            && climb_player)
                        {
                            move.x = 0;
                            //登る壁オブジェの取得
                            climbWall = i;
                        }
                        //プレイヤーでないオブジェのとき
                        else if (cubeCollision.blocks[i] != cubeCollision.player)
                        {
                            move.x = 0;
                            //登る壁オブジェの取得
                            climbWall = i;
                        }

                    }

                }
            }

            

            // 壁のぼり
            if (move.x == 0)
            {
                //壁オブジェの情報を取得
                ObjInfo blockInfo = cubeCollision.blocks[climbWall].GetComponent<ObjInfo>();
                //前フレームと行動が変わるとき
                if (climb != maeClimb)
                {
                    //大きさの変更
                    Vector3 changeS = objInfo.objScale;
                    objInfo.objScale.x = objInfo.objScale.y;
                    objInfo.objScale.y = changeS.x;
                    // objの向き変更
                    transform.localEulerAngles = new Vector3(0, 0, -90);
                    Vector3 nowPos = transform.position;
                    nowPos.x = blockInfo.objPosition.x + (blockInfo.objScale.x / 2) + (objInfo.objScale.x / 2);

                    transform.position = nowPos;
                }

                Walk_Y(speed);
                maeClimb = climb;
                climb = true;
                timeJump = 0;
                canJump = 0;

                
            }
            else
            {
                //前フレームと行動が変わるとき
                if (climb != maeClimb)
                {
                    //大きさの変更
                    Vector3 changeS = objInfo.objScale;
                    objInfo.objScale.x = objInfo.objScale.y;
                    objInfo.objScale.y = changeS.x;
                    // objの向き変更
                    transform.localEulerAngles = new Vector3(0, 0, 0);

                    ObjInfo blockInfo = cubeCollision.blocks[climbWall].GetComponent<ObjInfo>();
                    Vector3 nowPos = transform.position;
                    nowPos.x = blockInfo.objPosition.x + (blockInfo.objScale.x / 2) + (objInfo.objScale.x / 2) - 0.01f;

                    transform.position = nowPos;

                    climb_player = false;
                }
                maeClimb = climb;
                climb = false;
            }
            ////

            Jump();
            //移動
            transform.position += move;
            

            //上ってるときはしない　床があるか
            if (!climb)
            {
                objInfo.ResetHitWall();
                // ｙ当たり判定　床がない時の落下用
                for (int i = 0; i < cubeCollision.blocks.Count; i++)
                {
                    if (change2D3D == 3)
                    {
                        cubeCollision.AtariHantei3D(this.gameObject, cubeCollision.blocks[i], 0);
                    }
                    else if (change2D3D != 3)
                    {
                        cubeCollision.AtariHantei2D(this.gameObject, cubeCollision.blocks[i], 0);
                    }

                }
                //床に足がついてる判定で実際にはついていないとき
                if (canJump == 0)
                {
                    //地面に足がついていないとき
                    if (!objInfo.sitaWall)
                    {
                        //　落下コード
                        canJump = 2;
                        startJump = startJump_zero;
                        timeJump = 0;

                    }
                }
                //////////////
            }


            // y当たり判定　ジャンプからの着地とブロックが頭に当たった時
            for (int i = 0; i < cubeCollision.blocks.Count; i++)
            {

                objInfo.ResetHitWall();

                // 3D用
                if (change2D3D == 3)
                {
                    //ブロックとの当たり判定
                    cubeCollision.AtariHantei3D(this.gameObject, cubeCollision.blocks[i], 0);
                    //地面に足がついてる　かつ　移動方向が下
                    if (objInfo.sitaWall && move.y < 0)
                    {
                        //着地コード
                        Jump_Tyakuti(i);

                    }
                    //頭がついてる　かつ　移動方向が上
                    else if (objInfo.ueWall && move.y > 0)
                    {
                        move.y = 0;
                        //頭当たった
                        Jump_AtamaHit(i);

                    }

                }
                // 2D用
                else if (change2D3D != 3)
                {
                    //ブロックとの当たり判定
                    cubeCollision.AtariHantei2D(this.gameObject, cubeCollision.blocks[i], 0);
                    //地面に足がついてる　かつ　移動方向が下
                    if (objInfo.sitaWall && move.y < 0)
                    {
                        //着地コード
                        Jump_Tyakuti(i);

                    }
                    //頭がついてる　かつ　移動方向が上
                    else if (objInfo.ueWall && move.y > 0)
                    {
                        move.y = 0;
                        //頭当たった
                        Jump_AtamaHit(i);

                    }


                }
                //足のついている地面を記憶
                if (objInfo.sitaWall)
                {
                    groundBlock = i;
                }

            }

            //奈落に落ちたとき敵を消す
            if (transform.position.y <= -10)
            {
                Destroy(this.gameObject);
            }

        }

    }

    //左右移動
    private void Walk_AD(float houkou)
    {

        move.x = houkou;
    }
    //上下移動
    private void Walk_Y(float houkou)
    {
        move.y = houkou;
    }
    //着地コード
    private void Jump_Tyakuti(int blocks_Number)   //ジャンプの着地時の地面のすり抜けを防止するため
    {

        // ジャンプの遷移
        canJump = 0;
        timeJump = 0;

        //ブロック情報の取得 地面に着地
        ObjInfo obj_B_Info = cubeCollision.blocks[blocks_Number].GetComponent<ObjInfo>();
        Vector3 obj_B_Position = obj_B_Info.objPosition;
        Vector3 obj_B_Scale = obj_B_Info.objScale;
        Vector3 nowPos = transform.position;
        nowPos.y = obj_B_Position.y + (obj_B_Scale.y / 2) + (objInfo.objScale.y / 2);
        transform.position = nowPos;

    }
    //頭が当たった
    private void Jump_AtamaHit(int blocks_Number)
    {
        // ジャンプの遷移　ジャンプ値のリセット
        canJump = 2;
        startJump = startJump_zero;
        timeJump = 0;

        //ブロック情報の取得 オブジェクトの移動
        ObjInfo obj_B_Info = cubeCollision.blocks[blocks_Number].GetComponent<ObjInfo>();
        Vector3 obj_B_Position = obj_B_Info.objPosition;
        Vector3 obj_B_Scale = obj_B_Info.objScale;
        Vector3 nowPos = transform.position;
        nowPos.y = obj_B_Position.y - (obj_B_Scale.y / 2) - (objInfo.objScale.y / 2);
        transform.position = nowPos;
    }

    private void Jump()
    {
        //上に移動中
        if (canJump == 1)
        {
            //鉛直投げ上げ
            move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;
            //時間の加算
            timeJump = timeJump + Time.deltaTime;

            // もし　次のフレームのｙ座標　が　今のｙ座標　より低くなったら
            if (objInfo.objPosition.y > objInfo.objPosition.y + move.y)
            {
                canJump = 2;
            }
        }
        //ジャンプ中（下降）
        else if (canJump == 2)
        {
            //自由落下
            move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;
            //時間の加算
            timeJump = timeJump + Time.deltaTime;
        }

    }
    //速度の設定
    public void SetSpeed(float change_speed)
    {
        speed = change_speed;
    }
}
