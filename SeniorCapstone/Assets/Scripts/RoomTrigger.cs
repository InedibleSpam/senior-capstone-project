using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [Header("Tour Settings")]
    public int stepToTrigger;
    public string narrationID;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        Debug.Log("Trigger entered by: " + other.name + " | Tag: " + other.tag);

        // Detect player via CharacterController (recommended for VR)
        if (other.GetComponentInParent<CharacterController>() != null)
        {
            Debug.Log("🟢 Player detected in trigger");

            TourManager manager = TourManager.Instance;

            if (manager == null)
            {
                Debug.LogError("TourManager instance not found!");
                return;
            }

            // Ensure correct step
            if (manager.currentStep == stepToTrigger)
            {
                Debug.Log("✅ Correct step. Triggering narration: " + narrationID);

                hasTriggered = true;

                manager.PlayNarration(narrationID);
            }
            else
            {
                Debug.Log("🟡 Wrong step. Current: " 
                    + manager.currentStep + 
                    " Expected: " + stepToTrigger);
            }
        }
        else
        {
            Debug.Log("🔴 Object entered trigger is not the player");
        }
    }
}