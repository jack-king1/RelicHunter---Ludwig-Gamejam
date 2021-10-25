using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

    private bool gamePaused = false;
    [SerializeField] private bool gameEnd = false;
    public Clock clock;

    private void Start()
    {
        ServiceLocator.Instance.audioManager.FadeInSound(this.gameObject,ServiceLocator.Instance.audioManager.GetSoundBank("GameMusic"),10,0.007f);
        gameData = new GameData();
        if (ServiceLocator.Instance.fileManager.FileExists("GAMEDATA"))
        {
            gameData = ServiceLocator.Instance.fileManager.LoadJson<GameData>("GAMEDATA");
            ServiceLocator.Instance.playerManager.SetPlayerPosition(gameData.playerPos);
            clock.elapsedTime = gameData.elapsedTime;
            ServiceLocator.Instance.uiManager.SetMusicSlider(gameData.musicVolume);
            ServiceLocator.Instance.uiManager.SetVolumeSlider(gameData.masterVolume);
            ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<AbilityController>().SetTeleportCount(gameData.teleportCount);

            ServiceLocator.Instance.uiManager.SetHighScore(clock.GetTimeFormatString(gameData.elapsedTimeHighScore), 
                gameData.percentageHighScore, gameData.teleportHighscore);
        }
        else
        {
            Debug.Log("File doent exist!");
            ServiceLocator.Instance.playerManager.SetPlayerPosition(new Vector3(0,0,0));
        }
        clock.BeginTimer(gameData.elapsedTime);
    }

    public void SetPercentageComplete(int percentage)
    {
        gameData.currentPercentage = percentage;
    }

    public bool GetGameEnd()
    {
        return gameEnd;
    }

    public void SetGameEnd()
    {
        gameEnd = true;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            if(gamePaused)
            {
                ResumeGame();    
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.AltGr) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            ApplyGameData();
        }

        if(gameData.currentPercentage > gameData.percentageHighScore)
        {
            gameData.elapsedTimeHighScore = clock.elapsedTime;
            gameData.percentageHighScore = gameData.currentPercentage;
            gameData.teleportHighscore = gameData.teleportCount;
            ServiceLocator.Instance.uiManager.SetHighScore(clock.timePlayingStr, gameData.currentPercentage, gameData.teleportCount);
        }
    }

    public void SetGameEndHighScore()
    {
        if(gameData.currentPercentage == gameData.percentageHighScore)
        {
            if (gameData.elapsedTime < gameData.elapsedTimeHighScore)
            {
                gameData.elapsedTimeHighScore = gameData.elapsedTime;
            }
        }
        else
        {
            gameData.elapsedTimeHighScore = gameData.elapsedTime;
        }

    }

    private void OnApplicationQuit()
    {
        ApplyGameData();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        gamePaused = true;
        ServiceLocator.Instance.uiManager.DisplaySettingsMenu("PauseGame");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        gamePaused = false;
        ServiceLocator.Instance.uiManager.DisplaySettingsMenu("ResumeGame");
    }

    public void ExitGame()
    {
        SetPlayerPosition();
        ApplyGameData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

#if UNITY_STANDALONE_WIN
        Application.Quit();
#endif

    }

    public void ResetGame()
    {
        ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<AbilityController>().SetTeleportCount(0);
        clock.elapsedTime = 0;
        ServiceLocator.Instance.playerManager.SetPlayerPosition(new Vector3(0,0,0));
        gameData.playerPos = new Vector3(0, 0, 0);
        gameData.elapsedTime = 0f;
        gameData.teleportCount = 0;
        ServiceLocator.Instance.fileManager.SaveIntoJson("GAMEDATA", gameData);
        ResumeGame();
        Debug.Log("ResetGame Is Being Called");
    }

    public void ApplyGameData()
    {
        SetPlayerPosition();
        ServiceLocator.Instance.fileManager.SaveIntoJson("GAMEDATA",gameData);
    }

    public void SetPlayerPosition()
    {
        gameData.playerPos = ServiceLocator.Instance.playerManager.GetPlayerPosition();
    }

    public void SetTeleportCount(int count)
    {
        gameData.teleportCount = count;
    }

    public void SetVolume(float vol)
    {
        gameData.masterVolume = vol;
    }

    public void SetMusicVolume(float vol)
    {
        gameData.musicVolume = vol;
    }

    public bool GetPaused()
    {
        return gamePaused;
    }
}
