using System;
using System.Collections.Generic;
using SkillzSDK;
using SkillzSDK.Settings;
using UnityEngine;

namespace SkillzSDK.Internal.API.UnityEditor
{
  public class APIResponseSimulator
  {
    private Dictionary<string, ProgressionValue> defaultProgressionPlayerData;
    private Dictionary<string, ProgressionValue> progressionPlayerData;
    private Dictionary<string, ProgressionValue> inGameItems;

    private Season currentSeason;
    private List<Season> previousSeasons;
    private List<Season> nextSeasons;

    private int randomSeed;

    public APIResponseSimulator()
    {
      if (SkillzSettings.Instance.ProgressionResponsesTemplate)
      {
        defaultProgressionPlayerData = InitializeProgressionData(SkillzSettings.Instance.ProgressionResponsesTemplate.defaultPlayerData);
        progressionPlayerData = InitializeProgressionData(SkillzSettings.Instance.ProgressionResponsesTemplate.playerData);
        inGameItems = InitializeInGameItems(SkillzSettings.Instance.ProgressionResponsesTemplate.inGameItems);
      }
      else
      {
        defaultProgressionPlayerData = new Dictionary<string, ProgressionValue>();
        progressionPlayerData = new Dictionary<string, ProgressionValue>();
        inGameItems = new Dictionary<string, ProgressionValue>();
      }


      if (SkillzSettings.Instance.SeasonsTemplate)
      {
        currentSeason = InitializeSeason(SkillzSettings.Instance.SeasonsTemplate.currentSeason);
        previousSeasons = InitializeSeasons(SkillzSettings.Instance.SeasonsTemplate.previousSeasons);
        nextSeasons = InitializeSeasons(SkillzSettings.Instance.SeasonsTemplate.nextSeasons);
      }
      else
      {
        previousSeasons = new List<Season>();
        nextSeasons = new List<Season>();
      }
    }

    public static void InitializeSimulatedMatch(int matchTypeIndex, int parametersIndex, int playerIndex, int randomSeed)
    {
      string matchInfoJson = APIResponseSimulator.BuildMatchJsonString(matchTypeIndex, parametersIndex, playerIndex);
      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, "Initializing Simulated Match: " + matchInfoJson);
      SkillzCrossPlatform.InitializeSimulatedMatch(matchInfoJson, randomSeed);
      SkillzState.NotifyMatchWillBegin(matchInfoJson);
    }

    public static string BuildMatchJsonString(int matchTypeIndex, int parametersIndex, int playerIndex)
    {
      MatchType matchType = new MatchType();
      PlayerTemplate player = new PlayerTemplate();
      List<MatchParameter> parameters = new List<MatchParameter>();

      if (SkillzSettings.Instance.MatchTypeTemplates && SkillzSettings.Instance.MatchTypeTemplates.templates.Count > 0)
      {
        matchType = SkillzSettings.Instance.MatchTypeTemplates.templates[matchTypeIndex];
      }
      if (SkillzSettings.Instance.PlayerTemplates && SkillzSettings.Instance.PlayerTemplates.templates.Count > 0)
      {
        player = SkillzSettings.Instance.PlayerTemplates.templates[playerIndex];
      }
      if (SkillzSettings.Instance.MatchParametersTemplates && SkillzSettings.Instance.MatchParametersTemplates.templates.Count > 0)
      {
        parameters = SkillzSettings.Instance.MatchParametersTemplates.templates[parametersIndex].parameters;
      }

      matchType.players = new List<PlayerTemplate>();
      matchType.players.Add(player);

      matchType.gameParameters = new List<MatchParameter>();
      foreach (MatchParameter p in parameters)
      {
        matchType.gameParameters.Add(p);
      }

      return JsonUtility.ToJson( matchType );
    }

    private List<Season> InitializeSeasons(List<CompanionSeason> companionSeasons)
    {
      List<Season> seasons = new List<Season>();
      foreach (CompanionSeason s in companionSeasons)
      {
        seasons.Add(InitializeSeason(s));
      }
      return seasons;
    }

    private Season InitializeSeason(CompanionSeason season)
    {
      if (season == null)
      {
        return null;
      }

      Dictionary<string, string> seasonParams = new Dictionary<string, string>();
      foreach (SeasonParameter p in season.SeasonParams)
      {
        seasonParams.Add(p.name, p.value);
      }

      SkillzSDK.Season newSeason = new Season(season.ID,
                        season.Name,
                        season.Descritpion,
                        season.IsActive,
                        season.StartTime,
                        season.EndTime,
                        seasonParams,
                        season.SeasonIndex);

      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"Inilizing Simulated Season: {SkillzDebug.Format(newSeason)}");

      return newSeason;

    }

    public void GetCurrentSeason(Action<SkillzSDK.Season> successCallback, Action<string> failureCallback)
    {
      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"GetCurrentSeason() Success callback, Season: '{SkillzDebug.Format(currentSeason)}'");
      successCallback(currentSeason);
    }

    public void GetPreviousSeasons(int count, Action<List<SkillzSDK.Season>> successCallback, Action<string> failureCallback)
    {
      List<SkillzSDK.Season> lastXSeasons = new List<Season>();

      for (int i = 0; i < count && i < previousSeasons.Count; i++)
      {
        lastXSeasons.Add(previousSeasons[i]);
      }

      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"GetPreviousSeasons() Success callback, Seasons: '{SkillzDebug.Format(lastXSeasons)}'");
      successCallback(lastXSeasons);
    }

    public void GetNextSeasons(int count, Action<List<SkillzSDK.Season>> successCallback, Action<string> failureCallback)
    {
      List<SkillzSDK.Season> nextXSeasons = new List<Season>();

      for (int i = 0; i < count && i < nextSeasons.Count; i++)
      {
        nextXSeasons.Add(nextSeasons[i]);
      }

      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"GetNextSeasons() Success callback, Seasons: '{SkillzDebug.Format(nextXSeasons)}'");
      successCallback(nextXSeasons);
    }


    private Dictionary<string, ProgressionValue> InitializeProgressionData(List<ProgressionData> keyValues)
    {
      Dictionary<string, ProgressionValue> valuesMap = new Dictionary<string, ProgressionValue>();
      string dataString = "";
      foreach (ProgressionData p in keyValues)
      {
        ProgressionValue value = new ProgressionValue(p.value,
                                                      ProgressionData.dataTypeMap[(int)p.dataType],
                                                      p.LastUpdatedTime,
                                                      "",
                                                      null);

        valuesMap.Add(p.key, value);
        dataString += $"(key: {p.key}, value: {p.value}) ";
      }
      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"Inilizing Progression Data: {{{dataString}}}");
      return valuesMap;
    }

    private Dictionary<string, ProgressionValue> InitializeInGameItems(List<InGameItem> keyValues)
    {
      Dictionary<string, ProgressionValue> valuesMap = new Dictionary<string, ProgressionValue>();
      string dataString = "";
      foreach (InGameItem item in keyValues)
      {
        ProgressionValue value = new ProgressionValue(item.value,
                                                  "integer",
                                                  item.LastUpdatedTime,
                                                  item.displayName,
                                                  new ProgressionMetadata(item.gameIDs));

        dataString += $"[key: {item.key}, value: {item.value}] ";
        valuesMap.Add(item.key, value);
      }
      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"Inilizing Progression InGameItems: {{{dataString}}}");
      return valuesMap;
    }

    public void SimulateGetProgressionUserData(string progressionNamespace, List<string> userDataKeys, Action<Dictionary<string, ProgressionValue>> successCallback, Action<string> failureCallback)
    {
      if (progressionNamespace == ProgressionNamespace.DEFAULT_PLAYER_DATA)
      {
        GetFromNamespaceMap(defaultProgressionPlayerData, userDataKeys, successCallback, failureCallback);
      }
      else if (progressionNamespace == ProgressionNamespace.PLAYER_DATA)
      {
        GetFromNamespaceMap(progressionPlayerData, userDataKeys, successCallback, failureCallback);
      }
      else if (progressionNamespace == ProgressionNamespace.IN_GAME_ITEMS)
      {
        GetFromNamespaceMap(inGameItems, userDataKeys, successCallback, failureCallback);
      }
      else
      {
        string failureMessage = "progressionNamespace: '" + progressionNamespace + "' is invalid";
        SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"GetProgressionUserData() Failure Callback: {failureMessage}");
        failureCallback(failureMessage);
      }
    }

    private void GetFromNamespaceMap(Dictionary<string, ProgressionValue> map, List<string> keys, Action<Dictionary<string, ProgressionValue>> successCallback, Action<string> failureCallback)
    {
      if (map == null)
      {
        SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"GetProgressionUserData() Success Callback, progression data: null");
        successCallback(null);
        return;
      }

      Dictionary<string, ProgressionValue> result = new Dictionary<string, ProgressionValue>();
      foreach (string key in keys)
      {
        if (map.ContainsKey(key))
        {
          result.Add(key, map[key]);
        }
        else
        {
          string failureMessage = "developer_key: '" + key + "' is invalid: null\n";
          SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"GetProgressionUserData() Failure Callback: {failureMessage}");
          failureCallback(failureMessage);

          return;
        }
      }

      if (result.Count > 0)
      {
        SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"GetProgressionUserData() Success Callback, progression data: {SkillzDebug.Format(map)}");
        successCallback(result);
        return;
      }
      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"GetProgressionUserData() Success Callback, progression data: null");
      successCallback(null);
    }

    public void SimulateUpdateProgressionUserData(string progressionNamespace, Dictionary<string, object> userDataUpdates, Action successCallback, Action<string> failureCallback)
    {
      if (progressionNamespace == ProgressionNamespace.DEFAULT_PLAYER_DATA)
      {
        string failureMessage = "Cannot update default player data.";
        SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"UpdateProgressionUserData() Failure Callback: {failureMessage}");
        failureCallback(failureMessage);
        return;
      }
      else if (progressionNamespace == ProgressionNamespace.PLAYER_DATA)
      {
        UpdateNamespaceMap(progressionPlayerData, userDataUpdates, successCallback, failureCallback);
      }
      else if (progressionNamespace == ProgressionNamespace.IN_GAME_ITEMS)
      {
        UpdateNamespaceMap(inGameItems, userDataUpdates, successCallback, failureCallback);
      }
      else
      {
        string failureMessage = "progressionNamespace: '" + progressionNamespace + "' is invalid";
        SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"UpdateProgressionUserData() Failure Callback: {failureMessage}");
        failureCallback(failureMessage);
        return;
      }
    }

    private void UpdateNamespaceMap(Dictionary<string, ProgressionValue> map, Dictionary<string, object> userDataUpdates, Action successCallback, Action<string> failureCallback)
    {
      if (userDataUpdates == null)
      {
        string failureMessage = "userDataUpdates is null";
        SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"UpdateProgressionUserData() Failure Callback: {failureMessage}");
        failureCallback(failureMessage);
        return;
      }

      foreach (string key in userDataUpdates.Keys)
      {
        if (map.ContainsKey(key))
        {
          ProgressionValue oldValue = map[key];
          if (userDataUpdates[key] == null)
          {
            userDataUpdates[key] = "null";
          }
          map[key] = new ProgressionValue(userDataUpdates[key].ToString(),
                                          oldValue.DataType,
                                          DateTime.Now.ToString("u"),
                                          oldValue.DisplayName,
                                          oldValue.Metadata
                                          );
        }
        else
        {
          string failureMessage = "developer_key: '" + key + "' is invalid: null\n";
          SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"UpdateProgressionUserData() Failure Callback: {failureMessage}");
          failureCallback(failureMessage);
          return;
        }
      }
      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, $"UpdateProgressionUserData() Success Callback");
      successCallback();
    }
  }
}