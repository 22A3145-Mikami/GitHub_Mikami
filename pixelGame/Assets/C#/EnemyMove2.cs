using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove2 : MonoBehaviour
{
    //�S��
    private Vector3 move;
    //2D�̈ړ�
    private Vector2 move2d;
    //3D�̈ړ�
    private Vector3 move3d;

    //2D�̈ړ��̒P�ʃx�N�g��
    private Vector2 houkou2d;
    //3D�̈ړ��̒P�ʃx�N�g��
    private Vector3 houkou3d;

    //[SerializeField]
    public float speed;
    //enemy�̉�]
    private float rotate;

    //[SerializeField]
    public bool mode2d3d; //  true �̂Ƃ��RD�ɂȂ�  false�̂Ƃ��QD�ɂȂ�

    //[SerializeField] // �m�F�p  0 = �W�����v�@�@�P�@���@�W�����v�����@�Q�@���@�~����
    public int canJump;
    //�@�W�����v�̌o�ߎ���
    public float timeJump;
    private float gravity;
    //[SerializeField]
    public float startVec;

    //[SerializeField]
    public Vector3 ground; //�@���̍���
    //[SerializeField]
    public float hitBlock; // �W�����v���Ă��������u���b�N�̉��ʁ@�[�@�v���C���[��localScale / 2 - �d������ground


    //[SerializeField]
    //private float zHozon;
    [SerializeField]
    private Quaternion enemyRot;

    [SerializeField]
    private GameObject stage;
    [SerializeField]
    private AllCollision allColl;
    //[SerializeField]
    public float stageColl;

    public Vector3 enemyScale;

    [SerializeField]
    private TikuwaList tList;

    private Environment environment;// �u���b�N�^�O��list�Ăяo��
    private AllCollision coll;// �u���b�N�^�Olist��AllColl
    [SerializeField]//�m�F
    private int iHozon;
    [SerializeField]//  �W�����v�����ۂ̃u���b�N���蔲���h�~
    private float jumpBlockRange;

    [SerializeField]
    bool clim; //���t���[��
    [SerializeField]
    bool maeClim; // ��O�̃t���[��
    [SerializeField]
    int climCo;

    //[SerializeField]
    //private GameObject[] blocks;

    // Start is called before the first frame update
    void Awake()
    {
        houkou2d = new Vector2(1, 0);
        houkou3d = new Vector3(0, 0, 1);

        rotate = 0.1f;

        mode2d3d = true;

        canJump = 0;
        gravity = 9.8f;
        startVec = 7f;

        stage = GameObject.Find("Stage");
        allColl = stage.GetComponent<AllCollision>();
        //allColl.sideHitBlock = true;

        ground.y = stage.transform.position.y + (stage.transform.localScale.y / 2);

        tList = GameObject.Find("TikuwaList").GetComponent<TikuwaList>();

        environment = GameObject.Find("SetStage").GetComponent<Environment>();
        iHozon = -1;

        Application.targetFrameRate = 60;

        clim = false;
        maeClim = false;


    }

    private void FixedUpdate()
    {

        //ground.y = environment.blocks[iHozon].transform.position.y + (coll.objScale.y / 2);
        /*
        if(clim)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            ground.y = environment.blocks[iHozon].transform.position.y + (coll.objScale.y / 2);
            stageColl = coll.OnEnemy(1, this.gameObject);
        }
        */
        /*
        else if(climCo > 0)
        {
            climCo++;
            if(climCo > 5)
            {
                clim = false;
                maeClim = false;
                climCo = 0;
            }
        }
        */

        if (iHozon != -1)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            stageColl = coll.OnEnemy(1, this.gameObject);
        }
        else
        {
            stageColl = allColl.OnEnemy(1, this.gameObject);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (mode2d3d == true)  // �@�RD�ɂȂ�
            {
                mode2d3d = false;

                transform.localRotation = enemyRot;
                // transform.position = new Vector3(transform.position.x, transform.position.y, zHozon); ;
                //�@true����true���́@�����Ƃ��Ɂ@�A�j���[�V�����łǂ��ɂ��Ȃ�H


            }
            else   //  �QD�ɂȂ�
            {
                mode2d3d = true;

                //zHozon = transform.position.z;
                //playerRot = transform.localEulerAngles;
                enemyRot = transform.localRotation;
                transform.localRotation = new Quaternion(0, 0, 0, 0);
                //transform.position = new Vector3(transform.position.x, transform.position.y, zHozon);
                //�@�����Ƃ��Ɂ@�A�j���[�V�����łǂ��ɂ��Ȃ�H

            }
        }

        if (mode2d3d == true)
        {
            Move2d();
            if(!clim)
            {
                Jump2d3d();
            }
            
        }
        else if (mode2d3d == false)
        {
            Move3d();
            if (!clim)
            {
                Jump2d3d();
            }
        }

        if(!clim && maeClim)
        {
            coll = environment.blocks[iHozon].GetComponent<AllCollision>();
            if ((transform.position.y - (enemyScale.y / 2) >= coll.objPos.y + (coll.objScale.y / 2)
            || (transform.position.y + (enemyScale.y / 2) <= coll.objPos.y - (coll.objScale.y / 2))))
            {
                //maeClim = false;
                ground.y = environment.blocks[iHozon].transform.position.y + (coll.objScale.y / 2);
                Vector3 ePos = transform.position;
                ePos.y = ground.y + (enemyScale.y / 2);
                transform.position = ePos;
                Debug.Log(iHozon + "desu");
                stageColl = coll.OnEnemy(1, this.gameObject);
                if(stageColl == 2)
                {
                    Debug.Log("imaha" + stageColl);
                }
                else if(stageColl == 1)
                {
                    Debug.Log("imaha" + stageColl);
                }
                else
                {
                    Debug.Log("imaha" + stageColl);
                }
                //Debug.Log("imaha" + stageColl);
                
                Jump2d3d();
            }
        }
    }

    void Move2d()
    {
        houkou2d.x = -speed;
        houkou2d.y = 0;

        maeClim = clim;
        clim = false;

        //houkou2d = transform.rotation * houkou2d;

        Vector3 ans = Vector3.one;
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            /*
            // �X�e�[�W����
            if (i == environment.blocks.Length)
            {
                ans = allColl.SideCollisionEnemy(new Vector3(houkou2d.x, houkou2d.y, 0), this.gameObject);
                if (allColl.Range(ans) < Mathf.Sqrt(3))
                {
                    Debug.Log("�͂�����");
                    allColl.sideHitBlock = true;
                    //Debug.Log(ans);
                    break;
                }
            }
            */
            // �����Ă�u���b�N����
            //else
            //{
                coll = environment.blocks[i].GetComponent<AllCollision>();
                if (coll.canSideHit)
                {
                    //  0 ���@�R���ł� houkou3d.z
                    ans = coll.SideCollisionEnemy(new Vector3(houkou2d.x, houkou2d.y, 0), this.gameObject);
                    /*
                    if (coll.Range(ans) < Mathf.Sqrt(3)) // �����ς���K�v����
                    {
                        Debug.Log("�͂�����2");
                        coll.sideHitBlock = true;

                        //houkou2d.x = 0;
                        //houkou2d.y = speed;

                        
                        clim = true;
                        iHozon = i;
                        //Debug.Log(ans);
                        break;
                    }
                    */
                    if(ans.x == 0)
                    {
                        Debug.Log("�͂�����2");
                        coll.sideHitBlock = true;

                        //houkou2d.x = 0;
                        //houkou2d.y = speed;


                        //clim = true;
                        iHozon = i;
                        //Debug.Log(ans);
                        break;
                    }
                }
            //}
            //ans.y = coll.OnPlayer(3);

        }

        houkou2d = new Vector2(houkou2d.x * ans.x, houkou2d.y * ans.y);
        if(houkou2d.x == 0)
        {
            clim = true;
            houkou2d.y = speed;
            //clim = true;
        }
        //Debug.Log(houkou2d);
        //  0 ���@�R���ł� houkou3d.z
        transform.position += new Vector3(houkou2d.x, houkou2d.y, 0);
    }

    void Jump2d3d()
    {
        if (canJump == 0)
        {
            //  true�̏�Ԃŗ������Ƃ��p
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                int iKeep = 0;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    if (coll.OnEnemy(1, this.gameObject) == 2)
                    {
                        iHozon = i;
                        //Debug.Log("����Ō��܂�I�F" + iHozon);
                        iKeep++;

                    }
                    else if (i == environment.blocks.Length - 1)
                    {
                        timeJump = 0;
                        canJump = 2;
                        startVec = 0;
                    }
                }

            }


            //  ����
            for (int i = 0; i < tList.tatumakiObjList.Length; i++)
            {
                coll = tList.tatumakiObjList[i].GetComponent<AllCollision>();
                if (coll.OnEnemy(4, this.gameObject) == 1)
                {
                    Debug.Log("omaeka");
                    timeJump = 0;
                    startVec = 10f;
                    canJump = 1;

                    break;
                }
            }
            ///

        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            //playerPos (����������)
            Vector3 pPos = transform.position;
            pPos.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (enemyScale.y / 2) + ground.y + hitBlock;

            // ���S��
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    if (coll.candownHit)
                    {
                        float ans = coll.OnEnemy(3, this.gameObject);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //�@�@�@�@���������u���b�N�@�u���b�N�X�P�[�� / 2    �v���C���[�X�P�[���@/ 2   �d������ground
                            hitBlock = coll.objPos.y - (coll.objScale.y / 2) - (enemyScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (enemyScale.y / 2) + ground.y + hitBlock;
                            break;
                        }
                    }
                }
            }
            //

            //  �����@�����W���O�̃t���[�����[�ɂ�������
            if (pPos.y <= transform.position.y)
            {
                canJump = 2;
                jumpBlockRange = transform.position.y - pPos.y;
            }

            // �X�e�[�W�i�u���b�N�j��ɂ���@�͂����@�v�Z�ŃX�e�[�W�i�u���b�N�j��艺�ɍs����������ꍇ
            // �X�V���������W �[ �X�P�[��/�Q
            if (pPos.y - (enemyScale.y / 2) <= ground.y
                && stageColl != -1)
            {
                //Debug.Log("���邩");
                if (iHozon == -1
                    && ground.y == allColl.objPos.y + (allColl.objScale.y / 2)
                    && !allColl.sideHitBlock)
                {
                    //Debug.Log("���邩���邩");
                    if (pPos.y - (enemyScale.y / 2) >= allColl.objPos.y + (allColl.objScale.y / 2) - (jumpBlockRange * 2))
                    {
                        //Debug.Log("���邩���邩���邩");
                        canJump = 0;
                        Debug.Log("�背�|");
                        //   �n�ʁi�u���b�N�j      �v���C���[�̂���
                        pPos.y = ground.y + (enemyScale.y / 2);

                        hitBlock = 0;
                        //allColl.sideHitBlock = false;

                    }
                    /*
                    else
                    {
                        Debug.Log("�����W�������艺");
                    }
                    */
                }
                else if (iHozon != -1)
                {
                    //Debug.Log("���邩���邩���邩���邩");
                    coll = environment.blocks[iHozon].GetComponent<AllCollision>();
                    if (ground.y == coll.objPos.y + (coll.objScale.y / 2)
                        && !coll.sideHitBlock)
                    {
                        //Debug.Log("���邩���邩���邩���邩���邩");
                        if (pPos.y - (enemyScale.y / 2) >= coll.objPos.y + (coll.objScale.y / 2) - (jumpBlockRange * 2)
                            && pPos.y - (enemyScale.y / 2) <= coll.objPos.y + (coll.objScale.y / 2) + jumpBlockRange)
                        {
                            //Debug.Log("���邩���邩���邩���邩���邩���邩");
                            canJump = 0;
                            Debug.Log("�背�|");
                            //   �n�ʁi�u���b�N�j      �v���C���[�̂���
                            pPos.y = ground.y + (enemyScale.y / 2);

                            hitBlock = 0;
                            //coll.sideHitBlock = false;
                        }
                    }
                }
            }
            for (int i = 0; i < environment.blocks.Length; i++)
            {
                coll = environment.blocks[i].GetComponent<AllCollision>();
                coll.sideHitBlock = false;
            }
            allColl.sideHitBlock = false;


            transform.position = pPos;


            // �~�����̂Ƃ��ɉ��Ƀu���b�N�����邩���ׂ�
            if (canJump == 2)
            {
                //��x�u���b�N���݂����true
                bool search = false;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    // �����Ă�u���b�N��Onplayer(2)�p
                    coll = environment.blocks[i].GetComponent<AllCollision>();
                    float ans = coll.OnEnemy(2, this.gameObject);

                    // �u���b�N�̍��W���Ԃ��ꂽ
                    if (ans == coll.objPos.y + (coll.objScale.y / 2))
                    {
                        //Debug.Log(ans);
                        //Debug.Log(transform.position.y - (transform.localScale.y / 2));
                        //  �����{�[�@�ł͂񂢂Ƃ�
                        if ((transform.position.y - (enemyScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (enemyScale.y / 2) >= ans - jumpBlockRange))
                        {
                            if (search)
                            {
                                // �����@i�@��ۑ�����K�v���� ���@������
                                // �u���b�N�ɂ���
                                Debug.Log(iHozon);
                                iHozon = i;
                                ground.y = ans;

                                canJump = 0;
                                hitBlock = 0;

                                Debug.Log(i);
                                Debug.Log("���������H");

                                // jumpBlockRange�̃��Z�b�g
                                jumpBlockRange = 0.1f;
                            }
                            else
                            {
                                // �����@i�@��ۑ�����K�v���� ���@������
                                // �u���b�N�ɂ���
                                iHozon = i;
                                ground.y = ans;

                                canJump = 0;
                                hitBlock = 0;
                                search = true;

                                // jumpBlockRange�̃��Z�b�g
                                jumpBlockRange = 0.1f;
                            }
                        }
                        continue;
                    }

                    // �X�e�[�W�̍��W���Ƃ낤�Ƃ��Ă�@�u���b�N��������Ȃ������@�����{�[�@�ł͂񂢂Ƃ�
                    else if (ans == coll.objPos.y + (coll.objScale.y / 2) - 1
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {
                        Debug.Log("�����I");
                        ans = allColl.OnEnemy(2, this.gameObject);
                        if ((transform.position.y - (enemyScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (enemyScale.y / 2) >= ans - jumpBlockRange))
                        {
                            ground.y = stage.transform.position.y + (stage.transform.localScale.y / 2);
                            // �X�e�[�W�ɂ���
                            iHozon = -1;

                            canJump = 0;
                            hitBlock = 0;
                            Debug.Log("�����I����");

                            // jumpBlockRange�̃��Z�b�g
                            jumpBlockRange = 0.1f;
                        }
                    }

                    // �X�e�[�W�O�i�������j
                    else if (ans == -10
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {

                        // �v���C���[��ans�ȉ�
                        if (transform.position.y - (enemyScale.y / 2) <= ans)
                        {
                            if (allColl.OnEnemy(2, this.gameObject) == -10)
                            {
                                Debug.Log("GAMEOVER");
                            }
                        }
                    }
                }
            }
        }
    }


    void Move3d()
    {
        //houkou3d.z = -speed;

        houkou3d.x = -speed;
        houkou3d.y = 0;

        houkou3d = transform.rotation * houkou3d;

        Vector3 ans = Vector3.one;
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            if (i == environment.blocks.Length)
            {
                ans = allColl.SideCollisionEnemy(new Vector3(houkou3d.x, houkou3d.y, houkou3d.z), this.gameObject);
                if (allColl.Range(ans) < Mathf.Sqrt(3))
                {
                    Debug.Log("�͂�����");
                    allColl.sideHitBlock = true;
                    //Debug.Log(ans);
                    break;
                }
            }
            else
            {
                coll = environment.blocks[i].GetComponent<AllCollision>();
                //  0 ���@�R���ł� houkou3d.z
                ans = coll.SideCollisionEnemy(new Vector3(houkou3d.x, houkou3d.y, houkou3d.z), this.gameObject);
                if (coll.Range(ans) < Mathf.Sqrt(3))
                {
                    Debug.Log("�͂�����");
                    coll.sideHitBlock = true;

                    houkou3d.x = 0;
                    houkou3d.y = speed;

                    maeClim = clim;
                    clim = true;

                    Debug.Log(ans);
                    break;
                }
            }

        }
        houkou3d = new Vector3(houkou3d.x * ans.x, houkou3d.y * ans.y, houkou3d.z * ans.z);
        //Debug.Log(houkou2d);
        //  0 ���@�R���ł� houkou3d.z
        transform.position += new Vector3(houkou3d.x, houkou3d.y, houkou3d.z);

    }
}
