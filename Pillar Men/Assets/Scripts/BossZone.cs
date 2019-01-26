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
    AudioSource audioSource;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    float playerFreezeDuration;

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
        playerController.FreezeMovement(Time.time + playerFreezeDuration);
    }
}
