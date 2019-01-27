using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
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
    public bool isStandingOnGround = true;
    bool isJumping = false;
    bool hasDoubleJumped = false;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float doubleJumpForce;

    [SerializeField]
    Collider2D[] collidersToDisableWhileJumping;
    #endregion

    #region ANIMATION
    Animator animator;
    #endregion
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
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
        /*if (isWalking)
        {
            CheckIfPlayerNearTargetPosition();
            Debug.Log("Target walking position " + targetPosition);
        }*/
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
        playerSword.EnableAttack(PlayerData.current.meleeDamage);
    }

    void ManageJumpInput()
    {
        if ( Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
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
        /*if (isWalking)
        {
            MouseWalk();
        }*/
    }

    /*void MouseWalk()
    {
        rigidBody2D.AddForce(dirNormalized * playerSpeed);
        //GetDirNormalized(targetPosition);
    }*/

    void Jump()
    {
        if (hasDoubleJumped)
            return;
        // double jump
        if (isJumping)
        {
            animator.SetTrigger("doubleJump");
            hasDoubleJumped = true;
        }
        // regular jump
        else
        {
            animator.SetBool("isJumping", true);
            isJumping = true;
        }
        DisableCollisionsWithPlatforms(true);
        rigidBody2D.AddForce(new Vector2(0, jumpForce));
    }

    void DisableCollisionsWithPlatforms(bool disable)
    {
        Physics.IgnoreLayerCollision(8, 11, !disable);
        /*foreach (Collider2D collider in collidersToDisableWhileJumping)
        {
            collider.enabled = !disable;
        }*/
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
        //StopWalking();
        //Debug.Log("Stop jumping " + Time.time);
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

        // ARTIFACT FROM MOUSE CONTROLS
        /*if (isFacingRight && dirNormalized.x < 0)
        {
            isFacingRight = false;
            gameObject.transform.localScale = new Vector2(-1f, 1f);
        }
        else if (!isFacingRight && dirNormalized.x > 0)
        {
            isFacingRight = true;
            gameObject.transform.localScale = new Vector2(1f, 1f);
        }*/
    }

}
