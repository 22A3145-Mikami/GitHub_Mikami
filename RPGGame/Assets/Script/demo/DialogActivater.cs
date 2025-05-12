using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogActivater : MonoBehaviour
{
    [SerializeField, Header("会話文章"), Multiline(3)]
    private string[] lines;

    private bool canActivater;

    [SerializeField]
    private bool savePoint;

    [SerializeField]
    GameObject rightclick;
    [SerializeField]
    classChange cChange;
    [SerializeField]
    GameObject eButton;
    [SerializeField]
    GameObject move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cChange.leftClick.SetActive(false);
            eButton.SetActive(false);
            move.SetActive(false);

            if (canActivater && !GameManager.instance.dialogBox.activeInHierarchy)
            {
                GameManager.instance.ShowDialog(lines);

                if (savePoint)
                {
                    GameManager.instance.SaveStatus();

                    Debug.Log("セーブおk");
                }
            }
        }
        else if(!canActivater)
        {
            cChange.leftClick.SetActive(cChange.leftClick_bool);
            eButton.SetActive(true);
            move.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canActivater = true;
            rightclick.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canActivater = false;

            GameManager.instance.ShowDialogChange(canActivater);

            rightclick.SetActive(false);
        }
    }
}
