using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private PlayerMove3 playerMove;
    //[SerializeField]
    //private GameObject obj;

    private Vector3 playerPos;
    //[SerializeField]
    public Vector3 objPos;

    private Vector3 playerScale;
    [SerializeField]
    public Vector3 objScale;

    //[SerializeField]
    //private GameObject enemy;
    [SerializeField]
    private EnemyMove2 enemyMove;
    private Vector3 enemyPos;
    private Vector3 enemyScale;

    //[SerializeField]
    //private PlayerMove playerMove;
    private float blockPos;

    [SerializeField]
    private GameObject stage;

    private Vector3 stagePos;
    private Vector3 stageScale;

    /////// �O�����///////
    // �v���C���[���I�u�W�F�̉��ʂɓ��S������̂�
    public bool candownHit;
    // �v���C���[�����ʂɓ����邱�Ƃ��o����I�u�W�F��
    public bool canSideHit;
    // �G���I�u�W�F�̉��ʂɓ��S������̂�
    public bool candownHitEnemy;
    // �G�����ʂɓ����邱�Ƃ��o����I�u�W�F��
    public bool canSideHitEnemy;
    /////////
    
    // �v���C���[����ɂ̂��Ă��邩
    public bool onBlock;
    // �v���C���[�����ʂɓ������Ă��邩
    public bool sideHitBlock;
    // �G����ɂ̂��Ă��邩
    public bool onBlockEnemy;
    // �G�����ʂɓ������Ă��邩
    public bool sideHitBlockEnemy;

    [SerializeField]
    private float range;


    private bool maeFrame;
    

    //[SerializeField]
    //private Vector3 obj2dModePos;
    //[SerializeField]
    public float obj3dModePosZ;
    
    /*
    [SerializeField]
    private Vector3 Wpos;
    [SerializeField]
    private Vector3 Lpos;
    */

    // Start is called before the first frame update
    void Start()
    {
        stage = GameObject.Find("Stage");
        stagePos = stage.transform.position;
        stageScale = stage.transform.localScale;

        Application.targetFrameRate = 60;
    }
    private void Awake()
    {
        player = GameObject.Find("player");
        playerMove = player.GetComponent<PlayerMove3>();

        stage = GameObject.Find("Stage");
        stagePos = stage.transform.position;
        stageScale = stage.transform.localScale;

        maeFrame = player.GetComponent<PlayerMove3>().mode2d3d;

        Application.targetFrameRate = 60;

        range = 0.1f;
    }

    private void FixedUpdate()
    {
        sideHitBlock = false;
        sideHitBlockEnemy = false;

        SetPos();
        if (maeFrame != player.GetComponent<PlayerMove3>().mode2d3d)
        {
            if (player.GetComponent<PlayerMove3>().mode2d3d)
            {
                Mode2d();
            }
            else
            {
                Mode3d();
            }
            maeFrame = player.GetComponent<PlayerMove3>().mode2d3d;
        }

        //Wpos = transform.position;
        //Lpos = transform.localPosition;
    }

    /*
    // Update is called once per frame
    void Update()
    {
        SetPos();
        if(maeFrame != player.GetComponent<PlayerMove3>().mode2d3d)
        {
            if (player.GetComponent<PlayerMove3>().mode2d3d)
            {
                Mode2d();
            }
            else
            {
                Mode3d();
            }
            maeFrame = player.GetComponent<PlayerMove3>().mode2d3d;
        }
        
        //Wpos = transform.position;
        //Lpos = transform.localPosition;
    }
    */
    // 3d�@����@2D�@���[�h�ɂȂ����Ƃ�
    public void Mode2d()
    {
        playerPos = player.transform.position;
        // ���Ƃ��͕ς��Ȃ��@���s������player�̍��W�ɏo�Ă���
        transform.position = new Vector3(objPos.x, objPos.y, playerPos.z);
        /*
        // 3D�@���[�h��player������Ă�����
        if(onBlock)
        {
            playerPos = player.transform.position;
            //playerPos.z += obj2dModePos.z;
            //player.transform.position = playerPos; 
        }
        */
    }

    // 2d�@����@3D�@���[�h�ɂȂ����Ƃ�
    public void Mode3d()
    {
        // ���Ƃ��͕ς��Ȃ��@���s�������ω�����
        transform.position = new Vector3(objPos.x, objPos.y, obj3dModePosZ);
        // �QD�@���[�h��player������Ă�����
        if (onBlock)
        {
            playerPos = player.transform.position;
            playerPos.z = obj3dModePosZ;
            player.transform.position = playerPos;
        }
    }


    //  changeMode = 1 : Onplayer   changeMode = 2 : Block  changeMode = 3 ; ������̂��蔲���h�~
    public float OnEnemy(float changeMode, GameObject e)
    {
        SetPosEnemy(e);
        // stage��xz�͈͂ɂ��邩
        if ((enemyPos.x + (enemyScale.x / 2) >= objPos.x - (objScale.x / 2))
            && (enemyPos.x - (enemyScale.x / 2) <= objPos.x + (objScale.x / 2))
            && (enemyPos.z + (enemyScale.z / 2) >= objPos.z - (objScale.z / 2))
            && (enemyPos.z - (enemyScale.z / 2) <= objPos.z + (objScale.z / 2)))
        {
            if (changeMode == 1)
            {
                // �X�e�[�W����
                if (enemyPos.y - (enemyScale.y / 2) > objPos.y + (objScale.y / 2))
                {
                    onBlockEnemy = false;
                    return 1.0f;
                }
                // �X�e�[�W��
                else if (enemyPos.y - (enemyScale.y / 2) >= objPos.y + (objScale.y / 2))
                {
                    onBlockEnemy = true;
                    return 2.0f;
                }
                // �X�e�[�W��艺
                else
                {
                    onBlockEnemy = false;
                    return 0;
                }
            }
            //
            else if (changeMode == 2)
            {
                // xz�͈͂ŕ����Ă�u���b�N�����@�G�̉��ɂ���
                if (enemyPos.y - (enemyScale.y / 2) >= objPos.y + (objScale.y / 2))
                {
                    return objPos.y + (objScale.y / 2);
                }
                //  �G�̏�ɂ���
                else
                {
                    return objPos.y + (objScale.y / 2) - 1;
                }
            }
            // �����u���b�N�ɂԂ�����
            else if (changeMode == 3)
            {
                // �G�̓����I�u�W�F�̒ꂪ��@�@�i0.05�͗V�ђl�j
                if (enemyPos.y + (enemyScale.y / 2) < objPos.y - (objScale.y / 2) + range
                    && (enemyPos.y + (enemyScale.y / 2) > objPos.y - (objScale.y / 2) - range))
                {
                    Debug.Log("����������");
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //
            else if (changeMode == 4)
            {
                //  �G�̓���肤���ɂ���   �G�̑���艺�ɂ���
                if (enemyPos.y + (enemyScale.y / 2) < objPos.y - (objScale.y / 2)
                    || enemyPos.y - (enemyScale.y / 2) > objPos.y + (objScale.y / 2))
                {
                    // �����ł͉������Ȃ�
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //
            else
            {
                Debug.Log("�G���[1");
                return 100;
            }
        }
        else
        {
            //  �X�e�[�W�O
            if (changeMode == 1)
            {
                return -1.0f;
            }
            //
            else if (changeMode == 2)
            {
                //Debug.Log("�G���[�H");
                // �����Ă�u���b�N����X�e�[�W�ɒ��n���鎞�@�@�z��ɃX�e�[�W�����΂����H
                if (this.gameObject != stage)
                {
                    //Debug.Log("����͌����Ă�H");
                    //AllCollision allCollision = stage.GetComponent<AllCollision>();
                    //return allCollision.OnEnemy(2, e);

                    return objPos.y + (objScale.y / 2) - 1;
                }
                else
                {
                    Debug.Log("�G���[�H");
                    return -10;
                }
            }
            //
            else if (changeMode == 3)
            {
                return 1;
            }
            //
            else if (changeMode == 4)
            {
                return 2;
            }
            //
            else
            {
                Debug.Log("�G���[2");
                return 100;
            }
        }
    }
    private void SetPosEnemy(GameObject e)
    {
        enemyPos = e.transform.position;
        enemyMove = e.GetComponent<EnemyMove2>();
        enemyScale = enemyMove.enemyScale;

        objPos = transform.position;
    }

    public Vector3 SideCollisionEnemy(Vector3 move, GameObject e)
    {
        Vector3 ans = new Vector3(1, 1, 1);
        SetPosEnemy(e);
        bool notHit = false;
        if((enemyPos.y - (enemyScale.y / 2) >= objPos.y + (objScale.y / 2)
            || (enemyPos.y + (enemyScale.y / 2) <= objPos.y - (objScale.y / 2))))
        {
            notHit = true; // �������Ⴄ�Ƃ�true
            Debug.Log("haitta");
        }
        //  y�͈͂������Ƃ�
        if(!notHit)
        {
            // z�͈͂������Ƃ�
            if ((enemyPos.z - (enemyScale.z / 2) < objPos.z + (objScale.z / 2))
            && (enemyPos.z + (enemyScale.z / 2) > objPos.z - (objScale.z / 2)))
            {
                if ((enemyPos.x - (enemyScale.x / 2) < objPos.x + (objScale.x / 2))
                    && (enemyPos.x + (enemyScale.x / 2) > objPos.x - (objScale.x / 2)))
                {
                    //�E�ʂ��������Ă�Ƃ� && �{�ړ����Ă�Ƃ� || ���ʂ��������Ă�Ƃ� && �[�ړ����Ă�Ƃ�
                    if ((enemyPos.x < objPos.x && move.x > 0)
                        || (enemyPos.x > objPos.x && move.x < 0))
                    {
                        // x�����ւ̈ړ���Î~
                        ans.x = 0;

                        Debug.Log("aaaaaaa");

                    }
                }
            }
            if ((enemyPos.x - (enemyScale.x / 2) < objPos.x + (objScale.x / 2))
            && (enemyPos.x + (enemyScale.x / 2) > objPos.x - (objScale.x / 2)))
            {
                if ((enemyPos.z - (enemyScale.z / 2) < objPos.z + (objScale.z / 2))
                    && (enemyPos.z + (enemyScale.z / 2) > objPos.z - (objScale.z / 2)))
                {
                    //�O�ʂ��������Ă�Ƃ� && �{�ړ����Ă�Ƃ� || ��ʂ��������Ă�Ƃ� && �[�ړ����Ă�Ƃ�
                    if ((enemyPos.z < objPos.z && move.z > 0)
                        || (enemyPos.z > objPos.z && move.z < 0))
                    {
                        // z�����ւ̈ړ���Î~
                        ans.z = 0;

                    }
                }
            }
        }
        return ans;
    }




    //  changeMode = 1 : Onplayer   changeMode = 2 : Block  changeMode = 3 ; ������̂��蔲���h�~  changeMode = 4 ; �����ƓG��p�H�i�I�u�W�F�ɐG�ꂽ�甽���j
    public float OnPlayer(float changeMode)
    {
        SetPos();
        // stage��xz�͈͂ɂ��邩
        if ( (playerPos.x + (playerScale.x / 2) >= objPos.x - (objScale.x / 2))
            &&(playerPos.x - (playerScale.x / 2) <= objPos.x + (objScale.x / 2))
            &&(playerPos.z + (playerScale.z / 2) >= objPos.z - (objScale.z / 2))
            &&(playerPos.z - (playerScale.z / 2) <= objPos.z + (objScale.z / 2)) )
        {
            if(changeMode == 1)
            {
                // �X�e�[�W����
                if(playerPos.y - (playerScale.y / 2) > objPos.y + (objScale.y / 2))
                {
                    onBlock = false;
                    return 1.0f;
                }
                // �X�e�[�W��
                else if(playerPos.y - (playerScale.y / 2) == objPos.y + (objScale.y / 2))
                        //&& playerPos.y - (playerScale.y / 2) > objPos.y + (objScale.y / 2) - 0.05) //���ꂢ��Ȃ�����
                {
                    onBlock = true;
                    return 2.0f;
                }
                // �X�e�[�W��艺
                else
                {
                    onBlock = false;
                    return 0;
                }
            }
            //
            else if(changeMode == 2)
            {
                // xz�͈͂ŕ����Ă�u���b�N�����@�v���C���[�̉��ɂ���
                if(playerPos.y - (playerScale.y / 2) >= objPos.y + (objScale.y / 2))
                {
                    return objPos.y + (objScale.y / 2);
                }
                //  �v���C���[�̏�ɂ���
                else
                {
                    //Debug.Log("kokoo1");
                    return objPos.y + (objScale.y / 2) - 1;
                }
            }
            // �����u���b�N�ɂԂ�����
            else if(changeMode == 3)
            {
                // �v���C���[�̓����I�u�W�F�̒ꂪ��@�@�i0.05�͗V�ђl�j
                if(playerPos.y + (playerScale.y / 2) < objPos.y - (objScale.y / 2) + range
                    && (playerPos.y + (playerScale.y / 2) > objPos.y - (objScale.y / 2) - range ))
                {
                    Debug.Log("����������");
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //
            else if(changeMode == 4)
            {
                //  �v���C���[�̓���肤���ɂ���   �v���C���[�̑���艺�ɂ���
                if(playerPos.y + (playerScale.y / 2) < objPos.y - (objScale.y / 2)
                    || playerPos.y - (playerScale.y / 2) > objPos.y + (objScale.y / 2))
                {
                    // �����ł͉������Ȃ�
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //
            else
            {
                Debug.Log("�G���[1");
                return 100;
            }
        }
        else
        {
            //  �X�e�[�W�O
            if(changeMode == 1)
            {
                return -1.0f;
            }
            //
            else if(changeMode == 2)
            {
                Debug.Log("�G���[�H");
                // �����Ă�u���b�N����X�e�[�W�ɒ��n���鎞�@�@�z��ɃX�e�[�W�����΂����H
                if(this.gameObject != stage)
                {
                    //Debug.Log("����͌����Ă�H");
                    //AllCollision allCollision = stage.GetComponent<AllCollision>();
                    //return allCollision.OnPlayer(2);

                    return objPos.y + (objScale.y / 2) - 1;
                }
                else
                {
                    Debug.Log("�G���[�H");
                    return -10;
                }
            }
            //
            else if(changeMode == 3)
            {
                return 1;
            }
            //
            else if(changeMode == 4)
            {
                return 2;
            }
            //
            else
            {
                Debug.Log("�G���[2");
                return 100;
            }
        }
    }
    void SetPos()
    {
        playerPos = player.transform.position;
        playerScale = playerMove.playerScale;

        objPos = transform.position;

        //blockPos = player.GetComponent<PlayerMove>().ground;
    }
    public Vector3 SideCollision(Vector3 move)
    {
        Vector3 ans = new Vector3(1, 1, 1);
        SetPos();
        //  y�͈͂������Ƃ�
        if((playerPos.y - (playerScale.y / 2) < objPos.y + (objScale.y / 2))
           && (playerPos.y + (playerScale.y / 2) > objPos.y - (objScale.y / 2)))
        {
            // z�͈͂������Ƃ�
            if ((playerPos.z - (playerScale.z / 2) < objPos.z + (objScale.z / 2))
            && (playerPos.z + (playerScale.z / 2) > objPos.z - (objScale.z / 2)))
            {
                if( (playerPos.x - (playerScale.x / 2) < objPos.x + (objScale.x / 2))
                    && (playerPos.x + (playerScale.x / 2) > objPos.x - (objScale.x / 2)))
                {
                    //�E�ʂ��������Ă�Ƃ� && �{�ړ����Ă�Ƃ� || ���ʂ��������Ă�Ƃ� && �[�ړ����Ă�Ƃ�
                    if( ( playerPos.x < objPos.x && move.x > 0 )
                        || ( playerPos.x > objPos.x && move.x < 0))
                    {
                        // x�����ւ̈ړ���Î~
                        ans.x = 0;
                    }
                }
            }
            if ((playerPos.x - (playerScale.x / 2) < objPos.x + (objScale.x / 2))
            && (playerPos.x + (playerScale.x / 2) > objPos.x - (objScale.x / 2)))
            {
                if ((playerPos.z - (playerScale.z / 2) < objPos.z + (objScale.z / 2))
                    && (playerPos.z + (playerScale.z / 2) > objPos.z - (objScale.z / 2)))
                {
                    //�O�ʂ��������Ă�Ƃ� && �{�ړ����Ă�Ƃ� || ��ʂ��������Ă�Ƃ� && �[�ړ����Ă�Ƃ�
                    if ((playerPos.z < objPos.z && move.z > 0)
                        || (playerPos.z > objPos.z && move.z < 0))
                    {
                        // z�����ւ̈ړ���Î~
                        ans.z = 0;
                    }
                }
            }
        }
        return ans;
    }

    //  �傫���v��
    public float Range(Vector3 a)
    {
        a.x = a.x * a.x;
        a.y = a.y * a.y;
        a.z = a.z * a.z;
        return Mathf.Sqrt(a.x + a.y + a.z);
    }



    /*
    // �n�ʂ̈ʒu��Ԃ�
    public float Block()
    {
        SetPos();
        // stage��xz�͈͂ɂ��邩
        if ((playerPos.x - (playerScale.x / 2) >= objPos.x - (objScale.x / 2)
            && playerPos.x + (playerScale.x / 2) <= objPos.x + (objScale.x / 2))
            && (playerPos.z - (playerScale.z / 2) >= objPos.z - (objScale.z / 2)
            && playerPos.z + (playerScale.z / 2) <= objPos.z + (objScale.z / 2)))
        {
            if(playerPos.y - (playerScale.y / 2) < objPos.y + (objScale.y / 2) )
            {
                //�@�n�ʂ̈ʒu
                return stagePos.y + (stageScale.y / 2);
            }
            else
            {
                //Debug.Log(objPos);

                //  �u���b�N�̈ʒu
                return objPos.y + (objScale.y / 2);
            }

        }
        else
        {
            //  �n�ʂ̈ʒu
            return stagePos.y + (stageScale.y / 2);
        }
    }
    */
}