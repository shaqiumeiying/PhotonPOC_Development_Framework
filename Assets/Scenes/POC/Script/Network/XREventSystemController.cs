// Manages the EventSystem for both VR and mobile platforms.
// Enables the appropriate input module based on whether the application
// is running in VR or on a mobile device.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class XREventSystemController : MonoBehaviour
{
    public static XREventSystemController Instance;

    void Awake() => Instance = this;

    public static void Configure(bool isVR)
    {
        if (Instance == null) return;

        var xrModule = Instance.GetComponent<UnityEngine.XR.Interaction.Toolkit.UI.XRUIInputModule>();
        var touchModule = Instance.GetComponent<InputSystemUIInputModule>();

        if (isVR)
        {
            // VR, enables XR module, disables touch module
            if (xrModule != null) xrModule.enabled = true;
            if (touchModule != null) touchModule.enabled = false;
        }
        else
        {
            // mobile, enables touch module, disables XR module
            if (xrModule != null) xrModule.enabled = false;
            if (touchModule == null)
                touchModule = Instance.gameObject.AddComponent<InputSystemUIInputModule>();
            touchModule.enabled = true;
        }

        Debug.Log($"[EventSystem] configureation: {(isVR ? "XR" : "touch pad")}");
    }
}