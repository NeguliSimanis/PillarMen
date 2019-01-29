using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [SerializeField]
    int bossMaxLife; // 350
    int bossCurrentLife;

    [SerializeField]
    Slider bossHPBar;
    [SerializeField]
    GameStateManager gameStateManager;

    bool isDead = false;
    float deathDelay = 3f; // delay before showing victory screen
    float victoryTime;

    Animator bossAnimator;

    #region ATTACKING
    bool canAttack = false;

    int attackTypeID;
    bool isAttacking = false;
    bool canDamagePlayer = false;

    float minDelayBetweenAttacks = 2f;
    float maxDelayBetweenAttacks = 5f;
    float nextAttackTime = -1;

    [SerializeField]
    AnimationClip attackAnimation1;
    [SerializeField]
    AnimationClip attackAnimation2;
    #endregion

    void Start()
    {
        bossCurrentLife = bossMaxLife;
        bossAnimator = GetComponent<Animator>();
    }

    public void StartAttacks()
    {
        canAttack = true;
        bossAnimator.SetTrigger("attack1");
    }

    public void Damage(int amount)
    {
        if (isDead)
            return;
        bossAnimator.SetTrigger("hurt");
        bossCurrentLife -= amount;
        UpdateHPBar();
        if (bossCurrentLife <= 0)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        bossHPBar.value = (bossCurrentLife * 1f) / bossMaxLife;
    }

    private void Die()
    {
        if (isDead)
            return;
        isDead = true;
        victoryTime = Time.time + deathDelay;   
    }

    void Update()
    {
        if (isDead && Time.time > victoryTime)
        {
            if (gameStateManager.currentState != GameStateManager.CurrentGameState.Victory)
                gameStateManager.WinGame();
        }
        if (isDead)
            return;

        if (canAttack)
        {
            if (nextAttackTime == -1)
            {
                Attack();
            }
            else if (Time.time > nextAttackTime)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        AnimationClip currentAttackAnimation; 
        attackTypeID = Random.Range((int)0,(int)2);

        if (attackTypeID == 0)
        {
            currentAttackAnimation = attackAnimation1;
            bossAnimator.SetTrigger("attack1");
        }
        else
        {
            currentAttackAnimation = attackAnimation2;
            bossAnimator.SetTrigger("attack2");
        }

        nextAttackTime = Time.time + currentAttackAnimation.length + Random.Range(minDelayBetweenAttacks, maxDelayBetweenAttacks);

        //nextAttackTime = 
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerSword"))
        {
            Sword playerSword = collision.gameObject.GetComponent<Sword>();
            if (playerSword.isAttacking)
            {
                playerSword.DealDamageToBoss(this);
            }
        }
    }
}
