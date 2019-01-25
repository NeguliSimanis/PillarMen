using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
            
    private Rigidbody2D rigidBody2D;
    bool isDead = false;

    #region Managers
    [SerializeField]
    GameStateManager gameStateManager;
    #endregion

    #region Movement
    bool isNearTargetPosition = false;
    bool isWalking = false;

    public float playerSpeed;

    private Vector2 targetPosition;
    private Vector2 dirNormalized;
    #endregion

    #region Jumping
    bool isJumping = false;
    float jumpForce = 350;
    #endregion

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (gameStateManager.currentState == GameStateManager.CurrentGameState.Paused ||
            gameStateManager.currentState == GameStateManager.CurrentGameState.Intro)
            return;

        if (isDead)
            return;

        ManageLeftMouseInput();
        ManageSpaceInput();
        if (isWalking)
        {
            CheckIfPlayerNearTargetPosition();
        }
    }

    void ManageSpaceInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    void ManageLeftMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GetTargetPositionAndDirection();
                CheckIfPlayerNearTargetPosition();
                if (!isNearTargetPosition)
                {
                    isWalking = true;
                }
            }
            else
            {
                Debug.Log("cant move if clicking on UI");
            }
        }
    }
    
    void FixedUpdate()
    {
        if (isWalking)
        {
            Walk(); 
        }
    }

    void Walk()
    {
        rigidBody2D.AddForce(dirNormalized * playerSpeed);
        GetDirNormalized(targetPosition);
    }

    void Jump()
    {
        isJumping = true;
        rigidBody2D.AddForce(new Vector2(0, jumpForce));
    }

    void GetTargetPositionAndDirection()
    {
        Debug.Log("getting target position");
        targetPosition = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        GetDirNormalized(targetPosition);
    }

    void GetDirNormalized(Vector2 sourceVector)
    {
        dirNormalized = new Vector2(sourceVector.x - transform.position.x, sourceVector.y - transform.position.y);
        dirNormalized = dirNormalized.normalized;
    }

    void CheckIfPlayerNearTargetPosition()
    {
        if (Vector2.Distance(targetPosition, transform.position) <= 0.2f)
        {
            isNearTargetPosition = true;
            StopWalking();
        }
        else
        {
            isNearTargetPosition = false;
        }
    }

    void StopWalking()
    {
        Debug.Log("STOP WALKING");
        isWalking = false;
        rigidBody2D.velocity = Vector2.zero;
        //rigidBody2D.angularVelocity = Vector2.zero; 
    }

    void  StopJumping()
    {
        StopWalking();
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
        Debug.Log("im player and im ded");
    }
        
}
