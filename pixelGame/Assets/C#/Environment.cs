using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    //[SerializeField]
    public GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void FixedUpdate()
    {
        if (blocks.Length == 0)
        {
            //  ground�ȊO��block�^�O���t�����I�u�W�F�N�g�������
            blocks = GameObject.FindGameObjectsWithTag("block");
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if(blocks.Length == 0)
        {
            //  ground�ȊO��block�^�O���t�����I�u�W�F�N�g�������
            blocks = GameObject.FindGameObjectsWithTag("block");
        }
  
    }
    */
}
