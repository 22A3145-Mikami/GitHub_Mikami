using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInfo : MonoBehaviour
{
    //[SerializeField]
    //コードがついてるオブジェの　座標　回転　スケール
    public GameObject obj;
    public Vector3 objPosition; 
    public Vector3 objRotate;
    public Vector3 objScale;

    //モード切り替えのコードと変数
    [SerializeField]
    private GameObject eventChange2D3D;
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;

    public GameObject obj3D; //３Dモード用オブジェ

    private int count2D; // 立ち絵の数
    private int cooltimeChangePicture; // 立ち絵遷移のクールタイム
    [SerializeField]
    private int changeTime; //立ち絵の遷移の間隔
    private int nowPicture; //　立ち絵の遷移
    public List<GameObject> obj2D;//２Dモード用オブジェ
    public List<GameObject> obj2D_Anotherplayer; // 立ち絵の遷移プレイヤーの別方向の

    public GameObject obj1D; // 1Dモードのオブジェ

    public bool player; //このオブジェクトがplayerであるか
    public bool block; //このオブジェクトがblockであるか
    public bool stage; //このオブジェクトがstageであるか　（ジャンプ着地時にテレポしないように）
    public bool enemy;  //このオブジェクトがenemyであるか
    public bool toreruyuka; //　このブロックが通れる床であるか
    public bool ukiyuka;  //このブロックが浮き床であるか    

    public bool hidariWall; //プレイヤーのお腹　（敵）
    public bool migiWall; //プレイヤーの背中　（敵）
    public bool okuWall; //プレイヤーの奥　（敵）
    public bool temaeWall; //プレイヤーの手前　（敵）
    public bool ueWall; //プレイヤーの頭　（敵）
    public bool sitaWall; //プレイヤーの足　（敵）

    public bool tatumakiHit; // これがたつまきにあたったか

    //ゲーム全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    private void Awake()
    {
        //フレームレート設定
        Application.targetFrameRate = 60;

        //初期設定
        count2D = obj2D.Count;
        cooltimeChangePicture = changeTime;
        nowPicture = 0;

        //モード切り替えの取得
        eventChange2D3D = GameObject.Find("EventChange2D3D");
        change2D3DCode = eventChange2D3D.GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //全モード非表示
        obj3D.SetActive(false);
        obj1D.SetActive(false);
        for(int i = 0; i < count2D; i++)
        {
            obj2D[i].SetActive(false);
            if(player)
            {
                obj2D_Anotherplayer[i].SetActive(false);
            }
        }

        //ゲーム全体の流れ取得
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //一時中断していない　かつ　コースプレイ中
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            //現座標の設定
            objPosition = transform.position;
            
            //モードの切り替えとオブジェの表示変更
            change2D3D = change2D3DCode.change2D3D;
            if (change2D3D == 3)
            {
                //3D以外を非表示
                obj1D.SetActive(false);
                for (int i = 0; i < count2D; i++)
                {
                    obj2D[i].SetActive(false);
                    if (player)
                    {
                        obj2D_Anotherplayer[i].SetActive(false);
                    }
                }
                obj3D.SetActive(true);

            }
            else if (change2D3D == 2)
            {
                //2D以外を非表示
                obj1D.SetActive(false);
                obj3D.SetActive(false);

                ChangePicture();

            }
            else if(change2D3D == 1)
            {
                //1D以外を非表示
                for (int i = 0; i < count2D; i++)
                {
                    obj2D[i].SetActive(false);
                    if (player)
                    {
                        obj2D_Anotherplayer[i].SetActive(false);
                    }
                }
                obj3D.SetActive(false);
                obj1D.SetActive(true);
            }
        }

    }

    public int changePlayerPNG;  //　1 歩いている　 0　止まっている   2 ジャンプしてる
    private Vector3 keepPlayer_move; //移動方向の取得用変数
    //２Dモードの表示オブジェの切り替え
    public void ChangePicture()
    {
        //プレイヤー用
        if(player)
        {
            //プレイヤー移動コードの取得
            PlayerMove4 playerMove = GetComponent<PlayerMove4>();
            //歩いているとき
            if (changePlayerPNG == 1)
            {
                //クールタイムが同じ値になったら
                if (cooltimeChangePicture == changeTime)
                {
                    //立ち絵を次のものに切り替え
                    nowPicture++;

                    obj2D[nowPicture - 1].SetActive(false);
                    obj2D_Anotherplayer[nowPicture - 1].SetActive(false);

                    if (nowPicture == count2D)
                    {
                        nowPicture = 0;
                    }
                    //切り替え時間をリセット
                    cooltimeChangePicture = 0;
                    
                    //右方向に移動しているとき
                    if (playerMove.move.x > 0)
                    {
                        obj2D[nowPicture].SetActive(true);
                        //今の移動方向を記憶
                        keepPlayer_move = playerMove.move;
                    }
                    //左方向に移動しているとき
                    else if (playerMove.move.x < 0)
                    {
                        obj2D_Anotherplayer[nowPicture].SetActive(true);
                        //今の移動方向を記憶
                        keepPlayer_move = playerMove.move;
                    }

                }
                cooltimeChangePicture++;

                //　クールタイム中に左右反転したとき用
                if(playerMove.move.x > 0 && keepPlayer_move.x < 0)
                {
                    obj2D_Anotherplayer[nowPicture].SetActive(false);

                    obj2D[nowPicture].SetActive(true);
                    //今の移動方向を記憶
                    keepPlayer_move = playerMove.move;
                }
                else if (playerMove.move.x < 0 && keepPlayer_move.x > 0)
                {
                    obj2D[nowPicture].SetActive(false);

                    obj2D_Anotherplayer[nowPicture].SetActive(true);
                    //今の移動方向を記憶
                    keepPlayer_move = playerMove.move;
                }


            }
            //ジャンプしているとき
            else if(changePlayerPNG == 2)
            {
                obj2D[nowPicture].SetActive(false);
                obj2D_Anotherplayer[nowPicture].SetActive(false);
                
                //ジャンプ中は立ち絵を固定
                nowPicture = 1;

                //切り替え時間をリセット
                cooltimeChangePicture = 0;
                //右移動　または　移動してない　かつ　右移動
                if (playerMove.move.x > 0 || (playerMove.move.x == 0 && keepPlayer_move.x > 0))
                {
                    obj2D[nowPicture].SetActive(true);
           
                }
                //左移動　または　移動してない　かつ　左移動
                else if (playerMove.move.x < 0 || (playerMove.move.x == 0 && keepPlayer_move.x < 0))
                {
                    obj2D_Anotherplayer[nowPicture].SetActive(true);

                }
                //左右移動してるときに移動を保存
                if (playerMove.move.x != 0)
                {
                    keepPlayer_move = playerMove.move;
                }
            }
            //止まっているとき
            else if (changePlayerPNG == 0)
            {
                obj2D[nowPicture].SetActive(false);
                obj2D_Anotherplayer[nowPicture].SetActive(false);
                //停止中は立ち絵を固定
                nowPicture = 0;
                // 切り替え時間をリセット
                cooltimeChangePicture = 0;

                //保存した移動が右移動
                if (keepPlayer_move.x > 0)
                {
                    obj2D[nowPicture].SetActive(true);
                }
                //保存した移動が左移動
                else if (keepPlayer_move.x < 0)
                {
                    obj2D_Anotherplayer[nowPicture].SetActive(true);
                }
                //移動中と保存した移動が停止だったら
                else if(keepPlayer_move.x == 0 && playerMove.move.x == 0)
                {
                    obj2D_Anotherplayer[nowPicture].SetActive(false);
                    obj2D[nowPicture].SetActive(true);
                }
            }
        }
        //プレイヤー以外
        else if (!player)
        {
            //クールタイムが同じ値になったら
            if (cooltimeChangePicture == changeTime)
            {
                nowPicture++;

                obj2D[nowPicture - 1].SetActive(false);

                if (nowPicture == count2D)
                {
                    nowPicture = 0;
                }
                //切り替え時間をリセット
                cooltimeChangePicture = 0;


            }
            cooltimeChangePicture++;

            obj2D[nowPicture].SetActive(true);
        }
        
        
    }

    //衝突判定を消す
    public void ResetHitWall()
    {
        hidariWall= false;
        migiWall= false;
        okuWall= false;
        temaeWall= false;
        ueWall= false;
        sitaWall= false;

        tatumakiHit = false;

    }
}
