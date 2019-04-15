using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    bool isMainCamera = true;
    [SerializeField]
    private GameObject player;       //variable to store a reference to the player game object
    [SerializeField]
    private Transform cameraBossPosition;
    [SerializeField]
    private Transform cameraBossPosition2;

    Camera camera;
    float cameraSizeAfterBoss = 8.97f;
    float camera2SizeAfterBoss = 8.67f;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    bool followPlayer = true;
    float cameraDefaultYpos;
    float cameraDefaultSize;
    float cameraZoomOutSize;
    float cameraZoomOutValue = 2f;
    bool cameraDisplaced = false;
    float cameraZoomOutTime = 2f;
    float maxYdiffUp = 5f;
    float maxYdiffDown = 2f;
    float cameraYdisplacement = 3f;
    float cameraYdisplacementDown = 5f;
    float cameraYdisplacementTime = 2f;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
        camera = GetComponent<Camera>();
        transform.position = player.transform.position + offset;
        cameraDefaultYpos = transform.position.y;
        cameraDefaultSize = camera.orthographicSize;
        cameraZoomOutSize = cameraDefaultSize + cameraZoomOutValue;
    }


    public void ScreenShake(float duration, float magnitude)
    {
       // Debug.Log("START SHAKING SCREEN");
        followPlayer = false;
        StartCoroutine(Shake(duration, magnitude));
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (!isMainCamera)
        {
            //Debug.Log("correct");
            //Debug.Log(transform.position + " " + Time.time);
        }
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        if (followPlayer && isMainCamera)
        {
            //transform.position = player.transform.position + offset;
            Vector3 desiredPos = player.transform.position + offset;

            if (transform.position.y - desiredPos.y > maxYdiffDown)
            {
                /*if (!cameraDisplaced)
                {
                    cameraDisplaced = true;
                    StartCoroutine(ZoomCamera(camera.orthographicSize, cameraZoomOutSize, cameraZoomOutTime));
                }*/
                if (!cameraDisplaced)
                {
                    cameraDisplaced = true;
                    StartCoroutine(MoveCameraOnYaxis(transform.position.y, transform.position.y - cameraYdisplacementDown, cameraYdisplacementTime));
                }
                //Debug.Log("camera too high");
            }
            else if (transform.position.y - desiredPos.y < -maxYdiffUp)
            {
                /*if (!cameraDisplaced)
                {
                    cameraDisplaced = true;
                    StartCoroutine(ZoomCamera(camera.orthographicSize, cameraZoomOutSize, cameraZoomOutTime));
                }*/

                if (!cameraDisplaced)
                {
                    cameraDisplaced = true;
                    StartCoroutine(MoveCameraOnYaxis(transform.position.y, transform.position.y + cameraYdisplacement, cameraYdisplacementTime));
                }
                //Debug.Log("camera too low");
            }
            else
            {
                /*if (cameraDisplaced)
                {
                    cameraDisplaced = false;
                    StartCoroutine(ZoomCamera(camera.orthographicSize, cameraDefaultSize, cameraZoomOutTime, false));
                }*/
                if (cameraDisplaced)
                {
                    cameraDisplaced = false;
                    StartCoroutine(MoveCameraOnYaxis(transform.position.y, cameraDefaultYpos, cameraYdisplacementTime));
                }
            }
            transform.position = new Vector3(player.transform.position.x + offset.x, transform.position.y, player.transform.position.z + offset.z);
        }
    }


    public IEnumerator MoveCameraOnYaxis(float startY, float endY, float lerpTime = 1, bool displaceOriginalY = true)
    {

        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        if (displaceOriginalY)
        {
            while (cameraDisplaced)
            {
                timeSinceStarted = Time.time - _timeStartedLerping;
                percentageComplete = timeSinceStarted / lerpTime;
                float currentCameraYpos = Mathf.Lerp(startY, endY, percentageComplete);

                transform.position = new Vector3 (transform.position.x, currentCameraYpos, transform.position.z);

                if (percentageComplete >= 1) break;

                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (!cameraDisplaced)
            {
                timeSinceStarted = Time.time - _timeStartedLerping;
                percentageComplete = timeSinceStarted / lerpTime;
                float currentCameraYpos = Mathf.Lerp(startY, endY, percentageComplete);

                transform.position = new Vector3(transform.position.x, currentCameraYpos, transform.position.z);

                if (percentageComplete >= 1) break;

                yield return new WaitForFixedUpdate();
            }
        }
    }

    public IEnumerator ZoomCamera(float cameraStartSize, float cameraEndSize, float lerpTime = 1, bool zoomOut = true)
    {
        
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        if (zoomOut)
        {
            while (cameraDisplaced)
            {
                timeSinceStarted = Time.time - _timeStartedLerping;
                percentageComplete = timeSinceStarted / lerpTime;
                float currentCameraSize = Mathf.Lerp(cameraStartSize, cameraEndSize, percentageComplete);

                camera.orthographicSize = currentCameraSize;

                if (percentageComplete >= 1) break;

                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (!cameraDisplaced)
            {
                timeSinceStarted = Time.time - _timeStartedLerping;
                percentageComplete = timeSinceStarted / lerpTime;
                float currentCameraSize = Mathf.Lerp(cameraStartSize, cameraEndSize, percentageComplete);

                camera.orthographicSize = currentCameraSize;

                if (percentageComplete >= 1) break;

                yield return new WaitForFixedUpdate();
            }
        }
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
        if (isMainCamera)
        {
            
            GameObject childCamera = transform.GetChild(0).gameObject;
            CameraFollow childCameraFollow = childCamera.GetComponent<CameraFollow>();
            childCameraFollow.MoveSecondaryCamera();
            
        }
        StartCoroutine(MoveCameraAfterBossShake(transform.position, cameraBossPosition2.position,
        camera.orthographicSize, cameraSizeAfterBoss, 1f));
        /* transform.position = originalPos;
         followPlayer = true;*/
    }

    public void MoveSecondaryCamera()
    {
        //Debug.Log("YAH"); 
        StartCoroutine(MoveCameraAfterBossShake(transform.position, new Vector3(34.1f, 1.5f, -19.9f),  //new Vector3(0, -1.56f, transform.position.z), 
       camera.orthographicSize, camera2SizeAfterBoss, 1f));
    }

    public IEnumerator MoveCameraAfterBossShake(Vector3 startPosition, Vector3 endPosition,
        float cameraStartSize, float cameraEndSize, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        if (!isMainCamera)
        {
            //Debug.Log("correct");
            //Debug.Log(transform.position);
        }
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
    }
}