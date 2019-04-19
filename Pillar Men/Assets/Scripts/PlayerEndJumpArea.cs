using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndJumpArea : MonoBehaviour
{

    PlayerController playerController;
    Rigidbody2D playerRigidbody;
    Animator animator;
    bool isFlying = false;
    bool endJumpTriggerSet = false;
    void Start()
    {
       /// playerController = //gameObject.GetComponent<PlayerController>();
        playerRigidbody = gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
        playerController = gameObject.transform.parent.gameObject.GetComponent<PlayerController>();
        animator = transform.parent.gameObject.GetComponent<Animator>(); 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Ground" && isFlying)
        {
            isFlying = false;
            EndJumpAnimationTrigger();
            animator.SetBool("isFlying", isFlying);
            playerController.StopJumping();
            // Debug.Log("must end jump " + Time.time);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       // Debug.Log("stop man0! " + Time.time + " " + collision.gameObject.layer);
        if (collision.gameObject.layer == 10)
        {
            endJumpTriggerSet = false;
            isFlying = true;
            playerController.isStandingOnGround = false;
            animator.SetBool("isFlying", isFlying);
            animator.ResetTrigger("endJump");
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        // Debug.Log("stop man0! " + Time.time + " " + collision.gameObject.layer);
        if (collision.gameObject.layer == 10)
        {
            //Debug.Log("stop man0! " + Time.time + " " + collision.gameObject.layer);
            playerController.hasDoubleJumped = false;
            isFlying = false;
            playerController.isStandingOnGround = true;
            playerController.StopJumping();
            animator.SetBool("isFlying", isFlying);
            EndJumpAnimationTrigger();
        }
    }

    private void EndJumpAnimationTrigger()
    {
        if (!endJumpTriggerSet && playerController.gameObject.GetComponent<Rigidbody2D>().velocity.y <=0)
        {
            animator.SetTrigger("endJump");
            endJumpTriggerSet = true;
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
       // Debug.Log("stop man0! " + Time.time + " " + collision.gameObject.layer);
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("stop man1!" + Time.time);
            if (playerRigidbody.velocity.y <= 0)
            {
                Debug.Log("stop man!" + Time.time);
                playerController.StopJumping();
            }
        }
    }*/
}
