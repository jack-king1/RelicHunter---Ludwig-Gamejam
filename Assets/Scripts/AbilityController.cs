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

    private void Start()
    {
        activeAbility = boomerangAbility;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //Do ability
            Instantiate(activeAbility,transform.position,Quaternion.identity);
        }
    }
}
