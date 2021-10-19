using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls animation and movement of player ONLY!
public class PlayerController : MonoBehaviour
{
    public float speed = 10;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
    }

    private void Move()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * speed) * Input.GetAxisRaw("Horizontal"));
    }
}
