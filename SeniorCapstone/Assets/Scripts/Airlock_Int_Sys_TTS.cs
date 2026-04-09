using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class Airlock_Int_Sys_TTS : MonoBehaviour
{
    private TTSSpeaker speaker;

    void Start()
    {
        // Automatically find TTSSpeaker on the same GameObject
        speaker = GetComponent<TTSSpeaker>();

        if (speaker == null)
        {
            Debug.LogError("TTSSpeaker component not found on this GameObject!");
        }
    }
    public void SpeakByID(string id)
{
    if (speaker == null) return;

    switch (id)
    {
        case "crewlock":
            speaker.Speak("This is the crew lock.");
            break;

        case "hatch":
            speaker.Speak("This is a hatch.");
            break;

        case "patches":
            speaker.Speak("These are patches.");
            break;

        case "stowage":
            speaker.Speak("This is the stowage area.");
            break;

        default:
            Debug.LogWarning("No narration found for ID: " + id);
            break;
    }
}   

    void OnTranscription(string text)
    {
        Debug.Log("Heard: " + text);

        // Respond using TTS
        if(text.Contains("hello"))
            speaker.Speak("Hi there! Nice to meet you.");
    }
    
}