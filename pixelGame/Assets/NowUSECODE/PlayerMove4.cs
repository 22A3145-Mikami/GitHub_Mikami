using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove4 : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private ObjInfo objInfo;//�v���C���[�̃I�u�W�F���

    public Vector3 move;//�ړ�����
    [SerializeField]
    private float speed;//�ړ����x
    //�W�����v�ɂ���
    public int canJump; // 0�@�W�����v�\�@1�@�W�����v�s�@�㏸���@�Q�@�W�����v�s�@���~��

    public float startJump; //�ŏ��̃W�����v�̗�
    [SerializeField]
    private float startJump_zero; // �u���b�N���Ȃ����̗����p

    public float startJump_playerself; // �v���C���[���g�ŃW�����v�������Ƃ��p
    public float startJump_tatumaki;

    public float timeJump; // �W�����v�̎���

    public float gravity; // �d��

    //���[�h�؂�ւ��̃R�[�h�ƕϐ�
    [SerializeField]
    private Change2D3D change2D3DCode;
    [SerializeField]
    private int change2D3D;
    
    [SerializeField]
    private CubeCollision cubeCollision; // �����蔻��

    [SerializeField]
    private int groundBlock; // ������n�ʂ̃u���b�N�̔ԍ��������

    /// CubeCollision�p�̕ϐ��@���蔲���h�~�p
    public float addJustBlockRange;//�����p�@justBlockRange
    public float keepMove_y; // �ЂƂO�̂����W
    public int keepCanJump; // �ЂƂO�̃W�����v���

    public bool onukiyuka;//�������ɏ���Ă��邩

    /// �v���C���[�A�j��
    public PlayerANIM playerANIM;

    //�Q�[���S�̗̂���I�u�W�F�ƃR�[�h
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    //���������@����
    private GameObject dead_abs;

    private void Awake()
    {
        //���[�h�؂�ւ��̎擾
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //�v���C���[���擾
        objInfo = GetComponent<ObjInfo>();

        //�����蔻��̎擾
        cubeCollision = GameObject.Find("JudgeCollision").GetComponent<CubeCollision>();

        startJump_zero = 0;

        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;

        //�Q�[���S�̗̂���擾
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
        
        //��������������
        dead_abs = gameMakerCode.dead_abs_moji;

    }

    //�PD�QD����RD�ɂȂ������Ƀv���C���[�̂����W��ύX����֐�
    private void Position3D()
    {
        
        ObjInfo blockInfo; //�u���b�N�̏��擾
        if (type_onPlayerBlock == 0)
        {
            blockInfo = cubeCollision.blocks[groundBlock].GetComponent<ObjInfo>();
        }
        else
        {
            blockInfo = cubeCollision.enemys[groundBlock].GetComponent<ObjInfo>();
        }
        //�����W�ړ�
        Vector3 nowPos = transform.position;
        if( !(nowPos.z - (objInfo.objScale.z / 2) < blockInfo.objPosition.z + (blockInfo.objScale.z / 2)
            && nowPos.z + (objInfo.objScale.z / 2) > blockInfo.objPosition.z - (blockInfo.objScale.z / 2) ))
        {
            nowPos.z = cubeCollision.blocks[groundBlock].transform.position.z;
        }
        
        transform.position = nowPos;
    }

    [SerializeField]
    int type_onPlayerBlock = 0;

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
            else if (change2D3D == 3 && change2D3DCode.change2D3D != 3)//�RD����P�c�Q�c�ɕς������
            {
                for (int i = 0; i < cubeCollision.blocks.Count; i++)
                {
                    cubeCollision.Death_Press(this.gameObject, cubeCollision.blocks[i]);//�v���C���[�̎�O���Ƀu���b�N������Ύ�
                }
            }

            change2D3D = change2D3DCode.change2D3D;

            //�������ɏ���Ă��Ȃ����false
            onukiyuka = false;
            //�ړ������̃��Z�b�g
            move = Vector3.zero;

            //AD�L�[�i���E�j�ړ�
            if (Input.GetKey(KeyCode.A))
            {
                Walk_AD(-speed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Walk_AD(speed);

            }

            //3D�p�@WS�L�[�i��O���j�ړ�
            if (change2D3D == 3)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Walk_WS(speed);

                }
                if (Input.GetKey(KeyCode.S))
                {
                    Walk_WS(-speed);

                }
            }

            //xz�����蔻��
            HitEnemyORBlock_xz(cubeCollision.blocks);
            
            // 1D�̎��̓G�Ƃ̓����蔻��@�@�u���b�N�ƓG�ɋ��܂ꂽ�Ƃ��Ɉ𒎂̓v���C���[��o��
            if(change2D3D == 1)
            {
                HitEnemyORBlock_xz(cubeCollision.enemys);

                // �v���C���[���u���b�N�ɖ��܂�Ȃ��悤��
                //xz�����蔻��
                objInfo.ResetHitWall();
                for (int i = 0; i < cubeCollision.blocks.Count; i++)
                {
                    cubeCollision.AtariHantei2D(this.gameObject, cubeCollision.blocks[i], 0);
                }

                if (player_position.x < 0 && objInfo.hidariWall)
                {
                    player_position.x = 0;

                    foreach(GameObject obj in pushPlayer_enemys)
                    {
                        if(!obj.GetComponent<EnemyMove3>().climb)
                        {
                            obj.GetComponent<EnemyMove3>().climb_player = true; //�v���C���[��o���悤�ɂ���
                        }
                        
                    }
                }

                if (player_position.x > 0 && objInfo.migiWall)
                {
                    player_position.x = 0;

                    foreach (GameObject obj in pushPlayer_enemys)
                    {
                        if (!obj.GetComponent<EnemyMove3>().climb)
                        {
                            obj.GetComponent<EnemyMove3>().climb_player = true;//�v���C���[��o���悤�ɂ���
                        }
                    }
                }

                // �𒎂ɉ�����Ă�
                transform.position += player_position;
                player_position = Vector3.zero;
            }



            // �摜�̕ύX
            if(objInfo.changePlayerPNG != 2)
            {
                if (move.x == 0 && move.z == 0)
                {
                    playerANIM.StopWalk();

                    objInfo.changePlayerPNG = 0;
                }
                else if (move.x != 0 || move.z != 0)
                {
                    playerANIM.Walk();
                    RotatePlayerOBJ();

                    objInfo.changePlayerPNG = 1;
                }
            }
            // 

            //space�L�[�ŃW�����v�o����
            Jump_Space();
            
            // ���ʂ����邩�炱���ŏ���
            transform.position += move;

            objInfo.ResetHitWall();
            // �������蔻��@�����Ȃ����̗����p
            HitEnemyORBlock_y(cubeCollision.blocks);
            
            if(change2D3D == 1) //�PD���[�h�ł͈𒎂��u���b�N�ɂȂ�
            {
                HitEnemyORBlock_y(cubeCollision.enemys);
            }
            
            if (canJump == 0) //�W�����v���ĂȂ��Ƃ���
            {
                if (!objInfo.sitaWall) //�����n�ʂɂ��Ă��Ȃ�������
                {
                    //�@�����R�[�h
                    canJump = 2;
                    startJump = startJump_zero;
                    timeJump = 0;

                    //Debug.Log("���蔲��");
                    //break;
                }

                //�ی�
                if( objInfo.sitaWall && keepMove_y > transform.position.y)//fallPos > transform.position.y)
                {
                    //�@�����R�[�h
                    canJump = 2;
                    startJump = startJump_zero;
                    timeJump = 0;

                    objInfo.sitaWall = false;

                    //Debug.Log("���蔲��2");
                    //break;
                }
            }
            //////////////


            // y�����蔻��@�W�����v����̒��n�ƃu���b�N�����ɓ���������
            HitEnemyORBlock_y_tyakuti_Atama(cubeCollision.blocks, 0);
            
            if (change2D3D == 1) //�PD���[�h�ł͈𒎂��u���b�N�ɂȂ�
            {
                HitEnemyORBlock_y_tyakuti_Atama(cubeCollision.enemys, 1);

                //�𒎂ɏ���Ă�
                transform.position += player_position;
                player_position = Vector3.zero;
            }



            //����
            if (canJump == 2 && transform.position.y <= -10)
            {
                //���������@����
                dead_abs.SetActive(true);

                gameMakerCode.gameOver = true;
                //Debug.Log("rakka");
                //�ޗ�����
                
                //
            }
            
            //Debug.Log("1���I���");
        }
        
    }

    Vector3 player_position; //�v���C���[�̍��W
    void HitEnemyORBlock_y_tyakuti_Atama(List<GameObject> obj, int enemyORblock)
    {
        int co = 0; //�v���C���[���𒎂ɏ���Ă�Ƃ��ɑ������d�ˊ|���o���Ȃ��悤��

        for (int i = 0; i < obj.Count; i++)
        {

            objInfo.ResetHitWall();

            // 3D�p
            if (change2D3D == 3)
            {
                //���̃u���b�N�ɒ��n���Ă��邩�@�����܂ɂ������Ă��邩
                cubeCollision.AtariHantei3D(this.gameObject, obj[i], 0);

                if (objInfo.sitaWall && move.y < 0) //���n���Ă��邩
                {
                    Jump_Tyakuti(i, enemyORblock);

                    type_onPlayerBlock = enemyORblock;
                }
                else if (objInfo.ueWall && move.y > 0)//�����܂ɂ������Ă��邩
                {
                    move.y = 0;

                    //Debug.Log("�����܂�������");

                    Jump_AtamaHit(i, enemyORblock);

                    //break;
                }

            }
            // 2D�p
            else if (change2D3D != 3)
            {
                //���̃u���b�N�ɒ��n���Ă��邩�@�����܂ɂ������Ă��邩
                cubeCollision.AtariHantei2D(this.gameObject, obj[i], 0);

                if ( (objInfo.sitaWall && move.y < 0 )) //���n���Ă��邩
                {
                    //player���u���b�N�ɂ��Ă��邩��
                    if(!obj[i].GetComponent<ObjInfo>().player)
                    {
                        Jump_Tyakuti(i, enemyORblock);
                    }

                    type_onPlayerBlock = enemyORblock;
                }
                else if (objInfo.ueWall && move.y > 0)//�����܂ɂ������Ă��邩
                {
                    move.y = 0;

                    //Debug.Log("�����܂�������");

                    //player���u���b�N�ɂ��Ă��邩��
                    if (!obj[i].GetComponent<ObjInfo>().player)
                    {
                        Jump_Tyakuti(i, enemyORblock);
                    }

                }
            }

            
            //�G�@�ڐG���̃v���C���[�ړ�
            if (obj[i].GetComponent<ObjInfo>().enemy == true)
            {
                if (change2D3D == 1)
                {
                    Vector3 addMove_player = obj[i].GetComponent<EnemyMove3>().move;
                    if (objInfo.sitaWall == true)
                    {
                        player_position.x += addMove_player.x;
                        player_position.y += addMove_player.y;
                        co++;

                    }
                }
            }

            // �������@�ڐG���̃v���C���[���ǂ�
            if (obj[i].GetComponent<ObjInfo>().ukiyuka)
            {
                UkiyukaMove ukimove = obj[i].GetComponent<UkiyukaMove>();
                if (objInfo.sitaWall)
                {
                    transform.position += ukimove.move * ukimove.speed;
                    if (canJump == 2)
                    {
                        canJump = 0;

                    }
                    if (timeJump == 0)
                    {
                        timeJump = 0;
                    }
                }
                
            }
            

            //�n�ʂƂȂ��Ă���u���b�N�̋L��
            if (objInfo.sitaWall)
            {
                groundBlock = i;
            }

        }
        //�����ړ����d�����Ȃ��悤��
        if(co != 0)
        {
            player_position.x /= co;
            if(player_position.y != 0)
            {
                player_position.y /= co;
            }
        }
        
    }

    //�v���C���[�ƓG�E�u���b�N�̓����蔻��֐��@y���p
    void HitEnemyORBlock_y(List<GameObject> obj)
    {
        // �������蔻��@�����Ȃ����̗����p (�������邩���m����)
        for (int i = 0; i < obj.Count; i++)
        {
            if (change2D3D == 3)
            {
                cubeCollision.AtariHantei3D(this.gameObject, obj[i], 0);
            }
            else if (change2D3D != 3)
            {
                cubeCollision.AtariHantei2D(this.gameObject, obj[i], 0);

            }

        }
    }

    [SerializeField]
    List<GameObject> pushPlayer_enemys; //�v���C���[�������Ă�𒎂�T��
    //�v���C���[�ƓG�E�u���b�N�̓����蔻��֐��@xz���p
    void HitEnemyORBlock_xz(List<GameObject> obj)
    {
        if(pushPlayer_enemys.Count >= 1)
        {
            pushPlayer_enemys.Clear(); //�O�t���[���Ŏc���Ă��郊�X�g�̍폜
        }

        int co = 0;
        //xz�����蔻��
        for (int i = 0; i < obj.Count; i++)
        {

            objInfo.ResetHitWall();

            if (change2D3D == 3)
            {
                cubeCollision.AtariHantei3D(this.gameObject, obj[i], 0);
                //�v���C���[�����͂��Ă�������ɃI�u�W�F������Ƃ�
                if (objInfo.migiWall && move.x >= 0)
                {
                    move.x = 0;
                }
                if (objInfo.hidariWall && move.x <= 0)
                {
                    move.x = 0;
                }
                if (objInfo.okuWall && move.z >= 0)
                {
                    move.z = 0;
                }
                if (objInfo.temaeWall && move.z <= 0)
                {
                    move.z = 0;
                }

            }
            else if (change2D3D != 3)
            {
                float keep_justBlockRange = cubeCollision.justBlockRange;
                cubeCollision.justBlockRange *= 2; 
                cubeCollision.AtariHantei2D(this.gameObject, obj[i], 0);
                cubeCollision.justBlockRange = keep_justBlockRange;
                //�v���C���[�����͂��Ă�������ɃI�u�W�F������Ƃ�
                if (objInfo.migiWall && move.x >= 0)
                {
                    move.x = 0;
                }
                if (objInfo.hidariWall && move.x <= 0)
                {
                    move.x = 0;
                }
                //�G�Ƃ̏Փ�
                if(obj[i].GetComponent<ObjInfo>().enemy == true)
                {
                    if (change2D3D == 1)
                    {
                        EnemyMove3 eMove = obj[i].GetComponent<EnemyMove3>();
                        Vector3 addMove_player = eMove.move;//�G�̈ړ�����
                        if (objInfo.migiWall)
                        {
                            if(move.x > 0) //�𒎂ɓːi���Ă�Ƃ�
                            {
                                player_position.x += addMove_player.x;
                                move.x = 0;

                                pushPlayer_enemys.Add(obj[i]);
                            }
                            else if(move.x == 0 && addMove_player.x < 0)
                            {
                                player_position.x += addMove_player.x;

                                pushPlayer_enemys.Add(obj[i]);
                            }

                            co++;
                        }
                        if (objInfo.hidariWall)
                        {
                            if (move.x < 0)//�𒎂ɓːi���Ă�Ƃ�
                            {
                                player_position.x += addMove_player.x;
                                move.x = 0;

                                pushPlayer_enemys.Add(obj[i]);
                            }
                            else if(move.x == 0 && addMove_player.x > 0)
                            {
                                player_position.x += addMove_player.x;

                                pushPlayer_enemys.Add(obj[i]);
                            }

                            co++;
                        }
                    }
                }
            }
        }

        if(co != 0)
        {
            player_position.x /= co;
        }
    }

    //��l�����i�s�����������悤��
    private void RotatePlayerOBJ()
    {
        //D�L�[
        if(move.x > 0)
        {
            if(move.z > 0)//W�L�[
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 135, 0);
            }
            else if(move.z < 0)//S�L�[
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 225, 0);
            }
            else
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }
        //A�L�[
        else if(move.x < 0)
        {
            if (move.z > 0)//W�L�[
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 45, 0);
            }
            else if (move.z < 0)//S�L�[
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 315, 0);
            }
            else
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        //�O��i��O���j
        else if(move.x == 0)
        {
            if (move.z > 0)//W�L�[
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else if (move.z < 0)//S�L�[
            {
                objInfo.obj3D.transform.localEulerAngles = new Vector3(0, 270, 0);
            }
        }
    }

    //�ړ��֐�
    private void Walk_AD(float houkou)
    {

        move.x = houkou;
    }
    private void Walk_WS(float houkou)
    {
        move.z = houkou;
    }
    //���n�R�[�h
    private void Jump_Tyakuti(int blocks_Number, int enemyORblock)   //�W�����v�̒��n���̒n�ʂ̂��蔲����h�~���邽��
    {

        // �W�����v�̑J��
        canJump = 0;
        timeJump = 0;
        ObjInfo obj_B_Info;
        if (enemyORblock == 0) // �u���b�N�̎�
        {
            obj_B_Info = cubeCollision.blocks[blocks_Number].GetComponent<ObjInfo>();
        }
        else // �G�̎�
        {
            obj_B_Info = cubeCollision.enemys[blocks_Number].GetComponent<ObjInfo>();
        }
        //�n�ʂɂȂ����I�u�W�FB��Ɉړ�����
        Vector3 obj_B_Position = obj_B_Info.objPosition;
        Vector3 obj_B_Scale = obj_B_Info.objScale;
        Vector3 nowPos = transform.position;
        nowPos.y = obj_B_Position.y + (obj_B_Scale.y / 2) + (objInfo.objScale.y / 2);
        transform.position = nowPos;

        //Debug.Log("���n");

        playerANIM.StopJump();
        objInfo.changePlayerPNG = 0;
    }
    private void Jump_AtamaHit(int blocks_Number, int enemyORblock)
    {
        //�W�����v�̑J��
        canJump = 2;
        startJump = startJump_zero;
        timeJump = 0;

        ObjInfo obj_B_Info;
        if (enemyORblock == 0) // �u���b�N�̎�
        {
            obj_B_Info = cubeCollision.blocks[blocks_Number].GetComponent<ObjInfo>();
        }
        else // �G�̎�
        {
            obj_B_Info = cubeCollision.enemys[blocks_Number].GetComponent<ObjInfo>();
        }
        //�������������I�u�W�F�̉��Ɉړ�����
        Vector3 obj_B_Position = obj_B_Info.objPosition;
        Vector3 obj_B_Scale = obj_B_Info.objScale;
        Vector3 nowPos = transform.position;
        nowPos.y = obj_B_Position.y - (obj_B_Scale.y / 2) - (objInfo.objScale.y / 2);
        transform.position = nowPos;
    }
    //�W�����v�֐�
    private void Jump_Space()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // �W�����v�����Ƃ�
            if (canJump == 0 || (onukiyuka && (canJump == 2 || canJump == 1)))
            {
                //�W�����v�J�n
                timeJump = 0;

                startJump = startJump_playerself;
                //���������グ
                move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;

                timeJump = timeJump + Time.deltaTime;

                canJump = 1;

                playerANIM.Jump();

                objInfo.changePlayerPNG = 2;
            }
        }
        //�W�����v���i�㏸�j
        else if(canJump == 1)
        {
            //���������グ
            move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;

            timeJump = timeJump + Time.deltaTime;

            // �����@���̃t���[���̂����W�@���@���̂����W�@���Ⴍ�Ȃ�����
            if(objInfo.objPosition.y > objInfo.objPosition.y + move.y)
            {
                canJump = 2;
            }
        }
        //�W�����v���i���~�j
        else if(canJump == 2)
        {
            //���������グ
            move.y = startJump * timeJump - 0.5f * gravity * timeJump * timeJump;

            timeJump = timeJump + Time.deltaTime;
        }

    }

}
