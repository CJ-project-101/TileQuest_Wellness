using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int PlayerLives =3;
    [SerializeField] int score =0;
    
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    void Awake()
    {
        int numGameSessions=FindObjectsOfType<GameSession>().Length;
        if(numGameSessions> 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text=PlayerLives.ToString();
        scoreText.text=score.ToString();
        
    }

    public void ProcessPlayerDeath()
    {
        if(PlayerLives>1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }


    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(1);
        Destroy(gameObject);
        
    }

    void TakeLife()
    {
        PlayerLives --;
        int currentSceneIndex=SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text=PlayerLives.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text=score.ToString();
    }
}
