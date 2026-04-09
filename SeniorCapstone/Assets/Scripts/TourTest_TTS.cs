using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;

public class TourTest_TTS : MonoBehaviour
{
    private TTSSpeaker speaker;

    private Dictionary<string, string> narrationMap;

    void Awake()
    {
        speaker = GetComponent<TTSSpeaker>();

        if (speaker == null)
        {
            Debug.LogError("TTSSpeaker component not found on this GameObject!");
        }

        // Centralized narration definitions
        narrationMap = new Dictionary<string, string>()
        {
            { "Airlock", "This is the airlock." },
            { "BEAM", "This is the BEAM Module." },
            { "PMM", "This is the PMM Module." },
            { "Cupola", "This is the Cupola Module." },
            { "US_Lab", "This is the US Lab Module." },
            { "Columbus", "This is the Columbus Module." },
            { "JPM", "This is the JPM Module." },
            { "JLP", "This is the JLP Module." }
        };
    }

    public void SpeakByID(string id)
    {
        if (speaker == null) return;

        if (narrationMap.TryGetValue(id, out string text))
        {
            speaker.Speak(text);
        }
        else
        {
            Debug.LogWarning("No narration found for ID: " + id);
        }
    }

    public void SpeakRaw(string text)
    {
        if (speaker == null) return;
        speaker.Speak(text);
    }

    void OnDisable()
    {
        Debug.Log("⚠️ TTSManager was disabled!");
    }
}