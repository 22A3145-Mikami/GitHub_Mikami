using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    private Vector2 move2d;
    private Vector3 move3d;
    [SerializeField]
    private float speed;
    private Vector2 houkou2d;
    private Vector3 houkou3d;
    private float rotate;

    [SerializeField]
    private bool mode2d3d; // 2d = true   3d = false

    [SerializeField] // �m�F�p  0 = �W�����v�@�@�P�@���@�W�����v�����@�Q�@���@�~����
    private int canJump;
    private float timeJump;
    private float gravity;
    [SerializeField]
    private float startVec;

    [SerializeField]
    private float ground; //�@���̍���

    //[SerializeField]
    //private float zHozon;
    [SerializeField]
    private Quaternion playerRot;

    [SerializeField]
    private GameObject stage;
    [SerializeField]
    private AllCollision allColl;
    [SerializeField]
    private float stageColl;



    
    [SerializeField]
    private TikuwaList tList;

    private Environment environment;// �u���b�N�^�O��list�Ăяo��
    private AllCollision coll;// �u���b�N�^�Olist��AllColl
    [SerializeField]//�m�F
    private int iHozon;
    [SerializeField]//  �W�����v�����ۂ̃u���b�N���蔲���h�~
    private float jumpBlockRange;
    

    //[SerializeField]
    //private GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        houkou2d = new Vector2(1, 0);
        houkou3d = new Vector3(0, 0, 1);

        rotate = 0.1f;

        mode2d3d = true;

        canJump = 0;
        gravity = 9.8f;
        startVec = 5f;

        stage = GameObject.Find("Stage");
        allColl = stage.GetComponent<AllCollision>();

        ground = stage.transform.position.y + (stage.transform.localScale.y / 2);

        tList = GameObject.Find("TikuwaList").GetComponent<TikuwaList>();

        environment = GameObject.Find("SetStage").GetComponent<Environment>();
        iHozon = -1;
    }

    // Update is called once per frame
    void Update()
    {


        // -1 = �X�e�[�W��ɂȂ��@0 = �������X�e�[�W��艺�@1 = �������X�e�[�W����@2 = �X�e�[�W��ɂ���
        if(iHozon != -1)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            stageColl = coll.OnPlayer(1);
        }
        else
        {
            stageColl = allColl.OnPlayer(1);
        }
        //stageColl = allColl.OnPlayer(1);

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (mode2d3d == true)  // �@�RD�ɂȂ�
            {
                mode2d3d = false;

                transform.localRotation = playerRot;
                // transform.position = new Vector3(transform.position.x, transform.position.y, zHozon); ;
                //�@true����true���́@�����Ƃ��Ɂ@�A�j���[�V�����łǂ��ɂ��Ȃ�H

            }
            else   //  �QD�ɂȂ�
            {
                mode2d3d = true;

                //zHozon = transform.position.z;
                //playerRot = transform.localEulerAngles;
                playerRot = transform.localRotation;
                transform.localRotation = new Quaternion(0, 0, 0, 0);
                //transform.position = new Vector3(transform.position.x, transform.position.y, zHozon);
                //�@�����Ƃ��Ɂ@�A�j���[�V�����łǂ��ɂ��Ȃ�H

            }
        }

        if (mode2d3d == true)
        {
            Move2d();
            Jump2d();
        }
        else if (mode2d3d == false)
        {
            Move3d();
            Jump3d();
        }
    }

    void Move2d()
    {
        if (Input.GetKey(KeyCode.D))
        {
            houkou2d.x = speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            houkou2d.x = -speed;
        }
        else
        {
            houkou2d.x = 0;
        }
        houkou2d = transform.rotation * houkou2d;
        
        Vector3 ans = Vector3.one;
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            coll = environment.blocks[i].GetComponent<AllCollision>();
            //ans = coll.SideCollision();
            //ans.y = coll.OnPlayer(3);
            if(coll.Range(ans) > 0)
            {
                break;
            }
        }
        houkou2d = new Vector2(houkou2d.x * ans.x, houkou2d.y * ans.y);

        transform.position += new Vector3(houkou2d.x, houkou2d.y, 0);
    }

    void Jump2d()
    {
        if (canJump == 0)
        {
            //  true�̏�Ԃŗ������Ƃ��p
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                timeJump = 0;
                canJump = 2;
                startVec = 0;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (canJump == 0)
                {
                    timeJump = 0;
                    startVec = 10f;
                    canJump = 1;
                }
            }

        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            //playerPos (����������)
            Vector3 a = transform.position;
            a.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground;
            
            
            if (canJump == 1)
            {
                for(int i = 0; i< environment.blocks.Length; i++)
                {
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    float ans = coll.OnPlayer(3);
                    if(ans == 0)
                    {
                        //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                        Debug.Log("hit");
                        timeJump = 0;
                        startVec = 0;
                        a.y = 0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground;
                        break;
                    }
                }
            }
            
            //  �����@�����W���O�̃t���[�����[�ɂ�������
            if(a.y <= transform.position.y)
            {
                canJump = 2;
            }
            transform.position = a;

            // �X�e�[�W�i�u���b�N�j��ɂ���@�͂����@�v�Z�ŃX�e�[�W�i�u���b�N�j��艺�ɍs����������ꍇ
            if (transform.position.y - (transform.localScale.y / 2) <= ground && stageColl != -1)
            {

                //Debug.Log("aiaia");
                canJump = 0;
                //   �n�ʁi�u���b�N�j      �v���C���[�̂���
                a.y = ground + (transform.localScale.y / 2);
                //Debug.Log(a.y);
                transform.position = a;
            }
            
            // �~�����̂Ƃ��ɉ��Ƀu���b�N�����邩���ׂ�
            if( canJump == 2)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    float ans = coll.OnPlayer(2);
                    //Debug.Log(ans);
                    if ( ans != stage.transform.position.y + (stage.transform.localScale.y / 2) ) 
                    {
                        //Debug.Log(ans);
                        //Debug.Log(transform.position.y - (transform.localScale.y / 2));
                        //  �����{�[�@�ł͂񂢂Ƃ�
                        if( (transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange )
                            && (transform.position.y - (transform.localScale.y / 2 ) >= ans - jumpBlockRange) )
                        {
                            canJump = 0;
                            // �����@i�@��ۑ�����K�v����
                            // �u���b�N�ɂ���
                            iHozon = i;
                            ground = ans;
                        }
                        break;
                    }
                    else if (ans == stage.transform.position.y + (stage.transform.localScale.y / 2 ) 
                            && ( i == environment.blocks.Length - 1)
                            && (transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
                    {
                        ground = stage.transform.position.y + (stage.transform.localScale.y / 2);
                        // �X�e�[�W�ɂ���
                        iHozon = -1;
                    }
                }
            }
            
            /*
            for(int i = 0; i < environment.blocks.Length; i++)
            {
                AllCollision coll = environment.blocks[i].GetComponent<AllCollision>();
                float ans = coll.Block();
                if(ans != 0)
                {
                    ground = ans;
                    break;
                }
                else if (ans == 0 && i == environment.blocks.Length - 1)
                {
                    ground = stage.transform.position.y + (stage.transform.localScale.y / 2);
                }
            }
            */

            /*
            for (int i = 0; i < environment.blocks.Length; i++)
            {
                AllCollision coll = environment.blocks[i].GetComponent<AllCollision>();
                float ans = coll.Block();

                //  ����łO�łȂ�������@player�̉���block������
                if (ans != 0)
                {
                    Debug.Log("ans = " + ans);
                    // �����̑��
                    ground = ans;
                    break;
                }
                else if (ans == 0 && i == environment.blocks.Length - 1)
                {
                    Debug.Log("nannde");
                    ground = stage.transform.position.y + (stage.transform.localScale.y / 2);
                }

            }
            */
        }

    }

    void Move3d()
    {
        if (Input.GetKey(KeyCode.W))
        {
            houkou3d.z = speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            houkou3d.z = -speed;
        }
        else
        {
            houkou3d.z = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.localEulerAngles += new Vector3(0, -rotate, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.localEulerAngles += new Vector3(0, rotate, 0);
        }

        transform.position += transform.rotation * houkou3d;

    }

    void Jump3d()
    {
        if (canJump == 0)
        {
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                timeJump = 0;
                canJump = 2;
                startVec = 0;
            }
            if (Input.GetKey(KeyCode.Space))
            {

                //  �����Ɂ@�u���b�N�̏�ɏ���Ă��邩�̔���
                /*
                if()
                {

                }
                */
                if (canJump == 0)
                {
                    timeJump = 0;
                    startVec = 10f;
                    canJump = 1;
                }
            }

        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            Vector3 a = transform.position;
            a.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground;

            //  �����@�����W���O�̃t���[�����[�ɂ�������
            if (a.y <= transform.position.y)
            {
                canJump = 2;
            }

            transform.position = a;

            if (transform.position.y - (transform.localScale.y / 2) <= ground && stageColl != -1)
            {
                canJump = 0;
                a.y = ground + (transform.localScale.y / 2);
                transform.position = a;
            }
        }
    }
}
