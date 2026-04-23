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
            speaker.Speak("The Crew Lock is connected directly into space and is where the crew can conduct spacewalks from. It can support two US suits on the Umbilical Interface Assembly.");
            break;

        case "hatch":
            speaker.Speak("This is hatch is specifically the part of the equipment lock segment, and it allows access between the Equipment lock segment and the crew lock segment.");
            break;

        case "patches":
            speaker.Speak("These patches represent missions, crews, and international partners of the space station. While decorative, they reflect the teamwork and history behind human space exploration.");
            break;

        case "stowage":
            speaker.Speak("This stowage area contains the EMU suits for spacewalking and the necessary equipment to check and maintain them.");
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