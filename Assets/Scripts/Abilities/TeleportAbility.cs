using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAbility : Ability
{
    public float thrust = 5;
    public float speed = 5;
    public float lifeTime = 10f;

    private Animator animator;
    private Vector3 directionVector;
    private Rigidbody2D rb;
    private bool isGrounded = false;

    public override void DoAbility()
    {

    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fire();
    }

    private void Fire()
    {
        GetDirectionVector();
        rb.AddRelativeForce((directionVector * -1 ) * thrust);
    }

    void GetDirectionVector()
    {
        var mouseScreenPos = Input.mousePosition;
        var startingScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;

        directionVector = (this.transform.position - new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f)).normalized;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<AbilityController>().RemoveTeleportAbilityRef(this.gameObject);
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
}
