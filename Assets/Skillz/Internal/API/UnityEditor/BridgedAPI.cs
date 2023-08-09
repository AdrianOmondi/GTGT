using System;
using System.Collections;
using System.Globalization;
using SkillzSDK.Settings;
using SkillzSDK.MiniJSON;
using UnityEngine;
using System.Collections.Generic;

namespace SkillzSDK.Internal.API.UnityEditor
{
	internal sealed class BridgedAPI : IBridgedAPI, IRandom
	{
		private const string DefaultScoreString = "0";

		private const float DefaultVolume = 0.5f;

		private bool matchInProgress;
		private bool inMatch = false;

		System.Random SeededRandom;

		public IRandom Random
		{
			get
			{
				return this;
			}
		}

		bool IAsyncAPI.IsMatchInProgress
		{
			get
			{
				return matchInProgress;
			}
		}

		public float SkillzMusicVolume
		{
			get
			{
				return backgroundMusicVolume;
			}
			set
			{
				backgroundMusicVolume = value > 1f ? 1f : value;
				backgroundMusicVolume = backgroundMusicVolume < 0f ? 0f : value;
			}
		}

		public float SoundEffectsVolume
		{
			get
			{
				return soundEffectsVolume;
			}
			set
			{
				soundEffectsVolume = value > 1f ? 1f : value;
				soundEffectsVolume = soundEffectsVolume < 0f ? 0f : soundEffectsVolume;
			}
		}

		private Match matchInfo;
		private float soundEffectsVolume;
		private float backgroundMusicVolume;
#pragma warning disable IDE0052 // Remove unread private members
		private string currentScore;
#pragma warning restore IDE0052 // Remove unread private members

		APIResponseSimulator responseSimulator;

		public BridgedAPI()
		{
			if (!Application.isEditor)
			{
				throw new InvalidOperationException("This can only be instantiated while the Unity editor is playing");
			}

			soundEffectsVolume = DefaultVolume;
			backgroundMusicVolume = DefaultVolume;
			responseSimulator = new APIResponseSimulator();
			
		}

		public void Initialize(int gameID, Environment environment, Orientation orientation)
		{
			currentScore = DefaultScoreString;
		}

		public void LaunchSkillz()
		{
			SDKScenesLoader.Load(SDKScenesLoader.TournamentSelectionScene);
		}

		public Hashtable GetMatchRules()
		{		
			if (!matchInProgress)
			{
				SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Cannot get match rules, match is not in progress");
				return new Hashtable();
			}

			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"Match rules: '{String.Join(" ", matchInfo.GameParams)}'");
			return new Hashtable(matchInfo.GameParams);
		}

		public Match GetMatchInfo()
		{
			if (!matchInProgress)
			{
				// This behavior is not consistent across platforms. iOS will return null,
				// while Android will throw an NRE because it assumes there is a match to
				// get match info from.
				SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Cannot get match info, match is not in progress");
				throw new InvalidOperationException("No simulated match in progress");
			}

			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"Match Info: '{matchInfo.ToString()}'");
			return matchInfo;
		}

		public void AbortMatch()
		{
			FinishSimulatedMatch();
			inMatch = false;
			SDKScenesLoader.Load(SDKScenesLoader.MatchAbortedScene);
		}

		public void AbortBotMatch(string botScore)
		{
			AbortMatch();
		}

		public void AbortBotMatch(int botScore)
		{
			AbortMatch();
		}

		public void AbortBotMatch(float botScore)
		{
			AbortMatch();
		}

		public void UpdatePlayersCurrentScore(string score)
		{
			currentScore = score;
		}

		public void UpdatePlayersCurrentScore(int score)
		{
			UpdatePlayersCurrentScore(score.ToString());
		}

		public void UpdatePlayersCurrentScore(float score)
		{
			UpdatePlayersCurrentScore(score.ToString());
		}

		public void DisplayTournamentResultsWithScore(string score)
		{
			FinishSimulatedMatch();
			SDKScenesLoader.Load(SDKScenesLoader.MatchCompletedScene);
		}

		public void DisplayTournamentResultsWithScore(int score)
		{
			DisplayTournamentResultsWithScore(score.ToString());
		}

		public void DisplayTournamentResultsWithScore(float score)
		{
			DisplayTournamentResultsWithScore(score.ToString(CultureInfo.InvariantCulture));
		}

		public void ReportFinalScoreForBotMatch(string playerScore, string botScore)
		{
			ReportFinalScoreForBotMatch(float.Parse(playerScore), float.Parse(botScore));
		}

		public void ReportFinalScoreForBotMatch(int playerScore, int botScore)
		{
			ReportFinalScoreForBotMatch((float)playerScore, (float)botScore);
		}

		public void ReportFinalScoreForBotMatch(float playerScore, float botScore)
		{
			FinishSimulatedMatch();
      SDKScenesLoader.Load(SDKScenesLoader.MatchCompletedScene);
		}

		public void SubmitScore(string score, Action successCallback, Action<string> failureCallback)
		{
			SkillzSettings.Instance.Score = score;
			FinishSimulatedMatch();
			if (successCallback != null)
			{
				SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"SkillzCrossPlatform.SubmitScore() Success Callback");
				successCallback();
			}
		}

		public void SubmitScore(int score, Action successCallback, Action<string> failureCallback)
		{
			SubmitScore(score.ToString(), successCallback, failureCallback);
		}

		public void SubmitScore(float score, Action successCallback, Action<string> failureCallback)
		{
			SubmitScore(score.ToString(), successCallback, failureCallback);
		}

		public bool EndReplay()
		{
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "EndReplay is not supported in Unity Editor");
			return true;
		}

		public bool ReturnToSkillz()
		{
			bool hasSubmittedScore = !matchInProgress;

			//If in Progression or Season room return to the tournament selection screen
			if (!inMatch)
			{
				SDKScenesLoader.Load(SDKScenesLoader.TournamentSelectionScene);
				return false;
			}

			inMatch = false;

			if (hasSubmittedScore)
			{
				SDKScenesLoader.Load(SDKScenesLoader.MatchCompletedScene);
			}
			return hasSubmittedScore;
		}

		public string SDKVersionShort()
		{
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "SDKVersionShort is not supported in Unity Editor");
			return string.Empty;
		}

		public Player GetPlayer()
		{
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Player: 'null'");
			return null;
		}

		public void AddMetadataForMatchInProgress(string metadataJson, bool forMatchInProgress)
		{
			// Do nothing. It appears this is an old API that doesn't have a corresponding getter.
			SkillzDebug.LogWarning(SkillzDebug.Type.SKILLZ, "AddMetadataForMatchInProgress is an outdated API that doesn't have a corresponding getter");
		}

		public void SetSkillzBackgroundMusic(string fileName)
		{
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "SetSkillzBackgroundMusicthis is not supported in Unity Editor");
		}

		public void GetProgressionUserData(string progressionNamespace, List<string> userDataKeys, Action<Dictionary<string, ProgressionValue>> successCallback, Action<string> failureCallback)
		{
			if (successCallback != null)
			{
				responseSimulator.SimulateGetProgressionUserData(progressionNamespace,
																												 userDataKeys,
																												 successCallback,
																												 failureCallback);
			}
		}

		public void UpdateProgressionUserData(string progressionNamespace, Dictionary<string, object> userDataUpdates, Action successCallback, Action<string> failureCallback)
		{
			responseSimulator.SimulateUpdateProgressionUserData(progressionNamespace,
																													userDataUpdates,
																													successCallback,
																													failureCallback);
		}

		public void GetCurrentSeason(Action<Season> successCallback, Action<string> failureCallback)
		{
			responseSimulator.GetCurrentSeason(successCallback, failureCallback);
		}

		public void GetPreviousSeasons(int count, Action<List<Season>> successCallback, Action<string> failureCallback)
		{
			responseSimulator.GetPreviousSeasons(count, successCallback, failureCallback);
		}

		public void GetNextSeasons(int count, Action<List<Season>> successCallback, Action<string> failureCallback)
		{
			responseSimulator.GetNextSeasons(count, successCallback, failureCallback);
		}

		internal void InitializeSimulatedMatch(string matchInfoJson, int randomSeed)
		{
			SkillzSettings.Instance.Score = "null";
			matchInProgress = true;
			inMatch = true;
			matchInfo = new Match((Dictionary<string, object>)Json.Deserialize(matchInfoJson));
			SeededRandom = new System.Random(randomSeed);
			SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, "Skillz random seeded with: " + randomSeed);
		}

		private void FinishSimulatedMatch()
		{
			matchInProgress = false;
			matchInfo = null;
		}

		float IRandom.Value()
		{
			if (matchInProgress)
			{
				return (float)SeededRandom.NextDouble();
			}
			else
			{
				return UnityEngine.Random.value;
			}
		}
	}
}