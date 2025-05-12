using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private GameObject goal;
    [SerializeField]
    private float time;

    private AllCollision allCollision;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;

    }

    private void FixedUpdate()
    {

    }

}
