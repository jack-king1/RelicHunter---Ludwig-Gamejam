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
    private GameObject weightAbilityRef;

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
                    if (teleportAbilityRef != null && abilityState == ABILITY.TELEPORT)
                    {
                        ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<PlayerController>().SetGrounded(false);
                        Vector3 pos = new Vector3(teleportAbilityRef.transform.position.x, teleportAbilityRef.transform.position.y + 0.5f, 0);
                        ServiceLocator.Instance.playerManager.SetPlayerPosition(pos);
                        Destroy(teleportAbilityRef);
                        teleportAbilityRef = null;
                    }
                    else if(weightAbilityRef != null && abilityState == ABILITY.WEIGHT)
                    {
                        Destroy(weightAbilityRef);
                        weightAbilityRef = null;
                    }
                    else
                    {
                        activateAbilityIndicator = true;
                        abilityIndicator.SetActive(activateAbilityIndicator);
                    }
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
                        else
                        {
                            teleportAbilityRef = Instantiate(activeAbility, transform.position, Quaternion.identity);
                        }
                    }
                    else if(abilityState == ABILITY.WEIGHT)
                    {
                        if (weightAbilityRef != null)
                        {
                            Destroy(weightAbilityRef);
                            weightAbilityRef = null;
                        }
                        else
                        {
                            weightAbilityRef = Instantiate(activeAbility, transform.position, Quaternion.identity);
                        }
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
