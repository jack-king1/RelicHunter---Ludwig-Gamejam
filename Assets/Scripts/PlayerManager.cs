using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public GameObject GetPlayer()
    {
        return player;
    }

    public Vector3 GetPlayerPosition()
    {
        return player.gameObject.transform.position;
    }


    public void SetPlayerPosition(Vector3 pos)
    {
        player.transform.position = pos;
    }

    public void SetPlayerSpeedY(float velocityY)
    {
        Rigidbody2D rb = player.GetComponent<PlayerController>().GetRigidBody();
        rb.velocity = new Vector3(rb.velocity.x, velocityY);
    }

    public AbilityController GetPlayerAbilityController()
    {
        return player.GetComponent<AbilityController>();
    }
}
