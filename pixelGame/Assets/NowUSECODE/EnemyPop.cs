using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPop : MonoBehaviour
{
    [SerializeField]
    private float coolTime; //�N�[���^�C��
    [SerializeField]
    private float popTime; // pop���鎞��

    [SerializeField]
    private GameObject enemy; //��

    private Vector3 popPos; //�����ꏊ

    //�Q�[���S�̗̂���I�u�W�F�ƃR�[�h
    [SerializeField]
    private GameObject gameMaker;
    private GameMaker gameMakerCode;

    //�R�[�X�����R�[�h
    private StageSet stageSet;

    //�G�̑傫���̏����ݒ�
    [SerializeField]
    private Vector3 scale = new Vector3(1, 1, 1);
    //�G�̈ړ����x
    [SerializeField]
    private float eSpeed = 0.08f;

    private void Awake()
    {
        //�t���[�����[�g�ݒ�
        Application.targetFrameRate = 60;

        //�����ꏊ
        popPos = transform.position;
        popPos.y += transform.localScale.y / 2 + 1;

        //�Đ�������܂ł̕b��
        popTime = 6;

        //�Q�[���S�̗̂���擾
        gameMaker = GameObject.Find("GameMaker");
        gameMakerCode = gameMaker.GetComponent<GameMaker>();
        //�R�[�X�����R�[�h�擾
        stageSet = gameMaker.GetComponent<StageSet>();


        scale = new Vector3(1, 1, 1);//�n�߂̑傫��

    }
    ///StageSet�R�[�h�p�֐�
    public void SetcoolTime(float time)
    {
        coolTime = time;
    }
    public void SetEnemyScale(Vector3 size)
    {
        scale = size;
    }
    public void SetEnemySpeed(float speed)
    {
        eSpeed = speed;
    }
    ///
    private void FixedUpdate()
    {
        //�ꎞ���f���Ă��Ȃ��@���@�R�[�X�v���C��
        if (!gameMakerCode.stop && gameMakerCode.changeScene == -1)
        {
            coolTime += Time.deltaTime;
            //�N�[���^�C���o�ߎ��ɐ���
            if (coolTime >= popTime)
            {

                GameObject imomusi;
                imomusi = stageSet.Instantiate_OBJ(enemy, popPos, scale);
                imomusi.GetComponent<EnemyMove3>().SetSpeed(eSpeed); //�𒎂̃R�[�h�ɓn��
                

                coolTime = 0;

                //Debug.Log("pop");
            }
        }
        
    }

}
