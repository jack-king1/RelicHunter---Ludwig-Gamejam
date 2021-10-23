using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

    private void Start()
    {
        ServiceLocator.Instance.audioManager.FadeInSound(this.gameObject,ServiceLocator.Instance.audioManager.GetSoundBank("GameMusic"),10,0.007f);
        gameData = new GameData();
        if (ServiceLocator.Instance.fileManager.FileExists("GAMEDATA"))
        {
            gameData = ServiceLocator.Instance.fileManager.LoadJson<GameData>("GAMEDATA");
            ServiceLocator.Instance.playerManager.SetPlayerPosition(gameData.playerPos);
        }
    }

    private void Update()
    {
        //Debug
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Debug Save Data...");
            gameData.masterVolume = 69f;
            ServiceLocator.Instance.fileManager.SaveIntoJson("GAMEDATA", gameData);
        }
    }

    public void ResumeGame()
    {

    }

    public void ExitGame()
    {

    }

    public void ResetGame()
    {

    }
}
