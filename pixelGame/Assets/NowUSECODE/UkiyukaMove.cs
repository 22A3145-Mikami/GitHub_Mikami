using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UkiyukaMove : MonoBehaviour
{
    //動きの終始ポイント１
    [SerializeField]
    private Vector3 position1;
    //動きの終始ポイント２
    [SerializeField]
    private Vector3 position2;
    private bool from1to2; // 終始ポイント１から２にいく事をtrue
    private bool maefrom1to2; //移動方向が変わった瞬間
    public Vector3 move; //その時の移動方向
    private Vector3 move_vec; //移動していないときにmoveを０にするため

    public float speed = 0.5f; //移動の速さ
    private float range; // 1から２までの大きさR

    public bool onPlayer; // プレイヤーが乗ったか
    private float onPlayerTime; //プレイヤーが乗ってから動くまでの経過時間
    private float moveStartTime; // プレイヤーが乗ってから動き始めるまでの時間

    [SerializeField]
    private bool moveStart; //移動の開始

    [SerializeField]
    private bool move_keep; //ずっと動く

    //モード切り替えのコードと変数
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;

    //プレイヤーオブジェ
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private CubeCollision cubeCollision; // 当たり判定

    //ゲーム全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    private void Awake()
    {
        //モード切り替えの取得
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        
        //当たり判定の取得
        cubeCollision = GameObject.Find("JudgeCollision").GetComponent<CubeCollision>();

        
        //試し書き
        //position1 = new Vector3(0, 0, 0);
        //position2 = new Vector3(10, 5, 10);
        //transform.position = position1;
        //position1 = transform.position;
        /*
        move = position2 - position1;
        range = Seikika(move);
        move = move / range;

        Debug.Log(move);
        */

        //２秒足ったら動く
        moveStartTime = 2f;

        //初期設定
        from1to2 = true;
        maefrom1to2 = true;
        
        //フレームレート設定
        Application.targetFrameRate = 60;

        //ゲーム全体の流れ取得
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
    }

    //StageSetコードで使う用関数
    public void SetPosition(Vector3 position_1, Vector3 position_2)
    {
        //終始ポイントの設定
        SetPosition1(position_1);
        SetPosition2(position_2);

        //移動方向の計算
        move = position2 - position1;
        //大きさを正規化する
        range = Seikika(move);
        move = move / range;
        
        move_vec = move; //moveの保存

        //Debug.Log(move);
    }
    public void SetPosition1(Vector3 pos)
    {
        position1 = pos;
    }
    public void SetPosition2(Vector3 pos)
    {
        position2 = pos;
    }

    // 動き続けるか設定
    public void Move_Keeper(bool select)
    {
        move_keep = select;
    }
    //速さの設定
    public void SetSpeed(float accelerator)
    {
        speed = accelerator;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //一時中断していない　かつ　コースプレイ中
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            //プレイヤー取得
            player = GameObject.FindGameObjectWithTag("Player");

            ///プレイヤーが乗っているか判定
            onPlayer = false;
            if (change2D3D == 3)
            {
                cubeCollision.AtariHantei3D(player, this.gameObject, 1);
            }
            else if (change2D3D != 3)
            {
                cubeCollision.AtariHantei2D(player, this.gameObject, 1);
            }
            ///
            
            //プレイヤーが乗っている時間が２秒足ったら動かす
            if (onPlayer)
            {
                onPlayerTime += Time.deltaTime;

                if (onPlayerTime >= moveStartTime)
                {
                    moveStart = true;
                }
            }//動く前に途中で離れたらリセット
            else
            {
                onPlayerTime = 0;
            }

            //移動開始するか動き続けるか判定がtrueだったら動かす
            if (moveStart || move_keep)
            {
                move = move_vec;
                //移動開始
                MoveStart();
                //Debug.Log("ugo");
            }//動き続けるがfalseのときで乗ってなかったら動かさない
            else if (!moveStart)
            {
                move = Vector3.zero;
            }
        }
        
    }

    private void MoveStart()
    {
        if(from1to2)
        {
            //動きの反転
            if(from1to2 != maefrom1to2)
            {
                speed *= -1;
                maefrom1to2 = from1to2;
            }
            //移動
            transform.position += move * speed;

            //移動距離が終始ポイント間の大きさより大きくなったら実行する
            Vector3 judge = transform.position - position1;
            if(Seikika(judge) > range)
            {
                transform.position = position2;

                onPlayerTime = 0;
                moveStart = false;
                from1to2 = false;
            }
        }
        else if(!from1to2) // 2から１に戻る　　　　上コードと同じやり方
        {
            if(from1to2 != maefrom1to2)
            {
                speed *= -1;
                maefrom1to2 = from1to2;
            }

            transform.position += move * speed;

            Vector3 judge = transform.position - position2;
            if (Seikika(judge) > range)
            {
                transform.position = position1;

                onPlayerTime = 0;
                moveStart = false;
                from1to2 = true;
            }
        }
        
    }

    //正規化処理
    private float Seikika(Vector3 kyori)
    {
        for (int i = 0; i < 3; i++)
        {
            kyori[i] = kyori[i] * kyori[i];
        }
        float ans = Mathf.Sqrt(kyori.x + kyori.y + kyori.z);
        return ans;
    }
}
