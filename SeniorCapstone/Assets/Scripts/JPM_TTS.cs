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
            speaker.Speak("These patches represent missions, astronauts, and Japan’s contribution to the International Space Station. They reflect the teamwork and scientific spirit of the Kibo laboratory");
            break;

        case "cryo_chiller":
            speaker.Speak("This cryo chiller keeps scientific samples and equipment at carefully controlled low temperatures. Cooling systems like this are important for biological and medical research in space.");
            break;

        case "iceberg":
            speaker.Speak("Iceberg is a refrigerated storage unit used to preserve scientific samples in orbit. Systems like this protect valuable research materials until they can be studied or returned to Earth.");
            break;

        case "merlin":
            speaker.Speak("Microgravity Experiment Research Locker / Incubator, or MERLIN, is a temperature-controlled research locker used to store and incubate scientific samples aboard the station. It helps scientists perform biological experiments in space.");
            break;

        case "adsep":
            speaker.Speak("ADSEP is a research payload used to support biomedical experiments aboard the station. Equipment like this helps scientists study health, diagnostics, and life sciences in microgravity.");
            break;

        case "bone_densinometer":
            speaker.Speak("This bone densitometer measures changes in bone strength during spaceflight. Scientists use it to study how microgravity affects the human body.");
            break;

        case "sabl":
            speaker.Speak("SABL stands for Space Automated Bioproduct Laboratory. It is an automated incubator used to study living organisms in microgravity. Research like this helps scientists understand how space affects biology and can lead to new discoveries in medicine and biotechnology.");
            break;

        case "mli":
            speaker.Speak("MLI is a rack-mounted laboratory instrument used for scientific research aboard the station. Systems like this allow astronauts to run controlled experiments in microgravity.");
            break;

        case "polar":
            speaker.Speak("POLAR is a scientific freezer used to preserve research samples aboard the station. It keeps valuable materials cold for later study or return to Earth.");
            break;

        case "mvp":
            speaker.Speak("MVP is a multi-purpose research unit used to support scientific experiments aboard the station. Flexible systems like this allow different studies to be performed in microgravity.");
            break;

        case "biofuel_fridge":
            speaker.Speak("This BioServe research unit supports biological experiments aboard the station. Systems like this help scientists study living organisms and human health in microgravity.");
            break;
        
        case "declic_ell":
            speaker.Speak("DECLIC is a fluid physics experiment that studies how liquids behave in microgravity. Research like this helps scientists better understand materials and thermal systems.");
            break;
        case "declic_exl":
            speaker.Speak("This unit is part of the DECLIC experiment, which studies how liquids and heat behave in microgravity. Different modules support various phases of the research.");
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