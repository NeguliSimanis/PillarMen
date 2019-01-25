using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntroManager : MonoBehaviour
{
    [SerializeField]
    GameStateManager gameStateManager;
    [SerializeField]
    GameObject[] introTexts;
    [SerializeField]
    GameObject introGameObject; 

    bool introStarted = false;
    bool introOver = false;

    float introTextDefaultLength = 2f;
    float lastIntroTextAdditionalLength = 1.5f;
    int currentIntroID = 0;
    float currentIntroImageStartTime;
    float currentIntroImageEndTime;

    public void StartIntro()
    {
        Debug.Log("starting intro");

        currentIntroImageStartTime = Time.time;
        currentIntroImageEndTime = currentIntroImageStartTime + introTextDefaultLength;
        introStarted = true;
        Debug.Log("timescale" + Time.timeScale);
        
    }

    void Update()
    {
        if (!introStarted)
            return;

        if (Time.time > currentIntroImageEndTime)
        {
            if (currentIntroID < introTexts.Length-1)
            {
                currentIntroID++;
                currentIntroImageEndTime += introTextDefaultLength + (float)(currentIntroID * 0.4);
                introTexts[currentIntroID - 1].SetActive(false);
                introTexts[currentIntroID].SetActive(true);
                Debug.Log("INTRO end time " + currentIntroImageEndTime);
            }
            else if (Time.time > currentIntroImageEndTime + lastIntroTextAdditionalLength)
            {
                if (introOver)
                    return;
                introOver = true;
                EndIntro();
            }
        }
    }

    private void EndIntro()
    {
        Debug.Log("ending intro");
        introGameObject.SetActive(false);
        //gameStateManager.c
    }
}
