using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls animation and movement of player ONLY!
public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float jumpForce = 10;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private bool isGrounded = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(!spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            if(spriteRenderer)
            {
                spriteRenderer.flipX = false;
            }
        }

        if(Input.GetAxis("Horizontal") == 1 || Input.GetAxis("Horizontal") == -1)
        {
            if(animator.GetBool("Walking") == false)
            {
                animator.SetBool("Walking", true);
            }
        }

        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("Walking", false);
        }

        Move();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * speed) * Input.GetAxisRaw("Horizontal"));
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
        isGrounded = false;
    }

    public void SetGrounded(bool setGrounded)
    {
        isGrounded = setGrounded;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

        if(collision.gameObject.CompareTag("TeleportAbility"))
        {
            if(collision.gameObject.GetComponent<TeleportAbility>().IsGrounded())
            {
                isGrounded = true;
            }
        }

        if (collision.gameObject.CompareTag("WeightAbility"))
        {
            if (collision.gameObject.GetComponent<WeightAbility>().IsGrounded())
            {
                isGrounded = true;
            }
        }
    }
}
