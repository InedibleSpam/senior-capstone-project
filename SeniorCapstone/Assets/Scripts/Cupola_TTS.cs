using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class Cupola_TTS : MonoBehaviour
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
        case "mssav":
            speaker.Speak("This Mobile Servicing System avionics, or MSSAV, supports the station’s robotic arm network. From the Cupola, astronauts use robotics systems to move cargo and perform external operations.");
            break;

        case "bottomwindow":
            speaker.Speak("These Cupola windows provide one of the best views on the station. Astronauts use them to observe Earth, watch station activities outside, and assist with robotic arm operations.");
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