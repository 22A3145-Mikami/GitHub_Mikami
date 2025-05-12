using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    /*
    public Vector3[] enemyList = new Vector3[2];
    public GameObject[] enemyObjList = new GameObject[2];
    public float[] enemyMode3dObjPosZ = new float[2];
    private Vector3[] enemyMoveList = new Vector3[2];

    private EnemyMove enemyMove;
    [SerializeField]
    private GameObject enemy;
    */

    private AllCollision allCollision;

    // “GPopList
    public Vector3[] enemyPopList = new Vector3[2];
    public GameObject[] enemyPopObjList = new GameObject[2];
    public float[] enemyPopMode3dObjPosZ = new float[2];
    [SerializeField]
    private GameObject enemyPop;

    // Start is called before the first frame update
    void Start()
    {
        /*
        enemyList[0] = new Vector3(7, 10f, 0);
        enemyList[1] = new Vector3(-1, 10f, 0);

        enemyMode3dObjPosZ[0] = 5f;
        enemyMode3dObjPosZ[1] = 2f;

        enemyMoveList[0] = new Vector3(-0.05f, 0, 0);
        enemyMoveList[1] = new Vector3(-0.06f, 0, 0);

        for (int i = 0; i < enemyObjList.Length; i++)
        {
            enemyObjList[i] = Instantiate(enemy, enemyList[i], Quaternion.identity);

            enemyMove = enemyObjList[i].GetComponent<EnemyMove>();
            enemyMove.move = enemyMoveList[i];
        }
        */

        enemyPopList[0] = new Vector3(10, 0f, 0);
        enemyPopList[1] = new Vector3(15, 0f, 0);

        enemyPopMode3dObjPosZ[0] = 5;
        enemyPopMode3dObjPosZ[1] = 0;

        for (int i = 0; i < enemyPopList.Length; i++)
        {
            enemyPopObjList[i] = Instantiate(enemyPop, enemyPopList[i], Quaternion.identity);
            allCollision = enemyPopObjList[i].GetComponent<AllCollision>();

            allCollision.candownHit = false;
            allCollision.canSideHit = false;

            allCollision.obj3dModePosZ = enemyPopMode3dObjPosZ[i];

            enemyPopObjList[i].transform.SetParent(transform);
        }


        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}