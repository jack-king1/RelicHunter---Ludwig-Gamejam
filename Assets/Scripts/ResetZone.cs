using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetZone : MonoBehaviour
{
    public Transform resetSpawn;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ServiceLocator.Instance.playerManager.SetPlayerPosition(resetSpawn.position);
        }

        if(collision.gameObject.CompareTag("TeleportAbility"))
        {
            Destroy(collision.gameObject);
        }
    }
}
