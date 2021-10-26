using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanelInit : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Image>().enabled = true;
    }
}
