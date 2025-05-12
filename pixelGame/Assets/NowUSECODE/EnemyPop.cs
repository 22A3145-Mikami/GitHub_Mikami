using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPop : MonoBehaviour
{
    [SerializeField]
    private float coolTime; //クールタイム
    [SerializeField]
    private float popTime; // popする時間

    [SerializeField]
    private GameObject enemy; //芋虫

    private Vector3 popPos; //生成場所

    //ゲーム全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    //コース生成コード
    private StageSet stageSet;

    //敵の大きさの初期設定
    [SerializeField]
    private Vector3 scale = new Vector3(1, 1, 1);
    //敵の移動速度
    [SerializeField]
    private float eSpeed = 0.08f;

    private void Awake()
    {
        //フレームレート設定
        Application.targetFrameRate = 60;

        //生成場所
        popPos = transform.position;
        popPos.y += transform.localScale.y / 2 + 1;

        //再生成するまでの秒数
        popTime = 6;

        //ゲーム全体の流れ取得
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
        //コース生成コード取得
        stageSet = gameMaker.GetComponent<StageSet>();


        scale = new Vector3(1, 1, 1);//始めの大きさ

    }
    ///StageSetコード用関数
    public void SetcoolTime(float time)
    {
        coolTime = time;
    }
    public void SetEnemyScale(Vector3 size)
    {
        scale = size;
    }
    public void SetEnemySpeed(float speed)
    {
        eSpeed = speed;
    }
    ///
    private void FixedUpdate()
    {
        //一時中断していない　かつ　コースプレイ中
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            coolTime += Time.deltaTime;
            //クールタイム経過時に生成
            if (coolTime >= popTime)
            {

                GameObject imomusi;
                imomusi = stageSet.Instantiate_OBJ(enemy, popPos, scale);
                imomusi.GetComponent<EnemyMove3>().SetSpeed(eSpeed); //芋虫のコードに渡す
                

                coolTime = 0;

                //Debug.Log("pop");
            }
        }
        
    }

}
