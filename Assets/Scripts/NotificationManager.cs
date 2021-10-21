using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public bool playerWantsNotifications = true;
    public List<ScriptableNotifications> notificationQueue = new List<ScriptableNotifications>();
    public List<ScriptableNotifications> tutorialNotifications = new List<ScriptableNotifications>();

    //how long the game has been playing for, perhaps store this in game manager.
    [SerializeField] private float notificationTimer = 0.0f;

    //How long the message needs to be displayed for.
    [SerializeField] private float displayMessageTimer = 0.0f;

    //The delay between current message and next after it goes off screen.
    [SerializeField] private const float messageDelay = 2.0f;

    //the timer for message delay
    [SerializeField] private float delayTimer = 0.0f;

    [SerializeField] private bool messageNotificationTimer_CR = false;
    public void AddNotification(ScriptableNotifications notification)
    {
        notificationQueue.Add(notification);
    }

    public void Update()
    {
        notificationTimer += Time.deltaTime;
        if(delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }

        //If tutorial message is ready to be displayed add it to notification q.
        if (playerWantsNotifications)
        {
            for (int i = 0; i < tutorialNotifications.Count; i++)
            {
                if (tutorialNotifications[i].activationTime <= notificationTimer)
                {
                    //add to notification queue.
                    //Debug.Log("adding message to queue: " + i);
                    notificationQueue.Add(tutorialNotifications[i]);
                    tutorialNotifications.Remove(tutorialNotifications[i]);
                    i--;
                }
            }
        }

        //Ready for next notification
        if(displayMessageTimer <= 0 && delayTimer <= 0)
        {
            if(notificationQueue.Count > 0)
            {
                if(!messageNotificationTimer_CR)
                {
                    if (delayTimer <= 0)
                    {
                        StartCoroutine(MessageDisplayTimer_Coroutine(notificationQueue[0]));
                        notificationQueue.RemoveAt(0);
                    }
                }
            }
        }
    }

    IEnumerator MessageDisplayTimer_Coroutine(ScriptableNotifications notification)
    {
        messageNotificationTimer_CR = true;
        float timer = notification.displayTime;
        //Debug.Log("Timer: " + timer.ToString());
        ServiceLocator.Instance.uiManager.DisplayNotification(notification);

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        //Debug.Log("Moving Notification panel offscreen.");
        ServiceLocator.Instance.uiManager.EndDisplayNotification();
        delayTimer = 2f;
        messageNotificationTimer_CR = false;
        yield return null;
    }
}
