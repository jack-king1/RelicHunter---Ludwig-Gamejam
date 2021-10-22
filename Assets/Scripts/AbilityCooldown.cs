using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    [SerializeField] private Text timerText;

    private float timer = 0;

    private void Update()
    {
        
        if(ServiceLocator.Instance.playerManager.GetPlayerAbilityController().GetActiveAbilityCooldown() > 0)
        {
            if(!timerText.enabled)
            {
                timerText.enabled = true;
            }
            int floatToInt = (int)ServiceLocator.Instance.playerManager.GetPlayerAbilityController().GetActiveAbilityCooldown();
            timerText.text = floatToInt.ToString();
        }
        else
        {
            timerText.enabled = false;
        }

    }


}
