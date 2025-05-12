using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPNG;//ゲームオーバー画像
    [SerializeField]
    private GameObject gameClearPNG;//ゲームクリア画像
    // 他コードで判定取る変数
    public bool gameOver;
    public bool gameClear;

    public bool stop; //全てのコードの一時停止   0 = start　にいるとき
    //[SerializeField]
    public int changeScene; // -1 == コースプレイ　 0 == タイトル画面　1 == コースセレクト画面　2 == フェード
    [SerializeField]
    private GameObject startGamenPNG;//start画像
    [SerializeField]
    private GameObject selectGamenPNG;//コースセレクト画像
    [SerializeField]
    private GameObject blackPanel; //　コースへの遷移

    [SerializeField]
    private StageSet stageSet; // オブジェクトを生成するコード

    [SerializeField]
    private GameObject playUi; //残り時間表示など


    public int keepStageSet;//リスタートのときに使う
    public bool pushR; // リトライしたとき cameraをつける　　GameMaker CameraMove StageSet

    public bool firstplay; //初めてのプレイ

    [SerializeField]
    private GameObject stopScenePNG;//一時停止画像

    private float time = 0;  // コース遷移の黒幕用
    private Image blackPanel_image;  //  黒幕のα値操作用
    private float alpha = -1;//画像の透明度

    private float gameTime = -1; // ゲーム終了までの時間  何もないときは-1
    [SerializeField]
    private TextMeshProUGUI timeText;//右上のTime

    [SerializeField]
    private GameObject keySetPNG;//操作画像
    private bool openKeySet;//Aキー入力

    
    [SerializeField]
    private GameObject creditSetPNG;
    private bool openCredit;
    

    //モード切り替えのコードと変数
    [SerializeField]
    private Change2D3D change2D3DCode;

    [SerializeField]
    private GameObject dead_time_moji;//死因文字　時間切れ
    //[SerializeField]
    public GameObject dead_press_moji;//死因文字　圧死
    //[SerializeField]
    public GameObject dead_enemy_moji;//死因文字　敵
    //[SerializeField]
    public GameObject dead_abs_moji;//死因文字　落下

    // Start is called before the first frame update
    private void Start()
    {
        firstplay = true;//初めてゲームを開始する

        ResetGame(0);

        //フレームレート設定
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (changeScene == 0)//スタート画面
        {
            StartGamen();

            playUi.SetActive(false);
        }
        else if (changeScene == 1)//コースセレクト画面
        {
            SelectGamen();

            playUi.SetActive(false);
        }
        else if (changeScene == 2 && !stop) // stopは一時停止　フェードイン
        {
            GameStart();
        }
        else if(changeScene == -1 && !stop)//コースプレイ中
        {
            if (gameClear && !gameOver)//ゲームクリア画面
            {
                gameClearPNG.SetActive(true);

                playUi.SetActive(false);

                if (Input.GetKey(KeyCode.S))//コースセレクトに戻る
                {
                    ResetGame(1);
                }
                else if (Input.GetKey(KeyCode.T))//タイトル画面に戻る
                {
                    ResetGame(0);
                }
            }
            else if (gameOver && !gameClear)//ゲームオーバー画面
            {
                gameOverPNG.SetActive(true);

                playUi.SetActive(false);

                if (Input.GetKey(KeyCode.S))//コースセレクトに戻る
                {
                    ResetGame(1);
                }
                else if (Input.GetKey(KeyCode.T))//タイトル画面に戻る
                {
                    ResetGame(0);
                }
            }


            if (Input.GetKey(KeyCode.R))//コースを最初に戻す
            {
                pushR = true;

                ResetGame(2); //そのコースの最初から
            }

            TimeText(300);
        }

        if(Input.GetKeyUp(KeyCode.Escape))//一時停止画面
        {
            StopGame();
        }
        Stopping();
    }

    //画面遷移関数　　
    private void ResetGame(int change)
    {

        changeScene = change;

        gameClearPNG.SetActive(false);
        gameClear = false;

        gameOverPNG.SetActive(false);
        gameOver = false;

        startGamenPNG.SetActive(false);

        selectGamenPNG.SetActive(false);

        blackPanel.SetActive(false);
        alpha = 1;
        time = 0;

        playUi.SetActive(false);
        gameTime = -1;

        stopScenePNG.SetActive(false);
        stop = false;

        keySetPNG.SetActive(false);

        creditSetPNG.SetActive(false);

        dead_abs_moji.SetActive(false);
        dead_enemy_moji.SetActive(false);
        dead_press_moji.SetActive(false);
        dead_time_moji.SetActive(false);

        if (change == 2)
        {
            stageSet.SetObj(keepStageSet);
        }

        change2D3DCode.change2D3D = 2;
    }
    //一時停止関数　その１
    private void StopGame()
    {
        //Debug.Log("呼んだ？");
        if (stop)
        {
            stop = false;
        }
        else
        {
            stop = true;
        }
    }
    //一時停止関数　その２
    private void Stopping()
    {
        if (stop)
        {
            stopScenePNG.SetActive(true);
            //
            if (Input.GetKeyUp(KeyCode.T))
            {
                ResetGame(0);
            }
            //
            else if (Input.GetKeyUp(KeyCode.S))
            {
                ResetGame(1);
            }
            //  1
            else if (Input.GetKeyUp(KeyCode.A))
            {
                OpenKeySet();
            }
            //  2 音源入れる場合
            
            else if(Input.GetKeyUp(KeyCode.C))
            {
                OpenCredit();
            }
            
            //  1
            KeyOption();
            //  2 音源入れる場合
            Credit();
            
        }
        else if (!stop)
        {
            stopScenePNG.SetActive(false);
        }
    }

    //start画面関数
    private void StartGamen()
    {
        startGamenPNG.SetActive(true);

        /*
        if(Input.anyKey)
        {
            if(!(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) 
             || Input.GetKey(KeyCode.Alpha3) || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)/* || Input.GetKey(KeyCode.Q)) )
            {
                changeScene++;
            }
        }
        */
        if(Input.GetKey(KeyCode.Space))
        {
            changeScene++;

        }
        
    }
    
    //コースセレクト関数
    private void SelectGamen()
    {
        startGamenPNG.SetActive(false);

        selectGamenPNG.SetActive(true);


        if(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))//１　か　テンキー１
        {
            keepStageSet = 1;

            stageSet.SetObj(1);//コース１を生成


            changeScene++;
        }
        
        else if(Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))//２　か　テンキー２
        {
            keepStageSet = 2;

            stageSet.SetObj(2);//コース２を生成

            changeScene++;
        }
        
        else if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))//３　か　テンキー３
        {
            keepStageSet = 3;

            stageSet.SetObj(3);//コース３を生成

            changeScene++;
        }
        

    }

    //start画面関数
    private void GameStart()
    {

        selectGamenPNG.SetActive(false);

        blackPanel.SetActive(true);
        blackPanel_image = blackPanel.GetComponent<Image>();
        //透明度の更新
        alpha = -(2 * time * time) + 1;
        time += Time.deltaTime;
        blackPanel_image.color = new Color(0,0,0,alpha);
        if(alpha < 0)
        {
            blackPanel.SetActive(false);
            
            alpha = -1;
            
            changeScene = -1;

        }
    }
    //時間関数
    private void TimeText(float setTime)
    {
        //コースプレイ中
        if(!gameClear && !gameOver)
        {
            playUi.SetActive(true);
        }

        if(gameTime == -1)
        {
            gameTime = setTime;
        }
        gameTime -= Time.deltaTime;
        //マイナスに行かないようにする
        if(gameTime < 0)
        {
            // ゲームオーバー
            Debug.Log("gameover"); // 帰りが遅くなった

            gameOver = true;
            //時間切れ
            dead_time_moji.SetActive(true);
            //

            gameTime = 0;
        }

        timeText.text = "Time\n" + (int)gameTime;
    }
    
    //操作画像関数（Aキー）
    private void KeyOption()
    {
        if(openKeySet)
        {
            keySetPNG.SetActive(true);
        }
        else if(!openKeySet)
        {
            keySetPNG.SetActive(false);
        }
    }
    //操作画像関数（Aキー）
    private void OpenKeySet()
    {
        if (openKeySet)
        {
            openKeySet = false;
        }
        else if (!openKeySet)
        {
            openKeySet = true;
        }
    }
    
    private void Credit()
    {
        if (openCredit)
        {
            creditSetPNG.SetActive(true);
        }
        else if (!openCredit)
        {
            creditSetPNG.SetActive(false);
        }
    }

    private void OpenCredit()
    {
        if (openCredit)
        {
            openCredit = false;
        }
        else if (!openCredit)
        {
            openCredit = true;
        }
    }
    
}
