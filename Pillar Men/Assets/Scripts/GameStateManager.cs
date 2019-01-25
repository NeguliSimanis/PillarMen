using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    // If true, player can move immediately. Else must wait for intro scene to end
    // set to true when testing game without intro screen/scene
    bool startGameWithPause = true;

    [SerializeField]
    GameObject defeatMenu;
    public enum CurrentGameState
    {
        Intro,
        Level1,
        Boss1,
        Victory,
        Defeat,
        Paused
    };

    public CurrentGameState currentState;

    void Start()
    {
        if (startGameWithPause)
        {
            currentState = CurrentGameState.Paused;
            Time.timeScale = 0f;
        }
        else
        {
            currentState = CurrentGameState.Level1;
            Time.timeScale = 1f;
        }
    }

    public void StartGameButton()
    {
        Time.timeScale = 1f;
        currentState = CurrentGameState.Intro;
    }

    public void EndGameIntro()
    {
        currentState = CurrentGameState.Level1;
    }

    public void LoseGame()
    {
        currentState = CurrentGameState.Defeat;
        defeatMenu.SetActive(true);
        
    }
}
