using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class POC_AutoConnect : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner runner;
    [SerializeField] NetworkObject gameManagerPrefab;

    [Header("Room Settings")]
    [SerializeField] string roomName = "Konnect_Room";

    async void Start()
    {
        DebugUI.SetStatus("Connecting to Photon...");
        runner.AddCallbacks(this);

        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = roomName,
            Scene = SceneRef.FromIndex(0),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
            DebugUI.SetStatus("Connected! Waiting for player...");
        else
            DebugUI.SetStatus($"Connection failed: {result.ShutdownReason}");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        DebugUI.SetStatus($"Player joined: {player}, IsMasterClient: {runner.IsSharedModeMasterClient}");

        if (runner.IsSharedModeMasterClient)
        {
            var existing = FindFirstObjectByType<POC_GameManager>();
            if (existing == null)
            {
                runner.Spawn(gameManagerPrefab);
                DebugUI.SetStatus("GameManager Spawned，Loading Lobby...");
            }
            else
            {
                DebugUI.SetStatus("GameManager already existed，skip Spawning...");
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        DebugUI.SetStatus($"Connection lost{shutdownReason}");
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        DebugUI.SetStatus("Server Connected");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        DebugUI.SetStatus($"Server isconnected: {reason}");
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        DebugUI.SetStatus($"Connection failed: {reason}");
    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
}