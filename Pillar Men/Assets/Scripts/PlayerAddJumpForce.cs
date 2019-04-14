using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAddJumpForce : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    private void OnEnable()
    {
        Debug.Log("yay " + Time.time);
        playerController.SetJumpTargetPosition();

    }
}
