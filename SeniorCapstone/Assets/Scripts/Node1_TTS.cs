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
            speaker.Speak("These patches in the unity module represent successful missions, spacewalks, or dockings by the crew.");
            break;

        case "biofuel_fridge":
            speaker.Speak("This is the Freezer/Refrigerator/Incubator Device for Galley Experimentation, or FRIDGE for short. It is used to store and incubate biological samples for scientific experiments. Or just food and drinks");
            break;

        case "food_warmer":
            speaker.Speak("This is the food warmer. It functions like a hot plate and heats up food for the crew in ten to thirty minutes.");
            break;

        case "galley_middle":
            speaker.Speak("This is part of the galley area that is used for daily dining and for storing food.");
            break;

        case "pwd":
            speaker.Speak("This is the Potable Water Dispenser or PWD for short. It provides clean, drinkable water for the crew.");
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