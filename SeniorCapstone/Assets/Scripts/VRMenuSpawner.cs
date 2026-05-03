using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System.Collections.Generic;

public class ISSMenuManager : MonoBehaviour
{
    [Header("Menu Setup")]
    public GameObject pauseMenuUI;  // Drag your Canvas or Menu Parent here
    public Transform playerHead;   // Drag your Main Camera here
    public float spawnDistance = 1.5f;

    [Header("Input Setup")]
    public InputActionProperty menuButton; // Map to XRI LeftHand/Menu

    private bool isPaused = false;

    void Update()
    {
        // Check for button press
        if (menuButton.action.WasPressedThisFrame())
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

  public void Pause()
{
    isPaused = true;
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;

    // 1. Break the parenting and reset "ghost" values
    pauseMenuUI.transform.parent = null; 
    pauseMenuUI.transform.localPosition = Vector3.zero;
    pauseMenuUI.transform.localRotation = Quaternion.identity;

    // 2. SNAP SCALE: This fixes the "getting really small" issue.
    // Replace (1,1,1) with the scale that looked good in your editor.
    pauseMenuUI.transform.localScale = new Vector3(1f, 1f, 1f);

    // 3. POSITION: Set to head and push forward
    Vector3 spawnPos = playerHead.position + (playerHead.forward * spawnDistance);
    
    // NUDGE: If it's slightly off-center, uncomment the line below to adjust it
    // spawnPos += playerHead.right * -0.1f; // -0.1 moves it a tiny bit to the left

    pauseMenuUI.transform.position = spawnPos;

    // 4. ROTATION: Look at you but keep it level (stops the menu from tilting)
    Vector3 lookAtTarget = new Vector3(playerHead.position.x, pauseMenuUI.transform.position.y, playerHead.position.z);
    pauseMenuUI.transform.LookAt(lookAtTarget);
    pauseMenuUI.transform.Rotate(0, 180, 0);
}

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Unfreeze physics
    }

    public void Recenter()
    {
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);

        foreach (var system in subsystems)
        {
            if (system.running)
            {
                system.TryRecenter();
            }
        }
    }

    public void SetVolume(float volume)
{
    // This controls the volume of everything the player hears
    AudioListener.volume = volume;
}

    public void RestartExperience()
    {
        Time.timeScale = 1f;
        // Reloads whatever scene you are currently in
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}