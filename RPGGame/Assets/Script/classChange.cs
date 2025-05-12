using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class classChange : MonoBehaviour
{
    [SerializeField]
    private GameObject sword;

    [SerializeField]
    private GameObject Gun;

    [SerializeField]
    private GameObject swordHolder;

    [SerializeField]
    private GameObject gunHolder;

    [SerializeField]
    private GameObject Tamami;
    [SerializeField]
    private GameObject Tamahi;
    [SerializeField]
    private GameObject Tamau;
    [SerializeField]
    private GameObject Tamasi;

    public bool classgun;

    public GameObject leftClick;
    public bool leftClick_bool;

    // Start is called before the first frame update
    void Start()
    {
        swordHolder.SetActive(false);

        gunHolder.SetActive(false);

        Tamami.SetActive(false);
        Tamahi.SetActive(false);
        Tamau.SetActive(false);
        Tamasi.SetActive(false);

        classgun = false;

        leftClick_bool = false;
        leftClick.SetActive(leftClick_bool);
    }


    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "sword")
        {
            swordHolder.SetActive(true);

            leftClick_bool = true;
            leftClick.SetActive(leftClick_bool);

            //
            if (PlayerPrefs.HasKey("weapon.attackDamage"))
            {

                PlayerPrefs.GetInt("weapon.attackDamage");
            }
            //

            gunHolder.SetActive(false);

            Tamami.SetActive(false);
            Tamahi.SetActive(false);
            Tamau.SetActive(false);
            Tamasi.SetActive(false);

            classgun = false;
        }

        if(collision.tag == "gun")
        {
            gunHolder.SetActive(true);

            leftClick_bool = true;
            leftClick.SetActive(leftClick_bool);

            Tamami.SetActive(true);
            Tamahi.SetActive(true);
            Tamau.SetActive(true);
            Tamasi.SetActive(true);

            classgun = true;

            swordHolder.SetActive(false);


        }
    }




    
}
