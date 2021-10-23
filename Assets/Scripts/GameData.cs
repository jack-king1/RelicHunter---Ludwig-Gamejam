using System;
using UnityEngine;

[Serializable]
public class GameData 
{
    public Vector3 playerPos;
    public float previousHighScore;
    public float timeElapsed;

    public bool muted;
    public float musicVolume;
    public float masterVolume;
}
