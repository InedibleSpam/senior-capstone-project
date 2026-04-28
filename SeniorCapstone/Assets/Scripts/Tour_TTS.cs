using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System.Text.RegularExpressions;

public class Tour_TTS : MonoBehaviour
{
    private TTSSpeaker speaker;

    private Dictionary<string, string> narrationMap;

    public event System.Action<string> OnNarrationFinished;

    private string currentNarrationID;

    private Queue<string> speakQueue = new Queue<string>();

    void Awake()
    {
        speaker = GetComponent<TTSSpeaker>();

        if (speaker == null)
        {
            Debug.LogError("TTSSpeaker component not found on this GameObject!");
        }

        // Subscribe to the finished speaking event
        speaker.Events.OnPlaybackComplete.AddListener((spk, clipId) => SpeakNext());

        // Centralized narration definitions
        narrationMap = new Dictionary<string, string>()
        {
            { "Airlock", "Welcome to the Airlock! While there are actually many airlocks on the International Space Station, the one you are currently in is refered to as the Quest Airlock. Installed in 2001, the Quest Airlock is the primary exit and reentry point for astronauts in US made spacesuits, while also supporting the Russian Orlan spacesuit. It contains two segments, an equipment lock segment and a crew lock segment." },
            { "BEAM", "Welcome to the Bigelow Expandable Activity Module, also known as BEAM! This is an expandable habitat aboard the International Space Station, installed in 2016. In addition to potentially providing astronauts with a comfortable living space and work area, expandable modules like BEAM greatly reduce the payload volume required for transportation. NASA believes this technology could also be beneficial in future missions to Mars. Initially meant to only serve as a two year test for the technology and its applications, BEAM has remained a part of the ISS, now serving as an extra storage module." },
            { "PMM", "Welcome to the Permanent Multipurpose Module! The PMM is more commonly known as Leonardo, named after the famous Italian artist and inventor. Leonardo is mainly used as a storage area,having 2,472 cubic feet of space inside. Leonardo was initially a Multi-Purpose Logistics Module, used by NASA to bring supplies to and from the space station. It was involved in 8 flights before the decision was made to convert it to a permanate fixture on the ISS in 2011." },
            { "Cupola", "Welcome to the Cupola Module! Cupola is the main observation module on the International Space Station. There are seven large windows that are positioned to provide astronauts a viewing area of the Earth, as well as watching incoming space shuttles and monitoring spacewalks. Additionally, the Cupola houses one of the workstations to control the robotic arm Canadarm2, which is used to assist space shuttle docking and the installation of new modules. Cupola was part of the original ISS design, and installed in 2010." },
            { "US_Lab", "Welcome to the Destiny module, also known as the US Lab! Destiny is the primary research laboratory for NASA. Experiments in many different fields, including physics, biotechnology, Earth science, medicine, and materials science have been conducted here since its installation in 2001. In addition to research, Destiny features the nadir window which offers Earth observation capabilities.Destiny can hold up to 24 equipment racks containing different types of scientific equipment." },
            { "Columbus", "Welcome to the Columbus Laboratory Module! Columbus is another multifunctional pressurized laboratory used by astronauts on the ISS for scientific research. Columbus also has the feature of enabling experiments outside the module in a weightless environment. Columbus was constructed by the European Space Agency and installed in 2008." },
            { "JPM", "Welcome to the Kibo Module! Kibo is the Japanese Experiment Module, and currently the largest single module on the ISS. It was developed by the Japan Aerospace Exploration Agency and installed in 2008. This part of the module, called the Pressurized Module, is used for various scientific experiments, including in biology, physics, and technology. Kibo also contains an airlock to be used for experimenting in space." },
            { "JLP", "Welcome to the the Kibo Logistics Module! This pressurized section of the Kibo module was a later addition, and is mainly used as a storage space for experiment payloads and maintenance tools and supplies." },
            { "Welcome", "Welcome to the International Space Station Guided Tour! Follow the arrows to explore different modules and learn about life in space." },
            { "Finished", "The Guided Tour is now complete! We hope you enjoyed exploring the International Space Station. You can either restart the tour, or return to the title screen. Thank you for visiting!" }
        };
    }

    private HashSet<string> spokenModules = new HashSet<string>();

    public void SpeakByID(string id)
    {
        if (speaker == null || spokenModules.Contains(id)) return;

        if (narrationMap.TryGetValue(id, out string text))
        {
            currentNarrationID = id;
            spokenModules.Add(id);

            // Split text into sentences
            string[] sentences = Regex.Split(text, @"(?<=[.!?])\s+");
            foreach (string sentence in sentences)
            {
                string trimmed = sentence.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    speakQueue.Enqueue(trimmed);
                }
            }

            SpeakNext();
        }
        else
        {
            Debug.LogWarning("No narration found for ID: " + id);
        }
    }

    private void SpeakNext()
    {
        if (speakQueue.Count > 0)
        {
            string part = speakQueue.Dequeue();
            speaker.Speak(part);
        }
        else
        {
            OnNarrationFinished?.Invoke(currentNarrationID);
        }
    }

    public void SpeakRaw(string text)
    {
        if (speaker == null) return;
        speaker.Speak(text);
    }

    // Check if the speaker is currently playing audio
    public bool IsAudioPlaying()
    {
        if (speaker == null) return false;
        
        AudioSource audioSource = speaker.GetComponent<AudioSource>();
        if (audioSource == null) return false;
        
        return audioSource.isPlaying;
    }

    void OnDisable()
    {
        Debug.Log("⚠️ TTSManager was disabled!");
    }
}