﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip backgroundMusic;
    [SerializeField]
    AudioClip bossAppearSFX;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip bossMusic;
    [SerializeField]
    GameObject bossAmbience;
    [SerializeField]
    BossController bossController;
    [SerializeField]
    Animator bossPlatformDarkenAnimator;

    #region PLATFORM SFX
    [SerializeField]
    AudioClip platformFallSFX;
    #endregion

    bool isBossAppeared = false;
    bool isBossMusicSet = false;
    float bossMusicStartTime;

    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("enumerator " + Time.time);
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    
    void Start()
    {
        audioSource.clip = backgroundMusic;
       // StartCoroutine(FadeOut(audioSource, 10f));
    }

    void Update()
    {
        if (isBossAppeared)
        {
            //Debug.Log("boss appeared " + Time.time);
            if (!isBossMusicSet & Time.time > bossMusicStartTime)
            {
                //Debug.Log("yay");
                isBossMusicSet = true;
                audioSource.volume = 1;
                audioSource.clip = bossMusic;
                audioSource.Play();
                bossAmbience.SetActive(true);
                
                bossController.StartAttacks();
            }
        }
    }

    public void SetBossMusic()
    {
        audioSource.clip = bossAppearSFX;
        audioSource.Play();
        bossPlatformDarkenAnimator.enabled = true;
        //audioSource.PlayOneShot(bossAppearSFX, 1f);
        bossMusicStartTime = Time.time + bossAppearSFX.length;
        Debug.Log("boss music start time " + bossMusicStartTime);
        isBossAppeared = true;
    }

    public void PlayPlatformFallSFX()
    {
        audioSource.PlayOneShot(platformFallSFX);
    }
}
