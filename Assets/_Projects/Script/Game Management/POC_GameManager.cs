using Fusion;
using System.Collections;
using UnityEngine;
using static AsymmetricSpawner;

public class POC_GameManager : NetworkBehaviour
{
    public enum State { Lobby, InLevel }
    [Networked] public State CurrentState { get; set; }
    [Networked] public int CurrentLevel { get; set; }

#if UNITY_EDITOR
    [SerializeField] bool editorForceVR = false;
#endif

    ChangeDetector changes;
    POC_SceneLoader sceneLoader;

    public override void Spawned()
    {
        Debug.Log("[GameManager] Spawned function activated");
        changes = GetChangeDetector(ChangeDetector.Source.SimulationState);

        // prevent potential issues with sceneLoader not being ready in the same frame
        StartCoroutine(InitAfterFrame());
    }

    IEnumerator InitAfterFrame()
    {
        yield return null; // wait a frame

        sceneLoader = FindFirstObjectByType<POC_SceneLoader>();

        if (sceneLoader == null)
        {
            Debug.LogError("[GameManager] Can't find SceneLoader! Check if Main Scene loaded POC_SceneLoader?");
            DebugUI.SetStatus("Error: SceneLoader not found");
            yield break;
        }

        Debug.Log("[GameManager] SceneLoader found!");

        bool isVR;
#if UNITY_EDITOR
        

        if (ParrelSync.ClonesManager.IsClone())
        {
            isVR = false;
            Debug.Log($"[GameManager] Editor mode. Forcing to{(isVR ? "VR" : "Mobile")}");
        }
        else
        {
            isVR = true;
            Debug.Log($"[GameManager] ParrelSync Cloned Editor，Forcing VR mode");
        }
#else
        isVR = UnityEngine.XR.XRSettings.isDeviceActive;
        Debug.Log($"[GameManager] real build，XR detection: {(isVR ? "VR" : "Mobile")}");
#endif

        sceneLoader.isVRClient = isVR;
        DebugUI.SetStatus($"Role: {(isVR ? "VR" : "Mobile")}, loading Lobby...");

        if (Object.HasStateAuthority)
            CurrentState = State.Lobby;

        sceneLoader.GoToLobby();
    }

    public override void Render()
    {
        if (changes == null) return; // avoid null reference if Render called before Spawned

        foreach (var change in changes.DetectChanges(this))
        {
            if (change == nameof(CurrentState) || change == nameof(CurrentLevel))
                ApplyState();
        }
    }

    void ApplyState()
    {
        if (sceneLoader == null) return;

        if (CurrentState == State.Lobby)
            sceneLoader.GoToLobby();
        else
            sceneLoader.GoToLevel(CurrentLevel);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SelectLevel(int levelIndex)
    {
        CurrentLevel = levelIndex;
        CurrentState = State.InLevel;
        Debug.Log($"[GameManager] Host received selection: Level {levelIndex}");
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_BackToLobby()
    {
        CurrentState = State.Lobby;
    }
}