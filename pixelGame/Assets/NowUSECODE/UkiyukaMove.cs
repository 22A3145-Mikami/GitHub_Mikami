using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UkiyukaMove : MonoBehaviour
{
    //�����̏I�n�|�C���g�P
    [SerializeField]
    private Vector3 position1;
    //�����̏I�n�|�C���g�Q
    [SerializeField]
    private Vector3 position2;
    private bool from1to2; // �I�n�|�C���g�P����Q�ɂ�������true
    private bool maefrom1to2; //�ړ��������ς�����u��
    public Vector3 move; //���̎��̈ړ�����
    private Vector3 move_vec; //�ړ����Ă��Ȃ��Ƃ���move���O�ɂ��邽��

    public float speed = 0.5f; //�ړ��̑���
    private float range; // 1����Q�܂ł̑傫��R

    public bool onPlayer; // �v���C���[���������
    private float onPlayerTime; //�v���C���[������Ă��瓮���܂ł̌o�ߎ���
    private float moveStartTime; // �v���C���[������Ă��瓮���n�߂�܂ł̎���

    [SerializeField]
    private bool moveStart; //�ړ��̊J�n

    [SerializeField]
    private bool move_keep; //�����Ɠ���

    //���[�h�؂�ւ��̃R�[�h�ƕϐ�
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;

    //�v���C���[�I�u�W�F
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private CubeCollision cubeCollision; // �����蔻��

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

        
        //��������
        //position1 = new Vector3(0, 0, 0);
        //position2 = new Vector3(10, 5, 10);
        //transform.position = position1;
        //position1 = transform.position;
        /*
        move = position2 - position1;
        range = Seikika(move);
        move = move / range;

        Debug.Log(move);
        */

        //�Q�b�������瓮��
        moveStartTime = 2f;

        //�����ݒ�
        from1to2 = true;
        maefrom1to2 = true;
        
        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;

        //�Q�[���S�̗̂���擾
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
    }

    //StageSet�R�[�h�Ŏg���p�֐�
    public void SetPosition(Vector3 position_1, Vector3 position_2)
    {
        //�I�n�|�C���g�̐ݒ�
        SetPosition1(position_1);
        SetPosition2(position_2);

        //�ړ������̌v�Z
        move = position2 - position1;
        //�傫���𐳋K������
        range = Seikika(move);
        move = move / range;
        
        move_vec = move; //move�̕ۑ�

        //Debug.Log(move);
    }
    public void SetPosition1(Vector3 pos)
    {
        position1 = pos;
    }
    public void SetPosition2(Vector3 pos)
    {
        position2 = pos;
    }

    // ���������邩�ݒ�
    public void Move_Keeper(bool select)
    {
        move_keep = select;
    }
    //�����̐ݒ�
    public void SetSpeed(float accelerator)
    {
        speed = accelerator;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�ꎞ���f���Ă��Ȃ��@���@�R�[�X�v���C��
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            //�v���C���[�擾
            player = GameObject.FindGameObjectWithTag("Player");

            ///�v���C���[������Ă��邩����
            onPlayer = false;
            if (change2D3D == 3)
            {
                cubeCollision.AtariHantei3D(player, this.gameObject, 1);
            }
            else if (change2D3D != 3)
            {
                cubeCollision.AtariHantei2D(player, this.gameObject, 1);
            }
            ///
            
            //�v���C���[������Ă��鎞�Ԃ��Q�b�������瓮����
            if (onPlayer)
            {
                onPlayerTime += Time.deltaTime;

                if (onPlayerTime >= moveStartTime)
                {
                    moveStart = true;
                }
            }//�����O�ɓr���ŗ��ꂽ�烊�Z�b�g
            else
            {
                onPlayerTime = 0;
            }

            //�ړ��J�n���邩���������邩���肪true�������瓮����
            if (moveStart || move_keep)
            {
                move = move_vec;
                //�ړ��J�n
                MoveStart();
                //Debug.Log("ugo");
            }//���������邪false�̂Ƃ��ŏ���ĂȂ������瓮�����Ȃ�
            else if (!moveStart)
            {
                move = Vector3.zero;
            }
        }
        
    }

    private void MoveStart()
    {
        if(from1to2)
        {
            //�����̔��]
            if(from1to2 != maefrom1to2)
            {
                speed *= -1;
                maefrom1to2 = from1to2;
            }
            //�ړ�
            transform.position += move * speed;

            //�ړ��������I�n�|�C���g�Ԃ̑傫�����傫���Ȃ�������s����
            Vector3 judge = transform.position - position1;
            if(Seikika(judge) > range)
            {
                transform.position = position2;

                onPlayerTime = 0;
                moveStart = false;
                from1to2 = false;
            }
        }
        else if(!from1to2) // 2����P�ɖ߂�@�@�@�@��R�[�h�Ɠ�������
        {
            if(from1to2 != maefrom1to2)
            {
                speed *= -1;
                maefrom1to2 = from1to2;
            }

            transform.position += move * speed;

            Vector3 judge = transform.position - position2;
            if (Seikika(judge) > range)
            {
                transform.position = position1;

                onPlayerTime = 0;
                moveStart = false;
                from1to2 = true;
            }
        }
        
    }

    //���K������
    private float Seikika(Vector3 kyori)
    {
        for (int i = 0; i < 3; i++)
        {
            kyori[i] = kyori[i] * kyori[i];
        }
        float ans = Mathf.Sqrt(kyori.x + kyori.y + kyori.z);
        return ans;
    }
}
