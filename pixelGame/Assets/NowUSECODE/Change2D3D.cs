using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change2D3D : MonoBehaviour
{
    //ゲーム全体で使う　モードチェンジの変数
    public int change2D3D;

    //ゲームの全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    /*
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }
    */
    private void Awake()
    {
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
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            //1Dモードへの変更
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                change2D3D = 1;

            }
            //2Dモードへの変更
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                change2D3D = 2;

            }
            //3Dモードへの変更
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                change2D3D = 3;

            }
        }
    }
}
