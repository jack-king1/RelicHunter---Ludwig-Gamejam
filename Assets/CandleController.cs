using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CandleController : MonoBehaviour
{
    public float candleLightSpeed = 5f;
    public float candleReduceSpeed = 0.25f;
    public float minCandleRadius = 1f;
    public float maxCandleRadius = 6f;


    private Light2D playerLight;
    private List<GameObject> bonfireList = new List<GameObject>();

    private void Start()
    {
        playerLight = GetComponentInChildren<Light2D>();
        StartCoroutine("CandleLightCoroutine");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bonfire"))
        {
            if(!bonfireList.Contains(collision.gameObject))
            {
                bonfireList.Add(collision.gameObject);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bonfire"))
        {
            if (bonfireList.Contains(collision.gameObject))
            {
                bonfireList.Remove(collision.gameObject);
            }
        }
    }

    IEnumerator CandleLightCoroutine()
    {
        while(true)
        {
            if(bonfireList.Count > 0)
            {
                playerLight.pointLightOuterRadius += (Time.deltaTime * candleLightSpeed);
                playerLight.pointLightOuterRadius = Mathf.Clamp(playerLight.pointLightOuterRadius,minCandleRadius, maxCandleRadius);
            }
            else
            {
                playerLight.pointLightOuterRadius -= (Time.deltaTime * candleReduceSpeed);
                playerLight.pointLightOuterRadius = Mathf.Clamp(playerLight.pointLightOuterRadius, minCandleRadius, maxCandleRadius);
            }
            yield return null;
        }
        yield return null;
    }

    public Light2D GetCandleLight()
    {
        return playerLight;
    }
}
