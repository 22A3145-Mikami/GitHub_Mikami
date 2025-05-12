using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;

    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Slider staminaSlider;


    public GameObject dialogBox;
    public Text dialogText;

    private string[] dialogLines;

    private int currentLine;

    private bool justStarted;


    public GameObject statusPanel;

    [SerializeField]
    private Text hptext, sttext, attext, gunattext;

    [SerializeField]
    private Weapon weapon;


    private int totalExp, currentLV;

    [SerializeField, Tooltip("レベルアップに必要な経験値")]
    private int[] requiredExp;

    [SerializeField]
    private GameObject levelUpText;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private EnemyKill enemyKill;

    //[SerializeField]
    //private Gunweapon gunweaponu;

    //[SerializeField]
    //private Gunweapon gunweaponsi;

    //[SerializeField]
    //private Gunweapon gunweaponhi;

    //[SerializeField]
    //private Gunweapon gunweaponmi;

    [SerializeField]
    private Toukatugun toukatugun;

    bool getKey_E = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("MaxHp"))
        {

            LoadStatus();
        }


        
    }

    // Update is called once per frame
    void Update()
    {

        

        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(1))
            {
                SoundManager.instance.PlaySE(4);
                if (!justStarted)
                {
                    currentLine++;

                    //Debug.Log(currentLine);
                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                    }
                    else
                    {
                        dialogText.text = dialogLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false;
                }

                //
                if (enemyKill.sinkae == true && currentLine == dialogLines.Length)
                {
                    SceneManager.LoadScene("GameClear");
                }
                //
            }
        }
        //if()
        //{
        //    sinkae = true;
        //}
        //else
        //{
        //    sinkae = false;
        //}

        

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!getKey_E)
            { 
                ShowStatusPanel();
            }
            else
            {
                CloseStatusPanel();
            }
            
        }
    }

    public void UpdateHealthUI()
    {
        hpSlider.maxValue = player.maxHealth;
        hpSlider.value = player.currentHealth;

    }

    public void UpdateStaminaUI()
    {
        staminaSlider.maxValue = player.totalStamina;
        staminaSlider.value = player.currentStamina;

    }

    public void ShowDialog(string[] lines)
    {
        dialogLines = lines;

        currentLine = 0;

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        justStarted = true;
    }

    public void ShowDialogChange(bool x)
    {
        dialogBox.SetActive(x);
    }


    public void Load()
    {
        SceneManager.LoadScene("Main");
    }

    public void ShowStatusPanel()
    {
        getKey_E = true;

        statusPanel.SetActive(true);

        Time.timeScale = 0f;

        //UI更新用関数の呼び出し
        StatusUpdate();
    }

    public void CloseStatusPanel()
    {
        getKey_E = false;

        statusPanel.SetActive(false);

        Time.timeScale = 1f;
    }


    public void StatusUpdate()
    {
        hptext.text = "体力　：" + player.maxHealth;
        sttext.text = "スタミナ　：" + player.totalStamina;
        attext.text = "攻撃力(剣)　：" + weapon.attackDamage;
        gunattext.text = "攻撃力(銃)　：" + toukatugun.damage;

    }


    public void AddExp(int exp)
    {
        if (requiredExp.Length <= currentLV)
        {
            return;
        }

        totalExp += exp;

        if (totalExp >= requiredExp[currentLV])
        {
            currentLV++;

            player.maxHealth += 5;
            player.totalStamina += 5;
            weapon.attackDamage += 2;
            //gunweaponu.attackDamage += 2;
            //gunweaponsi.attackDamage += 2;
            //gunweaponhi.attackDamage += 2;
            //gunweaponmi.attackDamage += 2;
            toukatugun.damage += 2;


            GameObject levelUp = Instantiate(levelUpText, player.transform.position, Quaternion.identity);
            levelUp.transform.SetParent(player.transform);
            //levelUp.transform.localPosition = player.transform.position + new Vector3(0, 100, 0);
        }
    }

    public void SaveStatus()
    {
        PlayerPrefs.SetInt("MaxHp", player.maxHealth);
        PlayerPrefs.SetFloat("MaxSt", player.totalStamina);
        PlayerPrefs.SetInt("weapon.attackDamage", weapon.attackDamage);
        //PlayerPrefs.SetInt("gunweaponu.attackDamage", gunweaponu.attackDamage);
        //PlayerPrefs.SetInt("gunweaponsi.attackDamage", gunweaponsi.attackDamage);
        //PlayerPrefs.SetInt("gunweaponhi.attackDamage", gunweaponhi.attackDamage);
        //PlayerPrefs.SetInt("gunweaponmi.attackDamage", gunweaponmi.attackDamage);
        PlayerPrefs.SetInt("gunweapon.attackDamage", toukatugun.damage);
        PlayerPrefs.SetInt("Level", currentLV);
        PlayerPrefs.SetInt("Exp", totalExp);
    }

    public void LoadStatus()
    {
        player.maxHealth = PlayerPrefs.GetInt("MaxHp");
        player.totalStamina = PlayerPrefs.GetFloat("MaxSt");
        weapon.attackDamage = PlayerPrefs.GetInt("weapon.attackDamage");
        //gunweaponu.attackDamage = PlayerPrefs.GetInt("gunweaponu.attackDamage");
        //gunweaponsi.attackDamage = PlayerPrefs.GetInt("gunweaponsi.attackDamage");
        //gunweaponhi.attackDamage = PlayerPrefs.GetInt("gunweaponhi.attackDamage");
        //gunweaponmi.attackDamage = PlayerPrefs.GetInt("gunweaponmi.attackDamage");
        toukatugun.damage = PlayerPrefs.GetInt("gunweapon.attackDamage");
        currentLV = PlayerPrefs.GetInt("Level");
        totalExp = PlayerPrefs.GetInt("Exp");

        //if(weapon.attackDamage == 10)
        //{
        //    gunweaponu.attackDamage = 10;
        //}
    }

}
