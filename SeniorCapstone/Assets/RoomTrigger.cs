using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public int stepToTrigger;
    public AudioClip narrationClip;
    public string narrationID;

    private void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag("Player")) return;

    if (TourManager.Instance.currentStep == stepToTrigger)
    {
        TourManager.Instance.PlayNarration(narrationID);
    }
}
}