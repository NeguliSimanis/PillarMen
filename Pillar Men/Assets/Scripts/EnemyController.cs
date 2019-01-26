﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    int maxHealth = 20;
    int currentHealth;

    Rigidbody2D rigidBody2D;

    #region DEATH
    bool isDying = false;
    float deathTime;
    [SerializeField]
    AnimationClip deathAnimation;
    #endregion

    #region ENEMY SIGHT & targetting
    PlayerController playerController;
    Transform playerTransform;
    private bool isPlayerVisible = false;
    private bool isPlayerNear = false;
    private bool isFollowingPlayer = false;
    private Vector2 targetPosition;
    private Vector2 dirNormalized;
    #endregion

    #region MOVEMENT
    private float enemySpeed = 10;
    private float maxVelocity = 1;
    private bool isFacingRight = false;
    #endregion

    #region ENEMY ATTACK
    private int enemyDamage = 22;
    bool isAttackingPlayer = false;
    private float nextAttackTime;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private Sword enemyWeapon;
    #endregion

    #region ANIMATION
    Animator enemyAnimator;
    [SerializeField]
    AnimationClip attackAnimation;
    #endregion
    private void Start()
    {
        currentHealth = maxHealth;
        enemyAnimator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerTransform = playerController.gameObject.transform;
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            isDying = true;
            enemyAnimator.SetTrigger("die");
            deathTime = Time.time + deathAnimation.length;
        }
    }

    private void Update()
    {
        if (!isDying)
            return;
        if (Time.time > deathTime)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        //Debug.Log(rigidBody2D.velocity);
        if (isPlayerVisible && !isPlayerNear)
        {
            if (!isPlayerNear)
                FollowPlayer();
        }

        if (isFollowingPlayer &&
            (rigidBody2D.velocity.x > 0 || rigidBody2D.velocity.x < 0))
        {
            CheckWhereEnemyIsFacing();
        }

        if (isAttackingPlayer)
        {
            if (nextAttackTime < Time.time)
            {
                MeleeAttack(false);
            }
        }
    }

    private void CheckWhereEnemyIsFacing()
    {
        if (!isFollowingPlayer)
            return;
        if (isFacingRight && rigidBody2D.velocity.x < 0)
        {
            isFacingRight = false;
            gameObject.transform.localScale = new Vector2(-1f, 1f);
        }
        else if (!isFacingRight && rigidBody2D.velocity.x > 0)
        {
            isFacingRight = true;
            gameObject.transform.localScale = new Vector2(1f, 1f);
        }
    }

    public void NoticePlayer(bool notice)
    {
        if (!isPlayerVisible && notice)
        {
            //Debug.Log("Player noticed!");
            isPlayerVisible = notice;
        }
        else if (!notice)
        {
            isPlayerVisible = false;
        }    
    }

    public void StandByForMeleeAttack(bool nearPlayer)
    {
        // stop to attack
        if (!isPlayerNear && nearPlayer)
        {
            isPlayerNear = true;
            isFollowingPlayer = false;
            if (!isAttackingPlayer)
                MeleeAttack(true);
            isAttackingPlayer = true;
            rigidBody2D.velocity = Vector2.zero;
        }
        else if (!nearPlayer)
        {
            isPlayerNear = nearPlayer;
        }
    }

    /// <summary>
    /// Attacks player immediately if this is the first time method is called
    /// Attacks after cooldown elsewise
    /// </summary>
    /// <param name="firstAttack">true if this is the first method call</param>
    void MeleeAttack(bool firstAttack)
    {
        if (firstAttack || Time.time > nextAttackTime)
            enemyAnimator.SetTrigger("attack");
        nextAttackTime = Time.time + attackAnimation.length + attackCooldown;
        enemyWeapon.EnableAttack(enemyDamage);
    }

    void GetTargetPositionAndDirection()
    {
        dirNormalized = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
        dirNormalized = dirNormalized.normalized;
    }

    private void MoveEnemy()
    {
        if (rigidBody2D.velocity.x < maxVelocity && rigidBody2D.velocity.x > -maxVelocity)
            rigidBody2D.AddForce(dirNormalized * enemySpeed);
    }

    private void FollowPlayer()
    {
        targetPosition = playerTransform.position;
        isFollowingPlayer = true;
        GetTargetPositionAndDirection();
        MoveEnemy();
    }
}