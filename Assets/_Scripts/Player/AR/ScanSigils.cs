using System;
using Unity.Notifications.Android;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScanSigils : MonoBehaviour
{
    [TagSelector]public string sigilTag;

    public GameObject memoryEffectPrefab;
    public LayerMask layer;

    void Awake()
    {
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, 100, layer))
        {
            Debug.Log($"{hit.collider.gameObject.name}: {hit.collider.gameObject.tag}");
            if (hit.collider.gameObject.CompareTag(sigilTag))
            {
                Debug.Log(hit.collider.gameObject.name);
                OnSigilScanned(hit.collider.gameObject);
            }
        }
    }


    void OnSigilScanned(GameObject sigil)
    {
        Debug.Log($"Sigil Scanned: {sigil.name}");

        // Instantiate ghost effect at fragment position
        if (memoryEffectPrefab != null)
        {
            Instantiate(memoryEffectPrefab, sigil.transform.position, sigil.transform.rotation);
        }

        //Example: Trigger story logic based on fragment name
        if (sigil.name.Contains("Sigil"))
        {
            AndroidNotification notify = new AndroidNotification();
            notify.Title = "Mem 1 Scanned";
            notify.FireTime = DateTime.Now.AddSeconds(5);

            AndroidNotificationCenter.SendNotification(notify, "default");
        }
        else if (sigil.name.Contains("memory02"))
        {
            AndroidNotification notify = new AndroidNotification();
            notify.Title = "Mem 2 Scanned";
            notify.FireTime = DateTime.Now.AddSeconds(5);

            AndroidNotificationCenter.SendNotification(notify, "default");
        }
    }
}
