using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField,Tooltip("移動スピード")]
    private int moveSpeed;

    [SerializeField]
    private Animator playerAnim;

    public Rigidbody2D rb;


    [SerializeField]
    private Animator weaponAnim;
    [System.NonSerialized]
    public int currentHealth;
    public int maxHealth;





    private bool isknockingback;
    private Vector2 knockDir;

    [SerializeField]
    private float knockbackTime, knockbackForce;
    private float knockbackCounter;

    [SerializeField]
    private float invincibilityTime;
    private float invincibilityCounter;


    
    public float totalStamina, recoverySpeed;

    [System.NonSerialized]
    public float currentStamina;

    [SerializeField]
    private float dashSpeed, dashLength, dashCost;

    private float dashCounter, activeMoveSpeed;


    //
    [SerializeField]
    private GameObject lazermigi;
    [SerializeField]
    private Transform attackPointmigi;
    [SerializeField]
    private GameObject lazerhidari;
    [SerializeField]
    private Transform attackPointhidari;
    [SerializeField]
    private GameObject lazerue;
    [SerializeField]
    private Transform attackPointue;
    [SerializeField]
    private GameObject lazersita;
    [SerializeField]
    private Transform attackPointsita;


    [SerializeField]
    private float attackTime = 0.2f;
    private float currentAttackTime;
    private bool canAttack;

    //
    [SerializeField]
    private int hoko;
    //


    // 1
    public classChange classChange;
    // 1


    //gun

    [SerializeField]
    private Animator gunAnim;

    //gun


    //

    private Flash flash;

    void Start()
    {
        currentHealth = maxHealth;

        GameManager.instance.UpdateHealthUI();

        activeMoveSpeed = moveSpeed;

        currentStamina = totalStamina;
        GameManager.instance.UpdateStaminaUI();


        flash = GetComponent<Flash>();

        //
        currentAttackTime = attackTime;


        hoko = 1;
        //
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.statusPanel.activeInHierarchy)
        {
            return;
        }

        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

        }

        if (isknockingback)
        {
            knockbackCounter -= Time.deltaTime;
            rb.velocity = knockDir * knockbackForce;

            if(knockbackCounter <= 0)
            {
                isknockingback = false;
            }
            else
            {
                return;
            }
        }





        //Debug.Log(Time.deltaTime);
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * activeMoveSpeed;
        
        if(rb.velocity != Vector2.zero)
        {
            playerAnim.enabled = true;

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    playerAnim.SetFloat("X", 1f);
                    playerAnim.SetFloat("Y", 0);


                    weaponAnim.SetFloat("X", 1f);
                    weaponAnim.SetFloat("Y", 0);

                    gunAnim.SetFloat("X", 1f);
                    gunAnim.SetFloat("Y", 0);

                    //attackPointmigi.SetActive(true);
                    hoko = 2; // 
                }
                else
                {
                    playerAnim.SetFloat("X", -1f);
                    playerAnim.SetFloat("Y", 0);


                    weaponAnim.SetFloat("X", -1f);
                    weaponAnim.SetFloat("Y", 0);

                    gunAnim.SetFloat("X", -1f);
                    gunAnim.SetFloat("Y", 0);

                    //attackPointhidari.SetActive(true);
                    hoko = 4;
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0) 
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 1);


                weaponAnim.SetFloat("X", 0);
                weaponAnim.SetFloat("Y", 1);

                gunAnim.SetFloat("X", 0);
                gunAnim.SetFloat("Y", 1);

                //attackPointue.SetActive(true);

                hoko = 1;
            }
            else
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", -1f);


                weaponAnim.SetFloat("X", 0);
                weaponAnim.SetFloat("Y", -1f);

                gunAnim.SetFloat("X", 0);
                gunAnim.SetFloat("Y", -1f);

                //attackPointsita.SetActive(true);

                hoko = 3;
            }
        }
        else
        {
            playerAnim.enabled = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            weaponAnim.SetTrigger("Attack");

            //
            if (classChange.classgun == true)
            {
                if(canAttack  == true)
                {
                    gunAnim.SetTrigger("gunAttack");
                }
                //gunAnim.SetTrigger("gunAttack");
                if(hoko == 1)
                {
                    Debug.Log("上");
                    Attackue();
                }
                else if(hoko == 2)
                {
                    Debug.Log("右");
                    Attackmigi();
                }
                else if (hoko == 3)
                {
                    Debug.Log("下");
                    Attacksita();
                }
                else
                {
                    Debug.Log("左");
                    Attackhidari();
                }


            }

            //
        }

        //
        attackTime += Time.deltaTime;

        if (attackTime > currentAttackTime)
        {
            canAttack = true;
        }

        //



        if (dashCounter <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && currentStamina > dashCost)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;

                currentStamina -= dashCost;

                GameManager.instance.UpdateStaminaUI();
            }
        }
        else
        {
            dashCounter -= Time.deltaTime;

            if(dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina + recoverySpeed * Time.deltaTime, 0, totalStamina);

        GameManager.instance.UpdateStaminaUI();


        

    }

    /// <summary>
    /// 吹き飛ばし用の関数
    /// </summary>
    /// <param name="position"></param>
    public void KnockBack(Vector3 position)
    {
        knockbackCounter = knockbackTime;
        isknockingback = true;

        knockDir = transform.position - position;

        knockDir.Normalize();
    }

    public void DamagePlayer(int damage)
    {
        if(invincibilityCounter <= 0)
        {

            flash.PlayFeedback();
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

            invincibilityCounter = invincibilityTime;

            SoundManager.instance.PlaySE(2);

            if (currentHealth == 0)
            {
                gameObject.SetActive(false);
                SoundManager.instance.PlaySE(0);
                //GameManager.instance.Load(); // ここでMainシーンに帰ってる
                SceneManager.LoadScene("GameOver"); //これおりじなる
            }
        }

        GameManager.instance.UpdateHealthUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "portion" && maxHealth != currentHealth && collision.GetComponent<Items>().waitTime <= 0)
        {
            Items items = collision.GetComponent<Items>();

            SoundManager.instance.PlaySE(1);

            currentHealth = Mathf.Clamp(currentHealth + items.healthItemRecoveryValue, 0, maxHealth);

            GameManager.instance.UpdateHealthUI();

            Destroy(collision.gameObject);
        }
    }

    //
    void Attackue()
    {

        Debug.Log(attackPointue);

        if (canAttack)
        {
            Instantiate(lazerue, attackPointue.position, Quaternion.identity);
            canAttack = false;
            attackTime = 0f;
        }
    }
    void Attacksita()
    {


        Debug.Log(attackPointsita);

        if (canAttack)
        {
            Instantiate(lazersita, attackPointsita.position, Quaternion.identity);
            canAttack = false;
            attackTime = 0f;
        }
    }
    void Attackhidari()
    {



        Debug.Log(attackPointhidari);

        if (canAttack)
        {
            Instantiate(lazerhidari, attackPointhidari.position, Quaternion.identity);
            canAttack = false;
            attackTime = 0f;
        }
    }
    void Attackmigi()
    {



        Debug.Log(attackPointmigi);

        if (canAttack)
        {
            Instantiate(lazermigi, attackPointmigi.position, Quaternion.identity);
            canAttack = false;
            attackTime = 0f;
        }
    }


}
