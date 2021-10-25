using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject boomerangAbilityUI;
    public GameObject teleportAbilityUI;
    public GameObject weightAbilityUI;

    //Notification Objects
    public Text notificationHeader;
    public Text notificationMessage;
    public GameObject notificationPanel;
    public Transform notificaitonPanelOffScreen;
    public Transform notificaitonPanelOnScreen;
    public Text percentageText;

    //Settings Menu
    public GameObject SettingsMenuPanel;
    public Transform SettingsMenuPanelOffScreen;
    public Transform SettingsMenuPanelOnScreen;
    private bool SettingsMenuActive = false;
    public Text timeHighscore;
    public Text percentageHighscore;
    public Text teleportHighscore;

    //Timer
    public Text timerText;

    //MenuSlider
    public Slider soundSlider;
    public Slider musicSlider;

    //Teleport Counter
    public Text teleportCounterText;

    public void SetAbilityUI(ABILITY type)
    {
        switch (type)
        {
            case ABILITY.NONE:
                boomerangAbilityUI.SetActive(false);
                teleportAbilityUI.SetActive(false);
                weightAbilityUI.SetActive(false);
                break;
            case ABILITY.BOOMERANG:
                boomerangAbilityUI.SetActive(true);
                teleportAbilityUI.SetActive(false);
                weightAbilityUI.SetActive(false);
                break;
            case ABILITY.TELEPORT:
                boomerangAbilityUI.SetActive(false);
                teleportAbilityUI.SetActive(true);
                weightAbilityUI.SetActive(false);
                break;
            case ABILITY.WEIGHT:
                boomerangAbilityUI.SetActive(false);
                teleportAbilityUI.SetActive(false);
                weightAbilityUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DisplayNotification(ScriptableNotifications notification)
    {
        notificationHeader.text = notification.header;
        notificationMessage.text = notification.message;
        LeanTween.moveLocalY(notificationPanel, notificaitonPanelOnScreen.localPosition.y, 1f).setEase(LeanTweenType.easeInSine);
    }

    public void EndDisplayNotification()
    {
        LeanTween.moveLocalY(notificationPanel, notificaitonPanelOffScreen.localPosition.y, 1f).setEase(LeanTweenType.easeInSine);
    }

    public void SetPercentageText(float perc)
    {
        percentageText.text = (perc.ToString() + " %");
    }

    public void DisplaySettingsMenu(string functionName)
    {
        Debug.Log("Called From: " + functionName);
        SettingsMenuActive = !SettingsMenuActive;

        if(SettingsMenuActive)
        {
            LeanTween.moveLocalY(SettingsMenuPanel, SettingsMenuPanelOnScreen.localPosition.y, 0.4f).setEase(LeanTweenType.easeInSine).setIgnoreTimeScale(true);
        }
        else
        {
            LeanTween.moveLocalY(SettingsMenuPanel, SettingsMenuPanelOffScreen.localPosition.y, 0.4f).setEase(LeanTweenType.easeInSine).setIgnoreTimeScale(true);
        }
    }

    public void SetTimerText(float minutes, float seconds)
    {
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public Text GetTimerTextReference()
    {
        return timerText;
    }

    public void SetVolumeSlider(float value)
    {
        soundSlider.value = value;
    }

    public void SetMusicSlider(float value)
    {
        musicSlider.value = value;
    }

    public void SetTeleportCountText(int value)
    {
        teleportCounterText.text = "TP: " + value.ToString();
    }

    public void SetHighScore(string eleapsedTime, int percentage, int teleportCount)
    {
        timeHighscore.text = eleapsedTime;
        percentageHighscore.text = percentage.ToString();
        teleportHighscore.text = teleportCount.ToString();
    }
}
