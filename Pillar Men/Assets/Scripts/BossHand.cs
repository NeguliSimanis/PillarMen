using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    int bossDamage = 44;
    float damageDealDelay = 1f; // how much time must pass before player can be damaged again
    float nextDamageTime = -1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamageToPlayer(collision.gameObject.GetComponent<PlayerController>());      
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamageToPlayer(collision.gameObject.GetComponent<PlayerController>());
        }
    }

    private void DealDamageToPlayer(PlayerController playerController)
    {
        if (nextDamageTime == -1 || Time.time > nextDamageTime)
        {
            nextDamageTime = Time.time + damageDealDelay;
            playerController.TakeDamage(bossDamage);
        }
    }
}
