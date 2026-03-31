using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class TourTest_TTS : MonoBehaviour
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
        case "Airlock":
            speaker.Speak("This is the airlock.");
            break;

        case "BEAM":
            speaker.Speak("This is the BEAM Module.");
            break;

        case "PMM":
            speaker.Speak("This is the PMM Module.");
            break;

        case "Cupola":
            speaker.Speak("This is the Cupola Module.");
            break;

        case "US_Lab":
            speaker.Speak("This is the US Lab Module.");
            break;

        case "Columbus":
            speaker.Speak("This is the Columbus Module.");
            break;

        case "JPM":
            speaker.Speak("This is the JPM Module.");
            break;

        case "JLP":
            speaker.Speak("This is the JLP Module.");
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