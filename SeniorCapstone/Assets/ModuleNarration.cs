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
        Debug.Log("Something entered: " + other.name);

        if (other.CompareTag("Player") && !hasPlayed)
        {
            Debug.Log("PLAYER ENTERED TRIGGER");
            speaker.Speak(narrationText);
            hasPlayed = true;
        }
    }
}