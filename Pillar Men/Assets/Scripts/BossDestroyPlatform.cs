using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDestroyPlatform : MonoBehaviour
{
    bool isFalling = false;
    float fallSpeed = 1.9f;
    

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

    void PlatformFall()
    {
        //if (isFallen)
        //    return;

        transform.position = new Vector2(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime);
    }
}
