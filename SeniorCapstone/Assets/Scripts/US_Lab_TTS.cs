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
            speaker.Speak("These patches represent missions, astronauts, and scientific achievements connected to the Destiny laboratory. They reflect the spirit of research and exploration aboard the station.");
            break;

        case "glacier":
            speaker.Speak("Glacier is a cold storage unit used to keep scientific samples chilled or frozen in space. Systems like this help protect valuable experiment materials.");
            break;

        case "iceberg":
            speaker.Speak("Iceberg is a refrigerated storage unit used to preserve scientific samples aboard the station. It helps keep research materials stable until they can be analyzed or returned to Earth.");
            break;

        case "merlin":
            speaker.Speak("MERLIN stands for Microgravity Experiment Research Locker Incubator. It stores and incubates scientific samples at controlled temperatures during space experiments.");
            break;

        case "sams":
            speaker.Speak("SAMS Two measures motion in three directions aboard the station. Scientists use this data to understand how vibrations may affect research in microgravity.");
            break;

        case "maritimeawareness":
            speaker.Speak("This experiment explores how the space station can support maritime awareness by observing ships, weather, and environmental conditions across Earth's oceans. It helps demonstrate the station's potential for Earth observation and monitoring.");
            break;

        case "sabl":
            speaker.Speak("SABL stands for Space Automated Bioproduct Laboratory. It is an automated incubator used to study living organisms and biological samples in microgravity.");
            break;

        case "thermalaminescrubber":
            speaker.Speak("The Thermal Amine Scrubber removes carbon dioxide from the station's air. Systems like this help astronauts breathe safely during long space missions.");
            break;

        case "bric-led":
            speaker.Speak("BRIC LED stands for Biological Research in Canisters Light Emitting Diodes. It studies how living organisms grow and respond to space conditions using controlled lighting.");
            break;

        case "polar":
            speaker.Speak("POLAR is a scientific freezer used aboard the station to preserve research samples at low temperatures. It helps keep biological materials safe for later study.");
            break;

        case "t-cmm":
            speaker.Speak("This Made In Space manufacturing system tests how tools and parts can be produced in orbit. Technologies like this may help future crews become more self-sufficient.");
            break;

        case "muses_mdcs":
            speaker.Speak("This MUSES control unit helps operate Earth observation instruments that monitor weather, coastlines, disasters, and environmental change. It demonstrates how the station can support Earth science research.");
            break;

        case "mvp":
            speaker.Speak("MVP is a multi-purpose research unit used to support a variety of scientific experiments aboard the station. Flexible systems like this allow different studies to be performed in microgravity.");
            break;

        case "nanoracks-zero-g-oven":
            speaker.Speak("This NanoRacks Zero-G Oven allows researchers to process materials in orbit. Experiments like this help develop new manufacturing techniques for future space missions.");
            break;

        case "cal_pwr._elec._sys":
            speaker.Speak("This unit powers the Cold Atom Lab, a NASA experiment that studies quantum physics by cooling atoms to extremely low temperatures aboard the station.");
            break;

        case "cal_sci_inst":
            speaker.Speak("This Cold Atom Lab instrument creates ultra-cold atoms near absolute zero. In microgravity, researchers can study quantum behavior more effectively than on Earth.");
            break;

        case "cgba":
            speaker.Speak("The Commercial Generic Bioprocessing Apparatus supports biological research in space. It helps scientists study cells, microbes, and other living samples in microgravity.");
            break;

        case "elbow_screens":
            speaker.Speak("These display screens help astronauts monitor station systems, review procedures, and manage experiments aboard the laboratory module.");
            break;
        
        case "mss_av":
            speaker.Speak("This workstation is used to monitor and control the station's robotic systems, including Canadarm Two. Astronauts use consoles like this for maintenance and cargo operations.");
            break;

        case "worf_camera_mount":
            speaker.Speak("This camera mount is used to capture high-quality images and videos of Earth, space, and station activities. It helps share the experience of living and working in orbit with people on Earth.");
            break;
        
        case "bike":
            speaker.Speak("This is the Cycle Ergometer with Vibration Isolation and Stabilization System, or CEVIS. It allows astronauts to exercise in microgravity while minimizing vibrations that could affect experiments.");
            break;
        
        case "CEVISmount":
            speaker.Speak("This is part of the CEVIS exercise support system. It helps secure the equipment and reduce vibrations while astronauts exercise aboard the station.");
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