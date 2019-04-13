﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    bool physicsMovementEnabled = false;

    #region PHYSICS
    [SerializeField]
    PhysicsMaterial2D playerSlipperyMaterial;
    [SerializeField]
    PhysicsMaterial2D playerNormalMaterial;
    Collider2D playerCollider;
    #endregion

    private Rigidbody2D rigidBody2D;
    bool isDead = false;

    #region Managers
    [SerializeField]
    GameStateManager gameStateManager;
    #endregion

    #region UI
    [SerializeField]
    Slider healthBar;
    float healthBarMaxWidth;
    #endregion

    #region MELEE ATTACK
    //float meleeAttackCooldown;
    [SerializeField]
    Sword playerSword;
    float nextMeleeAttackTime = -1;
    [SerializeField]
    AnimationClip meleeAttackAnimation;
    #endregion

    #region Movement
    bool isMoveFrozen = false;
    float freezeMovementEndTime;

    float horizontalMoveInput = 0;
    bool isFacingRight = true;
    bool isNearTargetPosition = false;
    bool isWalking = false;

    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerMaxSpeedXaxis = 3;

    private Vector2 targetPosition;
    private Vector2 dirNormalized;
    #endregion

    #region Jumping
    // 13 April
    private bool canJumpNow = false; // becomes false in most collisions, becomes true on player input
    private float yJumpHeight = 5f;
    private float targetHeight;
    // eof

    public bool isStandingOnGround = true;
    bool isJumping = false;
    bool hasDoubleJumped = false;
    [SerializeField]
    float jumpForce;
    // doubleJumpForce;

    [SerializeField]
    Collider2D[] collidersToDisableWhileJumping;
    #endregion

    #region ANIMATION
    Animator animator;
    #endregion

    #region AUDIO
    AudioSource audioSource;
    [SerializeField]
    AudioClip attackSFX;
    float attackSFXDelay = 0.1f;
    #endregion
    void Start()
    {
        Debug.Log(gameObject.GetComponent<SpriteRenderer>().color);

        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        //healthBarMaxWidth = healthBar.rectTransform.rect.width;

        if (PlayerData.current == null)
            PlayerData.current = new PlayerData();

        PlayerData.current.currentLife = PlayerData.current.maxLife;
    }

    void UpdateHUD()
    {
        healthBar.value = (PlayerData.current.currentLife * 1f) / PlayerData.current.maxLife;
    }

    void Update()
    {
        //Debug.Log(rigidBody2D.velocity + " " + Time.time);
        if (gameStateManager.currentState == GameStateManager.CurrentGameState.Paused ||
            gameStateManager.currentState == GameStateManager.CurrentGameState.Intro ||
            gameStateManager.currentState == GameStateManager.CurrentGameState.Defeat ||
            gameStateManager.currentState == GameStateManager.CurrentGameState.Victory)
            return;

        UpdateHUD();

        if (isDead)
            return;

        if (!isMoveFrozen)
        {
            //ManageLeftMouseInput();
            ManageAttackInput();
            ManageHorizontalMoveInput();
            ManageJumpInput();
        }
        else if (Time.time > freezeMovementEndTime)
        {
            isMoveFrozen = false;
        }
    }

    void ManageAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0))
        {
            if (nextMeleeAttackTime == -1)
            {
                MeleeAttack();
            }
            else if (Time.time > nextMeleeAttackTime)
            {
                MeleeAttack();
            }         
        }
    }

    void MeleeAttack()
    {
        nextMeleeAttackTime = Time.time + meleeAttackAnimation.length;
        animator.SetTrigger("attack");
        StartCoroutine(PlayAttackSFXAfterDelay());
        playerSword.EnableAttack(PlayerData.current.meleeDamage);
    }

    private IEnumerator PlayAttackSFXAfterDelay()
    {
        yield return new WaitForSeconds(attackSFXDelay);
        audioSource.PlayOneShot(attackSFX);
    }

    void ManageJumpInput()
    {
        if ( Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (isJumping)
        {
            MoveUpwards();
        }
    }

    void ManageHorizontalMoveInput()
    {
        if (Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.D))
        {
            horizontalMoveInput = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.A))
        {
            horizontalMoveInput = -1;
        }
        else
        {
            horizontalMoveInput = 0;
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(rigidBody2D.velocity);
        CheckWherePlayerIsFacing();

        // horizontal movement
        Vector2 movement = new Vector2(horizontalMoveInput, 0);
        if (rigidBody2D.velocity.x < playerMaxSpeedXaxis && rigidBody2D.velocity.x > -playerMaxSpeedXaxis)
        {
            if (!isMoveFrozen)
                rigidBody2D.AddForce(movement * playerSpeed);
        }

        if (rigidBody2D.velocity.x > 0.01 || rigidBody2D.velocity.x < -0.01)
        {
            animator.SetBool("isRunning", true);
            CheckWherePlayerIsFacing();
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        // player is pressing move key, but is stuck
        if (horizontalMoveInput > 0 || horizontalMoveInput < 0)
        {
            playerCollider.sharedMaterial = playerSlipperyMaterial;
            //rigidBody2D.AddForce(new Vector2(0, playerSpeed * 2));
        }
        else
        {
            playerCollider.sharedMaterial = playerNormalMaterial;
        }
    }

    void Jump()
    {
        if (hasDoubleJumped)
            return;
        DisableCollisionsWithPlatforms(true);
        // double jump
        if (isJumping)
        {
            animator.SetTrigger("doubleJump");
            hasDoubleJumped = true;
            SetJumpTargetPosition();
        }
        // regular jump
        else
        {
            animator.SetBool("isJumping", true);
            isJumping = true;
        }
    }

    public void SetJumpTargetPosition()
    {
        canJumpNow = true;
        gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + yJumpHeight);
        //rigidBody2D.AddForce(new Vector2(0, jumpForce));
    }

    private void MoveUpwards()
    {
        if (!canJumpNow)
            return;
    }


    void DisableCollisionsWithPlatforms(bool disable)
    {
        Physics.IgnoreLayerCollision(8, 11, !disable);
    }

    void GetTargetPositionAndDirection()
    {
        Debug.Log("getting target position");
        targetPosition = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        GetDirNormalized(targetPosition);
    }

    void Teleport()
    {
        targetPosition = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        transform.position = targetPosition;
        rigidBody2D.velocity = Vector2.zero;
    }

    void GetDirNormalized(Vector2 sourceVector)
    {
        dirNormalized = new Vector2(sourceVector.x - transform.position.x, sourceVector.y - transform.position.y);
        dirNormalized = dirNormalized.normalized;
    }

    void CheckIfPlayerNearTargetPosition()
    {
        if (Vector2.Distance(targetPosition, transform.position) <= 0.12f)
        {
            isNearTargetPosition = true;
            //StopWalking();
        }
        else
        {
            isNearTargetPosition = false;
        }
    }

    /*void StopWalking()
    {
        Debug.Log("STOP WALKING " + Time.time);
        isWalking = false;
        rigidBody2D.velocity = Vector2.zero;
        //rigidBody2D.angularVelocity = Vector2.zero; 
    }*/

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;
        animator.SetTrigger("hurt");
        PlayerData.current.currentLife -= amount;
        if (PlayerData.current.currentLife <= 0)
        {
            Die();
        }
    }
    
    public void  StopJumping()
    {
        isJumping = false;
        hasDoubleJumped = false;
        animator.SetBool("isJumping", false);
        DisableCollisionsWithPlatforms(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            StopJumping();
        }
    }

    public void Die()
    {
        gameStateManager.LoseGame();
    }

    public void FreezeMovement(float unFreezeTime)
    {
        isMoveFrozen = true;
        rigidBody2D.velocity = Vector2.zero;
        playerCollider.sharedMaterial = playerNormalMaterial;
        freezeMovementEndTime = unFreezeTime;
    }

    void CheckWherePlayerIsFacing()
    {
        if (isFacingRight && horizontalMoveInput < 0)
        {
            isFacingRight = false;
            gameObject.transform.localScale = new Vector2(-1f, 1f);
        }
        else if (!isFacingRight && horizontalMoveInput > 0)
        {
            isFacingRight = true;
            gameObject.transform.localScale = new Vector2(1f, 1f);
        }
    }

}
