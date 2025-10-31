using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Play & Rest Timing (seconds)")]
    public float startPlayTime = 15f;      // 15 minutes
    public float maxPlayTime = 60f;       // 60 minutes
    public float resetPlayTime = 60f;     // 60 minutes (after reaching max)
    public float startRestTime = 5f;      // 5 minutes
    public float maxRestTime = 15f;        // 15 minutes
    public float growthStep = 10f;         // +10 minutes each cycle

    [Header("UI Display (Optional)")]
    public TMP_Text infoText;               // Assign in Inspector
    public Image timerBar;                  // Optional progress bar

    private float currentPlayTime;
    private float currentRestTime;
    private float timer = 0f;
    private bool inRest = false;
    private int cycleCount = 0;

    void Start()
    {
        currentPlayTime = startPlayTime;
        currentRestTime = startRestTime;
        timer = 0f;
        UpdateInfo();
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;

        float remaining = inRest ? currentRestTime - timer : currentPlayTime - timer;
        float total = inRest ? currentRestTime : currentPlayTime;

        // update progress bar
        if (timerBar)
        {
            timerBar.fillAmount = Mathf.Clamp01(remaining / total);
            timerBar.color = inRest ? Color.cyan : Color.green;
        }

        // update info text
        if (infoText)
        {
            string mode = inRest ? "REST" : "PLAY";
            infoText.text = $"{mode} MODE\nRemaining: {(remaining / 60f):F1} min";
        }

        // check if it's time to switch
        if (!inRest && timer >= currentPlayTime)
            StartRest();
        else if (inRest && timer >= currentRestTime)
            EndRest();
    }

    void StartRest()
    {
        inRest = true;
        timer = 0f;

        // Optionally disable player control
        var player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player) player.enabled = false;

        Debug.Log($"🔵 REST started for {currentRestTime / 60f:F1} min.");

        // 🌿 Load the Break Scene overlay (if not already loaded)
        if (!SceneManager.GetSceneByName("BreakScene").isLoaded)
        {
            SceneManager.LoadScene("BreakScene", LoadSceneMode.Additive);
            Debug.Log("🌿 BreakScene loaded additively (overlay mode).");
        }

        UpdateInfo();
    }

    void EndRest()
    {
        inRest = false;
        timer = 0f;
        cycleCount++;

        // Re-enable player control
        var player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player) player.enabled = true;

        // Adaptive time adjustment
        if (currentPlayTime < maxPlayTime)
        {
            currentPlayTime = Mathf.Min(currentPlayTime + growthStep, maxPlayTime);
            currentRestTime = Mathf.Min(currentRestTime + growthStep / 4f, maxRestTime);
        }
        else
        {
            // reset after max
            currentPlayTime = resetPlayTime;
            currentRestTime = maxRestTime;
            Debug.Log("♻️ Cycle reset: play = 52 min, rest = 15 min");
        }

        Debug.Log($"Cycle #{cycleCount}: 🟢 Play = {currentPlayTime / 60f:F1} min | 🔵 Rest = {currentRestTime / 60f:F1} min");
        UpdateInfo();
    }

    // Called by BreakController when "Resume Game" is pressed
    public void EndBreakAndResume()
    {
        inRest = false;
        timer = 0f;
        Time.timeScale = 1f;
        Debug.Log("✅ Game resumed after wellness break!");

        // Safely unload BreakScene (if still loaded)
        if (SceneManager.GetSceneByName("BreakScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("BreakScene");
            Debug.Log("🟢 BreakScene unloaded.");
        }

        // re-enable player
        var player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player) player.enabled = true;
    }

    void UpdateInfo()
    {
        if (infoText)
        {
            string mode = inRest ? "REST" : "PLAY";
            float remaining = inRest ? currentRestTime - timer : currentPlayTime - timer;
            infoText.text = $"{mode} MODE\nNext switch in {(remaining / 60f):F1} min";
        }
    }
}
