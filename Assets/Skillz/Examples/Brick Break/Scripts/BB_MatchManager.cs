using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SkillzExamples
{
  public class BB_MatchManager : MonoBehaviour
  {
    public GameMode gameMode = GameMode.PRACTICE;

    [SerializeField] private List<BB_GameBoard> possibleBoards;
    [SerializeField] private Transform gameBoardParent;

    public BB_GameBoard gameBoard;

    private BB_AntiCheatVault score;
    private BB_AntiCheatVault matchTime;
    private Hashtable matchParameters;

    private float timeRemaining = 60;
    private bool isMatchRunning = false;
    
    private bool isMultiBall = false;

    private const string gameModeParamName = "game_mode";
    private const string multiBallParamName = "multi_ball";
    private const string timeParamName = "time";
    private const float defaultGameTime = 60;

    public enum GameMode { PRACTICE, DELUX }

    private void SetMatchParameters()
    {
      matchParameters = SkillzCrossPlatform.GetMatchRules();

      if (matchParameters.ContainsKey(gameModeParamName))
      {
        if((string)matchParameters[gameModeParamName] == "delux")
        {
          gameMode = GameMode.DELUX;
        }
      }

      if (matchParameters.ContainsKey(timeParamName))
      {
        timeRemaining = int.Parse(((string)matchParameters[timeParamName]));
        matchTime.Set((int)timeRemaining);
      }

      if (matchParameters.ContainsKey(multiBallParamName))
      {
        if ((string)matchParameters[multiBallParamName] == "true")
        {
          isMultiBall = true;
        }
      }
    }

    private void Awake()
    {
      //Storing important game variables in an Anti-Cheat Vault to prevent memory modification 
      score = new BB_AntiCheatVault();
      score.Set(0);
      matchTime = new BB_AntiCheatVault();
      matchTime.Set(60);

      BB_Managers.matchManager = this;
      Time.timeScale = 0;

      SetMatchParameters();
    }

    private void Start()
    {
        gameBoard = BB_Fairness.GenerateFairGame(possibleBoards, gameMode == GameMode.DELUX, gameBoardParent);
    }

    private void Update()
    {
      CheckForEndOfMatch();
      UpdateTime();
      if (isMultiBall)
      {
        BB_Managers.ballManager.SpawnExtraBalls();
      }
    }

    private void CheckForEndOfMatch()
    {
      if (timeRemaining <= 0)
      {
        timeRemaining = 0f;
        StopMatch();
      }
      if (gameBoard.AllBricksBroken())
      {
        StopMatch();
      }
    }

    private void UpdateTime()
    {
      BB_Managers.matchUIManager.SetTime(timeRemaining);

      if (isMatchRunning)
      {
        timeRemaining -= Time.deltaTime;
      }
    }

    public void AddScore(int deltaScore)
    {
      score.Set(score.Get() + deltaScore);
      BB_Managers.matchUIManager.SetScore(GetScore());
    }

    public int GetScore()
    {
      if (!score.IsValid())
      {
        return 0;
      }
      return score.Get();
    }

    public int GetTimeBonus()
    {
      if (gameBoard.AllBricksBroken())
      {
        return (int)(timeRemaining * 100);
      }
      else
      {
        return 0;
      }
    }

    public int GetTotalScore()
    {
      return GetScore() + GetTimeBonus();
    }

    public float GetTime()
    {
      if (!matchTime.IsValid())
      {
        return defaultGameTime - timeRemaining;
      }
      return ((float)matchTime.Get()) - timeRemaining;
    }

    public float GetRemainingTime()
    {
      return timeRemaining;
    }

    public void StartMatch()
    {
      BB_Managers.ballManager.SpawnBallStartLocation();
      isMatchRunning = true;
      Time.timeScale = 1;
    }

    public void StopMatch()
    {
      isMatchRunning = false;
      Time.timeScale = 0;
      BB_Managers.matchUIManager.ShowSubmitScoreMenu();
      BB_Managers.skillzMatchManager.UpdateProgressionData();
    }

    public void PauseMatch()
    {
      isMatchRunning = false;
      Time.timeScale = 0;
    }

    public void ResumeMatch()
    {
      isMatchRunning = true;
      Time.timeScale = 1;
    }
  }
}
