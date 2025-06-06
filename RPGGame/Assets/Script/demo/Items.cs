using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{

    public int healthItemRecoveryValue;

    [SerializeField]
    private float lifeTime;

    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }
}
