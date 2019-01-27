using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpArea : MonoBehaviour
{
    [SerializeField]
    bool isJumpRightZone;
    [SerializeField]
    bool isJumpLeftZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().canJumpLeft = isJumpLeftZone;
            collision.gameObject.GetComponent<EnemyController>().canJumpRight = isJumpRightZone;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().canJumpLeft = isJumpLeftZone;
            other.gameObject.GetComponent<EnemyController>().canJumpRight = isJumpRightZone;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().canJumpLeft = false;
            collision.gameObject.GetComponent<EnemyController>().canJumpRight = false;
        }
    }
}
