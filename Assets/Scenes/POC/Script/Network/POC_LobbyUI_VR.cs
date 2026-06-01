using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class POC_LobbyUI_VR : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] GameObject debugPanel;

    POC_GameManager gm;
    bool debugUnlocked = false;

    void Start()
    {
        gm = FindFirstObjectByType<POC_GameManager>();
        debugPanel.SetActive(false);
    }

    void Update()
    {
        // ` for debug panel to show
        if (Keyboard.current != null && Keyboard.current.backquoteKey.wasPressedThisFrame)
        {
            debugUnlocked = !debugUnlocked;
            debugPanel.SetActive(debugUnlocked);
            Debug.Log($"[VR Debug] panel: {(debugUnlocked ? "On" : "Off")}");
        }
    }

    public void Debug_SelectLevel(int index)
    {
        Debug.Log($"[VR Debug] force select: Level {index}");
        gm.RPC_SelectLevel(index);
    }
}