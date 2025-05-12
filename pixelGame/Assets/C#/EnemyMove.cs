using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //[SerializeField]
    public Vector3 move;

    [SerializeField]
    private int canJump;
    public float timeJump;
    [SerializeField]
    private float gravity;
    public Vector3 ground;

    private GameObject stage;

    private AllCollision allCollision;
    private AllCollision allColl; // �X�e�[�W�p

    private Environment environment;

    [SerializeField]
    private float stageColl;
    private int iHozon;

    private float jumpBlockRange;

    private float hitBlock;

    private float startVec;

    [SerializeField]
    private TikuwaList tList;

    [SerializeField]
    private int moveChange;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        allCollision = GetComponent<AllCollision>();

        environment = GameObject.Find("SetStage").GetComponent<Environment>();

        tList = GameObject.Find("TikuwaList").GetComponent<TikuwaList>();

        gravity = 9.8f;
        timeJump = 0;

        stage = GameObject.Find("Stage");
        allColl = stage.GetComponent<AllCollision>();

        /*
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            allCollision = environment.blocks[i].GetComponent<AllCollision>();
            float ans = allCollision.OnEnemy(1, this.gameObject);
            if (ans == 2)
            {
                ground.y = environment.blocks[i].transform.position.y;
                stageColl = allCollision.OnEnemy(1, this.gameObject);
                iHozon = i;
                break;
            }
            else if (i == environment.blocks.Length - 1
                && allCollision.OnEnemy(1, this.gameObject) != 2)
            {
                ground.y = stage.transform.position.y;
                stageColl = allColl.OnEnemy(1, this.gameObject);
                iHozon = -1;

                //hitBlock = transform.position.y;
            }
        }
        */
    }

    [SerializeField]
    float speed;
    void Move()
    {
        speed = -0.1f;
        move = new Vector3(transform.position.x + speed, ground.y, 0);
        transform.position = move;
    }

    //  ��������W�����v���邱�Ƃ͂Ȃ�
    void Jump()
    {
        if (canJump == 0)
        {
            timeJump = 0;
            startVec = 5;
            canJump = 1;

        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;

            Vector3 pPos = transform.position;
            pPos.y = startVec * timeJump - 0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground.y + hitBlock;

            // ���S��
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.candownHit)
                    {
                        float ans = allCollision.OnEnemy(3, this.gameObject);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //�@�@�@�@���������u���b�N�@�u���b�N�X�P�[�� / 2    �v���C���[�X�P�[���@/ 2   �d������ground
                            hitBlock = allCollision.objPos.y - (allCollision.objScale.y / 2) - (transform.localScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground.y + hitBlock;
                            break;
                        }
                    }
                }
            }
        }

        /*
        if (canJump == 0)
        {
            //  true�̏�Ԃŗ������Ƃ��p
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                int iKeep = 0;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.OnEnemy(1, this.gameObject) == 2)
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
        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            //playerPos (����������)
            Vector3 pPos = transform.position;
            pPos.y = ground.y + hitBlock;

            // ���S��
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.candownHit)
                    {
                        float ans = allCollision.OnEnemy(3, this.gameObject);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //�@�@�@�@���������u���b�N�@�u���b�N�X�P�[�� / 2    �v���C���[�X�P�[���@/ 2   �d������ground
                            hitBlock = allCollision.objPos.y - (allCollision.objScale.y / 2) - (transform.localScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground.y + hitBlock;
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
            if (pPos.y - (transform.localScale.y / 2) <= ground.y
                && stageColl != -1)
            {
                //Debug.Log("���邩");
                if (iHozon == -1
                    && ground.y == allColl.objPos.y + (allColl.objScale.y / 2)
                    && !allColl.sideHitBlockEnemy)
                {
                    //Debug.Log("���邩���邩");
                    if (pPos.y - (transform.localScale.y / 2) >= allColl.objPos.y + (allColl.objScale.y / 2) - (jumpBlockRange * 2))
                    {
                        //Debug.Log("���邩���邩���邩");
                        canJump = 0;
                        Debug.Log("�背�|");
                        //   �n�ʁi�u���b�N�j      �v���C���[�̂���
                        pPos.y = ground.y + (transform.localScale.y / 2);

                        hitBlock = 0;
                        //allColl.sideHitBlockEnemy = false;
                    }

                    else
                    {
                        Debug.Log("�����W�������艺");
                    }

                }
                else if (iHozon != -1)
                {
                    //Debug.Log("���邩���邩���邩���邩");
                    allCollision = environment.blocks[iHozon].GetComponent<AllCollision>();
                    if (ground.y == allCollision.objPos.y + (allCollision.objScale.y / 2)
                        && !allCollision.sideHitBlockEnemy)
                    {
                        //Debug.Log("���邩���邩���邩���邩���邩");
                        if (pPos.y - (transform.localScale.y / 2) >= allCollision.objPos.y + (allCollision.objScale.y / 2) - (jumpBlockRange * 2)
                            && pPos.y - (transform.localScale.y / 2) <= allCollision.objPos.y + (allCollision.objScale.y / 2) + jumpBlockRange)
                        {
                            //Debug.Log("���邩���邩���邩���邩���邩���邩");
                            canJump = 0;
                            Debug.Log("�背�|");
                            //   �n�ʁi�u���b�N�j      �v���C���[�̂���
                            pPos.y = ground.y + (transform.localScale.y / 2);

                            hitBlock = 0;
                            //coll.sideHitBlockEnemy = false;
                        }
                    }
                }
            }
            for (int i = 0; i < environment.blocks.Length; i++)
            {
                allCollision = environment.blocks[i].GetComponent<AllCollision>();
                allCollision.sideHitBlockEnemy = false;
            }
            allColl.sideHitBlockEnemy = false;


            transform.position = pPos;


            // �~�����̂Ƃ��ɉ��Ƀu���b�N�����邩���ׂ�
            if (canJump == 2)
            {
                //��x�u���b�N���݂����true
                bool search = false;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    // �����Ă�u���b�N��Onplayer(2)�p
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    float ans = allCollision.OnEnemy(2, this.gameObject);

                    // �u���b�N�̍��W���Ԃ��ꂽ
                    if (ans == allCollision.objPos.y + (allCollision.objScale.y / 2))
                    {
                        //Debug.Log(ans);
                        //Debug.Log(transform.position.y - (transform.localScale.y / 2));
                        //  �����{�[�@�ł͂񂢂Ƃ�
                        if ((transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
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
                    else if (ans == allCollision.objPos.y + (allCollision.objScale.y / 2) - 1
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {
                        Debug.Log("�����I");
                        ans = allColl.OnEnemy(2, this.gameObject);
                        if ((transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
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
                        if (transform.position.y - (transform.localScale.y / 2) <= ans)
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




        /*
        //transform.position += move;

        if (allCollision.OnPlayer(4) == 1)
        {
            Debug.Log("�G�ɓ���������");
        }
        */
    }

    void DownMove()
    {
        speed = -0.1f;
        move = new Vector3(0, speed, 0);
        transform.position += move;
        Debug.Log("��������");
    }

    void UpMove()
    {
        speed = 0.1f;
        move = new Vector3(0, speed, 0);
        transform.position += move;

        Debug.Log("���");
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            allCollision = environment.blocks[i].GetComponent<AllCollision>();
            float ans = allCollision.OnEnemy(1, this.gameObject);
            if (ans == 2)
            {
                ground.y = environment.blocks[i].transform.position.y;
                stageColl = allCollision.OnEnemy(1, this.gameObject);
                iHozon = i;

                moveChange = 0;
                break;
            }
            else if (i == environment.blocks.Length - 1
                && allCollision.OnEnemy(1, this.gameObject) != 2)
            {
                ans = allColl.OnEnemy(1, this.gameObject);
                if (ans == 2)
                {
                    ground.y = stage.transform.position.y;
                }
                else if (ans == 1)
                {
                    // �W�����v���Ă��邩
                    if (canJump == 1)
                    {
                        moveChange = 1;
                    }
                    else if (canJump == 0)
                    {
                        moveChange = 2;
                    }
                    else
                    {
                        moveChange = -1;
                    }
                }
            }
        }
        for (int i = 0; i < environment.blocks.Length; i++)
        {
            allCollision = environment.blocks[i].GetComponent<AllCollision>();
            Vector3 ans = allCollision.SideCollisionEnemy(move, this.gameObject);
            if (ans.x == 0 && moveChange == 0)
            {
                /*
                ground.y = environment.blocks[i].transform.position.y;
                stageColl = allCollision.OnEnemy(1, this.gameObject);
                iHozon = i;
                */
                // ���
                if (moveChange == 0 || moveChange == 1)
                {
                    moveChange = 3;
                    break;
                }
                else
                {
                    Debug.Log("�G���[");
                }
            }
        }

        //  �������̈ړ�
        if (moveChange == 0)
        {
            Move();
        }
        //  �����̂���
        else if (moveChange == 1)
        {
            Jump();
        }
        //�@�؂���~���
        else if (moveChange == 2)
        {
            DownMove();
        }
        //  �؂ɓo��
        else if (moveChange == 3)
        {
            UpMove();
        }























        /*
        if (canJump == 0)
        {
            //  true�̏�Ԃŗ������Ƃ��p
            if (stageColl != 2) // transform.position.y - (transform.localScale.y / 2) > ground
            {
                int iKeep = 0;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.OnEnemy(1, this.gameObject) == 2)
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
        }
        else if (canJump != 0)
        {
            timeJump += Time.deltaTime;
            //playerPos (����������)
            Vector3 pPos = transform.position;
            pPos.y = ground.y + hitBlock;

            // ���S��
            if (canJump == 1)
            {
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    if (allCollision.candownHit)
                    {
                        float ans = allCollision.OnEnemy(3, this.gameObject);
                        if (ans == 0)
                        {
                            //ground = environment.blocks[i].transform.position.y + environment.blocks[i].transform.localScale
                            Debug.Log("hit");
                            timeJump = 0;
                            startVec = 0;
                            //�@�@�@�@���������u���b�N�@�u���b�N�X�P�[�� / 2    �v���C���[�X�P�[���@/ 2   �d������ground
                            hitBlock = allCollision.objPos.y - (allCollision.objScale.y / 2) - (transform.localScale.y) - ground.y;
                            pPos.y = -0.5f * gravity * timeJump * timeJump + (transform.localScale.y / 2) + ground.y + hitBlock;
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
            if (pPos.y - (transform.localScale.y / 2) <= ground.y
                && stageColl != -1)
            {
                //Debug.Log("���邩");
                if (iHozon == -1
                    && ground.y == allColl.objPos.y + (allColl.objScale.y / 2)
                    && !allColl.sideHitBlockEnemy)
                {
                    //Debug.Log("���邩���邩");
                    if (pPos.y - (transform.localScale.y / 2) >= allColl.objPos.y + (allColl.objScale.y / 2) - (jumpBlockRange * 2))
                    {
                        //Debug.Log("���邩���邩���邩");
                        canJump = 0;
                        Debug.Log("�背�|");
                        //   �n�ʁi�u���b�N�j      �v���C���[�̂���
                        pPos.y = ground.y + (transform.localScale.y / 2);

                        hitBlock = 0;
                        //allColl.sideHitBlockEnemy = false;
                    }

                    else
                    {
                        Debug.Log("�����W�������艺");
                    }

                }
                else if (iHozon != -1)
                {
                    //Debug.Log("���邩���邩���邩���邩");
                    allCollision = environment.blocks[iHozon].GetComponent<AllCollision>();
                    if (ground.y == allCollision.objPos.y + (allCollision.objScale.y / 2)
                        && !allCollision.sideHitBlockEnemy)
                    {
                        //Debug.Log("���邩���邩���邩���邩���邩");
                        if (pPos.y - (transform.localScale.y / 2) >= allCollision.objPos.y + (allCollision.objScale.y / 2) - (jumpBlockRange * 2)
                            && pPos.y - (transform.localScale.y / 2) <= allCollision.objPos.y + (allCollision.objScale.y / 2) + jumpBlockRange)
                        {
                            //Debug.Log("���邩���邩���邩���邩���邩���邩");
                            canJump = 0;
                            Debug.Log("�背�|");
                            //   �n�ʁi�u���b�N�j      �v���C���[�̂���
                            pPos.y = ground.y + (transform.localScale.y / 2);

                            hitBlock = 0;
                            //coll.sideHitBlockEnemy = false;
                        }
                    }
                }
            }
            for (int i = 0; i < environment.blocks.Length; i++)
            {
                allCollision = environment.blocks[i].GetComponent<AllCollision>();
                allCollision.sideHitBlockEnemy = false;
            }
            allColl.sideHitBlockEnemy = false;


            transform.position = pPos;


            // �~�����̂Ƃ��ɉ��Ƀu���b�N�����邩���ׂ�
            if (canJump == 2)
            {
                //��x�u���b�N���݂����true
                bool search = false;
                for (int i = 0; i < environment.blocks.Length; i++)
                {
                    // �����Ă�u���b�N��Onplayer(2)�p
                    allCollision = environment.blocks[i].GetComponent<AllCollision>();
                    float ans = allCollision.OnEnemy(2, this.gameObject);

                    // �u���b�N�̍��W���Ԃ��ꂽ
                    if (ans == allCollision.objPos.y + (allCollision.objScale.y / 2))
                    {
                        //Debug.Log(ans);
                        //Debug.Log(transform.position.y - (transform.localScale.y / 2));
                        //  �����{�[�@�ł͂񂢂Ƃ�
                        if ((transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
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
                    else if (ans == allCollision.objPos.y + (allCollision.objScale.y / 2) - 1
                            && (i == environment.blocks.Length - 1)
                            && !search)
                    {
                        Debug.Log("�����I");
                        ans = allColl.OnEnemy(2, this.gameObject);
                        if ((transform.position.y - (transform.localScale.y / 2) <= ans + jumpBlockRange)
                            && (transform.position.y - (transform.localScale.y / 2) >= ans - jumpBlockRange))
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
                        if (transform.position.y - (transform.localScale.y / 2) <= ans)
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





        //transform.position += move;

        if (allCollision.OnPlayer(4) == 1)
        {
            Debug.Log("�G�ɓ���������");
        }

    }

        */
        /*
        // Update is called once per frame
        void Update()
        {
            for(int i = 0; i < environment.blocks.Length; i++)
            {
                allCollision = environment.blocks[i].GetComponent<AllCollision>();
                if (allCollision.OnEnemy(1, this.gameObject) == 1)
                {

                }
            }
            transform.position += move;

            if(allCollision.OnPlayer(4) == 1)
            {
                Debug.Log("�G�ɓ���������");
            }
        }
        */
    }
}
