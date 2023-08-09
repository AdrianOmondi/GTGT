using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillzSDK;
using TMPro;

namespace SkillzExamples
{
  public class BB_ProgressionRoomController : MonoBehaviour
  {
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI averageScoreText;
    [SerializeField] private TextMeshProUGUI gamesPlayedText;
    [SerializeField] private TextMeshProUGUI gamesWonText;

    private List<BB_AchievementUIController> achievements;

    private void Awake()
    {

      closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void Start()
    {
      achievements = new List<BB_AchievementUIController>( GetComponentsInChildren<BB_AchievementUIController>(true) );
    }

    private void Update()
    {
      foreach(BB_AchievementUIController achievement in achievements)
      {
        string progressionNamespace = ProgressionNamespace.DEFAULT_PLAYER_DATA;
        if(achievement.progressionNamespace == BB_AchievementUIController.ProgressionNamespace.CUSTOM)
        {
          progressionNamespace = ProgressionNamespace.PLAYER_DATA;
        }
        string progressionData = BB_Managers.progressionManager.GetData(progressionNamespace, achievement.progressionDataKey);
        if (progressionData != null)
        {
          achievement.SetBar( int.Parse(progressionData) );
        }
      }

        highScoreText.text = BB_Managers.progressionManager.GetData(ProgressionNamespace.DEFAULT_PLAYER_DATA, "best_score_lifetime");

        averageScoreText.text = BB_Managers.progressionManager.GetData(ProgressionNamespace.DEFAULT_PLAYER_DATA, "average_score");

        gamesPlayedText.text = BB_Managers.progressionManager.GetData(ProgressionNamespace.DEFAULT_PLAYER_DATA, "games_played");

        gamesWonText.text = BB_Managers.progressionManager.GetData(ProgressionNamespace.DEFAULT_PLAYER_DATA, "games_won");
    }

    private void OnCloseButtonClicked()
    {
      SkillzCrossPlatform.ReturnToSkillz();
    }
  }
}
