using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ABILITY
{
    NONE = 0,
    BOOMERANG,
    TELEPORT,
    WEIGHT
}

public class AbilityController : MonoBehaviour
{
    public GameObject activeAbility;

    public GameObject boomerangAbility;
    public GameObject teleportAbility;
    public GameObject weightAbility;

    public ABILITY abilityState = ABILITY.NONE;

    public GameObject abilityIndicator;
    public bool activateAbilityIndicator = false;

    private GameObject teleportAbilityRef;

    private void Start()
    {
        activeAbility = boomerangAbility;
        abilityState = ABILITY.BOOMERANG;
        ServiceLocator.Instance.uiManager.SetAbilityUI(abilityState);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //Do ability
            if(abilityState != ABILITY.NONE)
            {
                if (!activateAbilityIndicator)
                {
                    activateAbilityIndicator = true;
                    abilityIndicator.SetActive(activateAbilityIndicator);
                }
                else
                {
                    if(abilityState == ABILITY.TELEPORT)
                    {
                        if(teleportAbilityRef != null)
                        {
                            Destroy(teleportAbilityRef);
                            teleportAbilityRef = null;
                        }
                        teleportAbilityRef =  Instantiate(activeAbility, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(activeAbility, transform.position, Quaternion.identity);
                    }
                    
                    activateAbilityIndicator = false;
                    abilityIndicator.SetActive(activateAbilityIndicator);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(activateAbilityIndicator)
            {
                activateAbilityIndicator = false;
                abilityIndicator.SetActive(activateAbilityIndicator);
            }

            if(abilityState == ABILITY.WEIGHT)
            {
                abilityState = ABILITY.NONE;
            }
            else
            {
                abilityState += 1;
            }
            SetActiveAbility();
            ServiceLocator.Instance.uiManager.SetAbilityUI(abilityState);
        }
    }

    private void SetActiveAbility()
    {
        switch (abilityState)
        {
            case ABILITY.NONE:
                activeAbility = null;
                break;
            case ABILITY.BOOMERANG:
                activeAbility = boomerangAbility;
                break;
            case ABILITY.TELEPORT:
                activeAbility = teleportAbility;
                break;
            case ABILITY.WEIGHT:
                activeAbility = weightAbility;
                break;
            default:
                break;
        }
    }

    public void RemoveTeleportAbilityRef(GameObject obj)
    {
        if(teleportAbilityRef == obj)
        {
            Destroy(teleportAbilityRef);
            teleportAbilityRef = null;
        }
    }
}
