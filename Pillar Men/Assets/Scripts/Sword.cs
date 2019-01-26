using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    bool isPlayerSword = true;
    bool dealtDamageThisAttack = false;
    public bool isAttacking = false;
    int damagePerAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isPlayerSword)
        {
            if (isAttacking)
            {
                DealDamageToEnemy(collision.gameObject.GetComponent<EnemyController>());
            }
        }
        else if (collision.gameObject.CompareTag("Player") && !isPlayerSword)
        {
            if (isAttacking)
            {
                DealDamageToPlayer(collision.gameObject.GetComponent<PlayerController>());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isPlayerSword)
        {
            if (isAttacking)
            {
                DealDamageToEnemy(collision.gameObject.GetComponent<EnemyController>());
            }
        }
        else if (collision.gameObject.CompareTag("Player") && !isPlayerSword)
        {
            if (isAttacking)
            {
                Debug.Log("Deal damage to player");
                DealDamageToPlayer(collision.gameObject.GetComponent<PlayerController>());
            }
        }
    }

    public void EnableAttack(int attackDamage)
    {
        isAttacking = true;
        dealtDamageThisAttack = false;
        damagePerAttack = attackDamage;
    }

    private void DealDamageToPlayer(PlayerController playerController)
    {
        if (dealtDamageThisAttack)
            return;
        dealtDamageThisAttack = true;
        isAttacking = false;
        playerController.TakeDamage(damagePerAttack);
    }

    private void DealDamageToEnemy(EnemyController enemyToDamage)
    {
        if (dealtDamageThisAttack)
            return;
        dealtDamageThisAttack = true;
        isAttacking = false;
        enemyToDamage.Damage(damagePerAttack);
    }
}
