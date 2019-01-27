using System.Collections;
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
        Debug.Log("started");
        audioSource.clip = backgroundMusic;
       // StartCoroutine(FadeOut(audioSource, 10f));
    }

    void Update()
    {
        if (isBossAppeared)
        {
            Debug.Log("boss appeared " + Time.time);
            if (!isBossMusicSet & Time.time > bossMusicStartTime)
            {
                Debug.Log("yay");
                isBossMusicSet = true;
                audioSource.volume = 1;
                audioSource.clip = bossMusic;
                audioSource.Play();
                bossAmbience.SetActive(true);
            }
        }
    }

    public void SetBossMusic()
    {
        audioSource.clip = bossAppearSFX;
        audioSource.Play();
        //audioSource.PlayOneShot(bossAppearSFX, 1f);
        bossMusicStartTime = Time.time + bossAppearSFX.length;
        Debug.Log("boss music start time " + bossMusicStartTime);
        isBossAppeared = true;
    }

}
