using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderValueChange : MonoBehaviour
{
    public Slider slider;

    public void OnSoundChange()
    {
        ServiceLocator.Instance.audioManager.MasterMixer.SetFloat("MasterVolume", Mathf.Log10(slider.value) * 20);
        ServiceLocator.Instance.gameManager.SetVolume(slider.value);
    }

    public void OnMusicChange()
    {
        ServiceLocator.Instance.audioManager.MasterMixer.SetFloat("MusicVolume", Mathf.Log10(slider.value) * 20);
        ServiceLocator.Instance.gameManager.SetMusicVolume(slider.value);
    }
}
