using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tikuwa : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private float time;  //落ちるまでの時間
    private float onTime = 0f;  //  ブロックにいる時間

    private float speed; // 落ちる速さ

    private Vector3 destroy3D;  //　消える場所３D
    private Vector2 destroy2D;  //  消える場所２D

    private Vector3 startPos;

    private int listI;

    private bool drop = false;
    [SerializeField]
    private Vector3 houkou;  //  消える場所への進行

    //[SerializeField]
    private GameObject tList;
    //[SerializeField]
    private TikuwaList tListScript;
    // Start is called before the first frame update
    void Start()
    {
        //transform.parent = GameObject.Find("TikuwaList").transform;
    }
    private void Awake()
    {
        tList = GameObject.Find("TikuwaList");
        
        //transform.parent = tList.transform;

        startPos = transform.position;

        tListScript = tList.GetComponent<TikuwaList>();
        player = tListScript.player;

        for (int i = 0; i < tListScript.tikuwaList.GetLength(0); i++)
        {
            if(transform.position == tListScript.tikuwaList[i, 1])
            {
                destroy3D = tListScript.tikuwaList[i, 2];
                time = tListScript.tikuwaList[i, 0].y;
                speed = tListScript.tikuwaList[i, 0].z;
                listI = i;
                break;
            }
        }

        houkou = destroy3D - transform.position;
        houkou = houkou / Seikika(houkou);


        Application.targetFrameRate = 60;
    }

    private void FixedUpdate()
    {
        if (player.GetComponent<PlayerMove3>().stageColl == 2
            && gameObject.GetComponent<AllCollision>().onBlock)
        {
            if (onTime < time)
            {
                onTime += Time.deltaTime;
            }
            else if (onTime >= time)
            {
                drop = true;
            }
        }
        else
        {
            onTime = 0;
            gameObject.GetComponent<AllCollision>().onBlock = false;
        }

        if (drop)
        {
            Drop();
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerMove3>().stageColl == 2
            && gameObject.GetComponent<AllCollision>().onBlock)
        {
            if (onTime < time)
            {
                onTime += Time.deltaTime;
            }
            else if(onTime >= time)
            {
                drop = true;
            }
        }
        else
        {
            onTime = 0;
            gameObject.GetComponent<AllCollision>().onBlock = false;
        }

        if(drop)
        {
            Drop();
        }

    }
    */

    private void Drop()
    {
        transform.position += houkou * speed;
        player.transform.position += houkou * speed;
        player.GetComponent<PlayerMove3>().ground += houkou * speed;
        if(gameObject.GetComponent<AllCollision>().Range(transform.position - startPos) >= gameObject.GetComponent<AllCollision>().Range(destroy3D - startPos))
        {
            tListScript.tikuwaList[listI, 0].x = 2;
            drop = false;
            tListScript.destroyJudge = true;
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }
    private float Seikika(Vector3 a)
    {
        for (int i = 0; i < 3; i++)
        {
            a[i] = a[i] * a[i];
        }
        float ans = Mathf.Sqrt(a.x + a.y + a.z);
        return ans;
    }



    /*
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject == player)
        {
            if(onTime < time)
            {
                onTime += Time.deltaTime;
            }
            else
            {
                drop = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == player)
        {
            if(onTime < time)
            {
                onTime = 0;
            }
        }
    }
    */
}
