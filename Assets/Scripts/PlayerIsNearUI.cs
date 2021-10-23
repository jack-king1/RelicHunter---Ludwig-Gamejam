using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerIsNearUI : MonoBehaviour
{
    [SerializeField] private bool startInvisible;
    [SerializeField] private float distanceToPlayer = 8f;
    [SerializeField] private float offSet = 10f;
    private Transform playerTransform;
    private Vector3 displayPos;
    private Vector3 offPos;
    private bool isCurrentlyDisplayed = false;
    private RectTransform myRectTransform;
    private float cooldown = 1f;
    private float timer = 0f;


    private void Start()
    {
        playerTransform = ServiceLocator.Instance.playerManager.GetPlayer().transform;
        displayPos = gameObject.transform.position;
        offPos = new Vector3(displayPos.x, displayPos.y + offSet, displayPos.z);
        myRectTransform = GetComponent<RectTransform>();
        if(startInvisible)
        {
            Color temp = GetComponent<Text>().color;
            GetComponent<Text>().color = new Color(temp.r,temp.g,temp.b,0f); 
        }
    }

    private void Update()
    {
        if(timer > 0)
        {
            return;
        }

        if (!isCurrentlyDisplayed)
        {
            Debug.Log(Vector3.Distance(gameObject.transform.position, playerTransform.position));
            if (Vector3.Distance(gameObject.transform.position, playerTransform.position) < distanceToPlayer)
            {
                LeanTween.cancel(gameObject);
                isCurrentlyDisplayed = true;
                LeanTween.moveY(gameObject, displayPos.y, 1f).setEase(LeanTweenType.easeInSine);
                LeanTween.alphaText(myRectTransform, 1f, 1f);
                timer = cooldown;
                StartCoroutine("Timer");
            }
        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, playerTransform.position) >distanceToPlayer)
            {
                LeanTween.cancel(gameObject);
                isCurrentlyDisplayed = false;
                LeanTween.moveY(gameObject, offPos.y, 1f).setEase(LeanTweenType.easeInSine);
                LeanTween.alphaText(myRectTransform, 0f, 1f);
                timer = cooldown;
                StartCoroutine("Timer");
            }
        }
    }

    IEnumerator Timer()
    {
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
