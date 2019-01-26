using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Die();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().Die();
        }
    }

}
