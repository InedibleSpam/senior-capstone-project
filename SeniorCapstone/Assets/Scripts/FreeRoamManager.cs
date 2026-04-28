using UnityEngine;

public class FreeRoamManager : MonoBehaviour
{
    public static FreeRoamManager Instance;

    [Header("TTS Reference")]
    public Free_Roam_TTS ttsSpeaker;

    private void Awake()
    {
        // Singleton pattern so the triggers can find this easily
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called by TriggerScript.cs when the player enters a module
    public void PlayNarration(string id)
    {
        if (ttsSpeaker == null)
        {
            // Try to find it if it wasn't assigned in the Inspector
            ttsSpeaker = Object.FindFirstObjectByType<Free_Roam_TTS>();
        }

        if (ttsSpeaker != null)
        {
            Debug.Log($"🔊 Requesting narration for: {id}");
            ttsSpeaker.SpeakByID(id);
        }
        else
        {
            Debug.LogError("ExperienceManager: No Free_Roam_TTS found in scene!");
        }
    }
}