using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class Node1_TTS : MonoBehaviour
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
        case "patches":
            speaker.Speak("These are patches.");
            break;

        case "biofuel_fridge":
            speaker.Speak("This is a biofuel fridge.");
            break;

        case "food_warmer":
            speaker.Speak("This is a food warmer.");
            break;

        case "galley_middle":
            speaker.Speak("This is the galley middle.");
            break;

        case "pwd":
            speaker.Speak("This is the PWD.");
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