using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animation))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlatformFall : MonoBehaviour
{
    [SerializeField] private float fallOffset = 100f;
    [SerializeField] private float fallTimer = 2f;
    [SerializeField] private float respawnTimer = 5f;
    [SerializeField] private float returnTimer = 3f;
    [SerializeField] private float physicsTimer = 3f;
    private Vector3 origin;

    private Animation animation;
    private Rigidbody2D rb;
    private bool alive = true;

    private void Start()
    {
        animation = GetComponent<Animation>();
        rb = GetComponent<Rigidbody2D>();
        origin = gameObject.transform.position;
        rb.isKinematic = true;
        animation.playAutomatically = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && alive)
        {
            animation.Play();
            StartCoroutine("FallCountdown");
            alive = false;
        }
    }

    IEnumerator FallCountdown()
    {
        yield return new WaitForSeconds(fallTimer);
        rb.freezeRotation = false;
        rb.isKinematic = false;
        StartCoroutine("RespawnCoroutine");
    }

    IEnumerator RespawnCoroutine()
    {
        StartCoroutine("PhysicsTimerCoroutine");
        LeanTween.alpha(gameObject, 0f, physicsTimer);

        yield return new WaitForSeconds(respawnTimer);
        
        LeanTween.move(gameObject, origin, returnTimer);
        LeanTween.rotateZ(gameObject, 0, returnTimer);
        LeanTween.alpha(gameObject, 1f, returnTimer);

        yield return new WaitForSeconds(returnTimer);
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
    }

    IEnumerator PhysicsTimerCoroutine()
    {
        yield return new WaitForSeconds(physicsTimer);
        rb.isKinematic = true;
        alive = true;
    }
}
