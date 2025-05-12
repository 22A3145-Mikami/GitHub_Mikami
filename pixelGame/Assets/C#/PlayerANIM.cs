using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerANIM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
    }

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Animator anim;

    // Update is called once per frame
    void Update()
    {

    }


    public void Walk()
    {
        anim.SetBool("wasd_push", true);
    }
    public void StopWalk()
    {
        anim.SetBool("wasd_push", false);
    }

    public void Jump()
    {
        anim.SetBool("space_push", true);
    }

    public void StopJump()
    {
        anim.SetBool("space_push", false);
    }
}
