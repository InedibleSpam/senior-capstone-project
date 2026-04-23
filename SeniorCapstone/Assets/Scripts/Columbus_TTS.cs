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
            speaker.Speak("“The Advanced Diagnostic System, or ADSEP,is part of the station’s internal support electronics. It helps monitor systems, manage data, and keep onboard equipment operating reliably.");
            break;

        case "biofab":
            speaker.Speak("The BioFabrication Facility is a space-based 3D bioprinter. Scientists use it to create tissue samples in microgravity for future medical breakthroughs.");
            break;

        case "SABL":
            speaker.Speak("The Space Automated Bioproduct Laboratory, or SABL, is a scientific incubator used to study living organisms in space. It carefully controls temperature and conditions for biological experiments.");
            break;

        case "techshot":
            speaker.Speak("This Techshot research unit supports advanced biotechnology experiments aboard the station. Systems like this help scientists study cells, tissues, and future medical manufacturing in microgravity.");
            break;

        case "veggielightcap":
            speaker.Speak("This Veggie light cap provides the special LED lighting used to grow plants aboard the station. It helps astronauts study space farming for future missions.");
            break;

        case "nanoracks":
            speaker.Speak("This NanoRacks unit supports commercial and educational research in microgravity. It allows organizations on Earth to run experiments aboard the space station.");
            break;
        
        case "manufacturingdevice":
            speaker.Speak("This manufacturing device allows astronauts to produce tools and parts in space. Technologies like this help future crews become more self-sufficient on long missions.");
            break;

        case "tangolab":
            speaker.Speak("TangoLab is an automated mini-laboratory used to run scientific experiments in space. It allows researchers on Earth to study biology and materials in microgravity without needing astronaut involvement.");
            break;

        case "xroots":
            speaker.Speak("eXposed Root On-Orbit Test System, or XROOTS, is a plant-growth technology experiment that studies how crops can grow with roots exposed to air in microgravity. Systems like this may help feed future crews on the Moon or Mars.");
            break;

        case "platereader":
            speaker.Speak("This plate reader is a scientific instrument used to analyze many small samples at once. It helps researchers study biology, chemistry, and medicine in microgravity.");
            break;

        case "spacefibers":
            speaker.Speak("SpaceFibers is a materials research experiment that studies how advanced fibers can be produced in microgravity. Some materials may be made with higher quality in space than on Earth.");
            break;

        case "spacebornecomputer":
            speaker.Speak("The Spaceborne Computer test high-performance computing in orbit. Installed in Columbus, it helps researchers explore faster onboard data processing for future deep-space missions.");
            break;

        case "staars":
            speaker.Speak("STaARS is an electronic control system used to manage research hardware inside Columbus. Panels like this help distribute power, monitor equipment, and support onboard experiments.");
            break;

        case "columbuspatches":
            speaker.Speak("These patches represent missions, crews, and international partners connected to the Columbus laboratory. They reflect the teamwork behind scientific research in orbit.");
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