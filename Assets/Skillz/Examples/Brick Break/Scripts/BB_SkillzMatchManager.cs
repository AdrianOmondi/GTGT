using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SkillzSDK;

namespace SkillzExamples
{
  public class BB_SkillzMatchManager : MonoBehaviour
  {
    [HideInInspector] public bool hasSubmittedScore = false;

    private void Awake()
    {
      BB_Managers.skillzMatchManager = this;
    }

    private void Update()
    {
      if (hasSubmittedScore)
      {
        MatchComplete();
      }
    }

    public void SubmitScore()
    {
      SkillzCrossPlatform.SubmitScore(GetScore(), OnScoreSubmitSuccess, OnScoreSubmitFailure);
    }

    private void OnScoreSubmitSuccess()
    {
      Debug.Log("Score Submit - Success");
      hasSubmittedScore = true;
    }

    //If the submit score fails then call fallback score submission
    private void OnScoreSubmitFailure(string reason)
    {
      Debug.Log("Score Submit - Failure: " + reason);
      SkillzCrossPlatform.DisplayTournamentResultsWithScore(GetScore());
    }

    public void MatchComplete()
    {
      SkillzCrossPlatform.ReturnToSkillz();
    }

    private int GetScore()
    {
      return BB_Managers.matchManager.GetTotalScore();
    }

    public void UpdateProgressionData()
    {
      //update bricks_broken
      BB_Managers.progressionManager.IncrementProgressionInt("bricks_broken", BB_Managers.matchManager.gameBoard.NumberOfBricksBroken());

      //update bricks_cleared
      if (BB_Managers.matchManager.gameBoard.AllBricksBroken())
      {
        BB_Managers.progressionManager.IncrementProgressionInt("bricks_cleared", 1);
      }

      //update only_bronze_remaining
      if (BB_Managers.matchManager.gameBoard.OnlyBronzeRemaining())
      {
        BB_Managers.progressionManager.IncrementProgressionInt("only_bronze_remaining", 1);
      }

      //update most_time_remaining
      if (BB_Managers.matchManager.GetRemainingTime() > 15f)
      {
        BB_Managers.progressionManager.IncrementProgressionInt("15_time_remaining", 1);
      }

      //update no_ball_lost
      if (!BB_Managers.progressionManager.ballLost)
      {
        BB_Managers.progressionManager.IncrementProgressionInt("no_ball_lost", 1);
      }

      BB_Managers.progressionManager.UpdateProgressionData();
    }
  }
}
