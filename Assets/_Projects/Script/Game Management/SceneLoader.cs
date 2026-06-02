using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class POC_SceneLoader : MonoBehaviour
{
    [HideInInspector] public bool isVRClient = false;

    private string currentContentScene = "";
    private string currentPlatformScene = "";
    private bool isTransitioning = false; // checking if a transition is in progress
                                          // to prevent overlapping requests

    public void GoToLobby()
    {
        string lobbyScene = isVRClient ? "Lobby_VR" : "Lobby_Mobile";
        StartCoroutine(Transition(lobbyScene, loadPlatform: false));
    }

    public void GoToLevel(int index)
    {
        StartCoroutine(Transition($"Level_0{index}", loadPlatform: true));
    }

    IEnumerator Transition(string contentScene, bool loadPlatform)
    {
        // If already transitioning, ignore new requests to prevent conflicts
        if (isTransitioning)
        {
            Debug.LogWarning($"[SceneLoader] transisionning, please ignore: {contentScene}");
            yield break;
        }

        isTransitioning = true;
        DebugUI.SetStatus($"Switching scene: {contentScene}...");

        // unload old scenes
        if (!string.IsNullOrEmpty(currentPlatformScene))
        {
            Debug.Log($"[SceneLoader] Unloading: {currentPlatformScene}");
            yield return SceneManager.UnloadSceneAsync(currentPlatformScene);
            currentPlatformScene = "";
        }

        // unload old content scene (if has mobile or vr)
        if (!string.IsNullOrEmpty(currentContentScene))
        {
            Debug.Log($"[SceneLoader] Unloading: {currentContentScene}");
            yield return SceneManager.UnloadSceneAsync(currentContentScene);
            currentContentScene = "";
        }

        // loads new content scene
        Debug.Log($"[SceneLoader] loading: {contentScene}");
        yield return SceneManager.LoadSceneAsync(contentScene, LoadSceneMode.Additive);
        currentContentScene = contentScene;

        // loads new platform scene if needed
        if (loadPlatform)
        {
            var gm = FindFirstObjectByType<POC_GameManager>();
            int level = gm != null ? gm.CurrentLevel : 1;
            string platform = isVRClient ? $"VR_Content_0{level}" : $"Mobile_Content_0{level}";

            Debug.Log($"[SceneLoader] loading: {platform}");
            yield return SceneManager.LoadSceneAsync(platform, LoadSceneMode.Additive);
            currentPlatformScene = platform;
        }

        isTransitioning = false;
        Debug.Log($"[SceneLoader] loading complete! {contentScene}" +
                  (!string.IsNullOrEmpty(currentPlatformScene) ? $" + {currentPlatformScene}" : ""));
        DebugUI.SetStatus($"Entering: {contentScene}");
    }
}