using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tatumaki : MonoBehaviour
{
    //プレイヤーオブジェ
    [SerializeField]
    private GameObject player;

    //当たり判定コード
    private CubeCollision cubeCollision;

    //private GameObject[] enemys;

    //モード切り替えのコードと変数
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;

    //プレイヤー敵が跳ぶためのちから
    [SerializeField]
    private float startJump_tatumaki;

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

        //フレームレート設定
        Application.targetFrameRate = 60;
        
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
            //プレイヤー取得
            player = GameObject.FindGameObjectWithTag("Player");

            //３Dモードのとき
            if (change2D3D == 3)
            {
                //// プレイヤーについて
                ///当たり判定
                cubeCollision.AtariHantei_tatumaki(player, this.gameObject);
                ///プレイヤーのオブジェ情報取得して当たっていたら力を加える
                ObjInfo objInfo = player.GetComponent<ObjInfo>();
                if (objInfo.tatumakiHit)
                {
                    TatumakiJump_player();
                }
                else if (!objInfo.tatumakiHit)
                {
                    ResetStartJump_tatumaki_player();
                }

                //// 敵について
                ///プレイヤーと同じ
                for (int i = 0; i < cubeCollision.enemys.Count; i++)
                {
                    cubeCollision.AtariHantei_tatumaki(cubeCollision.enemys[i], this.gameObject);

                    objInfo = cubeCollision.enemys[i].GetComponent<ObjInfo>();
                    if (objInfo.tatumakiHit)
                    {
                        TatumakiJump_enemy(i);
                    }
                    else if (!objInfo.tatumakiHit)
                    {
                        ResetStartJump_tatumaki_enemy(i);
                    }
                }
            }//３Dモード以外　３Dモードと同様
            else if (change2D3D != 3)
            {
                //// プレイヤーについて
                cubeCollision.AtariHantei_tatumaki(player, this.gameObject);

                ObjInfo objInfo = player.GetComponent<ObjInfo>();
                if (objInfo.tatumakiHit)
                {
                    TatumakiJump_player();
                }
                else if (!objInfo.tatumakiHit)
                {
                    ResetStartJump_tatumaki_player();
                }

                //// 敵について
                for (int i = 0; i < cubeCollision.enemys.Count; i++)
                {

                    if(cubeCollision.enemys[i] != null)
                    {
                        cubeCollision.AtariHantei_tatumaki(cubeCollision.enemys[i], this.gameObject);

                        objInfo = cubeCollision.enemys[i].GetComponent<ObjInfo>();
                        if (objInfo.tatumakiHit)
                        {
                            TatumakiJump_enemy(i);
                        }
                        else if (!objInfo.tatumakiHit)
                        {
                            ResetStartJump_tatumaki_enemy(i);
                        }
                    }
                    
                }
            }
        }
        
    }
    [SerializeField]
    private float addJump;// 2次関数で力量を増加
    public void SetAddJump(float add)
    {
        addJump = add;
    }
    //プレイヤー用
    public void TatumakiJump_player()
    {
        //プレイヤー移動コードの取得
        PlayerMove4 playerMove = player.GetComponent<PlayerMove4>();
        //プレイヤーのジャンプ経過時間をリセット
        playerMove.timeJump = 0;
        //プレイヤージャンプ値の変更
        playerMove.startJump = playerMove.startJump_tatumaki;
        //値が３を超えるまで加算
        if(playerMove.startJump_tatumaki <= 3)
        {
            playerMove.startJump_tatumaki += addJump;
        }

        ///自由落下の公式
        Vector3 move = Vector3.zero;
        move.y = playerMove.startJump * playerMove.timeJump - 0.5f * playerMove.gravity * playerMove.timeJump * playerMove.timeJump;
        
        //プレイヤーのy軸の移動
        player.transform.position += move;

        //経過時間
        playerMove.timeJump = playerMove.timeJump + Time.deltaTime;

        //上方向に移動中に設定
        playerMove.canJump = 1;

        //アニメーション
        playerMove.playerANIM.Jump();

    }
    //プレイヤー用
    private void ResetStartJump_tatumaki_player()
    {
        //プレイヤー移動コードの取得
        PlayerMove4 playerMove = player.GetComponent<PlayerMove4>();
        
        //竜巻用ジャンプ値のリセット
        playerMove.startJump_tatumaki = 1;
    }

    //敵用　プレイヤーと同じ
    public void TatumakiJump_enemy(int enemy_Number)
    {
        EnemyMove3 enemyMove = cubeCollision.enemys[enemy_Number].GetComponent<EnemyMove3>();

        enemyMove.timeJump = 0;

        enemyMove.startJump = enemyMove.startJump_tatumaki;

        if (enemyMove.startJump_tatumaki <= 3)
        {
            enemyMove.startJump_tatumaki += addJump;
        }

        Vector3 move = Vector3.zero;
        move.y = enemyMove.startJump * enemyMove.timeJump - 0.5f * enemyMove.gravity * enemyMove.timeJump * enemyMove.timeJump;

        cubeCollision.enemys[enemy_Number].transform.position += move;

        enemyMove.timeJump = enemyMove.timeJump + Time.deltaTime;

        enemyMove.canJump = 1;
    }
    //敵用　プレイヤーと同じ
    private void ResetStartJump_tatumaki_enemy(int enemy_Number)
    {
        EnemyMove3 enemyMove = cubeCollision.enemys[enemy_Number].GetComponent<EnemyMove3>();

        enemyMove.startJump_tatumaki = 0;
    }

}
