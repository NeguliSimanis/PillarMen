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
           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.StopJumping();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.isStandingOnGround = false;
        }
    }

}
