using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove3 : MonoBehaviour
{
    //[SerializeField]
    //private GameObject player;
    [SerializeField]
    private ObjInfo objInfo; //�G�I�u�W�F���

    public Vector3 move; //�ړ�����
    [SerializeField]
    private float speed; //�ړ����x
    //�W�����v�ɂ���
    public int canJump; // 0�@�W�����v�\�@1�@�W�����v�s�@�㏸���@�Q�@�W�����v�s�@���~��
    //[SerializeField]
    public float startJump; //�ŏ��̃W�����v�̗�
    [SerializeField]
    private float startJump_zero; // �u���b�N���Ȃ����̗����p
    public float startJump_tatumaki;
    //[SerializeField]
    //private float startJump_playerself; // �v���C���[���g�ŃW�����v�������Ƃ��p
    //[SerializeField]
    public float timeJump; // �W�����v�̎���
    //[SerializeField]
    public float gravity; // �d��
    [SerializeField]
    private Vector3 jumpMaePosition; //�W�����v�O�̒n�ʂɂ����Ƃ��̍��W

    //���[�h�؂�ւ��̃R�[�h�ƕϐ�
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;

    //�u���b�N��o��
    public bool climb;
    //1�t���[���O�ɓo���Ă�����
    private bool maeClimb;
    //�o��u���b�N�̏��
    private int climbWall;

    //public bool pushPlayer;
    //public bool climbPlayer;

    [SerializeField]
    private CubeCollision cubeCollision; // �����蔻��

    [SerializeField]
    private int groundBlock; // ������n�ʂ̃u���b�N�̔ԍ��������

    /// CubeCollision�p�̕ϐ��@���蔲���h�~�p
    public float addJustBlockRange;//�����p�@justBlockRange
    public float keepMove_y; // �ЂƂO�̂����W
    public int keepCanJump; // �ЂƂO�̃W�����v���

    //�Q�[���S�̗̂���I�u�W�F�ƃR�[�h
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    //�v���C���[��o�邱�Ƃł��邩
    public bool climb_player;

    private void Awake()
    {
        //���[�h�؂�ւ��̎擾
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //�I�u�W�F���擾
        objInfo = GetComponent<ObjInfo>();

        //�����蔻��̎擾
        cubeCollision = GameObject.Find("JudgeCollision").GetComponent<CubeCollision>();

        //�W�����v�l�O�̕ۑ��p
        startJump_zero = 0;

        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;

        //�Q�[���S�̗̂���擾
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();

        //�v���C���[��o��邩
        climb_player = false;
    }

    //�RD�ύX����Z���W�ړ�
    private void Position3D()
    {
        //�n�ʃu���b�N�̏��擾
        ObjInfo blockInfo = cubeCollision.blocks[groundBlock].GetComponent<ObjInfo>();
        Vector3 nowPos = transform.position;
        if (!(nowPos.z - (objInfo.objScale.z / 2) < blockInfo.objPosition.z + (blockInfo.objScale.z / 2)
            && nowPos.z + (objInfo.objScale.z / 2) > blockInfo.objPosition.z - (blockInfo.objScale.z / 2)))
        {
            nowPos.z = cubeCollision.blocks[groundBlock].transform.position.z;
        }
        //�@�I�u�W�F�̈ړ�
        transform.position = nowPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�ꎞ���f���Ă��Ȃ��@���@�R�[�X�v���C��
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            // ���[�h�ؑ�
            if (change2D3D != change2D3DCode.change2D3D && change2D3DCode.change2D3D == 3) //2D����RD�ɕς������
            {
                Position3D();
            }
            else if (change2D3D == 3 && change2D3DCode.change2D3D != 3) //�QD�PD�ɂȂ����Ƃ��̈����p
            {
                for (int i = 0; i < cubeCollision.blocks.Count; i++)
                {
                    //�����R�[�h
                    cubeCollision.Death_Press(this.gameObject, cubeCollision.blocks[i]);
                }
            }

            change2D3D = change2D3DCode.change2D3D;

            //�ړ������̃��Z�b�g
            move = Vector3.zero;

            //���ړ�
            Walk_AD(-speed);


            //xz�����蔻��
            for (int i = 0; i < cubeCollision.blocks.Count; i++)
            {
                //�Փ˔�����O��
                objInfo.ResetHitWall();
                //3D���[�h�p
                if (change2D3D == 3)
                {
                    //�I�u�W�F�ƃu���b�N�ł̓����蔻��
                    cubeCollision.AtariHantei3D(this.gameObject, cubeCollision.blocks[i], 0);

                    //�E�ɓ���������ړ����Ȃ���
                    if (objInfo.migiWall && move.x >= 0)
                    {
                        move.x = 0;
                    }
                    //���ɓ���������ړ����Ȃ����@�u���b�N��o��
                    if (objInfo.hidariWall && move.x <= 0)
                    {

                        //�v���C���[���u���b�N�Ńv���C���[�ɓo���Ƃ�
                        if (cubeCollision.blocks[i].GetComponent<ObjInfo>().player
                            && climb_player)
                        {
                            move.x = 0;
                            //�o��ǃI�u�W�F�̎擾
                            climbWall = i;
                        }
                        //�v���C���[�łȂ��I�u�W�F�̂Ƃ�
                        else if (cubeCollision.blocks[i] != cubeCollision.player)
                        {
                            move.x = 0;
                            //�o��ǃI�u�W�F�̎擾
                            climbWall = i;
                        }
                    }
                    //���ɓ���������ړ����Ȃ���
                    if (objInfo.okuWall && move.z >= 0)
                    {
                        move.z = 0;
                    }
                    //��O�ɓ���������ړ����Ȃ���
                    if (objInfo.temaeWall && move.z <= 0)
                    {
                        move.z = 0;
                    }
                }
                //3D���[�h�ȊO
                else if (change2D3D != 3)
                {
                    //�u���b�N�Ƃ̓����蔻��
                    cubeCollision.AtariHantei2D(this.gameObject, cubeCollision.blocks[i], 0);
                    //�E�ɓ���������ړ����Ȃ���
                    if (objInfo.migiWall && move.x >= 0)
                    {
                        move.x = 0;
                    }
                    //���ɓ���������ړ����Ȃ����@�u���b�N��o��
                    if (objInfo.hidariWall && move.x <= 0)
                    {
                        //�v���C���[���u���b�N�Ńv���C���[�ɓo���Ƃ�
                        if (cubeCollision.blocks[i].GetComponent<ObjInfo>().player
                            && climb_player)
                        {
                            move.x = 0;
                            //�o��ǃI�u�W�F�̎擾
                            climbWall = i;
                        }
                        //�v���C���[�łȂ��I�u�W�F�̂Ƃ�
                        else if (cubeCollision.blocks[i] != cubeCollision.player)
                        {
                            move.x = 0;
                            //�o��ǃI�u�W�F�̎擾
                            climbWall = i;
                        }

                    }

                }
            }

            

            // �ǂ̂ڂ�
            if (move.x == 0)
            {
                //�ǃI�u�W�F�̏����擾
                ObjInfo blockInfo = cubeCollision.blocks[climbWall].GetComponent<ObjInfo>();
                //�O�t���[���ƍs�����ς��Ƃ�
                if (climb != maeClimb)
                {
                    //�傫���̕ύX
                    Vector3 changeS = objInfo.objScale;
                    objInfo.objScale.x = objInfo.objScale.y;
                    objInfo.objScale.y = changeS.x;
                    // obj�̌����ύX
                    transform.localEulerAngles = new Vector3(0, 0, -90);
                    Vector3 nowPos = transform.position;
                    nowPos.x = blockInfo.objPosition.x + (blockInfo.objScale.x / 2) + (objInfo.objScale.x / 2);

                    transform.position = nowPos;
                }

                Walk_Y(speed);
                maeClimb = climb;
                climb = true;
                timeJump = 0;
                canJump = 0;

                
            }
            else
            {
                //�O�t���[���ƍs�����ς��Ƃ�
                if (climb != maeClimb)
                {
                    //�傫���̕ύX
                    Vector3 changeS = objInfo.objScale;
                    objInfo.objScale.x = objInfo.objScale.y;
                    objInfo.objScale.y = changeS.x;
                    // obj�̌����ύX
                    transform.localEulerAngles = new Vector3(0, 0, 0);

                    ObjInfo blockInfo = cubeCollision.blocks[climbWall].GetComponent<ObjInfo>();
                    Vector3 nowPos = transform.position;
                    nowPos.x = blockInfo.objPosition.x + (blockInfo.objScale.x / 2) + (objInfo.objScale.x / 2) - 0.01f;

                    transform.position = nowPos;

                    climb_player = false;
                }
                maeClimb = climb;
                climb = false;
            }
            ////

            Jump();
            //�ړ�
            transform.position += move;
            

            //����Ă�Ƃ��͂��Ȃ��@�������邩
            if (!climb)
            {
                objInfo.ResetHitWall();
                // �������蔻��@�����Ȃ����̗����p
                for (int i = 0; i < cubeCollision.blocks.Count; i++)
                {
                    if (change2D3D == 3)
                    {
                        cubeCollision.AtariHantei3D(this.gameObject, cubeCollision.blocks[i], 0);
                    }
                    else if (change2D3D != 3)
                    {
                        cubeCollision.AtariHantei2D(this.gameObject, cubeCollision.blocks[i], 0);
                    }

                }
                //���ɑ������Ă锻��Ŏ��ۂɂ͂��Ă��Ȃ��Ƃ�
                if (canJump == 0)
                {
                    //�n�ʂɑ������Ă��Ȃ��Ƃ�
                    if (!objInfo.sitaWall)
                    {
                        //�@�����R�[�h
                        canJump = 2;
                        startJump = startJump_zero;
                        timeJump = 0;

                    }
                }
                //////////////
            }


            // y�����蔻��@�W�����v����̒��n�ƃu���b�N�����ɓ���������
            for (int i = 0; i < cubeCollision.blocks.Count; i++)
            {

                objInfo.ResetHitWall();

                // 3D�p
                if (change2D3D == 3)
                {
                    //�u���b�N�Ƃ̓����蔻��
                    cubeCollision.AtariHantei3D(this.gameObject, cubeCollision.blocks[i], 0);
                    //�n�ʂɑ������Ă�@���@�ړ���������
                    if (objInfo.sitaWall && move.y < 0)
                    {
                        //���n�R�[�h
                        Jump_Tyakuti(i);

                    }
                    //�������Ă�@���@�ړ���������
                    else if (objInfo.ueWall && move.y > 0)
                    {
                        move.y = 0;
                        //����������
                        Jump_AtamaHit(i);

                    }

                }
                // 2D�p
                else if (change2D3D != 3)
                {
                    //�u���b�N�Ƃ̓����蔻��
                    cubeCollision.AtariHantei2D(this.gameObject, cubeCollision.blocks[i], 0);
                    //�n�ʂɑ������Ă�@���@�ړ���������
                    if (objInfo.sitaWall && move.y < 0)
                    {
                        //���n�R�[�h
                        Jump_Tyakuti(i);

                    }
                    //�������Ă�@���@�ړ���������
                    else if (objInfo.ueWall && move.y > 0)
                    {
                        move.y = 0;
                        //����������
                        Jump_AtamaHit(i);

                    }


                }
                //���̂��Ă���n�ʂ��L��
                if (objInfo.sitaWall)
                {
                    groundBlock = i;
                }

            }

            //�ޗ��ɗ������Ƃ��G������
            if (transform.position.y <= -10)
            {
                Destroy(this.gameObject);
            }

        }

    }

    //���E�ړ�
    private void Walk_AD(float houkou)
    {

        move.x = houkou;
    }
    //�㉺�ړ�
    private void Walk_Y(float houkou)
    {
        move.y = houkou;
    }
    //���n�R�[�h
    private void Jump_Tyakuti(int blocks_Number)   //�W�����v�̒��n���̒n�ʂ̂��蔲����h�~���邽��
    {

        // �W�����v�̑J��
        canJump = 0;
        timeJump = 0;

        //�u���b�N���̎擾 �n�ʂɒ��n
        ObjInfo obj_B_Info = cubeCollision.blocks[blocks_Number].GetComponent<ObjInfo>();
        Vector3 obj_B_Position = obj_B_Info.objPosition;
        Vector3 obj_B_Scale = obj_B_Info.objScale;
        Vector3 nowPos = transform.position;
        nowPos.y = obj_B_Position.y + (obj_B_Scale.y / 2) + (objInfo.objScale.y / 2);
        transform.position = nowPos;

    }
    //������������
    private void Jump_AtamaHit(int blocks_Number)
    {
        // �W�����v�̑J�ځ@�W�����v�l�̃��Z�b�g
        canJump = 2;
        startJump = startJump_zero;
        timeJump = 0;

        //�u���b�N���̎擾 �I�u�W�F�N�g�̈ړ�
        ObjInfo obj_B_Info = cubeCollision.blocks[blocks_Number].GetComponent<ObjInfo>();
        Vector3 obj_B_Position = obj_B_Info.objPosition;
        Vector3 obj_B_Scale = obj_B_Info.objScale;
        Vector3 nowPos = transform.position;
        nowPos.y = obj_B_Position.y - (obj_B_Scale.y / 2) - (objInfo.objScale.y / 2);
        transform.position = nowPos;
    }

    private void Jump()
    {
        //��Ɉړ���
        if (canJump == 1)
        {
            //���������グ
            move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;
            //���Ԃ̉��Z
            timeJump = timeJump + Time.deltaTime;

            // �����@���̃t���[���̂����W�@���@���̂����W�@���Ⴍ�Ȃ�����
            if (objInfo.objPosition.y > objInfo.objPosition.y + move.y)
            {
                canJump = 2;
            }
        }
        //�W�����v���i���~�j
        else if (canJump == 2)
        {
            //���R����
            move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;
            //���Ԃ̉��Z
            timeJump = timeJump + Time.deltaTime;
        }

    }
    //���x�̐ݒ�
    public void SetSpeed(float change_speed)
    {
        speed = change_speed;
    }
}
