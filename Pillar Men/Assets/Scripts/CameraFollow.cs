using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    private GameObject player;       //variable to store a reference to the player game object
    [SerializeField]
    private Transform cameraBossPosition;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    bool followPlayer = true;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }


    public void ScreenShake(float duration, float magnitude)
    {
        Debug.Log("START SHAKING SCREEN");
        followPlayer = false;
        StartCoroutine(Shake(duration, magnitude));
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        if (followPlayer)
            transform.position = player.transform.position + offset;
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = new Vector3(cameraBossPosition.position.x, 
            cameraBossPosition.position.y,
            cameraBossPosition.position.z)
            + offset; ;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(cameraBossPosition.position.x + x,
                cameraBossPosition.position.y + y, 
                originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
        followPlayer = true;
    }
}