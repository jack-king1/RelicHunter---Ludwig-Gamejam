using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightAbility : Ability
{
    private Rigidbody2D rb;
    private bool isGrounded = false;

    public override void DoAbility()
    {

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    public bool IsGrounded()
    {

        return isGrounded;
    }
}
