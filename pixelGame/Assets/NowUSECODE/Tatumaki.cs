using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tatumaki : MonoBehaviour
{
    //�v���C���[�I�u�W�F
    [SerializeField]
    private GameObject player;

    //�����蔻��R�[�h
    private CubeCollision cubeCollision;

    //private GameObject[] enemys;

    //���[�h�؂�ւ��̃R�[�h�ƕϐ�
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;

    //�v���C���[�G�����Ԃ��߂̂�����
    [SerializeField]
    private float startJump_tatumaki;

    //�Q�[���S�̗̂���I�u�W�F�ƃR�[�h
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    private void Awake()
    {
        //���[�h�؂�ւ��̎擾
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //�����蔻��̎擾
        cubeCollision = GameObject.Find("JudgeCollision").GetComponent<CubeCollision>();

        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;
        
        //�Q�[���S�̗̂���擾
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�ꎞ���f���Ă��Ȃ��@���@�R�[�X�v���C��
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            //�v���C���[�擾
            player = GameObject.FindGameObjectWithTag("Player");

            //�RD���[�h�̂Ƃ�
            if (change2D3D == 3)
            {
                //// �v���C���[�ɂ���
                ///�����蔻��
                cubeCollision.AtariHantei_tatumaki(player, this.gameObject);
                ///�v���C���[�̃I�u�W�F���擾���ē������Ă�����͂�������
                ObjInfo objInfo = player.GetComponent<ObjInfo>();
                if (objInfo.tatumakiHit)
                {
                    TatumakiJump_player();
                }
                else if (!objInfo.tatumakiHit)
                {
                    ResetStartJump_tatumaki_player();
                }

                //// �G�ɂ���
                ///�v���C���[�Ɠ���
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
            }//�RD���[�h�ȊO�@�RD���[�h�Ɠ��l
            else if (change2D3D != 3)
            {
                //// �v���C���[�ɂ���
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

                //// �G�ɂ���
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
    private float addJump;// 2���֐��ŗ͗ʂ𑝉�
    public void SetAddJump(float add)
    {
        addJump = add;
    }
    //�v���C���[�p
    public void TatumakiJump_player()
    {
        //�v���C���[�ړ��R�[�h�̎擾
        PlayerMove4 playerMove = player.GetComponent<PlayerMove4>();
        //�v���C���[�̃W�����v�o�ߎ��Ԃ����Z�b�g
        playerMove.timeJump = 0;
        //�v���C���[�W�����v�l�̕ύX
        playerMove.startJump = playerMove.startJump_tatumaki;
        //�l���R�𒴂���܂ŉ��Z
        if(playerMove.startJump_tatumaki <= 3)
        {
            playerMove.startJump_tatumaki += addJump;
        }

        ///���R�����̌���
        Vector3 move = Vector3.zero;
        move.y = playerMove.startJump * playerMove.timeJump - 0.5f * playerMove.gravity * playerMove.timeJump * playerMove.timeJump;
        
        //�v���C���[��y���̈ړ�
        player.transform.position += move;

        //�o�ߎ���
        playerMove.timeJump = playerMove.timeJump + Time.deltaTime;

        //������Ɉړ����ɐݒ�
        playerMove.canJump = 1;

        //�A�j���[�V����
        playerMove.playerANIM.Jump();

    }
    //�v���C���[�p
    private void ResetStartJump_tatumaki_player()
    {
        //�v���C���[�ړ��R�[�h�̎擾
        PlayerMove4 playerMove = player.GetComponent<PlayerMove4>();
        
        //�����p�W�����v�l�̃��Z�b�g
        playerMove.startJump_tatumaki = 1;
    }

    //�G�p�@�v���C���[�Ɠ���
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
    //�G�p�@�v���C���[�Ɠ���
    private void ResetStartJump_tatumaki_enemy(int enemy_Number)
    {
        EnemyMove3 enemyMove = cubeCollision.enemys[enemy_Number].GetComponent<EnemyMove3>();

        enemyMove.startJump_tatumaki = 0;
    }

}
