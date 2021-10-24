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

    public ScriptableNotifications boomerangNotification;
    public ScriptableNotifications teleportNotification;
    public ScriptableNotifications weightNotification;

    private bool boomerangNotificationActivated = false;
    private bool teleportNotificationActivated = false;
    private bool weightNotificationActivated = false;

    [SerializeField] private float teleportCooldown = 1.5f;
    private float teleportTimer = 0f;


    private GameObject teleportAbilityRef;
    private GameObject weightAbilityRef;

    private List<GameObject> weightAbilityList = new List<GameObject>();

    private void Start()
    {
        activeAbility = boomerangAbility;
        abilityState = ABILITY.BOOMERANG;
        ServiceLocator.Instance.uiManager.SetAbilityUI(abilityState);
    }

    private void Update()
    {
        if(teleportTimer > 0)
        {
            teleportTimer -= Time.deltaTime;
            if(teleportTimer < 0)
            {
                teleportTimer = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilityState = ABILITY.BOOMERANG;
            ServiceLocator.Instance.uiManager.SetAbilityUI(abilityState);
            SetActiveAbility();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            abilityState = ABILITY.TELEPORT;
            ServiceLocator.Instance.uiManager.SetAbilityUI(abilityState);
            SetActiveAbility();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            abilityState = ABILITY.WEIGHT;
            ServiceLocator.Instance.uiManager.SetAbilityUI(abilityState);
            SetActiveAbility();
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Do ability
            if(abilityState != ABILITY.NONE)
            {
                if (!activateAbilityIndicator)
                {
                    if (teleportAbilityRef != null && (abilityState == ABILITY.TELEPORT || Input.GetKeyDown(KeyCode.Alpha2)))
                    {
                        ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<PlayerController>().SetGrounded(false);
                        Vector3 pos = new Vector3(teleportAbilityRef.transform.position.x, teleportAbilityRef.transform.position.y + 0.5f, 0);
                        ServiceLocator.Instance.playerManager.SetPlayerPosition(pos);
                        ServiceLocator.Instance.playerManager.SetPlayerSpeedY(0f);
                        Destroy(teleportAbilityRef);
                        teleportAbilityRef = null;
                        SetActiveAbilityCooldown();
                        
                    }
                    else if(weightAbilityRef != null && abilityState == ABILITY.WEIGHT && weightAbilityRef.GetComponent<WeightAbility>().GetReturning() == false)
                    {

                        //Return To Player Code
                        weightAbilityRef.GetComponent<WeightAbility>().ReturnToPlayer();
                    }
                    else
                    {
                        if (AbilityOffCooldown() && abilityState == ABILITY.TELEPORT)
                        {
                            activateAbilityIndicator = true;
                            abilityIndicator.SetActive(activateAbilityIndicator);
                        }
                        else if (abilityState != ABILITY.TELEPORT)
                        {
                            activateAbilityIndicator = true;
                            abilityIndicator.SetActive(activateAbilityIndicator);
                        }
                    }
                }
                else
                {
                    if(abilityState == ABILITY.TELEPORT)
                    {
                        if(teleportAbilityRef != null)
                        {
                            ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<PlayerController>().SetGrounded(false);
                            Vector3 pos = new Vector3(teleportAbilityRef.transform.position.x, teleportAbilityRef.transform.position.y + 0.5f, 0);
                            ServiceLocator.Instance.playerManager.SetPlayerPosition(pos);
                            ServiceLocator.Instance.playerManager.SetPlayerSpeedY(0f);
                            Destroy(teleportAbilityRef);
                            teleportAbilityRef = null;
                            SetActiveAbilityCooldown();
                        }
                        else
                        {
                            if(AbilityOffCooldown())
                            {
                                if (!teleportNotificationActivated)
                                {
                                    teleportNotificationActivated = true;
                                    ServiceLocator.Instance.notificationManager.AddNotification(teleportNotification);
                                }
                                teleportAbilityRef = Instantiate(activeAbility, transform.position, Quaternion.identity);
                            }
                        }
                    }
                    else if(abilityState == ABILITY.WEIGHT)
                    {
                        if (weightAbilityRef != null)
                        {
                            weightAbilityRef.GetComponent<WeightAbility>().ReturnToPlayer();
                            weightAbilityRef = null;
                        }
                        else
                        {
                            if (!weightNotificationActivated)
                            {
                                weightNotificationActivated = true;
                                ServiceLocator.Instance.notificationManager.AddNotification(weightNotification);
                            }
                            weightAbilityRef = Instantiate(activeAbility, transform.position, Quaternion.identity);
                            Debug.Break();
                            weightAbilityList.Add(weightAbilityRef);
                        }
                    }
                    else
                    {
                        if(!boomerangNotificationActivated)
                        {
                            boomerangNotificationActivated = true;
                            ServiceLocator.Instance.notificationManager.AddNotification(boomerangNotification);
                        }
                        Instantiate(activeAbility, transform.position, Quaternion.identity);
                    }
                    
                    activateAbilityIndicator = false;
                    abilityIndicator.SetActive(activateAbilityIndicator);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Mouse1))
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

    private void SetActiveAbilityCooldown()
    {
        switch (abilityState)
        {
            case ABILITY.NONE:
                break;
            case ABILITY.BOOMERANG:
                //boomerangTimer = boomerangCooldown;
                break;
            case ABILITY.TELEPORT:
                teleportTimer = teleportCooldown;
                break;
            case ABILITY.WEIGHT:
                //weightTimer = weightCooldown;
                break;
            default:
                break;
        }
    }

    private bool AbilityOffCooldown()
    {
        switch (abilityState)
        {
            case ABILITY.NONE:
                return true;
            case ABILITY.TELEPORT:
                return teleportTimer <= 0;
        }

        return false;
    }

    public void RemoveTeleportAbilityRef(GameObject obj)
    {
        if(teleportAbilityRef == obj)
        {
            Destroy(teleportAbilityRef);
            teleportAbilityRef = null;
        }
    }

    public float GetActiveAbilityCooldown()
    {
        switch (abilityState)
        {
            case ABILITY.NONE:
                return 0;
            case ABILITY.TELEPORT:
                return teleportTimer;
        }

        return 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("WeightAbility"))
        {
            Destroy(weightAbilityRef);
            Debug.Log("Destroying Weight.");
            foreach (var weight in weightAbilityList)
            {
                Destroy(weight);
            }
            weightAbilityRef = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeightAbility"))
        {
            Destroy(weightAbilityRef);
            Debug.Log("Destroying Weight.");
            foreach (var weight in weightAbilityList)
            {
                Destroy(weight);
            }
            weightAbilityRef = null;
        }
    }
}
