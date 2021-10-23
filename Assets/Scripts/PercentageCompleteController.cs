using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageCompleteController : MonoBehaviour
{
    public Transform start;
    public Transform end;
    [SerializeField] private float percentageComplete = 0f;

    private Transform playerReference;

    private void Start()
    {
        playerReference = ServiceLocator.Instance.playerManager.GetPlayer().transform;
    }

    private void Update()
    {
        percentageComplete = playerReference.position.y / end.position.y * 100;
        percentageComplete = Mathf.Clamp(percentageComplete, 0, 100);
        percentageComplete = Mathf.RoundToInt(percentageComplete);
        ServiceLocator.Instance.uiManager.SetPercentageText(percentageComplete);
    }
}
