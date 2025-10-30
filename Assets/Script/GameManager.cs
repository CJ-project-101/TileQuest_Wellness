using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [Tooltip("Seconds before a break is triggered")]
    public float reminderInterval = 60f; // use 60s for testing, 300s+ for final
    float timer;
    public bool breakActive;

    void Update()
    {
        if (breakActive) return;
        timer += Time.unscaledDeltaTime; // keeps counting even if you pause via timescale later
        if (timer >= reminderInterval)
        {
            TriggerBreak();
        }
    }

    void TriggerBreak()
    {
        breakActive = true;
        Time.timeScale = 0f; // pause physics & movement
        // Load break scene additively
        SceneManager.LoadScene("BreakScene", LoadSceneMode.Additive);
    }

    public void EndBreakAndResume()
    {
        timer = 0f;
        breakActive = false;
        Time.timeScale = 1f;
    }
}
