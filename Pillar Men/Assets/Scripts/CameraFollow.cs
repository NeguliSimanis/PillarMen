﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    private GameObject player;       //variable to store a reference to the player game object
    [SerializeField]
    private Transform cameraBossPosition;
    [SerializeField]
    private Transform cameraBossPosition2;

    Camera camera;
    float cameraSizeAfterBoss = 8.97f;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    bool followPlayer = true;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
        camera = GetComponent<Camera>();
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

        /*StartCoroutine(MoveCameraAfterBossShake(transform.position, player.transform.position + offset,
            camera.orthographicSize, cameraSizeAfterBoss,1f));*/
        StartCoroutine(MoveCameraAfterBossShake(transform.position, cameraBossPosition2.position,
        camera.orthographicSize, cameraSizeAfterBoss, 1f));
        /* transform.position = originalPos;
         followPlayer = true;*/
    }


    public IEnumerator MoveCameraAfterBossShake(Vector3 startPosition, Vector3 endPosition,
        float cameraStartSize, float cameraEndSize, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            Vector3 currentPosition = new Vector3 
                (Mathf.Lerp(startPosition.x, endPosition.x, percentageComplete),
                Mathf.Lerp(startPosition.y, endPosition.y, percentageComplete),
                Mathf.Lerp(startPosition.z, endPosition.z, percentageComplete));

            float currentCameraSize = Mathf.Lerp(cameraStartSize, cameraEndSize, percentageComplete);

            transform.position = currentPosition;
            camera.orthographicSize = currentCameraSize;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
        Debug.Log("PROCESS OVER");
    }
}