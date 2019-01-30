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
    float maxDelayBetweenAttacks = 4f;
    float nextAttackTime = -1;

    [SerializeField]
    AnimationClip attackAnimation1;
    [SerializeField]
    AnimationClip attackAnimation2;
    [SerializeField]
    AnimationClip deathAnimation;
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
        bossAnimator.SetTrigger("die");
        victoryTime = Time.time + deathDelay + deathAnimation.length;   
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

    #region WOUNDED ANIMATION
    /*
 * color switch 
 * default - RGBA(1.000, 1.000, 1.000, 1.000)
 * hurt - RGBA(1.000, 0.684, 0.684, 1.000)
 */

    /*void Test()
    {
        
        animation.Play("boss_hurt");
        animation["boss_hurt"].layer = 1;
        
    }*/

    /*float woundedAnimDelay = 0.1f;
    float animFadeSpeed = 0.1f;
    int animRepeatTimes = 3;
    [SerializeField]
    SpriteRenderer[] bodyPartsToAnimate;

    void Test()
    {
        bodyPartsToAnimate[0].color = Color.red;
        bodyPartsToAnimate[1].color = Color.red;
        bodyPartsToAnimate[2].color = Color.red;
        bodyPartsToAnimate[3].color = Color.red;

        foreach (SpriteRenderer bossBodyPart in bodyPartsToAnimate)
        {
            Debug.Log("im workig");
            bossBodyPart.color = Color.red;
        }
    }

    private IEnumerator StartWoundedAnimAfterDelay()
    {
        
        Debug.Log("coroutine started");
        yield return new WaitForSeconds(woundedAnimDelay);
        Debug.Log("begin fade");
        FadeToColor(bodyPartsToAnimate[0].color, new Color(1.000f, 0.684f, 0.684f));
    }

    public IEnumerator FadeToColor(Color startColor, Color endColor, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            Color currentColor = new Color
                (Mathf.Lerp(startColor.r, endColor.r, percentageComplete),
                Mathf.Lerp(startColor.g, endColor.g, percentageComplete),
                Mathf.Lerp(startColor.b, endColor.b, percentageComplete));

            foreach (SpriteRenderer bossBodyPart in bodyPartsToAnimate)
            {
                bossBodyPart.color = currentColor;
            }

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
    }*/

    #endregion


}
