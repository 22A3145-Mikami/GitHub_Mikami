using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChanger : MonoBehaviour
{
    //[SerializeField]
    public Vector3 change; //親のスケールいれる
    [SerializeField]
    private List<ObjInfo> childrenObj; //オブジェの子オブジェを付ける
    // オブジェが生成されて一度だけ
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
            //ScaleのVector3のxが０、１ではないときに乗算する
            if (!(change.x == 0 || change.x == 1))
            {
                for (int i = 0; i < childrenObj.Count; i++)
                {
                    childrenObj[i].objScale.x *= change.x;
                }
            }
            //ScaleのVector3のyが０、１ではないときに乗算する
            if (!(change.y == 0 || change.y == 1))
            {
                for (int i = 0; i < childrenObj.Count; i++)
                {
                    childrenObj[i].objScale.y *= change.y;
                }
            }
            //ScaleのVector3のzが０、１ではないときに乗算する
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
