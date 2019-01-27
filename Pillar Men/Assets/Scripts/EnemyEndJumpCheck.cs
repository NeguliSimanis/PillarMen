using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEndJumpCheck : MonoBehaviour
{
    [SerializeField]
    EnemyController enemyController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyController.EndJump();
        }
    }

 
}
