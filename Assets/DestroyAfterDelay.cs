using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    private float timer = 2f;
    public void DestroyOnDelay(float time)
    {
        timer = time;
        StartCoroutine("DestroyThis");
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
