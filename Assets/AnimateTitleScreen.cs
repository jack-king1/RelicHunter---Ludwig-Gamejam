using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTitleScreen : MonoBehaviour
{
    public GameObject title;
    void Start()
    {
        LeanTween.moveLocalY(title, title.transform.localPosition.y + 10f, 5).setLoopPingPong().setEaseInSine();
    }
}
