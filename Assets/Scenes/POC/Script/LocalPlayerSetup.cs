using Fusion;
using UnityEngine;

public class LocalPlayerSetup : NetworkBehaviour
{
    [Header("player camera and listener")]
    public Camera localCamera;
    public AudioListener localAudioListener;

    public override void Spawned()
    {
        bool isMyCharacter = HasInputAuthority || (Runner.GameMode == GameMode.Shared && HasStateAuthority);

        if (!isMyCharacter)
        {
            // if this is not my character
            // disable the camera and audio listener
            // to avoid conflicts with the local player's own camera and audio listener
            if (localCamera != null)
                localCamera.enabled = false;

            if (localAudioListener != null)
                localAudioListener.enabled = false;

            // if there are other components that should only be active for the local player,
            // disable them here as well
            // if (mobileCanvas != null) mobileCanvas.enabled = false;
        }
        else
        {
            // this is my character, enable the camera and audio listener
            if (localCamera != null)
                localCamera.enabled = true;

            if (localAudioListener != null)
                localAudioListener.enabled = true;
        }
    }
}