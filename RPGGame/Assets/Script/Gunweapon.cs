using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunweapon : MonoBehaviour
{
    [SerializeField]
    public int attackDamage;

    [SerializeField]
    public Toukatugun toukatugun;


    private void Awake()
    {
        if (PlayerPrefs.HasKey("MaxHp"))
        {

            attackDamage = PlayerPrefs.GetInt("gunweapon.attackDamage");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //attackDamage = toukatugun.damage;
    }

    // Update is called once per frame
    void Update()
    {
        //if (PlayerPrefs.HasKey("MaxHp"))
        //{

        //    attackDamage = PlayerPrefs.GetInt("gunweapon.attackDamage");
        //}
        //attackDamage = PlayerPrefs.GetInt("gunweapon.attackDamage");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position);
            SoundManager.instance.PlaySE(3);
        }
    }
}
