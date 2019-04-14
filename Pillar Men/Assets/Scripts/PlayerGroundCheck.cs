using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.isStandingOnGround = true;
            playerController.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.StopJumping();
            //Debug.Log("HEY STOP! " + Time.time);
        }
    }
    /*private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.StopJumping();
            Debug.Log("HEY STOP!");
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.isStandingOnGround = false;
            playerController.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }

}
