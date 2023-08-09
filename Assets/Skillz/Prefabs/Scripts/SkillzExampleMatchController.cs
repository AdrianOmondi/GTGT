using System.Collections;
using System.Collections.Generic;
using SkillzSDK;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillzExampleMatchController : MonoBehaviour
{

  [SerializeField] private Button endMatch;

  [SerializeField] private Button abortMatch;

  [SerializeField] private Button leaveMatch;

  [SerializeField] TMP_InputField scoreInput;

  private int submitScorFfailureCount = 0;

  private int submitScoreMaxFailures = 2;

  private bool hasSubmittedScore = false;

  private void Awake()
  {
    if (endMatch != null)
    {
      endMatch.onClick.AddListener(EndMatchClicked);
    }

    if(abortMatch != null)
    {
      abortMatch.onClick.AddListener(AbortMatch);
    }

    if (leaveMatch != null)
    {
      leaveMatch.onClick.AddListener(MatchComplete);
    }
  }

  private void FixedUpdate()
  {
    if(hasSubmittedScore)
    {
      MatchComplete();
    }
  }

  void EndMatchClicked()
  {
    TryToSubmitScore();
  }

  void TryToSubmitScore()
  {
    SkillzCrossPlatform.SubmitScore(GetScore(), OnScoreSubmitSuccess, OnScoreSubmitFailure);
  }

  void OnScoreSubmitSuccess()
  {
    Debug.Log("Score Submit - Success");
    hasSubmittedScore = true;
  }

  void OnScoreSubmitFailure(string reason)
  {
    Debug.Log("Score Submit - Failure: " + reason);
    submitScorFfailureCount++;
    if (submitScorFfailureCount <= submitScoreMaxFailures)
    {
      Debug.Log("Retrying...");
      StartCoroutine(RetrySubmit());    
    }
    else
    {
      MaxFailuresReached();
    }
  }

  IEnumerator RetrySubmit()
  {
    yield return new WaitForSeconds(1);
    TryToSubmitScore();
  }

  void MaxFailuresReached()
  {
    Debug.LogWarning("Score Submit - Max Retries Reached");
    SkillzCrossPlatform.DisplayTournamentResultsWithScore(GetScore());
  }

  void MatchComplete()
  {
    SkillzCrossPlatform.ReturnToSkillz();
  }

  void AbortMatch()
  {
    SkillzCrossPlatform.AbortMatch();
  }

  int GetScore()
  {
    int score = 0;
    try
    {
      score = int.Parse(scoreInput.text);
      return score;
    }
    catch
    {
      return 0;
    }
  }
}