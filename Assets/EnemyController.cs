using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float speedIncrement = 0.5f;
    public Text scoreText;
    public Text highScoreText;
    public Text levelText;
    public Button resetButton;
    public Button pauseButton;

    private Transform player;
    private float startTime; // Declare the startTime variable
    private float highScore;
    private int currentLevel = 1;
    private float speed;
    private bool isPaused = false;
    private string previousSceneName; // Store the name of the previous scene

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startTime = Time.time;
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        highScoreText.text = "High Score: " + highScore.ToString("0.00");

        resetButton.onClick.AddListener(OnResetButtonClick);
        resetButton.gameObject.SetActive(false); // Hide the reset button initially

        pauseButton.onClick.AddListener(PauseGame);

        UpdateEnemySpeed();
    }

    private void FixedUpdate()
    {
        if (isPaused) return; // Stop movement when the game is paused

        Vector2 direction = (player.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Initialize the startTime when the player collides with an enemy
            startTime = Time.time;

            float endTime = Time.time;
            float score = endTime - startTime;
            scoreText.text = "Score: " + score.ToString("0.00");

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetFloat("HighScore", highScore);
                highScoreText.text = "High Score: " + highScore.ToString("0.00");
            }

            Time.timeScale = 0;
            resetButton.gameObject.SetActive(true); // Show the reset button

            // Check if the player reached the score threshold for the next level
            if (score >= currentLevel * 100)
            {
                currentLevel++;
                UpdateEnemySpeed();

                // Display level text
                levelText.text = "Level: " + currentLevel.ToString();
            }
        }
    }

    private void OnResetButtonClick()
    {
        // Submit the score to Skillz platform before resetting the game
        float endTime = Time.time;
        float score = endTime - startTime;
        SubmitScore(score);
    }

    private void SubmitScore(float score)
    {
        // Call SkillzCrossPlatform.SubmitScore with your score, success, and failure callback functions
        SkillzCrossPlatform.SubmitScore((long)(score * 100), SubmitScoreSuccess, SubmitScoreFailure);
    }

    private void SubmitScoreSuccess()
    {
        Debug.Log("Score Submission Success");
        // Store the current scene name before loading Skillz UI
        previousSceneName = SceneManager.GetActiveScene().name;

        // Return to the Skillz UI
        SkillzCrossPlatform.ReturnToSkillz();

        // The rest of the code remains the same
        StartCoroutine(ReloadSceneAfterDelay());
    }

    private void SubmitScoreFailure(string reason)
    {
        Debug.LogWarning("Score Submission Failed: " + reason);
        // Handle the failure, if needed
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        // Wait for 1 second before reloading the scene to allow Skillz UI to show up
        yield return new WaitForSeconds(1f);

        // Reset the game state
        Time.timeScale = 1;

        // Load the previous scene using the stored scene name
        SceneManager.LoadScene(previousSceneName);
    }

    private void UpdateEnemySpeed()
    {
        speed = initialSpeed + (speedIncrement * (currentLevel - 1));
    }

    private void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}

