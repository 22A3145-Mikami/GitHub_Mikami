using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    
    // ���[�h�؂�ւ��R�[�h�ƕϐ������
    [SerializeField]
    private Change2D3D change2D3DCode;
    private int change2D3D;

    //�Q�[���̑S�̗̂���I�u�W�F�ƃR�[�h
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    //�v���C���[�I�u�W�F�ƃ��[���h���W
    [SerializeField]
    private GameObject player;
    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        //���[�h�؂�ւ��̎擾
        change2D3DCode = GameObject.Find("EventChange2D3D").GetComponent<Change2D3D>();
        change2D3D = change2D3DCode.change2D3D;

        //�����̃J�����R���|�[�l���g�̎擾
        camera = GetComponent<Camera>();

        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;

        //�Q�[���̗�����擾
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�Q�[�����ꎞ���f���ĂȂ��@���@�R�[�X�v���C��
        if(!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            //���g���C��������ĂȂ��Ƃ�
            if(!gameMakerCode.pushR)
            {
                //�v���C���[���W�擾�@
                player = GameObject.FindGameObjectWithTag("Player");
                playerPos = player.transform.position;

                //3D���[�h�̂Ƃ�
                if (change2D3D == 3)
                {
                    //�����̍��W�E��]�ύX
                    transform.position = playerPos + new Vector3(1, 3, -10);
                    transform.localEulerAngles = new Vector3(13, 0, 0);
                }
                //2D1D���[�h�̂Ƃ�
                else if (change2D3D != 3)
                {
                    //�����̍��W�E��]�ύX
                    //Debug.Log("ts");
                    transform.position = playerPos + new Vector3(1, 1, -100);
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
            else
            {
                transform.position = Vector3.zero;
            }


            //���[�h�؂�ւ��̎擾
            change2D3D = change2D3DCode.change2D3D;

            if (change2D3D == 3)
            {
                //�������e�ɕύX
                camera.orthographic = false;
            }
            else
            {
                //���s���e�ɕύX
                camera.orthographic = true;
            }
        }
        
    }

}
