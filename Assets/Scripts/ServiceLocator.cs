using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance;

    public CameraManager cameraManager;
    public PlayerManager playerManager;
    public UIManager uiManager;
    public AudioManager audioManager;
    public GameManager gameManager;
    public NotificationManager notificationManager;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


}
