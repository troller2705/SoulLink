using System;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScanSigils : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    public GameObject memoryEffectPrefab;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var image in args.added)
        {
            OnSigilScanned(image);
        }

        foreach (var image in args.updated)
        {
            if (image.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
                OnSigilScanned(image);
        }
    }

    void OnSigilScanned(ARTrackedImage image)
    {
        Debug.Log($"Sigil Scanned: {image.referenceImage.name}");

        // Instantiate ghost effect at fragment position
        if (memoryEffectPrefab != null)
        {
            Instantiate(memoryEffectPrefab, image.transform.position, image.transform.rotation);
        }

        // Example: Trigger story logic based on fragment name
        if (image.referenceImage.name == "memory01")
        {
            AndroidNotification notify = new AndroidNotification();
            notify.Title = "Mem 1 Scanned";
            notify.FireTime = DateTime.Now.AddSeconds(5);

            AndroidNotificationCenter.SendNotification(notify, "default");
        }
        else if (image.referenceImage.name == "memory02")
        {
            AndroidNotification notify = new AndroidNotification();
            notify.Title = "Mem 2 Scanned";
            notify.FireTime = DateTime.Now.AddSeconds(5);

            AndroidNotificationCenter.SendNotification(notify, "default");
        }
    }
}
