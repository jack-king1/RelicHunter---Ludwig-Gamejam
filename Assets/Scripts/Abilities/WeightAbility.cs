using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightAbility : Ability
{
    public float thrust = 5;
    public float downwardThrust = 500;
    public float massAfterWait = 7;
    public float gravityScaleAfterWait = 5;
    public float waitTimeUntillAddForce;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Vector3 directionVector;
    AudioManager.AudioInstance soundInstance;

    public override void DoAbility()
    {

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fire();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

    }

    private void Fire()
    {

        GetDirectionVector();
        rb.AddForce((directionVector * -1) * thrust);
        soundInstance = ServiceLocator.Instance.audioManager.PlaySound(this.gameObject, ServiceLocator.Instance.audioManager.GetSoundBank("Weight"), 0.5f);
        StartCoroutine("AddForce");
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
        ServiceLocator.Instance.audioManager.PlaySound(ServiceLocator.Instance.playerManager.GetPlayer().gameObject, ServiceLocator.Instance.audioManager.GetSoundBank("WeightReturn"), 4f);
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

    IEnumerator AddForce()
    {
        yield return new WaitForSeconds(waitTimeUntillAddForce);

        rb.velocity = new Vector2(0,rb.velocity.y);
        rb.AddForce(-transform.up * downwardThrust);
        rb.mass = massAfterWait;
        rb.gravityScale = gravityScaleAfterWait;
    }
}
