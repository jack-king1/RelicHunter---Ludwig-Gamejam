using System;
using UnityEngine;

[Serializable]
public class GameData 
{
    public Vector3 playerPos;
    public float elapsedTime;
    public int currentPercentage;

    public bool muted;
    public float musicVolume = 1;
    public float masterVolume = 1;
    public int percentageHighScore;
    public int teleportCount;
    public float elapsedTimeHighScore;
    public int teleportHighscore;

    //highscore 
}
