using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    AudioSource cameraAudioSource;
    [SerializeField]
    AudioClip pickupSFX;
    // Start is called before the first frame update
    void Start()
    {
        cameraAudioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Heal(10);
            cameraAudioSource.PlayOneShot(pickupSFX);
            Destroy(transform.parent.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
