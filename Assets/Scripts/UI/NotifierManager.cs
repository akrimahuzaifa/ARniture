using Michsky.MUIP;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NotifierManager : MonoBehaviour
{
    public string[] typesOfNotification;
    public List<NotificationsKeyValue> notificationsKeyValues = new List<NotificationsKeyValue>();
    
    [SerializeField] private NotificationManager[] notificationPanels;
    NotificationsKeyValue obj;

    private void Reset()
    {
        notificationPanels = GetComponentsInChildren<NotificationManager>();
        typesOfNotification = new string[notificationPanels.Length];

        for (int i = 0; i < notificationPanels.Length; i++)
        {
            typesOfNotification[i] = notificationPanels[i].name[(notificationPanels[i].name.LastIndexOf("-") + 1)..];
        }

        for (int i = 0; i < notificationPanels.Length; i++)
        {
            obj = new NotificationsKeyValue
            {
                Key = typesOfNotification[i],
                Value = notificationPanels[i]
            };
            notificationsKeyValues.Add(obj);
        }
    }

    private void ClearAllLists()
    {
        notificationPanels = null;
        Array.Clear(typesOfNotification, 0, typesOfNotification.Length);
    }

    public void Notify(string type)
    {
        for (int i = 0; i < notificationsKeyValues.Count; i++)
        {
            if (type == notificationsKeyValues[i].Key)
            {
                notificationsKeyValues[i].Value.OpenNotification();
            }
        }
    }
}

[System.Serializable]
public class NotificationsKeyValue
{
    public string Key;
    public NotificationManager Value;
}
