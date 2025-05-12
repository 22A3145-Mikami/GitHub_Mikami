using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change2D3D : MonoBehaviour
{
    //�Q�[���S�̂Ŏg���@���[�h�`�F���W�̕ϐ�
    public int change2D3D;

    //�Q�[���̑S�̗̂���I�u�W�F�ƃR�[�h
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    /*
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }
    */
    private void Awake()
    {
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
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            //1D���[�h�ւ̕ύX
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                change2D3D = 1;

            }
            //2D���[�h�ւ̕ύX
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                change2D3D = 2;

            }
            //3D���[�h�ւ̕ύX
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                change2D3D = 3;

            }
        }
    }
}
