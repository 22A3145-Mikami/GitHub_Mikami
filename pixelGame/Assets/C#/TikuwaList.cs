using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikuwaList : MonoBehaviour
{
    // （ X ; 生成するか (0 = 消えてる　1 = ある　２ = ２回目以上)　Y : 時間　Z : 速さ）　生成場所　消滅場所　
    public Vector3[,] tikuwaList = new Vector3[2, 3];
    public GameObject[] tikuwaObjList = new GameObject[2];
    public float[] tikuwaMode3dObjPosZ = new float[2];
    [SerializeField]
    private GameObject tikuwa;
    public bool destroyJudge;  // true = 生成するものがある状態　false = 生成するものがない状態　　生成オブジェクトで変更

    //　すり抜けブロック
    public Vector3[] surinukeList = new Vector3[2];
    public GameObject[] surinukeObjList = new GameObject[2];
    public float[] surinukeMode3dObjPosZ = new float[2];
    [SerializeField]
    private GameObject surinuke;


    //  普通のブロック
    public Vector3[] normalList = new Vector3[2];
    public GameObject[] normalObjList = new GameObject[2];
    public float[] normalMode3dObjPosZ = new float[2];
    [SerializeField]
    private GameObject normal;

    //　ステージ（大きいブロック）
    public Vector3[] stageList = new Vector3[2];
    public GameObject[] stageObjList = new GameObject[2];
    public float[] stageMode3dObjPosZ = new float[2];
    [SerializeField]
    private GameObject stage;


    // 竜巻
    public Vector3[] tatumakiList = new Vector3[2];
    public GameObject[] tatumakiObjList = new GameObject[2];
    public float[] tatumakiMode3dObjPosZ = new float[2];
    [SerializeField]
    private GameObject tatumaki;

    /*
    // 敵PopList
    public Vector3[] enemyPopList = new Vector3[2];
    public GameObject[] enemyPopObjList = new GameObject[2];
    public float[] enemyPopMode3dObjPosZ = new float[2];
    [SerializeField]
    private GameObject enemyPop;
    */

    private AllCollision allCollision;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        tikuwaList[0, 0] = new Vector3(0, 5, 0.01f);
        tikuwaList[0, 1] = new Vector3(1, 0.5f, 0);
        tikuwaList[0, 2] = new Vector3(1, 0, 0);

        tikuwaList[1, 0] = new Vector3(0, 5, 0.01f);
        tikuwaList[1, 1] = new Vector3(2, 4, 0);
        tikuwaList[1, 2] = new Vector3(2, 3, 2);

        tikuwaMode3dObjPosZ[0] = 5;
        tikuwaMode3dObjPosZ[1] = 0;

        destroyJudge = true;

        player = GameObject.Find("player");

        //////////              すりぬけ                ////////////
        surinukeList[0] = new Vector3(-7, 4, 0);
        surinukeList[1] = new Vector3(-7, 5, 0);

        surinukeMode3dObjPosZ[0] = 2;
        surinukeMode3dObjPosZ[1] = 4;

        for (int i = 0; i < surinukeList.Length; i++)
        {
            surinukeObjList[i] = Instantiate(surinuke, surinukeList[i], Quaternion.identity);
            allCollision = surinukeObjList[i].GetComponent<AllCollision>();
            
            allCollision.candownHit = false;
            allCollision.canSideHit = false;
            // 奥行きの座標
            allCollision.obj3dModePosZ = surinukeMode3dObjPosZ[i];

            surinukeObjList[i].transform.SetParent(transform);
        }


        //////////              普通のブロック               ////////////
        normalList[0] = new Vector3(7, 6, 0);
        normalList[1] = new Vector3(7, 5, 0);

        normalMode3dObjPosZ[0] = 3;
        normalMode3dObjPosZ[1] = -2;

        for(int i = 0; i< normalList.Length; i++)
        {
            normalObjList[i] = Instantiate(normal, normalList[i], Quaternion.identity);
            allCollision = normalObjList[i].GetComponent<AllCollision>();

            allCollision.candownHit = true;
            allCollision.canSideHit = true;

            allCollision.obj3dModePosZ = normalMode3dObjPosZ[i];

            normalObjList[i].transform.SetParent(transform);
        }

        
        //////////              ステージ               ////////////
        stageList[0] = new Vector3(30, -5, 0);
        stageList[1] = new Vector3(50, -5, 0);

        stageMode3dObjPosZ[0] = 0;
        stageMode3dObjPosZ[1] = -5;

        for(int i = 0; i < stageList.Length; i++)
        {
            stageObjList[i] = Instantiate(stage, stageList[i], Quaternion.identity);
            allCollision = stageObjList[i].GetComponent<AllCollision>();

            allCollision.candownHit = true;
            allCollision.canSideHit = true;

            allCollision.obj3dModePosZ = stageMode3dObjPosZ[i];

            stageObjList[i].transform.SetParent(transform);
        }
        


        //////////              竜巻               ////////////
        tatumakiList[0] = new Vector3(-5, 0.5f, 0);
        tatumakiList[1] = new Vector3(-6, 0.5f, 0);

        tatumakiMode3dObjPosZ[0] = 1;
        tatumakiMode3dObjPosZ[1] = 2;

        for (int i = 0; i < tatumakiList.Length; i++)
        {
            tatumakiObjList[i] = Instantiate(tatumaki, tatumakiList[i], Quaternion.identity);
            allCollision = tatumakiObjList[i].GetComponent<AllCollision>();

            allCollision.candownHit = false;
            allCollision.canSideHit = false;

            allCollision.obj3dModePosZ = tatumakiMode3dObjPosZ[i];

            tatumakiObjList[i].transform.SetParent(transform);
        }

        /*
        //////////              敵Pop               ////////////
        enemyPopList[0] = new Vector3(10, 0.5f, 0);
        enemyPopList[1] = new Vector3(15, 0.5f, 0);

        enemyPopMode3dObjPosZ[0] = 5;
        enemyPopMode3dObjPosZ[1] = 0;

        for(int i = 0; i < enemyPopList.Length; i++)
        {
            enemyPopObjList[i] = Instantiate(enemyPop, enemyPopList[i], Quaternion.identity);
            allCollision = enemyPopObjList[i].GetComponent<AllCollision>();

            allCollision.candownHit = false;
            allCollision.canSideHit = false;

            allCollision.obj3dModePosZ = enemyPopMode3dObjPosZ[i];

            enemyPopObjList[i].transform.SetParent(transform);
        }
        */
        Application.targetFrameRate = 60;
    }

    private void FixedUpdate()
    {
        if (destroyJudge)
        {
            for (int i = 0; i < tikuwaList.GetLength(0); i++)
            {
                if (tikuwaList[i, 0].x == 0)
                {

                    tikuwaObjList[i] = Instantiate(tikuwa, tikuwaList[i, 1], Quaternion.identity);
                    tikuwaList[i, 0].x = 1;

                    allCollision = tikuwaObjList[i].GetComponent<AllCollision>();

                    allCollision.candownHit = true;
                    allCollision.canSideHit = true;
                    allCollision.candownHitEnemy = true;
                    allCollision.canSideHitEnemy = true;

                    allCollision.obj3dModePosZ = tikuwaMode3dObjPosZ[i];

                    tikuwaObjList[i].transform.SetParent(transform);
                }
                else if (tikuwaList[i, 0].x == 2)
                {
                    tikuwaObjList[i].SetActive(true);
                    tikuwaObjList[i].transform.position = tikuwaList[i, 1];
                    tikuwaList[i, 0].x = 1;
                }
            }
            destroyJudge = false;
        }
    }


    /*
    // Update is called once per frame
    void Update()
    {
        if(destroyJudge)
        {
            for(int i = 0; i < tikuwaList.GetLength(0); i++)
            {
                if(tikuwaList[i, 0].x == 0)
                {
                    
                    tikuwaObjList[i] = Instantiate(tikuwa, tikuwaList[i, 1], Quaternion.identity);
                    tikuwaList[i, 0].x = 1;

                    allCollision = tikuwaObjList[i].GetComponent<AllCollision>();

                    allCollision.candownHit = true;
                    allCollision.canSideHit = true;

                    allCollision.obj3dModePosZ = tikuwaMode3dObjPosZ[i];

                    tikuwaObjList[i].transform.SetParent(transform);
                }
                else if (tikuwaList[i, 0].x == 2)
                {
                    tikuwaObjList[i].SetActive(true);
                    tikuwaObjList[i].transform.position = tikuwaList[i, 1];
                    tikuwaList[i, 0].x = 1;
                }
            }
            destroyJudge = false;
        }
    }
    */
}
