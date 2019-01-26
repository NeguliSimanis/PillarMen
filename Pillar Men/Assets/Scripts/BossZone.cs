using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    [SerializeField]
    GameObject boss;

    [SerializeField]
    AudioClip bossSFX;
    [SerializeField]
    CameraFollow cameraController;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    float playerFreezeDuration;
    [SerializeField]
    AnimationClip bossAppearAnimation;

    float screenShakeDuration = 2.15f;
    float screenShakeMagnitude = 0.03f;

    bool isBossActive = false;

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
        boss.SetActive(true);
        isBossActive = true;
        audioSource.PlayOneShot(bossSFX);

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.FreezeMovement(Time.time + playerFreezeDuration);
        //cameraController.ScreenShake(screenShakeDuration,screenShakeMagnitude);
        cameraController.ScreenShake(bossAppearAnimation.length, screenShakeMagnitude);
    }
}
