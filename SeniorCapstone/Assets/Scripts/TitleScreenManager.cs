using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartFreeRoam()
    {
        SceneManager.LoadScene("FreeRoamScene");
    }

    public void StartGuidedTour()
    {
        SceneManager.LoadScene("GuidedTourScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}