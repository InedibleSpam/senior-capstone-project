using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class TTSTest : MonoBehaviour
{
    private TTSSpeaker speaker;

    void Start()
    {
        // Automatically find TTSSpeaker on the same GameObject
        speaker = GetComponent<TTSSpeaker>();

        if (speaker != null)
        {
            speaker.Speak("Hello. If you hear this, text to speech is working.");
        }
        else
        {
            Debug.LogError("TTSSpeaker component not found on this GameObject!");
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

