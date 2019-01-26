using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    //SpriteRenderer spriteRenderer;
    public SpriteRenderer cloudSprite;
    private float fadeOutTime = 2f;
    private float fadeInTime = 2f;
    private float minDelayAfterFadeIn = 3f;
    private float maxDelayAfterFadeIn = 9f;
    private float delayAfterFadeIn;// = 5f; // how long after fading in does the cloud start to fade out

    private bool isFadedIn = false;
    private bool isFadingOut = false;
    private float fadeOutStartTime;

    [SerializeField]
    private float cloudMoveSpeed = 0.01f;
    private void Start()
    {
        cloudSprite = GetComponent<SpriteRenderer>();
        FadeIn();
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + cloudMoveSpeed, transform.position.y);// + dirNormalized * PlayerData.current.moveSpeed * Time.deltaTime;
            
        if (isFadedIn)
        {
            if (Time.time > fadeOutStartTime && !isFadingOut)
            {
                isFadingOut = true;
                FadeOut();
            }
        }
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCloud(cloudSprite, 0, 1, true, fadeInTime));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCloud(cloudSprite, cloudSprite.material.color.a, 0, false, fadeOutTime));
    }

    public IEnumerator FadeCloud(SpriteRenderer cg, float start, float end, bool fadeIn, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.material.color = new Color(cg.material.color.r, cg.material.color.g, cg.material.color.b, currentValue);

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
        if (fadeIn)
        {
            isFadedIn = true;
            delayAfterFadeIn = Random.Range(minDelayAfterFadeIn, maxDelayAfterFadeIn);
            fadeOutStartTime = Time.time + delayAfterFadeIn;
        }
        else
        {
            DestroyCloud();
        }
    }
    void DestroyCloud()
    {
        Destroy(gameObject);
    }
}
