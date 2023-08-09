using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SkillzExamples
{
  public class BB_BrickBreakScore : MonoBehaviour
  {
    [SerializeField] TextMeshPro scoreText;
    [SerializeField] float duration = 1;

    private Color originalColor;
    private Color fadeToColor = new Color(1, 1, 1, 0);
    private float originalFontSize;
    private float timeEnabled;

    private void OnEnable()
    {
      originalFontSize = scoreText.fontSize;
      originalColor = scoreText.color;
      timeEnabled = Time.time;
      scoreText.gameObject.SetActive(true);
    }

    public void SetScore(int score)
    {
      scoreText.text = score.ToString();
    }

    private void Update()
    {
      float time = Time.time - timeEnabled;
      if (time > duration)
      {
        scoreText.fontSize = originalFontSize;
        scoreText.color = originalColor;
        scoreText.gameObject.SetActive(false);
      }
      else
      {
        scoreText.fontSize = originalFontSize + (time / duration) * 15;
        scoreText.color = Color.Lerp(originalColor, fadeToColor, time / duration);
      }
    }
  }
}
