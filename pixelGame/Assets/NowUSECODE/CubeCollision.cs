using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    private GameObject obj_A; //�v���C���[�ƃG�l�~�[
    private ObjInfo allInfo_A;//obj_A��ObjInfo�R�[�h
    private Vector3 obj_A_Position; //�v���C���[�ƃG�l�~�[
    private Vector3 obj_A_Scale; //�v���C���[�ƃG�l�~�[

    private GameObject obj_B; // �u���b�N�ƃG�l�~�[
    private ObjInfo allInfo_B;//obj_A��ObjInfo�R�[�h
    private Vector3 obj_B_Position; // �u���b�N�ƃG�l�~�[
    private Vector3 obj_B_Scale; // �u���b�N�ƃG�l�~�[

    public GameObject player;//�v���C���[

    public List<GameObject> enemys;//�G�̔z��

    public List<GameObject> kis;//�؂̔z��

    public List<GameObject> blocks;//�u���b�N�̔z��

    public List<GameObject> tatumakis;//�����̔z��

    public GameObject goal;//�ƃI�u�W�F

    //���[�h�؂�ւ��̃R�[�h�ƕϐ�
    [SerializeField]
    private Change2D3D change2D3DCode;
    private int change2D3D;


    public float justBlockRange; // �u���b�N�́@�[�P�@�����₷�����邽�߂̒l  y�̓��Ƒ�����  ���@�S���ɒ�����
    [SerializeField]
    private float keepJustBlockRange;//�ۑ��p

    //�Q�[���S�̗̂���I�u�W�F�ƃR�[�h
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    private GameObject dead_press_moji; //���������@����

    private GameObject dead_enemy_moji;//���������@�G


    private void Awake()
    {
        //���[�h�؂�ւ��̎擾
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;

        //���݈ʒu�̃u���b�N
        keepJustBlockRange = justBlockRange;

        //�Q�[���S�̗̂���擾
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
        
        //��������������
        dead_press_moji = gameMakerCode.dead_press_moji;
        dead_enemy_moji = gameMakerCode.dead_enemy_moji;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�ꎞ���f���Ă��Ȃ��@���@�R�[�X�v���C��
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1 && !gameMakerCode.pushR)
        {
            //�v���C���[�擾
            player = GameObject.FindGameObjectWithTag("Player");



            //�G�l�~�[�̌���
            enemys = new List<GameObject>(GameObject.FindGameObjectsWithTag("enemy"));

            // �u���b�N�̌���
            blocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("block"));

            
            //�PD���[�h�̂Ƃ��̓v���C���[���u���b�N�ɂ���
            if(change2D3D == 1)
            {
                player.GetComponent<ObjInfo>().block = true;
                blocks.Add(player);
            }
            else
            {
                player.GetComponent<ObjInfo>().block = false;
            }
            //

            // �����̌���
            tatumakis = new List<GameObject>(GameObject.FindGameObjectsWithTag("tatumaki"));

            // �؂̌���
            kis = new List<GameObject>(GameObject.FindGameObjectsWithTag("tree"));

            //�S�[���擾
            goal = GameObject.FindGameObjectWithTag("goal"); 

            //���[�h�l�X�V
            change2D3D = change2D3DCode.change2D3D;


            // �v���C���[�ƓG�̓����蔻��
            for (int i = 0; i < enemys.Count; i++)
            {
                allInfo_A = player.GetComponent<ObjInfo>();
                allInfo_A.ResetHitWall();

                if (change2D3D == 3)
                {
                    AtariHantei3D(player, enemys[i], 2);
                }
                else if (change2D3D == 2)
                {
                    AtariHantei2D(player, enemys[i], 2);
                }
            }

            //�Ƃɒ�������
            if (change2D3D == 3)
            {
                AtariHantei3D(player, goal, 3);
            }
            else if (change2D3D != 3)
            {
                AtariHantei2D(player, goal, 3);
            }
        }
        

    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public void AtariHantei3D(GameObject playerORenemy, GameObject blockORenemy, int typeAns) // typeAns == 0 ���܂Œʂ�́@typeAns == 1 �������ƃv���C���[���ڂ��Ă��邩   typeAns == 2 �v���C���[�ƓG�̓����蔻��  typeAns == 3 �v���C���[�ƃS�[��
    {
        SetPositionScale_object_A(playerORenemy); // object_A  playerORenemy

        SetPositionScale_object_B(blockORenemy); // object_B  blockORenemy

        if(!allInfo_B.toreruyuka) //�ʂ�鏰�̏ꍇ���ꂵ�Ȃ�
        {
            // x�������蔻��
            if (obj_A_Position.x + (obj_A_Scale.x / 2) + justBlockRange > obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.x + (obj_A_Scale.x / 2) - justBlockRange < obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2)
                    && obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//�I�u�W�F�Փ�
                {
                    allInfo_A.migiWall = true;
                }
                else if(typeAns == 2)//�v���C���[�ƓG�Փ�
                {
                    gameMakerCode.gameOver = true;

                    //�G�ɓ�������
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//�v���C���[�ƉƏՓ�
                {
                    gameMakerCode.gameClear = true;
                }
            }

            if (obj_A_Position.x - (obj_A_Scale.x / 2) - justBlockRange < obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.x - (obj_A_Scale.x / 2) + justBlockRange > obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2)
                    && obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//�I�u�W�F�Փ�
                {
                    allInfo_A.hidariWall = true;
                }
                else if (typeAns == 2)//�v���C���[�ƓG�Փ�
                {
                    gameMakerCode.gameOver = true;

                    //�G�ɓ�������
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//�v���C���[�ƉƏՓ�
                {
                    gameMakerCode.gameClear = true;
                }

            }

            // z�������蔻��
            if (obj_A_Position.z + (obj_A_Scale.z / 2) + justBlockRange > obj_B_Position.z - (obj_B_Scale.z / 2)
                    && obj_A_Position.z + (obj_A_Scale.z / 2) - justBlockRange < obj_B_Position.z - (obj_B_Scale.z / 2)
                    && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//�I�u�W�F�Փ�
                {
                    allInfo_A.okuWall = true;
                }
                else if (typeAns == 2)//�v���C���[�ƓG�Փ�
                {
                    gameMakerCode.gameOver = true;

                    //�G�ɓ�������
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//�v���C���[�ƉƏՓ�
                {
                    gameMakerCode.gameClear = true;
                }

            }

            if (obj_A_Position.z - (obj_A_Scale.z / 2) - justBlockRange < obj_B_Position.z + (obj_B_Scale.z / 2)
                    && obj_A_Position.z - (obj_A_Scale.z / 2) + justBlockRange > obj_B_Position.z + (obj_B_Scale.z / 2)
                    && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//�I�u�W�F�Փ�
                {
                    allInfo_A.temaeWall = true;
                }
                else if (typeAns == 2)//�v���C���[�ƓG�Փ�
                {
                    gameMakerCode.gameOver = true;

                    //�G�ɓ�������
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//�v���C���[�ƉƏՓ�
                {
                    gameMakerCode.gameClear = true;
                }
            }


        }

        // y�������蔻��
        ChangeJustBlockRange();
        if(!allInfo_B.toreruyuka) //�ʂ�鏰�̏ꍇ���ꂵ�Ȃ�
        {
            if (obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2) - justBlockRange
            && obj_A_Position.y + (obj_A_Scale.y / 2) < obj_B_Position.y - (obj_B_Scale.y / 2) + justBlockRange
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
            && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2)
            && obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2))
            {
                if(typeAns == 0)//�I�u�W�F�Փ�
                {
                    allInfo_A.ueWall = true;
                }
                else if (typeAns == 2)//�v���C���[�ƓG�Փ�
                {
                    gameMakerCode.gameOver = true;

                    //�G�ɓ�������
                    dead_enemy_moji.SetActive(true);
                    //
                }
                else if (typeAns == 3)//�v���C���[�ƉƏՓ�
                {
                    gameMakerCode.gameClear = true;
                }

            }
        }
        if (obj_A_Position.y - (obj_A_Scale.y / 2) <= obj_B_Position.y + (obj_B_Scale.y / 2) + 0.01f
            && obj_A_Position.y - (obj_A_Scale.y / 2) > obj_B_Position.y + (obj_B_Scale.y / 2) - justBlockRange
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
            && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2)
            && obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2))
        {
            //�I�u�W�F�Փ�
            if (typeAns == 0
                && (obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y)
                && !(allInfo_A.hidariWall || allInfo_A.migiWall || allInfo_A.temaeWall || allInfo_A.okuWall))
            {
                allInfo_A.sitaWall = true;
            }

            // �������p�R�[�h
            else if (typeAns == 1)
            {
                UkiyukaMove ukiyukaMove = obj_B.GetComponent<UkiyukaMove>();
                ukiyukaMove.onPlayer = true;
            }
            else if (typeAns == 2)//�v���C���[�ƓG�Փ�
            {
                gameMakerCode.gameOver = true;

                //�G�ɓ�������
                dead_enemy_moji.SetActive(true);
                //
            }
            else if (typeAns == 3)//�v���C���[�ƉƏՓ�
            {
                gameMakerCode.gameClear = true;
            }

        }
        ResetJustBlockRane();

    }

    [SerializeField]
    private float addJustBlockRange;//�����p
    private void ChangeJustBlockRange()
    {
        //�v���C���[�p
        if(obj_A == player)
        {
            PlayerMove4 playerMove = obj_A.GetComponent<PlayerMove4>();

            if(!playerMove.onukiyuka) //�������ɂ���Ƃ��͎��s���Ȃ�
            {
                //�@�㏸���牺�~�ɂȂ������̉��Z�����Z�b�g����
                if (playerMove.canJump != playerMove.keepCanJump)
                {
                    playerMove.addJustBlockRange = 0;
                }

                float distance = playerMove.keepMove_y - obj_A_Position.y;
                if (distance < 0)
                {
                    distance *= -1;
                }
                playerMove.addJustBlockRange += distance;
                //�㉺�����Ɉړ����͓����蔻���傫������
                if (playerMove.canJump == 1)
                {
                    justBlockRange += playerMove.addJustBlockRange / 2;
                }
                else if ((playerMove.canJump == 2) || (playerMove.canJump == 0 && distance != 0))
                {
                    justBlockRange += playerMove.addJustBlockRange;
                }
                else
                {
                    playerMove.addJustBlockRange = 0;
                }
                playerMove.keepCanJump = playerMove.canJump;
                playerMove.keepMove_y = obj_A_Position.y;
            }
            
        }
        //�G�p
        else if(allInfo_A.enemy)
        {
            EnemyMove3 enemyMove = obj_A.GetComponent<EnemyMove3>();


            //�@�㏸���牺�~�ɂȂ������̉��Z�����Z�b�g����
            if (enemyMove.canJump != enemyMove.keepCanJump)
            {
                enemyMove.addJustBlockRange = 0;
            }

            float distance = enemyMove.keepMove_y - obj_A_Position.y;
            if (distance < 0)
            {
                distance *= -1;
            }
            enemyMove.addJustBlockRange += distance;

            //�㉺�����Ɉړ����͓����蔻���傫������
            if (enemyMove.canJump == 1)
            {
                justBlockRange += enemyMove.addJustBlockRange / 2;
            }
            else if (enemyMove.canJump == 2)
            {
                justBlockRange += enemyMove.addJustBlockRange;
            }
            else
            {
                enemyMove.addJustBlockRange = 0;
            }
            enemyMove.keepCanJump = enemyMove.canJump;
            enemyMove.keepMove_y = obj_A_Position.y;
        }
    }
    //�����l�ɖ߂�
    private void ResetJustBlockRane()
    {
        justBlockRange = keepJustBlockRange;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public void AtariHantei_tatumaki(GameObject playerORenemy, GameObject blockORenemy)
    {
        SetPositionScale_object_A(playerORenemy); // object_A  playerORenemy

        SetPositionScale_object_B(blockORenemy); // object_B  blockORenemy
        //XY�������蔻��
        if (obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2)
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2)
            && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2))
        {
            if (change2D3D == 1)//�PD���[�h
            {
                //�������Ȃ�
            }
            else if(change2D3D == 2)//�QD���[�h
            {
                allInfo_A.tatumakiHit = true;
            }
            //Z�������蔻��@�RD���[�h�p
            else if(obj_A_Position.z + (obj_A_Scale.z / 2) > obj_B_Position.z - (obj_B_Scale.z / 2)
                && obj_A_Position.z - (obj_A_Scale.z / 2) < obj_B_Position.z + (obj_B_Scale.z / 2))
            {
                if (change2D3D == 3)
                {
                    allInfo_A.tatumakiHit = true;
                }
            }
        }
    }
    //�����R�[�h
    public void Death_Press(GameObject playerORenemy, GameObject block)
    {
        SetPositionScale_object_A(playerORenemy); // object_A  playerORenemy

        SetPositionScale_object_B(block); // object_B  blockORenemy
        if(!allInfo_B.toreruyuka)
        {
            //XY�������蔻��
            if (obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2) - justBlockRange
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2) + justBlockRange
            && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2) - justBlockRange
            && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2) + justBlockRange)
            {
                //�v���C���[�p
                if(obj_A == player)
                {
                    gameMakerCode.gameOver = true;

                    Destroy(obj_A);
                    //��������
                    dead_press_moji.SetActive(true);
                    //
                }
                //�G�p
                else if (allInfo_A.enemy)
                {
                    Destroy(obj_A);
                }
            }
        }

    }

    public void AtariHantei2D(GameObject playerORenemy, GameObject blockORenemy, int typeAns) // typeAns == 0 ���܂Œʂ�́@typeAns == 1 �������ƃv���C���[���ڂ��Ă��邩�@typeAns == 2 �G�ɓ���������
    {
        SetPositionScale_object_A(playerORenemy); // object_A  playerORenemy

        SetPositionScale_object_B(blockORenemy); // object_B  blockORenemy
        if(!allInfo_B.toreruyuka)
        {
            // x�������蔻��
            if (obj_A_Position.x + (obj_A_Scale.x / 2) + justBlockRange > obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.x + (obj_A_Scale.x / 2) - justBlockRange < obj_B_Position.x - (obj_B_Scale.x / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//�I�u�W�F�Փ�
                {
                    allInfo_A.migiWall = true;
                }
                else if (typeAns == 2)//�v���C���[�ƓG�Փ�
                {
                    if(change2D3D == 2)
                    {
                        gameMakerCode.gameOver = true;
                        //�G�ɓ�������
                        dead_enemy_moji.SetActive(true);
                        //
                        //Debug.Log("�����F�G");
                    }
                }
                else if (typeAns == 3)//�v���C���[�ƉƏՓ�
                {
                    gameMakerCode.gameClear = true;
                }


            }
            
            if (obj_A_Position.x - (obj_A_Scale.x / 2) - justBlockRange < obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.x - (obj_A_Scale.x / 2) + justBlockRange > obj_B_Position.x + (obj_B_Scale.x / 2)
                    && obj_A_Position.y - (obj_A_Scale.y / 2) < obj_B_Position.y + (obj_B_Scale.y / 2)
                    && obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2))
            {
                if(typeAns == 0)//�I�u�W�F�Փ�
                {
                    allInfo_A.hidariWall = true;
                }
                else if (typeAns == 2)//�v���C���[�ƓG�Փ�
                {
                    if (change2D3D == 2)
                    {
                        gameMakerCode.gameOver = true;
                        //�G�ɓ�������
                        dead_enemy_moji.SetActive(true);
                        //
                        //Debug.Log("�����F�G");
                    }
                }
                else if (typeAns == 3)//�v���C���[�ƉƏՓ�
                {
                    gameMakerCode.gameClear = true;
                }

            }
        }

        
        // y�������蔻��
        ChangeJustBlockRange();
        if(!allInfo_B.toreruyuka)
        {
            if (obj_A_Position.y + (obj_A_Scale.y / 2) > obj_B_Position.y - (obj_B_Scale.y / 2) - justBlockRange
            && obj_A_Position.y + (obj_A_Scale.y / 2) < obj_B_Position.y - (obj_B_Scale.y / 2) + justBlockRange
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2))
            {
                if (typeAns == 0)//�I�u�W�F�Փ�
                {
                    allInfo_A.ueWall = true;
                }
                else if (typeAns == 2)//�v���C���[�ƓG�Փ�
                {
                    if (change2D3D == 2)
                    {
                        gameMakerCode.gameOver = true;
                        //�G�ɓ�������
                        dead_enemy_moji.SetActive(true);
                        //
                        //Debug.Log("�����F�G");
                    }
                }
                else if (typeAns == 3)//�v���C���[�ƉƏՓ�
                {
                    gameMakerCode.gameClear = true;
                }


            }
        }
        if (obj_A_Position.y - (obj_A_Scale.y / 2) <= obj_B_Position.y + (obj_B_Scale.y / 2) + (justBlockRange / 5) //�@0.01�����ƈ𒎂Ɨ����ŗ�����
            && obj_A_Position.y - (obj_A_Scale.y / 2) > obj_B_Position.y + (obj_B_Scale.y / 2) -  justBlockRange 
            && obj_A_Position.x - (obj_A_Scale.x / 2) < obj_B_Position.x + (obj_B_Scale.x / 2)
            && obj_A_Position.x + (obj_A_Scale.x / 2) > obj_B_Position.x - (obj_B_Scale.x / 2))
        {
            //�I�u�W�F�Փ�
            if (typeAns == 0
                && (obj_A_Position.y + ( obj_A_Scale.y / 2 ) > obj_B_Position.y)
                && !(allInfo_A.hidariWall || allInfo_A.migiWall || allInfo_A.temaeWall || allInfo_A.okuWall))
            {
                allInfo_A.sitaWall = true;
            }

            // �������p�R�[�h
            else if(typeAns == 1)
            {
                UkiyukaMove ukiyukaMove = obj_B.GetComponent<UkiyukaMove>();
                ukiyukaMove.onPlayer = true;

                allInfo_A.sitaWall = true;

                if(allInfo_A.player)
                {
                    PlayerMove4 pmove = obj_A.GetComponent<PlayerMove4>();
                    pmove.onukiyuka = true;
                }
                //Debug.Log("�͂���" + obj_B);
            }
            else if (typeAns == 2)//�v���C���[�ƓG�Փ�
            {
                if (change2D3D == 2)
                {
                    gameMakerCode.gameOver = true;
                    //�G�ɓ�������
                    dead_enemy_moji.SetActive(true);
                    //
                    //Debug.Log("�����F�G");
                }
            }
            else if (typeAns == 3)//�v���C���[�ƉƏՓ�
            {
                gameMakerCode.gameClear = true;
            }


        }
        ResetJustBlockRane();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    //�Փ˔������肽���I�u�W�F������
    private void SetPositionScale_object_A(GameObject allObject)
    {
        obj_A = allObject;

        allInfo_A = obj_A.GetComponent<ObjInfo>();
        obj_A_Position = allInfo_A.objPosition;
        obj_A_Scale = allInfo_A.objScale;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    //�Փ˔������肽���I�u�W�F������
    private void SetPositionScale_object_B(GameObject allObject)
    {
        obj_B = allObject;

        allInfo_B = obj_B.GetComponent<ObjInfo>();
        obj_B_Position = allInfo_B.objPosition;
        obj_B_Scale = allInfo_B.objScale;
    }
}
