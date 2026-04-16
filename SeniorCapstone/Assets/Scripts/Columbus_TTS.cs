using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class Columbus_TTS : MonoBehaviour
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
        case "ADSEP":
            speaker.Speak("This is the ADSEP.");
            break;

        case "biofab":
            speaker.Speak("This is a biofabricator.");
            break;

        case "SABL":
            speaker.Speak("This is the SABL.");
            break;

        case "techshot":
            speaker.Speak("This is the tech shot.");
            break;

        case "veggielightcap":
            speaker.Speak("This is the veggie light cap.");
            break;

        case "nanoracks":
            speaker.Speak("These are the nanoracks.");
            break;
        
        case "manufacturingdevice":
            speaker.Speak("This is the manufacturing device.");
            break;

        case "tangolab":
            speaker.Speak("This is the Tango lab.");
            break;

        case "xroots":
            speaker.Speak("This is the X-Roots.");
            break;

        case "platereader":
            speaker.Speak("This is the plate reader.");
            break;

        case "spacefibers":
            speaker.Speak("This is the space fibers.");
            break;

        case "spacebornecomputer":
            speaker.Speak("This is the spacebornecomputer.");
            break;

        case "staars":
            speaker.Speak("This is the STaARS.");
            break;

        case "columbuspatches":
            speaker.Speak("This is the Columbus Patches.");
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