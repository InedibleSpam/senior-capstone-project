using UnityEngine;
using UnityEngine.UI; // This lets us use buttons
using Meta.WitAi.TTS.Utilities; // This is for TTSSpeaker (adjust if your namespace is different, e.g., Oculus.VoiceSDK.UX)

public class TTSButtonHandler : MonoBehaviour
{
    [SerializeField] private TTSSpeaker speaker; // We'll drag the speaker here later

    public void TriggerTTS()
    {
        if (speaker != null)
        {
            speaker.Speak("If you hear this TTS is working"); // You can change this text later
        }
        else
        {
            Debug.LogError("No TTSSpeaker assigned!");
        }
    }
}