using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toukatugun : MonoBehaviour
{
    [SerializeField]
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage, transform.position);
    //        SoundManager.instance.PlaySE(3);
    //    }
    //}
}
