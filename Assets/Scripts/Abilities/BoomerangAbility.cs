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

    public override void DoAbility()
    {
        
    }

    private void Start()
    {
        ServiceLocator.Instance.cameraManager.SetDynamicCameraObject(this.gameObject);
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        Fire();
        StartCoroutine("LifeTimeCountDown");

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        ServiceLocator.Instance.cameraManager.ClearDynamicCameraObject(this.gameObject);
    }

    private void Fire()
    {
        GetDirectionVector();
        StartCoroutine("MoveCoroutine");
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
        if(collision.gameObject.tag == "Floor")
        {
            directionVector = new Vector3(directionVector.x , directionVector.y * -1, 0f);
            bouncesCount++;
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
            transform.Translate((directionVector * -1) * (Time.deltaTime * speed));
            yield return null;
        }
        animator.SetBool("Rotate", false);
        yield return null;
    }
}
