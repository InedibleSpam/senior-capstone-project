using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class ModuleNarration : MonoBehaviour
{
    public TTSSpeaker speaker; 

    [TextArea]
    public string narrationText;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;

            if (speaker != null)
            {
                speaker.Stop(); 
                speaker.Speak(narrationText);
            }
        }
    }
}