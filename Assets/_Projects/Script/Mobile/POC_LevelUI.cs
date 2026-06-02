using UnityEngine;

public class POC_LevelUI : MonoBehaviour
{
    public void BackToLobby()
    {
        FindFirstObjectByType<POC_GameManager>().RPC_BackToLobby();
    }
}