using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public GameObject[] stepArrows;

    [Header("Arrow Animation Settings")] // ⭐ NEW
    public float floatHeight = 0.2f;
    public float floatSpeed = 2f;
    public float pulseSpeed = 2f;
    public float emissionIntensity = 2f;
    public Color emissionColor = Color.cyan;

    private Dictionary<Transform, Vector3> arrowStartPositions = new Dictionary<Transform, Vector3>(); // ⭐ NEW

    [Header("Doors")]
    public Collider[] lockedDoors;

    [Header("TTS")]
    public TourTest_TTS ttsSpeaker;
    private bool isNarrating = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CacheArrowStartPositions(); // ⭐ NEW
        PlayNarration(welcomeNarrationID);
    }

    void Update() // ⭐ NEW
    {
        AnimateActiveArrows();
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
        SetAllDoors(true);
        SetAllArrows(false);

        ttsSpeaker.SpeakByID(id);

        StartCoroutine(WaitAndAdvance());
    }

    IEnumerator WaitAndAdvance()
    {
        yield return new WaitForSeconds(4f);

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
                    bool shouldUnlock =
                        (i == currentStep) ||
                        (i == currentStep - 1);

                    // Special case: doors 4 and 5 are connected (opposite ends of a hallway)
                    // If either should be unlocked, unlock both
                    if ((i == 4 || i == 5))
                    {
                        bool is4or5Active = (currentStep == 4 || currentStep == 5) || 
                                            (currentStep - 1 == 4 || currentStep - 1 == 5);
                        if (is4or5Active)
                        {
                            shouldUnlock = true;
                        }
                    }

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

        foreach (var arrow in stepArrows)
        {
            if (arrow != null)
                arrow.SetActive(false);
        }

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

    // ⭐ NEW: Cache starting positions
    void CacheArrowStartPositions()
    {
        foreach (var arrowGroup in stepArrows)
        {
            if (arrowGroup == null) continue;

            foreach (Transform child in arrowGroup.transform)
            {
                arrowStartPositions[child] = child.position;
            }
        }
    }

    // ⭐ NEW: Animate arrows
    void AnimateActiveArrows()
    {
        if (currentStep >= stepArrows.Length) return;

        GameObject activeGroup = stepArrows[currentStep];
        if (activeGroup == null) return;

        foreach (Transform arrow in activeGroup.transform)
        {
            // FLOATING
            if (arrowStartPositions.ContainsKey(arrow))
            {
                Vector3 startPos = arrowStartPositions[arrow];
                float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
                arrow.position = new Vector3(startPos.x, newY, startPos.z);
            }

            // EMISSION PULSE
            Renderer rend = arrow.GetComponent<Renderer>();
            if (rend != null)
            {
                Material mat = rend.material;

                float emission = Mathf.PingPong(Time.time * pulseSpeed, emissionIntensity);
                Color finalColor = emissionColor * emission;

                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", finalColor);
            }
        }
    }
}