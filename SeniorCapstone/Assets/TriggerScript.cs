using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    [Header("Narration Settings")]
    public string narrationID; // e.g., "Cupola" or "Airlock"

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Don't play if already triggered
        if (hasTriggered) return;

        // Detect player (VR cameras/hands usually sit under a CharacterController or Rig)
        if (other.GetComponentInParent<CharacterController>() != null || other.CompareTag("Player"))
        {
            // Find your TTS script in the scene
            Free_Roam_TTS ttsSystem = Object.FindFirstObjectByType<Free_Roam_TTS>();

            if (ttsSystem != null)
            {
                ttsSystem.SpeakByID(narrationID);
                hasTriggered = true; 
                Debug.Log($"Triggered narration for: {narrationID}");
            }
            else
            {
                Debug.LogError("Free_Roam_TTS script not found in scene!");
            }
        }
    }
}