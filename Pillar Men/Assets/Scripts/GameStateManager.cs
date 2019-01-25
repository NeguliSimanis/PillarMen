using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum CurrentGameState
    {
        Intro,
        Level1,
        Boss1,
        Victory,
        Defeat,
        Paused
    };

    CurrentGameState currentState;

    void Start()
    {
        currentState = CurrentGameState.Paused;
        Time.timeScale = 0f;
    }

    public void StartGameButton()
    {
        Time.timeScale = 1f;
        currentState = CurrentGameState.Intro;
    }
}
