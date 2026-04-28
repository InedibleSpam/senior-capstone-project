using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TourManager : MonoBehaviour
{
    public static TourManager Instance;

    [Header("VR UI Positioning")]
    public Transform playerCamera; 
    public float uiDistance = 2f;
    public float verticalOffset = -0.2f;

    [Header("Player Start Position")]
    public Transform playerStartPosition;

    [Header("Interaction")]
    public GameObject[] handRays; 

    [Header("Tour State")]
    public int currentStep = 0;
    private bool isWelcome = true;
    private bool isFinished = false;

    [Header("Narration IDs")]
    public string welcomeNarrationID = "Welcome";
    public string finishedNarrationID = "Finished";

    [Header("Arrows")]
    public GameObject[] stepArrows;

    [Header("Arrow Animation Settings")] 
    public float floatHeight = 0.2f;
    public float floatSpeed = 2f;
    public float pulseSpeed = 2f;
    public float emissionIntensity = 2f;
    public Color emissionColor = Color.cyan;

    private Dictionary<Transform, Vector3> arrowStartPositions = new Dictionary<Transform, Vector3>(); 

    [Header("Doors")]
    public Collider[] lockedDoors;

    [Header("End UI")]
    public GameObject endScreenUI;
    public CanvasGroup endScreenCanvasGroup; 
    public float fadeDuration = 1f;

    [Header("TTS")]
    public Tour_TTS ttsSpeaker;
    private bool isNarrating = false;
    private string lastNarrationID;

    private void Awake()
    {
        Instance = this;
        if (ttsSpeaker != null)
        {
            ttsSpeaker.OnNarrationFinished += OnNarrationFinished;
        }
    }

    void Start()
    {
        CacheArrowStartPositions();
        PlayNarration(welcomeNarrationID);
    }

    void Update() 
    {
        AnimateActiveArrows();
    }

    public void PlayNarration(string id)
    {
        if (isNarrating)
        {
            Debug.Log("🔊 Narration already in progress, skipping: " + id);
            return;
        }

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
        SetInteraction(false); 

        lastNarrationID = id; 

        ttsSpeaker.SpeakByID(id);
    }

    public void SkipNarration()
    {
        if (!isNarrating) return;

        Debug.Log("⏭️ Skipping narration");

        if (ttsSpeaker != null)
        {
            ttsSpeaker.Stop();
        }

        isNarrating = false;
        SetInteraction(true);

        OnNarrationFinished(lastNarrationID);
    }

    public void ReplayNarration()
    {
        if (string.IsNullOrEmpty(lastNarrationID)) return;

        Debug.Log("🔁 Replaying narration: " + lastNarrationID);

        if (ttsSpeaker != null)
        {
            ttsSpeaker.Stop();
        }

        isNarrating = false; 
        PlayNarration(lastNarrationID);
    }

    private void OnNarrationFinished(string id)
    {
        Debug.Log("➡️ Advancing after narration: " + id);

        isNarrating = false;
        SetInteraction(true); 

        if (isWelcome)
        {
            isWelcome = false;
            currentStep = 0;
            UpdateStep();
        }
        else if (isFinished)
        {
            Debug.Log("🎉 Tour finished!");
            OnTourFinished();
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

    void SetInteraction(bool enabled)
    {
        if (handRays == null) return;

        foreach (var ray in handRays)
        {
            if (ray != null)
                ray.SetActive(enabled);
        }
    }

    void PositionEndScreen()
    {
        if (playerCamera == null || endScreenUI == null) return;

        // Position in front of player
        Vector3 forward = playerCamera.forward;
        Vector3 position = playerCamera.position + forward * uiDistance;

        // Optional vertical adjustment (slightly lower than eye level)
        position.y += verticalOffset;

        endScreenUI.transform.position = position;

        // Make UI face the player
        endScreenUI.transform.LookAt(playerCamera);

        // Flip it so it's not backwards
        endScreenUI.transform.forward *= -1f;
    }

    IEnumerator FadeInEndScreen()
    {
        float time = 0f;
        endScreenCanvasGroup.alpha = 0f;
        endScreenUI.transform.localScale = Vector3.one * 0.8f;

        // Disable interaction during fade
        endScreenCanvasGroup.interactable = false;
        endScreenCanvasGroup.blocksRaycasts = false;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            endScreenCanvasGroup.alpha = time / fadeDuration;
            endScreenUI.transform.localScale = Vector3.Lerp(
                Vector3.one * 0.8f,
                Vector3.one,
                time / fadeDuration
            );
            yield return null;
        }

        endScreenCanvasGroup.alpha = 1f;
        endScreenUI.transform.localScale = Vector3.one;

        // Enable interaction after fade
        endScreenCanvasGroup.interactable = true;
        endScreenCanvasGroup.blocksRaycasts = true;
    }

    void OnTourFinished()
    {
        SetAllDoors(true);
        SetAllArrows(false);

        PositionEndScreen(); 

        if (endScreenUI != null)
            endScreenUI.SetActive(true);

        if (endScreenCanvasGroup != null)
            StartCoroutine(FadeInEndScreen());
    }

    public void RestartTour()
    {
        Debug.Log("🔄 Restarting tour");

        currentStep = 0;
        isWelcome = true;
        isFinished = false;

        if (endScreenUI != null)
            endScreenUI.SetActive(false);

        if (endScreenCanvasGroup != null)
            endScreenCanvasGroup.alpha = 0f;

        // Reset player position to start
        if (playerStartPosition != null && playerCamera != null)
        {
            // Assuming playerCamera is attached to the player or XR Origin
            playerCamera.position = playerStartPosition.position;
            playerCamera.rotation = playerStartPosition.rotation;
        }

        PlayNarration(welcomeNarrationID);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}