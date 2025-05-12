using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSet : MonoBehaviour
{
    //ゲーム全体の流れオブジェとコード
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    // 生成用
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject tree;
    [SerializeField]
    private GameObject stage;
    [SerializeField]
    private GameObject normalblock;
    [SerializeField]
    private GameObject ukiyuka;
    [SerializeField]
    private GameObject tooreruyuka;
    [SerializeField]
    private GameObject tatumaki;
    [SerializeField]
    private GameObject goal;
    [SerializeField]
    private GameObject block1;
    [SerializeField]
    private GameObject block2;
    [SerializeField]
    private GameObject block3;
    [SerializeField]
    private GameObject block4;
    [SerializeField]
    private GameObject kaidan;

    // 削除用
    [SerializeField]
    private GameObject player_Delete;
    [SerializeField]
    private GameObject[] enemy_Delete;
    [SerializeField]
    private GameObject[] block_Delete;
    [SerializeField]
    private GameObject[] ki_Delete;
    [SerializeField]
    private GameObject[] tatumaki_Delete;
    [SerializeField]
    private GameObject goal_Delete;

    //当たり判定コード
    [SerializeField]
    private CubeCollision cubeCollision;

    // Start is called before the first frame update
    void Start()
    {
        //フレームレート設定
        Application.targetFrameRate = 60;

        //ゲーム全体の流れ取得
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
    }

    public void SetObj(int select_Number)
    {
        //Debug.Log(select_Number + "よんだ");
        if(select_Number == 1)
        {
            if(gameMakerCode.firstplay)
            {
                gameMakerCode.firstplay = false;
            }
            else if(!gameMakerCode.firstplay)
            {
                DestroyOBJ();
                
            }
            Obj1();

        }
        else if(select_Number == 2)
        {
            if (gameMakerCode.firstplay)
            {
                gameMakerCode.firstplay = false;
            }
            else if (!gameMakerCode.firstplay)
            {
                DestroyOBJ();

            }
            Obj2();
        }
        else if(select_Number == 3)
        {
            if (gameMakerCode.firstplay)
            {
                gameMakerCode.firstplay = false;
            }
            else if (!gameMakerCode.firstplay)
            {
                DestroyOBJ();

            }
            Obj3();
        }
    }

    //破壊用オブジェクト変数
    private void AllObj()
    {
        player_Delete = GameObject.FindGameObjectWithTag("Player");

        //エネミーの検索
        enemy_Delete = GameObject.FindGameObjectsWithTag("enemy");

        // ブロックの検索
        block_Delete = GameObject.FindGameObjectsWithTag("block");

        // 竜巻の検索
        tatumaki_Delete = GameObject.FindGameObjectsWithTag("tatumaki");

        // 木の検索
        ki_Delete = GameObject.FindGameObjectsWithTag("tree");

        goal_Delete = GameObject.FindGameObjectWithTag("goal");
    }
    
    //ゲームオーバーなどでオブジェを破壊する
    private void DestroyOBJ()
    {
        AllObj();

        Destroy(player_Delete);
        Destroy(goal_Delete);
        for(int i = 0; i < enemy_Delete.Length; i++)
        {
            Destroy(enemy_Delete[i]);
        }
        for(int i = 0; i < ki_Delete.Length; i++)
        {
            Destroy(ki_Delete[i]);
        }
        for (int i = 0; i < block_Delete.Length; i++)
        {
            Destroy(block_Delete[i]);
        }
        for(int i = 0; i < tatumaki_Delete.Length; i++)
        {
            Destroy(tatumaki_Delete[i]);
        }
        gameMakerCode.pushR = false;
    }

    //オブジェの生成関数　（大きさ変更可）
    public GameObject Instantiate_OBJ(GameObject obj, Vector3 position, Vector3 scale)
    {
        GameObject popObj =  Instantiate(obj, position, Quaternion.identity);

        Vector3 localScale = popObj.transform.localScale;
        localScale.x *= scale.x;
        localScale.y *= scale.y;
        localScale.z *= scale.z;
        popObj.transform.localScale = localScale;
        ScaleChanger changer = popObj.GetComponent<ScaleChanger>();
        changer.change = scale;

        return popObj;
    }
    //オブジェの生成関数
    public GameObject Instantiate_OBJ(GameObject obj, Vector3 position)
    {
        return Instantiate(obj, position, Quaternion.identity);
    }

    //ステージセット　コース１
    private void Obj1()
    {
        Instantiate_OBJ(player, new Vector3(0, 10, 0));
        
        Instantiate_OBJ(stage, new Vector3(0, 0, 0));
        Instantiate_OBJ(stage, new Vector3(22.5f, 0, 0), new Vector3(0.5f, 1, 1));
        Instantiate_OBJ(stage, new Vector3(37.5f, 0, -40), new Vector3(0.5f, 1, 1));
        Instantiate_OBJ(stage, new Vector3(60, 0, 0));
        Instantiate_OBJ(stage, new Vector3(90, 0, 0));
        Instantiate_OBJ(stage, new Vector3(120, 0, 0));

        Instantiate_OBJ(normalblock, new Vector3(15, 10, 0), new Vector3(5, 10, 5));
        Instantiate_OBJ(normalblock, new Vector3(37, 10, 0), new Vector3(5, 10, 5));
        Instantiate_OBJ(normalblock, new Vector3(78, 10, 10), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(78, 20, 10), new Vector3(10, 10, 10));

        Instantiate_OBJ(kaidan, new Vector3(78, 10, -5), new Vector3(10, 10, 20));

        Instantiate_OBJ(tree, new Vector3(80, 25, 10));
        Instantiate_OBJ(tree, new Vector3(59, 5, 4));
        Instantiate_OBJ(tree, new Vector3(59, 5, -4));

        Instantiate_OBJ(goal, new Vector3(117, 10.6f, 0));
        
    }

    //ステージセット　コース２
    private void Obj2()
    {
        //
        Instantiate_OBJ(player, new Vector3(0, 10, 0));
        //
        //
        Instantiate_OBJ(stage, new Vector3(0, 0, 0));
        Instantiate_OBJ(stage, new Vector3(30, 15, 0));

        Instantiate_OBJ(stage, new Vector3(60, 15, -10), new Vector3(1, 1, 0.3f));
        Instantiate_OBJ(stage, new Vector3(49.5f, 15, 0), new Vector3(0.3f, 1, 0.4f));
        Instantiate_OBJ(stage, new Vector3(70, 15, 0), new Vector3(0.3f, 1, 0.4f));
        Instantiate_OBJ(stage, new Vector3(60, 15, 10), new Vector3(1, 1, 0.3f));

        Instantiate_OBJ(stage, new Vector3(90, 40, 0));
        Instantiate_OBJ(stage, new Vector3(140, 40, 0));
        Instantiate_OBJ(stage, new Vector3(90, 20, 0));
        Instantiate_OBJ(stage, new Vector3(90, 30, 0));
        
        //下√
        Instantiate_OBJ(stage, new Vector3(40, 0, -1), new Vector3(0.3f, 1, 1));
        Instantiate_OBJ(stage, new Vector3(60, 0, -1));
        Instantiate_OBJ(stage, new Vector3(90, 0, -1));
        Instantiate_OBJ(stage, new Vector3(90, 15, 0));

        Instantiate_OBJ(normalblock, new Vector3(17.5f, 7.5f, 0), new Vector3(5, 5, 30));
        //
        //
        GameObject uki;
        uki = Instantiate_OBJ(ukiyuka, new Vector3(13, 6, 20));
        UkiyukaMove ukimove;
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(13, 6, 20), new Vector3(13, 19, 20));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(22, 25, 0));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(22, 25, 0), new Vector3(22, 40, 0));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.05f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(26, 37.5f, -15));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(25, 37.5f, -15), new Vector3(34, 37.5f, 20));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(45, 37, -25));
        ukimove = uki.GetComponent<UkiyukaMove>(); 
        ukimove.SetPosition(new Vector3(45, 37, -25), new Vector3(54, 37, 0));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(33, 25, -20));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(33, 25, -20), new Vector3(43, 29, -20));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.05f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(45, 30, 30));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(45, 30, 30), new Vector3(50, 30, 10));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(53.5f, 21, -5));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(53.5f, 21, -5), new Vector3(53.5f, 30, 20));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(58, 27, 15));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(58, 22, 15), new Vector3(62, 25, 0));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(63, 36, 30));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(64, 36, 0), new Vector3(73, 45f, 0));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);


        uki = Instantiate_OBJ(ukiyuka, new Vector3(109, 4.3f, 0), new Vector3(2f, 1, 7));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(109, 4.3f, 0), new Vector3(109, 21.5f, 0));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(121, 35.7f, 30), new Vector3(2, 1, 4));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(121, 35.7f, 30), new Vector3(121, 18.5f, 30));
        ukimove.Move_Keeper(true);
        ukimove.SetSpeed(0.1f);

        //
        Instantiate_OBJ(tooreruyuka, new Vector3(115, 20, 0));
        Instantiate_OBJ(tatumaki, new Vector3(111, 35.5f, 30), new Vector3(6, 6, 6));

        //
        //
        Instantiate_OBJ(tooreruyuka, new Vector3(13, 19.6f, -20));
        Instantiate_OBJ(tooreruyuka, new Vector3(28, 26, 10), new Vector3(1.5f, 1.5f, 1.5f));
        Instantiate_OBJ(tooreruyuka, new Vector3(39.5f, 37, -20));
        Instantiate_OBJ(tooreruyuka, new Vector3(67, 27, 20));
        Instantiate_OBJ(tooreruyuka, new Vector3(59f, 36, 0));

        Instantiate_OBJ(tooreruyuka, new Vector3(110, 44, 0), new Vector3(2.5f, 1, 2.5f));
        Instantiate_OBJ(tooreruyuka, new Vector3(120, 44, 0), new Vector3(2.5f, 1, 2.5f));
        //
        Instantiate_OBJ(kaidan, new Vector3(70, 25, 0), new Vector3(10, 10, 30));

        GameObject wood;
        EnemyPop pop;
        wood = Instantiate_OBJ(tree, new Vector3(90, 5, -8));
        pop = wood.GetComponent<EnemyPop>();
        pop.SetcoolTime(3);
        pop.SetEnemyScale(new Vector3(5,5,5));

        wood = Instantiate_OBJ(tree, new Vector3(70, 5, -11));
        pop = wood.GetComponent<EnemyPop>();
        pop.SetcoolTime(6);
        pop.SetEnemyScale(new Vector3(5, 5, 5));

        wood = Instantiate_OBJ(tree, new Vector3(100, 5, -13));
        pop = wood.GetComponent<EnemyPop>();
        pop.SetcoolTime(5);
        pop.SetEnemyScale(new Vector3(5, 5, 5));

        wood = Instantiate_OBJ(tree, new Vector3(90, 5, 8));
        pop = wood.GetComponent<EnemyPop>();
        pop.SetcoolTime(3);
        pop.SetEnemyScale(new Vector3(5, 5, 5));

        wood = Instantiate_OBJ(tree, new Vector3(70, 5, 11));
        pop = wood.GetComponent<EnemyPop>();
        pop.SetcoolTime(5);
        pop.SetEnemyScale(new Vector3(5, 5, 5));

        wood = Instantiate_OBJ(tree, new Vector3(100, 5, 13));
        pop = wood.GetComponent<EnemyPop>();
        pop.SetcoolTime(6);
        pop.SetEnemyScale(new Vector3(5, 5, 5));

        Instantiate_OBJ(goal, new Vector3(137, 50.5f));

    }

    //ステージセット　コース３
    private void Obj3()
    {
        Instantiate_OBJ(player, new Vector3(0, 10, 0));

        Instantiate_OBJ(stage, new Vector3(0, 0, 0));
        //
        Instantiate_OBJ(normalblock, new Vector3(-10, 10, 0), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(-10, 20, 0), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(-10, 30, 0), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(-10, 40, 0), new Vector3(10, 10, 10));
        //
        //
        Instantiate_OBJ(tree, new Vector3(10, 5, 0));
        //

        Instantiate_OBJ(stage, new Vector3(12, 30, 0));

        //
        GameObject uki;
        uki = Instantiate_OBJ(ukiyuka, new Vector3(37, 32, 0), new Vector3(5, 5, 5));
        UkiyukaMove ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(37, 32, 0), new Vector3(80, 32, 0));
        ukimove.SetSpeed(0.1f);
        ukimove.Move_Keeper(false);

        uki = Instantiate_OBJ(ukiyuka, new Vector3(100, 32, 23), new Vector3(5, 5, 5));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(100, 32, 23), new Vector3(100, 32, -50));
        ukimove.SetSpeed(0.1f);
        ukimove.Move_Keeper(false);
        //

        Instantiate_OBJ(normalblock, new Vector3(115, 28, 0), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, 0), new Vector3(10, 10, 10));
        GameObject wood;
        wood = Instantiate_OBJ(tree, new Vector3(115, 43, 0));
        wood.GetComponent<EnemyPop>().SetcoolTime(3);

        Instantiate_OBJ(normalblock, new Vector3(115, 28, 10), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, 10), new Vector3(10, 10, 10));
        wood = Instantiate_OBJ(tree, new Vector3(115, 43, 10));
        wood.GetComponent<EnemyPop>().SetcoolTime(4);

        Instantiate_OBJ(normalblock, new Vector3(115, 28, 20), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, 20), new Vector3(10, 10, 10));
        wood = Instantiate_OBJ(tree, new Vector3(115, 43, 20));
        wood.GetComponent<EnemyPop>().SetcoolTime(5);

        Instantiate_OBJ(normalblock, new Vector3(115, 28, 30), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, 30), new Vector3(10, 10, 10));
        wood = Instantiate_OBJ(tree, new Vector3(115, 43, 30));
        wood.GetComponent<EnemyPop>().SetcoolTime(3);

        Instantiate_OBJ(normalblock, new Vector3(115, 28, -10), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, -10), new Vector3(10, 10, 10));
        wood = Instantiate_OBJ(tree, new Vector3(115, 43, -10));
        wood.GetComponent<EnemyPop>().SetcoolTime(5);

        Instantiate_OBJ(normalblock, new Vector3(115, 28, -20), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, -20), new Vector3(10, 10, 10));
        wood = Instantiate_OBJ(tree, new Vector3(115, 43, -20));
        wood.GetComponent<EnemyPop>().SetcoolTime(4);

        Instantiate_OBJ(normalblock, new Vector3(115, 28, -30), new Vector3(10, 10, 10));

        Instantiate_OBJ(normalblock, new Vector3(115, 28, -40), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, -40), new Vector3(10, 10, 10));
        wood = Instantiate_OBJ(tree, new Vector3(115, 43, -40));
        wood.GetComponent<EnemyPop>().SetcoolTime(7);

        Instantiate_OBJ(normalblock, new Vector3(115, 28, -50), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, -50), new Vector3(10, 10, 10));
        Instantiate_OBJ(tree, new Vector3(115, 43, -50));
        wood.GetComponent<EnemyPop>().SetcoolTime(9);

        Instantiate_OBJ(normalblock, new Vector3(115, 28, -60), new Vector3(10, 10, 10));
        Instantiate_OBJ(normalblock, new Vector3(115, 38, -60), new Vector3(10, 10, 10));
        wood = Instantiate_OBJ(tree, new Vector3(115, 43, -60));
        wood.GetComponent<EnemyPop>().SetcoolTime(6);

        //
        uki = Instantiate_OBJ(ukiyuka, new Vector3(130, 32, -30), new Vector3(5, 5, 5));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(130, 32, -30), new Vector3(180, 32, -30));
        ukimove.SetSpeed(0.1f);
        ukimove.Move_Keeper(false);

        Instantiate_OBJ(normalblock, new Vector3(190, 35.5f, -30), new Vector3(10, 5, 20));
        wood = Instantiate_OBJ(tree, new Vector3(190, 38, -30));
        wood.GetComponent<EnemyPop>().SetEnemyScale(new Vector3(1.5f, 1.5f, 1.5f));
        wood.GetComponent<EnemyPop>().SetEnemySpeed(0.1f);

        Instantiate_OBJ(tooreruyuka, new Vector3(130, 42, -30), new Vector3(5,5,5));

        Instantiate_OBJ(tatumaki, new Vector3(138, 45, -30), new Vector3(3, 3, 3));

        uki = Instantiate_OBJ(ukiyuka, new Vector3(150, 45f, -30), new Vector3(5, 5, 5));
        ukimove = uki.GetComponent<UkiyukaMove>();
        ukimove.SetPosition(new Vector3(150, 45f, -30), new Vector3(180, 45f, -30));
        ukimove.SetSpeed(0.1f);
        ukimove.Move_Keeper(false);

        Instantiate_OBJ(stage, new Vector3(205, 28, -30));

        Instantiate_OBJ(goal, new Vector3(208, 39, -30));
    }
}
