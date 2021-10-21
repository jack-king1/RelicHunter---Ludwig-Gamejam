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
}
