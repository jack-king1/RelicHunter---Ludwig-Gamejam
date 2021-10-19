using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangIndicator : MonoBehaviour
{
    public GameObject boomerangIndicator;

    void Update()
    {
        PointToMouse(boomerangIndicator.transform);
    }

    private void PointToMouse(Transform boomerangUI)
    {
        var mouseScreenPos = Input.mousePosition;
        var startingScreenPos = Camera.main.WorldToScreenPoint(boomerangUI.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;
        var angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
        boomerangUI.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
