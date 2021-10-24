using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightAbility : Ability
{
    public float totalLifetime = 15f;
    public float thrust = 5;
    public float downwardThrust = 500;
    public float massAfterWait = 7;
    public float gravityScaleAfterWait = 5;
    public float waitTimeUntillAddForce;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Vector3 directionVector;
    private CircleCollider2D circleCollider;
    AudioManager.AudioInstance soundInstance;
    private bool returningToPlayer = false;

    bool firstContact = false;
    ParticleSystem ps;

    public override void DoAbility()
    {

    }

    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider.isTrigger = false;
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
        soundInstance = ServiceLocator.Instance.audioManager.PlaySound(this.gameObject, ServiceLocator.Instance.audioManager.GetSoundBank("Weight"), 0.2f);
        StartCoroutine("AddForce");
    }

    public bool GetReturning()
    {
        return returningToPlayer;
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
        ServiceLocator.Instance.audioManager.PlaySound(ServiceLocator.Instance.playerManager.GetPlayer().gameObject, ServiceLocator.Instance.audioManager.GetSoundBank("WeightReturn"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!firstContact && !collision.gameObject.CompareTag("Player"))
        {
            firstContact = true;
            ServiceLocator.Instance.audioManager.PlaySound(this.gameObject, ServiceLocator.Instance.audioManager.GetSoundBank("LoudThud"), 0.5f);
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

    IEnumerator AddForce()
    {
        yield return new WaitForSeconds(waitTimeUntillAddForce);

        rb.velocity = new Vector2(0,rb.velocity.y);
        rb.AddForce(-transform.up * downwardThrust);
        rb.mass = massAfterWait;
        rb.gravityScale = gravityScaleAfterWait;
        StartCoroutine("ReturnAfterTimeDelay");
    }

    IEnumerator ReturnAfterTimeDelay()
    {
        yield return new WaitForSeconds(totalLifetime);
        ReturnToPlayer();
    }

    IEnumerator ReturnToPlayerCoroutine()
    {
        StopCoroutine("AddForce");
        while(Vector3.Distance(this.gameObject.transform.position, ServiceLocator.Instance.playerManager.GetPlayerPosition()) > 0.1f)
        {
            transform.Translate(GetDirectionVectorToPlayer() * Time.deltaTime * 50);
            yield return null;
        }
    }

    Vector3 GetDirectionVectorToPlayer()
    {
        Vector3 tempVec = new Vector3();
        tempVec = (ServiceLocator.Instance.playerManager.GetPlayerPosition() - gameObject.transform.position).normalized;
        return tempVec;
    }

    public void ReturnToPlayer()
    {
        returningToPlayer = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.gravityScale = 0;
        rb.mass = 0;
        rb.freezeRotation = true;
        circleCollider.isTrigger = true;
        SetParticleLifetime();
        StartCoroutine("ReturnToPlayerCoroutine");
    }

    private void SetParticleLifetime()
    {
        var particleEmission = ps.enableEmission;
        particleEmission = false;
    }
}
