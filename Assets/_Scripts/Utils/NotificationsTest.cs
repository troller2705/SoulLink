using UnityEngine;
using Unity.Notifications.Android;
using System.Collections;
using System;

public class NotificationsTest : MonoBehaviour
{
    IEnumerator RequestNotificationPermission()
    {
        PermissionRequest request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;

        if (request.Status == PermissionStatus.Denied) Application.Quit();
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AndroidNotificationChannelGroup group = new AndroidNotificationChannelGroup()
        {
            Id = "Main",
            Name = "Main Notifies"
        };

        AndroidNotificationChannel channel = new AndroidNotificationChannel()
        {
            Id = "default",
            Name = "default channel",
            Importance = Importance.Default,
            Description = "Generic Notifications",
            Group = "Main"
        };

        AndroidNotificationCenter.RegisterNotificationChannelGroup(group);
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        StartCoroutine(RequestNotificationPermission());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
