using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject dynamicCameraObject;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(dynamicCameraObject != null)
        {
            cam.orthographicSize = Mathf.Clamp((10 * (Vector3.Distance(dynamicCameraObject.transform.position,
                ServiceLocator.Instance.playerManager.GetPlayerPosition()) * 0.2f)), 5, 15);
        }
    }

    public void SetDynamicCameraObject(GameObject obj)
    {
        dynamicCameraObject = obj;
    }

    public void ClearDynamicCameraObject(GameObject obj)
    {
        if(obj == dynamicCameraObject)
        {
            dynamicCameraObject = null;
            if(cam != null)
            {
                cam.orthographicSize = 5;
            }
        }
    }
}
