using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangAbility : Ability
{
    public int maxBounces;

    public float speed = 5;
    public float boomerangReturnTime;
    public float lifeTime = 10f;

    private int bouncesCount = 0;

    private Animator animator;
    private Camera mainCamera;
    private Vector3 directionVector;
    Rigidbody2D rb;
    Vector3 lastVelocity;
    AudioManager.AudioInstance soundInstance; 


    public override void DoAbility()
    {
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ServiceLocator.Instance.cameraManager.SetDynamicCameraObject(this.gameObject);
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        Fire();
        StartCoroutine("LifeTimeCountDown");

    }

    private void Update()
    {
        //Debug.Log("Distance: " + Vector3.Distance(this.gameObject.transform.position, ServiceLocator.Instance.playerManager.GetPlayerPosition()));
        lastVelocity = rb.velocity;
        if(Vector3.Distance(this.gameObject.transform.position, ServiceLocator.Instance.playerManager.GetPlayerPosition()) > 20f)
        {
            ServiceLocator.Instance.cameraManager.ClearDynamicCameraObject(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        ServiceLocator.Instance.cameraManager.ClearDynamicCameraObject(this.gameObject);
    }

    private void Fire()
    {
        GetDirectionVector();
        rb.AddForce((directionVector * -1) * speed * 50);
        StartCoroutine("MoveCoroutine");
        soundInstance = ServiceLocator.Instance.audioManager.PlaySound(this.gameObject,ServiceLocator.Instance.audioManager.GetSoundBank("Boomerang"),0.5f);
    }

    void GetDirectionVector()
    {
        var mouseScreenPos = Input.mousePosition;
        var startingScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;

        directionVector = (this.transform.position - new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f)).normalized;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("TeleportAbility") || 
            collision.gameObject.CompareTag("WeightAbility") || collision.gameObject.CompareTag("VerticalFloor"))
        {
            if(maxBounces > bouncesCount)
            {
                //directionVector = new Vector3(directionVector.x , directionVector.y * -1, 0f);
                var localSpeed = lastVelocity.magnitude;
                var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
                rb.velocity = direction * Mathf.Max(speed, 0f);
                bouncesCount++;
            }
            else
            {
                rb.gravityScale = 0;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                animator.SetBool("Rotate", false);
                soundInstance.AudioSource.Stop();
            }
        }
    }

    IEnumerator LifeTimeCountDown()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }

    IEnumerator MoveCoroutine()
    {
        Debug.Log("Coroutine running");
        while (bouncesCount <= maxBounces)
        {

            yield return null;
        }
        animator.SetBool("Rotate", false);
        yield return null;
    }
}
