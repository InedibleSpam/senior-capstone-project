using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class JPM_TTS : MonoBehaviour
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

        case "cryo_chiller":
            speaker.Speak("These are cryo chillers.");
            break;

        case "iceberg":
            speaker.Speak("These are icebergs.");
            break;

        case "merlin":
            speaker.Speak("This is the Merlin.");
            break;

        case "adsep":
            speaker.Speak("This is ADSEP.");
            break;

        case "bone_densinometer":
            speaker.Speak("This is the Bone Densinometer.");
            break;

        case "sabl":
            speaker.Speak("This is SABL.");
            break;

        case "mli":
            speaker.Speak("This is the MLI.");
            break;

        case "polar":
            speaker.Speak("This is the polar.");
            break;

        case "mvp":
            speaker.Speak("This is the MVP.");
            break;

        case "biofuel_fridge":
            speaker.Speak("This is a biofuel fridge.");
            break;
        
        case "declic_ell":
            speaker.Speak("This is the Declic Ell.");
            break;
        case "declic_exl":
            speaker.Speak("This is the Declic Exl.");
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