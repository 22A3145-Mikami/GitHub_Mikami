using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPNG;//�Q�[���I�[�o�[�摜
    [SerializeField]
    private GameObject gameClearPNG;//�Q�[���N���A�摜
    // ���R�[�h�Ŕ�����ϐ�
    public bool gameOver;
    public bool gameClear;

    public bool stop; //�S�ẴR�[�h�̈ꎞ��~   0 = start�@�ɂ���Ƃ�
    //[SerializeField]
    public int changeScene; // -1 == �R�[�X�v���C�@ 0 == �^�C�g����ʁ@1 == �R�[�X�Z���N�g��ʁ@2 == �t�F�[�h
    [SerializeField]
    private GameObject startGamenPNG;//start�摜
    [SerializeField]
    private GameObject selectGamenPNG;//�R�[�X�Z���N�g�摜
    [SerializeField]
    private GameObject blackPanel; //�@�R�[�X�ւ̑J��

    [SerializeField]
    private StageSet stageSet; // �I�u�W�F�N�g�𐶐�����R�[�h

    [SerializeField]
    private GameObject playUi; //�c�莞�ԕ\���Ȃ�


    public int keepStageSet;//���X�^�[�g�̂Ƃ��Ɏg��
    public bool pushR; // ���g���C�����Ƃ� camera������@�@GameMaker CameraMove StageSet

    public bool firstplay; //���߂Ẵv���C

    [SerializeField]
    private GameObject stopScenePNG;//�ꎞ��~�摜

    private float time = 0;  // �R�[�X�J�ڂ̍����p
    private Image blackPanel_image;  //  �����̃��l����p
    private float alpha = -1;//�摜�̓����x

    private float gameTime = -1; // �Q�[���I���܂ł̎���  �����Ȃ��Ƃ���-1
    [SerializeField]
    private TextMeshProUGUI timeText;//�E���Time

    [SerializeField]
    private GameObject keySetPNG;//����摜
    private bool openKeySet;//A�L�[����

    
    [SerializeField]
    private GameObject creditSetPNG;
    private bool openCredit;
    

    //���[�h�؂�ւ��̃R�[�h�ƕϐ�
    [SerializeField]
    private Change2D3D change2D3DCode;

    [SerializeField]
    private GameObject dead_time_moji;//���������@���Ԑ؂�
    //[SerializeField]
    public GameObject dead_press_moji;//���������@����
    //[SerializeField]
    public GameObject dead_enemy_moji;//���������@�G
    //[SerializeField]
    public GameObject dead_abs_moji;//���������@����

    // Start is called before the first frame update
    private void Start()
    {
        firstplay = true;//���߂ăQ�[�����J�n����

        ResetGame(0);

        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (changeScene == 0)//�X�^�[�g���
        {
            StartGamen();

            playUi.SetActive(false);
        }
        else if (changeScene == 1)//�R�[�X�Z���N�g���
        {
            SelectGamen();

            playUi.SetActive(false);
        }
        else if (changeScene == 2 && !stop) // stop�͈ꎞ��~�@�t�F�[�h�C��
        {
            GameStart();
        }
        else if(changeScene == -1 && !stop)//�R�[�X�v���C��
        {
            if (gameClear && !gameOver)//�Q�[���N���A���
            {
                gameClearPNG.SetActive(true);

                playUi.SetActive(false);

                if (Input.GetKey(KeyCode.S))//�R�[�X�Z���N�g�ɖ߂�
                {
                    ResetGame(1);
                }
                else if (Input.GetKey(KeyCode.T))//�^�C�g����ʂɖ߂�
                {
                    ResetGame(0);
                }
            }
            else if (gameOver && !gameClear)//�Q�[���I�[�o�[���
            {
                gameOverPNG.SetActive(true);

                playUi.SetActive(false);

                if (Input.GetKey(KeyCode.S))//�R�[�X�Z���N�g�ɖ߂�
                {
                    ResetGame(1);
                }
                else if (Input.GetKey(KeyCode.T))//�^�C�g����ʂɖ߂�
                {
                    ResetGame(0);
                }
            }


            if (Input.GetKey(KeyCode.R))//�R�[�X���ŏ��ɖ߂�
            {
                pushR = true;

                ResetGame(2); //���̃R�[�X�̍ŏ�����
            }

            TimeText(300);
        }

        if(Input.GetKeyUp(KeyCode.Escape))//�ꎞ��~���
        {
            StopGame();
        }
        Stopping();
    }

    //��ʑJ�ڊ֐��@�@
    private void ResetGame(int change)
    {

        changeScene = change;

        gameClearPNG.SetActive(false);
        gameClear = false;

        gameOverPNG.SetActive(false);
        gameOver = false;

        startGamenPNG.SetActive(false);

        selectGamenPNG.SetActive(false);

        blackPanel.SetActive(false);
        alpha = 1;
        time = 0;

        playUi.SetActive(false);
        gameTime = -1;

        stopScenePNG.SetActive(false);
        stop = false;

        keySetPNG.SetActive(false);

        creditSetPNG.SetActive(false);

        dead_abs_moji.SetActive(false);
        dead_enemy_moji.SetActive(false);
        dead_press_moji.SetActive(false);
        dead_time_moji.SetActive(false);

        if (change == 2)
        {
            stageSet.SetObj(keepStageSet);
        }

        change2D3DCode.change2D3D = 2;
    }
    //�ꎞ��~�֐��@���̂P
    private void StopGame()
    {
        //Debug.Log("�Ă񂾁H");
        if (stop)
        {
            stop = false;
        }
        else
        {
            stop = true;
        }
    }
    //�ꎞ��~�֐��@���̂Q
    private void Stopping()
    {
        if (stop)
        {
            stopScenePNG.SetActive(true);
            //
            if (Input.GetKeyUp(KeyCode.T))
            {
                ResetGame(0);
            }
            //
            else if (Input.GetKeyUp(KeyCode.S))
            {
                ResetGame(1);
            }
            //  1
            else if (Input.GetKeyUp(KeyCode.A))
            {
                OpenKeySet();
            }
            //  2 ���������ꍇ
            
            else if(Input.GetKeyUp(KeyCode.C))
            {
                OpenCredit();
            }
            
            //  1
            KeyOption();
            //  2 ���������ꍇ
            Credit();
            
        }
        else if (!stop)
        {
            stopScenePNG.SetActive(false);
        }
    }

    //start��ʊ֐�
    private void StartGamen()
    {
        startGamenPNG.SetActive(true);

        /*
        if(Input.anyKey)
        {
            if(!(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) 
             || Input.GetKey(KeyCode.Alpha3) || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)/* || Input.GetKey(KeyCode.Q)) )
            {
                changeScene++;
            }
        }
        */
        if(Input.GetKey(KeyCode.Space))
        {
            changeScene++;

        }
        
    }
    
    //�R�[�X�Z���N�g�֐�
    private void SelectGamen()
    {
        startGamenPNG.SetActive(false);

        selectGamenPNG.SetActive(true);


        if(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))//�P�@���@�e���L�[�P
        {
            keepStageSet = 1;

            stageSet.SetObj(1);//�R�[�X�P�𐶐�


            changeScene++;
        }
        
        else if(Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))//�Q�@���@�e���L�[�Q
        {
            keepStageSet = 2;

            stageSet.SetObj(2);//�R�[�X�Q�𐶐�

            changeScene++;
        }
        
        else if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))//�R�@���@�e���L�[�R
        {
            keepStageSet = 3;

            stageSet.SetObj(3);//�R�[�X�R�𐶐�

            changeScene++;
        }
        

    }

    //start��ʊ֐�
    private void GameStart()
    {

        selectGamenPNG.SetActive(false);

        blackPanel.SetActive(true);
        blackPanel_image = blackPanel.GetComponent<Image>();
        //�����x�̍X�V
        alpha = -(2 * time * time) + 1;
        time += Time.deltaTime;
        blackPanel_image.color = new Color(0,0,0,alpha);
        if(alpha < 0)
        {
            blackPanel.SetActive(false);
            
            alpha = -1;
            
            changeScene = -1;

        }
    }
    //���Ԋ֐�
    private void TimeText(float setTime)
    {
        //�R�[�X�v���C��
        if(!gameClear && !gameOver)
        {
            playUi.SetActive(true);
        }

        if(gameTime == -1)
        {
            gameTime = setTime;
        }
        gameTime -= Time.deltaTime;
        //�}�C�i�X�ɍs���Ȃ��悤�ɂ���
        if(gameTime < 0)
        {
            // �Q�[���I�[�o�[
            Debug.Log("gameover"); // �A�肪�x���Ȃ���

            gameOver = true;
            //���Ԑ؂�
            dead_time_moji.SetActive(true);
            //

            gameTime = 0;
        }

        timeText.text = "Time\n" + (int)gameTime;
    }
    
    //����摜�֐��iA�L�[�j
    private void KeyOption()
    {
        if(openKeySet)
        {
            keySetPNG.SetActive(true);
        }
        else if(!openKeySet)
        {
            keySetPNG.SetActive(false);
        }
    }
    //����摜�֐��iA�L�[�j
    private void OpenKeySet()
    {
        if (openKeySet)
        {
            openKeySet = false;
        }
        else if (!openKeySet)
        {
            openKeySet = true;
        }
    }
    
    private void Credit()
    {
        if (openCredit)
        {
            creditSetPNG.SetActive(true);
        }
        else if (!openCredit)
        {
            creditSetPNG.SetActive(false);
        }
    }

    private void OpenCredit()
    {
        if (openCredit)
        {
            openCredit = false;
        }
        else if (!openCredit)
        {
            openCredit = true;
        }
    }
    
}
