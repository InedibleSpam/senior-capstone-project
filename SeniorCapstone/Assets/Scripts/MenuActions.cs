using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System.Collections.Generic;

public class VRMenuController : MonoBehaviour
{
    [Header("Menu Setup")]
    public GameObject pauseMenuUI;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Unfreeze time
        // If you are using a physics movement system, 
        // ensure you re-enable your controller inputs here.
    }

public void Recenter()
{
    // 1. Create a list to hold the subsystems
    List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();

    // 2. The NEW way: use GetSubsystems instead of GetInstances
    SubsystemManager.GetSubsystems(subsystems);

    // 3. Run the recenter command on any active system
    foreach (var system in subsystems)
    {
        if (system.running)
        {
            system.TryRecenter();
        }
    }
}

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Make sure "MainMenu" matches your scene name
    }

    public void QuitExperience()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}