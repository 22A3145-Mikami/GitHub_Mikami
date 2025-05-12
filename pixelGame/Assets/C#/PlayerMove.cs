using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
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

    [SerializeField] // �m�F�p
    private bool canJump;
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
    private int stageColl;

    [SerializeField]
    private TikuwaList tList;

    private Environment environment;

    //[SerializeField]
    //private GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        houkou2d = new Vector2(1, 0);
        houkou3d = new Vector3(0, 0, 1);

        rotate = 0.1f;

        mode2d3d = true;

        canJump = true;
        gravity = 9.8f;
        startVec = 10f;

        stage = GameObject.Find("Stage");
        allColl = stage.GetComponent<AllCollision>();

        ground = stage.transform.position.y + (stage.transform.localScale.y / 2);

        tList = GameObject.Find("TikuwaList").GetComponent<TikuwaList>();

        environment = GameObject.Find("SetStage").GetComponent<Environment>();

    }

    // Update is called once per frame
    void Update()
    {
        /*

        // -1 = �X�e�[�W��ɂȂ��@0 = �������X�e�[�W��艺�@1 = �������X�e�[�W����@2 = �X�e�[�W��ɂ���
        stageColl = allColl.OnPlayer();

        if(Input.GetKeyUp(KeyCode.Q))
        {
            if(mode2d3d == true)  // �@�RD�ɂȂ�
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

        if(mode2d3d == true)
        {
            Move2d();
            Jump2d();
        }
        else if(mode2d3d == false)
        {
            Move3d();
            Jump3d();
        }
    }

    void Move2d()
    {
        if(Input.GetKey(KeyCode.D))
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
        transform.position += new Vector3(houkou2d.x, houkou2d.y, 0);
    }

    void Jump2d()
    {
        if(canJump == true)
        {
            //  true�̏�Ԃŗ������Ƃ��p
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                timeJump = 0;
                canJump = false;
                startVec = 0;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (canJump == true)
                {
                    timeJump = 0;
                    startVec = 10f;
                    canJump = false;
                }
            }
            
        }
        else if(canJump == false)
        {
            timeJump += Time.deltaTime;
            //playerPos (����������)
            Vector3 a = transform.position;
            a.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground;
            transform.position = a;

            // �X�e�[�W�i�u���b�N�j��ɂ���@�͂����@�v�Z�ŃX�e�[�W�i�u���b�N�j��艺�ɍs����������ꍇ
            if (transform.position.y - (transform.localScale.y / 2) <= ground && stageColl != -1)
            {

                //Debug.Log("aiaia");
                canJump = true;
                //   �n�ʁi�u���b�N�j      �v���C���[�̂���
                a.y = ground + (transform.localScale.y / 2);
                Debug.Log(a.y);
                transform.position = a;
            }

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
        if (canJump == true)
        {
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                timeJump = 0;
                canJump = false;
                startVec = 0;
            }
            if (Input.GetKey(KeyCode.Space))
            {

                //  �����Ɂ@�u���b�N�̏�ɏ���Ă��邩�̔���
               
                if()
                {

                }
                
                if (canJump == true)
                {
                    timeJump = 0;
                    startVec = 10f;
                    canJump = false;
                }
            }

        }
        else if (canJump == false)
        {
            timeJump += Time.deltaTime;
            Vector3 a = transform.position;
            a.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2);
            transform.position = a;

            if (transform.position.y - (transform.localScale.y / 2) <= ground && stageColl != -1)
            {
                canJump = true;
                a.y = ground + (transform.localScale.y / 2);
                transform.position = a;
            }
        }
        */
    }
}
