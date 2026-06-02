using UnityEngine;

public class POC_LobbyUI_Mobile : MonoBehaviour
{
    POC_GameManager gm;

    void Start()
    {
        gm = FindFirstObjectByType<POC_GameManager>();
    }

    // Button OnClick
    public void SelectLevel(int index)
    {
        Debug.Log($"[LobbyUI_Mobile] Seletcted Level {index}");
        gm.RPC_SelectLevel(index);
    }
}