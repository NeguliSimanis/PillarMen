using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    bool isBossActive = false;
    [SerializeField]
    GameObject boss;
    [SerializeField]
    AudioClip bossSFX;

    [SerializeField]
    CameraFollow cameraController;
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    Transform playerTeleportLocation; // where teleport player when he enters boss zone
    [SerializeField]
    PlayerController playerController;
    float playerFreezeDuration;


    [SerializeField]
    AnimationClip bossAppearAnimation;
    [SerializeField]
    GameObject bossHPBar;
    [SerializeField]
    MusicManager musicManager;
    [SerializeField]
   

    float screenShakeDuration = 2.15f;
    float screenShakeMagnitude = 0.03f;

    

    #region moving platforms
    bool movePlatformSet1 = false;
    bool moveAllPlatformSets = false;
    [SerializeField]
    GameObject platformSet1;
    [SerializeField]
    GameObject platformSet2;
    [SerializeField]
    GameObject platformSet3;
    [SerializeField]
    Transform platformSet3InitialPos;
    [SerializeField]
    Transform platformSet1TargetPos; // once platform set 1 reaches this, start moving both set 1 and 2

    float platformMoveSpeed1 = -0.66f; // speed at which platform set 1 moves
    float platformMoveSpeed2 = -2.5f; // speed at which platform set 1 and move together
    float platformMoveSpeedIncrease = -0.95f;

    bool isPlatformDestroyedAtLeastOnce = false;
    [SerializeField]
    Transform platformDestroyPos; // destroy a platform once it reaches this point
    [SerializeField]
    Transform firstPlatformDestroyPos; // only the very first platform that is destroyed will be destroyed here
    #endregion

    private void Start()
    {
        //playerController.gameObject.transform.position = playerTeleportLocation.position;
        playerFreezeDuration = bossAppearAnimation.length;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBossActive)
            return;
        if (other.gameObject.CompareTag("Player"))
        {
            ActivateBoss();
        }
    }

    void ActivateBoss()
    {
        // MANAGE BOSS
        bossHPBar.SetActive(true);
        boss.SetActive(true);
        isBossActive = true;
        musicManager.SetBossMusic();//audioSource.PlayOneShot(bossSFX);

        // MANAGE PLAYER
        RelocatePlayerToBossZone();
        //StartCoroutine(StartMovingBossPlatformsAfterDelay());
        
        // MANAGE CAMERA SHAKE
        cameraController.ScreenShake(bossAppearAnimation.length, screenShakeMagnitude);

        // MANAGE PLATFORMS
        movePlatformSet1 = true;
        //bossPlatformAnimator.enabled = true;
    }

    void RelocatePlayerToBossZone()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.FreezeMovement(Time.time + playerFreezeDuration);
        playerController.gameObject.transform.position = playerTeleportLocation.position;
    }

    private IEnumerator StartMovingBossPlatformsAfterDelay()
    {
        yield return new WaitForSeconds(playerFreezeDuration);
        movePlatformSet1 = true;
    }

    private void Update()
    {
        if (movePlatformSet1)
        {
            MovePlatformSet1();
        }
        else if (moveAllPlatformSets)
        {
            MoveAllPlatformSets();
        }
    }

    private void MovePlatformSet1()
    {
        if (platformSet1.transform.position.x <= platformSet1TargetPos.transform.position.x)
        {
            movePlatformSet1 = false;
            moveAllPlatformSets = true;
            return;
        }

        platformSet1.transform.position = new Vector2(platformSet1.transform.position.x + platformMoveSpeed1 * Time.deltaTime,
            platformSet1.transform.position.y);

            //transform.position = new Vector2(transform.position.x, transform.position.y) + dirNormalized * PlayerData.current.moveSpeed * Time.deltaTime;
    }

    private void MoveAllPlatformSets()
    {
        platformSet1.transform.position = new Vector2(platformSet1.transform.position.x + platformMoveSpeed2 * Time.deltaTime,
             platformSet1.transform.position.y);
        platformSet2.transform.position = new Vector2(platformSet2.transform.position.x + platformMoveSpeed2 * Time.deltaTime,
             platformSet2.transform.position.y);

        // SECOND PLATFORM SET
        if (!isPlatformDestroyedAtLeastOnce && platformSet2.transform.position.x <= firstPlatformDestroyPos.position.x)     
        {
            isPlatformDestroyedAtLeastOnce = true;
            Destroy(platformSet2.gameObject);
            platformSet2 = Instantiate(platformSet3, platformSet3InitialPos);
        }
        else if (platformSet2.transform.position.x <= platformDestroyPos.position.x)
        {
            Destroy(platformSet2.gameObject);
            platformSet2 = Instantiate(platformSet3, platformSet3InitialPos);
            platformMoveSpeed2 += platformMoveSpeedIncrease;
        }

        // FIRST PLATFORM SET
        if (platformSet1.transform.position.x <= platformDestroyPos.position.x)
        {
            Destroy(platformSet1.gameObject);
            platformSet1 = Instantiate(platformSet3, platformSet3InitialPos);
            platformMoveSpeed2 += platformMoveSpeedIncrease;
        }
    }
}
