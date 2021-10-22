using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        ServiceLocator.Instance.audioManager.FadeInSound(this.gameObject,ServiceLocator.Instance.audioManager.GetSoundBank("GameMusic"),10,0.007f);
    }
}
