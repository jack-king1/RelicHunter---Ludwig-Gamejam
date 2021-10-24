using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

    private bool gamePaused = false;

    private void Start()
    {
        ServiceLocator.Instance.audioManager.FadeInSound(this.gameObject,ServiceLocator.Instance.audioManager.GetSoundBank("GameMusic"),10,0.007f);
        gameData = new GameData();
        if (ServiceLocator.Instance.fileManager.FileExists("GAMEDATA"))
        {
            gameData = ServiceLocator.Instance.fileManager.LoadJson<GameData>("GAMEDATA");
            ServiceLocator.Instance.playerManager.SetPlayerPosition(gameData.playerPos);

        }
        else
        {
            Debug.Log("File doent exist!");
            ServiceLocator.Instance.playerManager.SetPlayerPosition(new Vector3(0,0,0));
        }
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
        ServiceLocator.Instance.uiManager.DisplaySettingsMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        gamePaused = false;
        ServiceLocator.Instance.uiManager.DisplaySettingsMenu();
    }

    public void ExitGame()
    {

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
        ServiceLocator.Instance.playerManager.SetPlayerPosition(new Vector3(0,0,0));
        gameData.playerPos = new Vector3(0, 0, 0);
        ServiceLocator.Instance.fileManager.SaveIntoJson("GAMEDATA", gameData);
        ResumeGame();
    }

    public void ApplyGameData()
    {
        gameData.playerPos = ServiceLocator.Instance.playerManager.GetPlayerPosition();
        ServiceLocator.Instance.fileManager.SaveIntoJson("GAMEDATA",gameData);
    }

    public void SaveHighscore()
    {

    }
}
