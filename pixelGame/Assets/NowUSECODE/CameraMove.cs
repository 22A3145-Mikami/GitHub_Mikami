using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    
    // モード切り替えコードと変数いれる
    [SerializeField]
    private Change2D3D change2D3DCode;
    private int change2D3D;

    //ゲームの全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    //プレイヤーオブジェとワールド座標
    [SerializeField]
    private GameObject player;
    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        //モード切り替えの取得
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //自分のカメラコンポーネントの取得
        camera = GetComponent<Camera>();

        //フレームレート設定
        Application.targetFrameRate = 60;

        //ゲームの流れを取得
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ゲームが一時中断してない　かつ　コースプレイ中
        if(!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            //リトライが押されてないとき
            if(!gameMakerCode.pushR)
            {
                //プレイヤー座標取得　
                player = GameObject.FindGameObjectWithTag("Player");
                playerPos = player.transform.position;

                //3Dモードのとき
                if (change2D3D == 3)
                {
                    //自分の座標・回転変更
                    transform.position = playerPos + new Vector3(1, 3, -10);
                    transform.localEulerAngles = new Vector3(13, 0, 0);
                }
                //2D1Dモードのとき
                else if (change2D3D != 3)
                {
                    //自分の座標・回転変更
                    //Debug.Log("ts");
                    transform.position = playerPos + new Vector3(1, 1, -100);
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
            else
            {
                transform.position = Vector3.zero;
            }


            //モード切り替えの取得
            change2D3D = change2D3DCode.change2D3D;

            if (change2D3D == 3)
            {
                //透視投影に変更
                camera.orthographic = false;
            }
            else
            {
                //平行投影に変更
                camera.orthographic = true;
            }
        }
        
    }

}
