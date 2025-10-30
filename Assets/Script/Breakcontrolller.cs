using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BreakController : MonoBehaviour
{
    public TMP_Text messageText;
    private HealthManager healthManager;

    void Start()
    {
        // Find HealthManager in the MainScene
        healthManager = FindObjectOfType<HealthManager>();

        if (messageText)
            messageText.text = "Take a quick wellness break!";
    }

    // Called by the Resume button or AI verification
    public void ResumeGame()
    {
        if (messageText)
            messageText.text = "Welcome back! Resuming...";
        // Unload this scene
        SceneManager.UnloadSceneAsync("BreakScene");
        // Tell HealthManager to resume the game
        if (healthManager != null)
            healthManager.EndBreakAndResume();
    }
}
