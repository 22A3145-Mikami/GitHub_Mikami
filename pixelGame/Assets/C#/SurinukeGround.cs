using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurinukeGround : MonoBehaviour
{
    //ê∂ê¨èÍèä
    public Vector3[] surinukeList = new Vector3[2];
    public GameObject[] surinukeObjList = new GameObject[2];
    [SerializeField]
    private GameObject surinuke;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        surinukeList[0] = new Vector3(0, 3, 0);
        surinukeList[1] = new Vector3(-1, 3, 0);

        player = GameObject.Find("player");

        for (int i = 0; i < surinukeList.Length; i++)
        {
            surinukeObjList[i] = Instantiate(surinuke, surinukeList[i], Quaternion.identity);
        }

        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
