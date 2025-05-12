using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{

    [SerializeField]
    private EnemyController enemyController;

    [SerializeField]
    private float enemyHP;

    [SerializeField]
    public bool sinkae;

    // Start is called before the first frame update
    void Start()
    {
        sinkae = false;
    }

    // Update is called once per frame
    void Update()
    {
        enemyHP = enemyController.currentHealth;
        if(enemyHP <= 0f)
        {
            sinkae = true;
        }
    }
}
