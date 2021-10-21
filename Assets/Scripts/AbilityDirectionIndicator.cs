using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDirectionIndicator : MonoBehaviour
{
    public GameObject directionIndicatorSprite;

    void Update()
    {
        PointToMouse(directionIndicatorSprite.transform);
    }

    private void PointToMouse(Transform directionSpriteGO)
    {
        var mouseScreenPos = Input.mousePosition;
        var startingScreenPos = Camera.main.WorldToScreenPoint(directionSpriteGO.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;
        var angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
        directionSpriteGO.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
