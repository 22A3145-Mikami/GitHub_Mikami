using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInfo : MonoBehaviour
{
    //[SerializeField]
    //�R�[�h�����Ă�I�u�W�F�́@���W�@��]�@�X�P�[��
    public GameObject obj;
    public Vector3 objPosition; 
    public Vector3 objRotate;
    public Vector3 objScale;

    //���[�h�؂�ւ��̃R�[�h�ƕϐ�
    [SerializeField]
    private GameObject eventChange2D3D;
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;

    public GameObject obj3D; //�RD���[�h�p�I�u�W�F

    private int count2D; // �����G�̐�
    private int cooltimeChangePicture; // �����G�J�ڂ̃N�[���^�C��
    [SerializeField]
    private int changeTime; //�����G�̑J�ڂ̊Ԋu
    private int nowPicture; //�@�����G�̑J��
    public List<GameObject> obj2D;//�QD���[�h�p�I�u�W�F
    public List<GameObject> obj2D_Anotherplayer; // �����G�̑J�ڃv���C���[�̕ʕ�����

    public GameObject obj1D; // 1D���[�h�̃I�u�W�F

    public bool player; //���̃I�u�W�F�N�g��player�ł��邩
    public bool block; //���̃I�u�W�F�N�g��block�ł��邩
    public bool stage; //���̃I�u�W�F�N�g��stage�ł��邩�@�i�W�����v���n���Ƀe���|���Ȃ��悤�Ɂj
    public bool enemy;  //���̃I�u�W�F�N�g��enemy�ł��邩
    public bool toreruyuka; //�@���̃u���b�N���ʂ�鏰�ł��邩
    public bool ukiyuka;  //���̃u���b�N���������ł��邩    

    public bool hidariWall; //�v���C���[�̂����@�i�G�j
    public bool migiWall; //�v���C���[�̔w���@�i�G�j
    public bool okuWall; //�v���C���[�̉��@�i�G�j
    public bool temaeWall; //�v���C���[�̎�O�@�i�G�j
    public bool ueWall; //�v���C���[�̓��@�i�G�j
    public bool sitaWall; //�v���C���[�̑��@�i�G�j

    public bool tatumakiHit; // ���ꂪ���܂��ɂ���������

    //�Q�[���S�̗̂���I�u�W�F�ƃR�[�h
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    private void Awake()
    {
        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;

        //�����ݒ�
        count2D = obj2D.Count;
        cooltimeChangePicture = changeTime;
        nowPicture = 0;

        //���[�h�؂�ւ��̎擾
        eventChange2D3D = GameObject.Find("EventChange2D3D");
        change2D3DCode = eventChange2D3D.GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //�S���[�h��\��
        obj3D.SetActive(false);
        obj1D.SetActive(false);
        for(int i = 0; i < count2D; i++)
        {
            obj2D[i].SetActive(false);
            if(player)
            {
                obj2D_Anotherplayer[i].SetActive(false);
            }
        }

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
            //�����W�̐ݒ�
            objPosition = transform.position;
            
            //���[�h�̐؂�ւ��ƃI�u�W�F�̕\���ύX
            change2D3D = change2D3DCode.change2D3D;
            if (change2D3D == 3)
            {
                //3D�ȊO���\��
                obj1D.SetActive(false);
                for (int i = 0; i < count2D; i++)
                {
                    obj2D[i].SetActive(false);
                    if (player)
                    {
                        obj2D_Anotherplayer[i].SetActive(false);
                    }
                }
                obj3D.SetActive(true);

            }
            else if (change2D3D == 2)
            {
                //2D�ȊO���\��
                obj1D.SetActive(false);
                obj3D.SetActive(false);

                ChangePicture();

            }
            else if(change2D3D == 1)
            {
                //1D�ȊO���\��
                for (int i = 0; i < count2D; i++)
                {
                    obj2D[i].SetActive(false);
                    if (player)
                    {
                        obj2D_Anotherplayer[i].SetActive(false);
                    }
                }
                obj3D.SetActive(false);
                obj1D.SetActive(true);
            }
        }

    }

    public int changePlayerPNG;  //�@1 �����Ă���@ 0�@�~�܂��Ă���   2 �W�����v���Ă�
    private Vector3 keepPlayer_move; //�ړ������̎擾�p�ϐ�
    //�QD���[�h�̕\���I�u�W�F�̐؂�ւ�
    public void ChangePicture()
    {
        //�v���C���[�p
        if(player)
        {
            //�v���C���[�ړ��R�[�h�̎擾
            PlayerMove4 playerMove = GetComponent<PlayerMove4>();
            //�����Ă���Ƃ�
            if (changePlayerPNG == 1)
            {
                //�N�[���^�C���������l�ɂȂ�����
                if (cooltimeChangePicture == changeTime)
                {
                    //�����G�����̂��̂ɐ؂�ւ�
                    nowPicture++;

                    obj2D[nowPicture - 1].SetActive(false);
                    obj2D_Anotherplayer[nowPicture - 1].SetActive(false);

                    if (nowPicture == count2D)
                    {
                        nowPicture = 0;
                    }
                    //�؂�ւ����Ԃ����Z�b�g
                    cooltimeChangePicture = 0;
                    
                    //�E�����Ɉړ����Ă���Ƃ�
                    if (playerMove.move.x > 0)
                    {
                        obj2D[nowPicture].SetActive(true);
                        //���̈ړ��������L��
                        keepPlayer_move = playerMove.move;
                    }
                    //�������Ɉړ����Ă���Ƃ�
                    else if (playerMove.move.x < 0)
                    {
                        obj2D_Anotherplayer[nowPicture].SetActive(true);
                        //���̈ړ��������L��
                        keepPlayer_move = playerMove.move;
                    }

                }
                cooltimeChangePicture++;

                //�@�N�[���^�C�����ɍ��E���]�����Ƃ��p
                if(playerMove.move.x > 0 && keepPlayer_move.x < 0)
                {
                    obj2D_Anotherplayer[nowPicture].SetActive(false);

                    obj2D[nowPicture].SetActive(true);
                    //���̈ړ��������L��
                    keepPlayer_move = playerMove.move;
                }
                else if (playerMove.move.x < 0 && keepPlayer_move.x > 0)
                {
                    obj2D[nowPicture].SetActive(false);

                    obj2D_Anotherplayer[nowPicture].SetActive(true);
                    //���̈ړ��������L��
                    keepPlayer_move = playerMove.move;
                }


            }
            //�W�����v���Ă���Ƃ�
            else if(changePlayerPNG == 2)
            {
                obj2D[nowPicture].SetActive(false);
                obj2D_Anotherplayer[nowPicture].SetActive(false);
                
                //�W�����v���͗����G���Œ�
                nowPicture = 1;

                //�؂�ւ����Ԃ����Z�b�g
                cooltimeChangePicture = 0;
                //�E�ړ��@�܂��́@�ړ����ĂȂ��@���@�E�ړ�
                if (playerMove.move.x > 0 || (playerMove.move.x == 0 && keepPlayer_move.x > 0))
                {
                    obj2D[nowPicture].SetActive(true);
           
                }
                //���ړ��@�܂��́@�ړ����ĂȂ��@���@���ړ�
                else if (playerMove.move.x < 0 || (playerMove.move.x == 0 && keepPlayer_move.x < 0))
                {
                    obj2D_Anotherplayer[nowPicture].SetActive(true);

                }
                //���E�ړ����Ă�Ƃ��Ɉړ���ۑ�
                if (playerMove.move.x != 0)
                {
                    keepPlayer_move = playerMove.move;
                }
            }
            //�~�܂��Ă���Ƃ�
            else if (changePlayerPNG == 0)
            {
                obj2D[nowPicture].SetActive(false);
                obj2D_Anotherplayer[nowPicture].SetActive(false);
                //��~���͗����G���Œ�
                nowPicture = 0;
                // �؂�ւ����Ԃ����Z�b�g
                cooltimeChangePicture = 0;

                //�ۑ������ړ����E�ړ�
                if (keepPlayer_move.x > 0)
                {
                    obj2D[nowPicture].SetActive(true);
                }
                //�ۑ������ړ������ړ�
                else if (keepPlayer_move.x < 0)
                {
                    obj2D_Anotherplayer[nowPicture].SetActive(true);
                }
                //�ړ����ƕۑ������ړ�����~��������
                else if(keepPlayer_move.x == 0 && playerMove.move.x == 0)
                {
                    obj2D_Anotherplayer[nowPicture].SetActive(false);
                    obj2D[nowPicture].SetActive(true);
                }
            }
        }
        //�v���C���[�ȊO
        else if (!player)
        {
            //�N�[���^�C���������l�ɂȂ�����
            if (cooltimeChangePicture == changeTime)
            {
                nowPicture++;

                obj2D[nowPicture - 1].SetActive(false);

                if (nowPicture == count2D)
                {
                    nowPicture = 0;
                }
                //�؂�ւ����Ԃ����Z�b�g
                cooltimeChangePicture = 0;


            }
            cooltimeChangePicture++;

            obj2D[nowPicture].SetActive(true);
        }
        
        
    }

    //�Փ˔��������
    public void ResetHitWall()
    {
        hidariWall= false;
        migiWall= false;
        okuWall= false;
        temaeWall= false;
        ueWall= false;
        sitaWall= false;

        tatumakiHit = false;

    }
}
