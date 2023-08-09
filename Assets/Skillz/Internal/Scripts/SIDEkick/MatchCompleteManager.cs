using UnityEngine;
using UnityEngine.UI;

namespace SkillzSDK
{
  public class MatchCompleteManager : MonoBehaviour
  {
    public Text scoreText;
    void Awake()
    {
      if (scoreText)
      {
        scoreText.text = "SUBMITTED SCORE: " + Settings.SkillzSettings.Instance.Score;
      }
    }
  }
}
