using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public string timePlayingStr;
    public TimeSpan timePlaying;

    public float elapsedTime;
    bool timerGoing = false;
    public Text clockText;

    private void Start()
    {
        clockText = ServiceLocator.Instance.uiManager.GetTimerTextReference();
    }

    public void BeginTimer(float previousTime)
    {
        elapsedTime = previousTime;
        timerGoing = true;
        StartCoroutine("UpdateTimer");

    }

    public string GetTimeFormatString(float elapsed)
    {
        TimeSpan temp = TimeSpan.FromSeconds(elapsed);
        string tempString = temp.ToString("hh':'mm':'ss'.'ff");
        return tempString;
    }

    private IEnumerator UpdateTimer()
    {
        while(timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            timePlayingStr = timePlaying.ToString("hh':'mm':'ss'.'ff");
            if(clockText == null)
            {
                clockText = ServiceLocator.Instance.uiManager.GetTimerTextReference();
            }
            clockText.text = timePlayingStr;
            ServiceLocator.Instance.gameManager.gameData.elapsedTime = elapsedTime;
            yield return null;
        }
    }
}
