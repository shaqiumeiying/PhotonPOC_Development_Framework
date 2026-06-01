using Fusion;
using UnityEngine;

public class AsymmetricSpawner : SimulationBehaviour, IPlayerJoined
{
    [Header("Player Prefabs")]
    public NetworkPrefabRef VR_Player_Prefab;
    public NetworkPrefabRef Mobile_Player_Prefab;

    public enum ClientPlatform { VR, Mobile }

    [Header("Current Platform (Auto-set in Editor)")]
    public ClientPlatform currentPlatform;

    private void Awake()
    {
        //this is for testing in editor only
#if UNITY_EDITOR
        if (ParrelSync.ClonesManager.IsClone())
        {
            currentPlatform = ClientPlatform.Mobile;
            Debug.Log("ParrelSync Clone: Mobile");
        }
        else
        {
            currentPlatform = ClientPlatform.VR;
            Debug.Log("ParrelSync Main: VR");
        }
#else
    // Real build Logic: Check XRSettings 
    currentPlatform = UnityEngine.XR.XRSettings.isDeviceActive
        ? ClientPlatform.VR
        : ClientPlatform.Mobile;
    Debug.Log($"Real build:{currentPlatform}, XRActive: {UnityEngine.XR.XRSettings.isDeviceActive}");
#endif
    }

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkPrefabRef prefabToSpawn = currentPlatform == ClientPlatform.VR ? VR_Player_Prefab : Mobile_Player_Prefab;
            Debug.Log($"Spawning {currentPlatform} Player...");

            Runner.Spawn(prefabToSpawn, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }
}