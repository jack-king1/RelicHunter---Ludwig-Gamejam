using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls animation and movement of player ONLY!
public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float jumpForce = 10;

    public BoxCollider2D facingLeftCollider;
    public BoxCollider2D facingRightCollider;

    public Transform candleLeft;
    public Transform candleRight;
    public GameObject playerCandle;

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
        if(ServiceLocator.Instance.gameManager.GetGameEnd())
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            if(!spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
                facingLeftCollider.enabled = true;
                facingRightCollider.enabled = false;
                SetCandlePosition();
            }
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            if(spriteRenderer)
            {
                spriteRenderer.flipX = false;
                facingLeftCollider.enabled = false;
                facingRightCollider.enabled = true;
                SetCandlePosition();
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

    public Rigidbody2D GetRigidBody()
    {
        return rb;
    }

    private void SetCandlePosition()
    {
        if(facingLeftCollider.enabled)
        {
            playerCandle.transform.position = candleLeft.position;
        }
        else
        {
            playerCandle.transform.position = candleRight.position;
        }
    }
}
