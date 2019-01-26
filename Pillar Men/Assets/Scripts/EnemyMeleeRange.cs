using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeRange : MonoBehaviour
{
    [SerializeField]
    EnemyController enemyController;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyController.StandByForMeleeAttack(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyController.StandByForMeleeAttack(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyController.StandByForMeleeAttack(false);
        }
    }
}
