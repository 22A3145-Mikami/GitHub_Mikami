using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveu : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;


    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 lazerPos = transform.position;

        lazerPos.y += speed * Time.deltaTime;

        transform.position = lazerPos;
    }

    private void OnBecameInvisible()
    {

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }

        if (collision.tag == "wall")
        {
            Destroy(this.gameObject);
        }
    }
}
