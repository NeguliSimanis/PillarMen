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
    }

    public void EnableAttack(int attackDamage)
    {
        isAttacking = true;
        dealtDamageThisAttack = false;
        damagePerAttack = attackDamage;
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
