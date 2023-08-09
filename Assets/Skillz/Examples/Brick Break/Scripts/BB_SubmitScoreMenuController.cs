using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SkillzExamples
{
  public class BB_SubmitScoreMenuController : MonoBehaviour
  {
    [SerializeField] private Button submitScoreButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeBonusText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    private void Awake()
    {
      if (submitScoreButton)
      {
        submitScoreButton.onClick.AddListener(SubmitScoreButtonClicked);
      }
    }

    private void Start()
    {
      scoreText.text = BB_Managers.matchManager.GetScore().ToString();
      timeBonusText.text = BB_Managers.matchManager.GetTimeBonus().ToString();
      totalScoreText.text = BB_Managers.matchManager.GetTotalScore().ToString();
    }

    private void SubmitScoreButtonClicked()
    {
      BB_Managers.skillzMatchManager.SubmitScore();
    }
  }
}
