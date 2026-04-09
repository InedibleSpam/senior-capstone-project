using UnityEngine;
using System.Collections;

public class TourManager : MonoBehaviour
{
    public static TourManager Instance;

    [Header("Tour State")]
    public int currentStep = 0;
    private bool isWelcome = true;
    private bool isFinished = false;

    [Header("Narration IDs")]
    public string welcomeNarrationID = "Welcome";
    public string finishedNarrationID = "Finished";

    [Header("Arrows")]
    public GameObject[] stepArrows; // Step-based arrow groups

    [Header("Doors")]
    public Collider[] lockedDoors; // Doors in order

    [Header("TTS")]
    public TourTest_TTS ttsSpeaker;
    private bool isNarrating = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayNarration(welcomeNarrationID);
    }

    // 🔊 Called by RoomTrigger
    public void PlayNarration(string id)
    {
        if (ttsSpeaker == null)
        {
            Debug.LogError("TTS Speaker not assigned!");
            return;
        }

        Debug.Log("🔊 Playing narration: " + id);

        isNarrating = true;

        StopAllCoroutines();
        SetAllDoors(true); // LOCK ALL DOORS
        SetAllArrows(false);

        ttsSpeaker.SpeakByID(id);

        StartCoroutine(WaitAndAdvance());
    }

    IEnumerator WaitAndAdvance()
    {
        yield return new WaitForSeconds(4f); // ideally replace with TTS completion later

        Debug.Log("➡️ Advancing after narration");

        isNarrating = false;

        if (isWelcome)
        {
            isWelcome = false;
            currentStep = 0;
            UpdateStep();
        }
        else if (isFinished)
        {
            // Tour finished, perhaps reset or do nothing
            Debug.Log("Tour finished!");
        }
        else
        {
            NextStep();
        }
    }

    public void NextStep()
    {
        if (currentStep + 1 >= stepArrows.Length)
        {
            isFinished = true;
            PlayNarration(finishedNarrationID);
        }
        else
        {
            currentStep++;
            UpdateStep();
        }
    }

void UpdateStep()
    {
        Debug.Log("➡️ Updating to Step: " + currentStep);

        if (lockedDoors != null)
        {
            for (int i = 0; i < lockedDoors.Length; i++)
            {
                if (lockedDoors[i] != null)
                {
                    // Unlock doors at (currentStep - 1) and (currentStep)
                    bool shouldUnlock =
                        (i == currentStep) ||
                        (i == currentStep - 1);

                    // Lock everything else
                    lockedDoors[i].enabled = !shouldUnlock;
                }
            }
        }

        UpdateArrows();
    }

    void SetAllDoors(bool locked)
    {
        if (lockedDoors == null) return;

        foreach (var door in lockedDoors)
        {
            if (door != null)
                door.enabled = locked;
        }
    }

    void UpdateArrows()
    {
        if (stepArrows == null) return;

        // Turn OFF all arrows
        foreach (var arrow in stepArrows)
        {
            if (arrow != null)
                arrow.SetActive(false);
        }

        // Turn ON current step arrows
        if (currentStep < stepArrows.Length && stepArrows[currentStep] != null)
        {
            stepArrows[currentStep].SetActive(true);
        }
    }

    void SetAllArrows(bool state)
    {
        if (stepArrows == null) return;

        foreach (var arrow in stepArrows)
        {
            if (arrow != null)
                arrow.SetActive(state);
        }
    }
}