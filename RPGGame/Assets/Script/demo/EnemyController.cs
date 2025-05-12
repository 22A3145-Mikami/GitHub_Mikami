using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb;
    private Animator enemyAnim;

    [SerializeField]
    private float moveSpeed, waitTime, walkTime;

    private float waitCounter, moveCounter;

    private Vector2 moveDir;

    [SerializeField]
    private BoxCollider2D area;



    [SerializeField, Tooltip("プレイヤーを追いかける？")]
    private bool chase;

    private bool isChaseing;

    [SerializeField]
    private float chaseSpeed, rangeToChase;
    
    private Transform target;

    [SerializeField]
    private float waitAfterHitting;

    [SerializeField]
    private int attackDamage;

    [SerializeField]
    private float maxHealth;
    //[SerializeField]
    public float currentHealth;

    private bool isKnockingBack;

    [SerializeField]
    private float knockBackTime, knockBackForce;

    private float knockBackCounter;

    private Vector2 knockDir;


    [SerializeField]
    private GameObject portion;

    [SerializeField]
    private float healthDropChance;
    [SerializeField]
    private GameObject blood;


    [SerializeField]
    private int exp;


    [SerializeField]
    private Image hpImage;


    private Flash flash;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();

        waitCounter = waitTime;


        target = GameObject.FindGameObjectWithTag("Player").transform;


        currentHealth = maxHealth;

        UpdateHealthImage();

        flash = GetComponent<Flash>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockingBack)
        {
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;
                rb.velocity = knockDir * knockBackForce;
            }
            else
            {
                rb.velocity = Vector2.zero;

                isKnockingBack = false;
            }
            return;
        }


        if (!isChaseing)
        {
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;

                if (waitCounter <= 0)
                {
                    moveCounter = walkTime;

                    enemyAnim.SetBool("moving", true);

                    moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    moveDir.Normalize();
                }
            }
            else
            {
                moveCounter -= Time.deltaTime;

                rb.velocity = moveDir * moveSpeed;

                if (moveCounter <= 0)
                {
                    enemyAnim.SetBool("moving", false);

                    waitCounter = waitTime;
                }
            }

            if (chase)
            {
                if (Vector3.Distance(transform.position,target.transform.position) < rangeToChase)
                {
                    isChaseing = true;
                }
            }
        }
        else
        {
            if(waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;

                if (waitCounter > 0)
                {
                    enemyAnim.SetBool("moving", true);
                }
            }
            else
            {
                moveDir = target.transform.position - transform.position;
                moveDir.Normalize();

                rb.velocity = moveDir * chaseSpeed;
            }

            if(Vector3.Distance(transform.position, target.transform.position) > rangeToChase)
                {
                isChaseing = false;

                waitCounter = waitTime;

                enemyAnim.SetBool("moving", false);
            }
        }


        transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
            Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (isChaseing)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);

                waitCounter = waitAfterHitting;

                enemyAnim.SetBool("moving", false);
            }
        }
    }

    public void KnockBack(Vector3 position)
    {
        isKnockingBack = true;
        knockBackCounter = knockBackTime;

        knockDir = transform.position - position;
        knockDir.Normalize();

        enemyAnim.SetBool("moving", false);
    }

    public void TakeDamage(int damage,Vector3 position)
    {
        currentHealth -= damage;

        UpdateHealthImage();

        flash.PlayFeedback();

        if (currentHealth <= 0)
        {

            Instantiate(blood, transform.position, transform.rotation);

            GameManager.instance.AddExp(exp);

            if (Random.Range(0,100) < healthDropChance &&portion != null)
            {
                Instantiate(portion, transform.position, transform.rotation);
            }
            
            Destroy(gameObject);
        }

        KnockBack(position);
    }

    private void UpdateHealthImage()
    {
        hpImage.fillAmount = currentHealth / maxHealth;
    }
}
