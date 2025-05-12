using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChanger : MonoBehaviour
{
    //[SerializeField]
    public Vector3 change; //�e�̃X�P�[�������
    [SerializeField]
    private List<ObjInfo> childrenObj; //�I�u�W�F�̎q�I�u�W�F��t����
    // �I�u�W�F����������Ĉ�x����
    private bool once;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        once = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(once)
        {
            once = false;
            //Scale��Vector3��x���O�A�P�ł͂Ȃ��Ƃ��ɏ�Z����
            if (!(change.x == 0 || change.x == 1))
            {
                for (int i = 0; i < childrenObj.Count; i++)
                {
                    childrenObj[i].objScale.x *= change.x;
                }
            }
            //Scale��Vector3��y���O�A�P�ł͂Ȃ��Ƃ��ɏ�Z����
            if (!(change.y == 0 || change.y == 1))
            {
                for (int i = 0; i < childrenObj.Count; i++)
                {
                    childrenObj[i].objScale.y *= change.y;
                }
            }
            //Scale��Vector3��z���O�A�P�ł͂Ȃ��Ƃ��ɏ�Z����
            if (!(change.z == 0 || change.z == 1))
            {
                for (int i = 0; i < childrenObj.Count; i++)
                {
                    childrenObj[i].objScale.z *= change.z;
                }
            }
        }
    }
}
