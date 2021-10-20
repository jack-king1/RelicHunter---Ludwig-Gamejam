using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject boomerangAbilityUI;
    public GameObject teleportAbilityUI;
    public GameObject weightAbilityUI;

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
}
