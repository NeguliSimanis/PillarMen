using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDestroyPlatform : MonoBehaviour
{
    bool isFalling = false;
    bool fallSFXPlayed = false;
    float fallSpeed = 1.9f;

    [SerializeField]
    MusicManager musicManager;
    //[SerializeField]
   // AudioClip platformFallSFX;

    private void Start()
    {
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    public void BeginFall()
    {
        isFalling = true;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("yay");
            isFalling = true;
        }*/
        if (isFalling)
        {
            PlatformFall();
            
        }
    }

    void PlayFallSFX()
    {
        musicManager.PlayPlatformFallSFX();
    }

    void PlatformFall()
    {
        //if (isFallen)
        //    return;
        if (!fallSFXPlayed)
        {
            fallSFXPlayed = true;
            PlayFallSFX();
        }
        transform.position = new Vector2(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime);
    }
}
