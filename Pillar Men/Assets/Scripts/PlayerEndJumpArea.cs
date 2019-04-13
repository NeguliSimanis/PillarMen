using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndJumpArea : MonoBehaviour
{

    PlayerController playerController;
    Animator animator;

    void Start()
    {
       // playerController = gameObject.GetComponent<PlayerController>();
        animator = transform.parent.gameObject.GetComponent<Animator>(); 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Ground")
        {
            animator.SetTrigger("endJump");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("isFlying", true);
        }
    }

}
