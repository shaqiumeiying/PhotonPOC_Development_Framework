using UnityEngine;

// for debugging only
//public class POC_LobbyUI : MonoBehaviour
//{
//    POC_GameManager gm;

//    void Start()
//    {
//        // Lobby is loaded Additively
//        gm = FindObjectOfType<POC_GameManager>();
//    }

//    public void SelectLevel(int index)
//    {
//        Debug.Log($"[LobbyUI] selected Level {index}");
//        gm.RPC_SelectLevel(index);
//    }
//}


/// ///////////////////////////////////////////////////////////////////


public class POC_LobbyUI : MonoBehaviour
{
    POC_GameManager gm;
    POC_SceneLoader sceneLoader;

    void Start()
    {
        gm = FindFirstObjectByType<POC_GameManager>();
        sceneLoader = FindFirstObjectByType<POC_SceneLoader>();
    }

    // not needed
    public void SelectLevel(int index)
    {
        // VR double-check
        if (sceneLoader.isVRClient)
        {
            Debug.Log("[LobbyUI] VR end cannot select level, Ignoring...");
            return;
        }

        Debug.Log($"[LobbyUI] Mobile end selected: Level {index}");
        gm.RPC_SelectLevel(index);
    }
}