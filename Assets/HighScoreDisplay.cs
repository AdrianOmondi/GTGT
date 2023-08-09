using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour
{
    public Text highScoreText;

    void Start()
    {
        // Load the high score from PlayerPrefs
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);

        // Display the high score
        highScoreText.text = "High Score: " + highScore.ToString("F2");
    }
}

