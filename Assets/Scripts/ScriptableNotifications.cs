using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "UIScriptedNotification", menuName = "Scripted Object/Notification")]
public class ScriptableNotifications : ScriptableObject
{
    public string header = "blank";
    public string message = "blank message";
    public float activationTime = 0.0f;//at what time in the game should this message display
    public float displayTime = 0.0f; //how long for 
}
