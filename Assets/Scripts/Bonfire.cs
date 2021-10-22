using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private void Start()
    {
        ServiceLocator.Instance.audioManager.PlaySound(this.gameObject, ServiceLocator.Instance.audioManager.GetSoundBank("FireCrackle"), 0.1f);
    }
}
