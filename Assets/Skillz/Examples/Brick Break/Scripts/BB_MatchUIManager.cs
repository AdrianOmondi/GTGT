using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SkillzExamples
{
  public class BB_MatchUIManager : MonoBehaviour
  {
    [Header("Game UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] Button pauseButton;

    [Header("Menus")]
    [SerializeField] BB_PauseMenuController pauseMenuController;
    [SerializeField] BB_SubmitScoreMenuController submitScoreMenuController;
    [SerializeField] BB_PreMatchMenuController preMatchMenuController;
    

    private void Awake()
    {

      BB_Managers.matchUIManager = this;
      if (pauseButton)
      {
        pauseButton.onClick.AddListener(PausePressed);
      }

      PrepareMenus();
    }

    private void PrepareMenus()
    {
      if (preMatchMenuController)
      {
        preMatchMenuController.gameObject.SetActive(true);
      }
      if (pauseMenuController)
      {
        pauseMenuController.gameObject.SetActive(false);
      }
      if (submitScoreMenuController)
      {
        submitScoreMenuController.gameObject.SetActive(false);
      }
    }

    private void Update()
    {
      PauseUnpause();
    }

    public void ShowSubmitScoreMenu()
    {
      submitScoreMenuController.gameObject.SetActive(true);
    }

    public void SetScore(int score)
    {
      if (scoreText)
      {
        scoreText.text = score.ToString();
      }
    }

    public void SetTime(float timeRemaining)
    {
      if (timeText)
      {
        timeText.text = timeRemaining.ToString("0.00").Replace('.', ':');
      }
    }

    private void PauseUnpause()
    {
      //Pause and Unpause on PC and Mac
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        if (pauseMenuController)
        {
          if (pauseMenuController.gameObject.activeInHierarchy)
          {
            pauseMenuController.gameObject.SetActive(false);
            BB_Managers.matchManager.ResumeMatch();
          }
          else
          {
            pauseMenuController.gameObject.SetActive(true);
            BB_Managers.matchManager.PauseMatch();
          }
        }
      }

    }

    private void PausePressed()
    {
      if (pauseMenuController)
      {
        pauseMenuController.gameObject.SetActive(true);
      }
      BB_Managers.matchManager.PauseMatch();
    }
  }
}
