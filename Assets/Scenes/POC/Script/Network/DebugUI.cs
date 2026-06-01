using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugUI : MonoBehaviour
{
    [SerializeField] Text statusText;

    public static DebugUI Instance;

    void Awake() => Instance = this;

    void Start() => SetStatus("Starting...");

    public static void SetStatus(string msg)
    {
        Debug.Log($"[DebugUI] {msg}");
        if (Instance != null && Instance.statusText != null)
            Instance.statusText.text = msg;
    }
}