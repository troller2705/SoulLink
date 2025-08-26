using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class Checker : MonoBehaviour
{
    public enum PlayerPlatform
    {
        VR,
        AR,
        Unknown
    }

    public static PlayerPlatform CurrentPlatform { get; private set; } = PlayerPlatform.Unknown;

    void Awake()
    {
        // VR Detection using OpenXR (Quest)
        if (XRSettings.isDeviceActive)
        {
            string deviceName = XRSettings.loadedDeviceName.ToLower();
            Debug.Log("XR Device: " + deviceName);

            // Set the initial orientation
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            CurrentPlatform = PlayerPlatform.VR;
            Debug.Log("Detected VR via OpenXR");
            return;
        }

        // AR Detection using AR Foundation (on Android)
        if (Application.platform == RuntimePlatform.Android)
        {
            // Set the initial orientation
            Screen.orientation = ScreenOrientation.Portrait;
            CurrentPlatform = PlayerPlatform.AR;
            Debug.Log("Detected AR via AR Foundation");
            return;
        }

        // Fallback
        CurrentPlatform = PlayerPlatform.Unknown;
        Debug.LogWarning("Platform could not be detected.");
    }

    void Start()
    {
        switch (CurrentPlatform)
        {
            case PlayerPlatform.VR:
                Debug.Log("Loading VR mode...");
                SceneManager.LoadScene("VR");
                break;
            case PlayerPlatform.AR:
                Debug.Log("Loading AR mode...");
                SceneManager.LoadScene("AR");
                break;
            case PlayerPlatform.Unknown:
                break;
        }
    }
}
