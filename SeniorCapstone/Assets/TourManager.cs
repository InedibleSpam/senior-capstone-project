using UnityEngine;

public class TourManager : MonoBehaviour
{
    public static TourManager Instance;

    public int currentStep = 0;

    public GameObject[] stepArrows;     // One arrow/path per step
    public Collider[] lockedDoors;      // Doors that block progress
    public AudioSource narration;       // Current narration
    public TourTest_TTS ttsSpeaker;     // Reference to TTS component
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateStep();
    }

    public void NextStep()
    {
        currentStep++;
        UpdateStep();
    }

    void UpdateStep()
    {
        // Disable all arrows
        foreach (var arrow in stepArrows)
            arrow.SetActive(false);

        // Enable current arrow
        if (currentStep < stepArrows.Length)
            stepArrows[currentStep].SetActive(true);

        // Lock all doors by default
        foreach (var door in lockedDoors)
            door.enabled = true;

        // Example logic per step
        switch (currentStep)
        {
            case 0:
                // First room: everything locked
                break;

            case 1:
                // Unlock first door
                lockedDoors[0].enabled = false;
                break;

            case 2:
                lockedDoors[1].enabled = false;
                break;
        }
    }

    public void PlayNarration(string id)
{
    if (ttsSpeaker == null)
    {
        Debug.LogError("TTS Speaker not assigned!");
        return;
    }

    ttsSpeaker.SpeakByID(id);

    // TEMP: move to next step after delay
    StartCoroutine(WaitAndAdvance());
}

System.Collections.IEnumerator WaitAndAdvance()
{
    yield return new WaitForSeconds(3f); // adjust per narration length
    NextStep();
}

    System.Collections.IEnumerator WaitForNarration()
    {
        yield return new WaitWhile(() => narration.isPlaying);
        NextStep();
    }
}