using UnityEngine;
using UnityEngine.UI;
 // Make sure you have the Skillz SDK properly integrated in your Unity project.

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    private float highScore;
    private float startTime; // Add this variable to track the start time
    // ... (Other variables and functions)

    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        highScoreText.text = "High Score: " + highScore.ToString("0.00");
        startTime = Time.time; // Initialize the start time when the game starts
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate the player's score
            float endTime = Time.time;
            float score = endTime - startTime;
            scoreText.text = "Score: " + score.ToString("0.00");

            // Check if the score is higher than the current high score
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetFloat("HighScore", highScore);
                highScoreText.text = "High Score: " + highScore.ToString("0.00");
            }

            // Submit the score to Skillz platform
            SubmitScore(score);
        }
    }

    void SubmitScore(float score)
    {
        // Call SkillzCrossPlatform.SubmitScore with your score, success, and failure callback functions
        SkillzCrossPlatform.SubmitScore((long)(score * 100), Success, Failure);
    }

    void Success()
    {
        Debug.Log("Success");
        // Return to the Skillz UI (This should be handled by the Skillz SDK)
    }

    void Failure(string reason)
    {
        Debug.LogWarning("Fail: " + reason);
        // Fallback score submission (You can handle this as per your requirements)
    }

    // ... (Other functions and logic)
}

