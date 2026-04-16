using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class US_Lab_TTS : MonoBehaviour
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

        case "glacier":
            speaker.Speak("These are glaciers.");
            break;

        case "iceberg":
            speaker.Speak("These are icebergs.");
            break;

        case "merlin":
            speaker.Speak("This is the Merlin.");
            break;

        case "sams":
            speaker.Speak("This is Sams.");
            break;

        case "maritimeawareness":
            speaker.Speak("This is Maritime Awareness.");
            break;

        case "sabl":
            speaker.Speak("This is SABL.");
            break;

        case "thermalaminescrubber":
            speaker.Speak("This is the Thermal Amine Scrubber.");
            break;

        case "bric-led":
            speaker.Speak("These are BRIC-LEDs.");
            break;

        case "polar":
            speaker.Speak("This is the polar.");
            break;

        case "t-cmm":
            speaker.Speak("This is the T-CMM.");
            break;

        case "muses_mdcs":
            speaker.Speak("This is the MUSES MDCS.");
            break;

        case "mvp":
            speaker.Speak("This is the MVP.");
            break;

        case "nanoracks-zero-g-oven":
            speaker.Speak("This is the Nanoracks Zero-G Oven.");
            break;

        case "cal_pwr._elec._sys":
            speaker.Speak("This is the CAL PWR._ELEC._SYS.");
            break;

        case "cal_sci_inst":
            speaker.Speak("This is the CAL SCI INST.");
            break;

        case "cgba":
            speaker.Speak("This is the CGBA.");
            break;

        case "elbow_screens":
            speaker.Speak("This is the Elbow Screens.");
            break;
        
        case "mss_av":
            speaker.Speak("This is the MSS AV.");
            break;

        case "worf_camera_mount":
            speaker.Speak("This is the Worf Camera Mount.");
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