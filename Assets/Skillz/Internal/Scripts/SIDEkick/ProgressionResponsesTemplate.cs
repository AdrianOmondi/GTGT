using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzSDK.Settings
{
  [CreateAssetMenu(fileName = "ProgressionResponses", menuName = "Skillz/SIDEkick/Progression Data Template", order = 4)]
  public class ProgressionResponsesTemplate : ScriptableObject
  {
    public List<ProgressionData> defaultPlayerData;
    public List<ProgressionData> playerData;
    public List<InGameItem> inGameItems;

    private void Reset()
    {
      //Adding the default values for the default player data
      defaultPlayerData = new List<ProgressionData>();
      defaultPlayerData.Add(new ProgressionData("games_played", "0", ProgressionData.DataType.INTEGER, "1970-01-01"));
      defaultPlayerData.Add(new ProgressionData("cash_games_played", "0", ProgressionData.DataType.INTEGER, "1970-01-01"));
      defaultPlayerData.Add(new ProgressionData("games_won", "0", ProgressionData.DataType.INTEGER, "1970-01-01"));
      defaultPlayerData.Add(new ProgressionData("cash_games_won", "0", ProgressionData.DataType.INTEGER, "1970-01-01"));
      defaultPlayerData.Add(new ProgressionData("best_score_lifetime", "0", ProgressionData.DataType.FLOAT, "1970-01-01"));
      defaultPlayerData.Add(new ProgressionData("average_score", "0", ProgressionData.DataType.FLOAT, "1970-01-01"));
      defaultPlayerData.Add(new ProgressionData("player_level", "1", ProgressionData.DataType.INTEGER, "1970-01-01"));
      defaultPlayerData.Add(new ProgressionData("skillz_level", "1", ProgressionData.DataType.INTEGER, "1970-01-01"));
      defaultPlayerData.Add(new ProgressionData("install_date", "1970-01-01", ProgressionData.DataType.DATE, "1970-01-01"));
    }
  }

  [Serializable]
  public class ProgressionData
  {

    public enum DataType { STRING, INTEGER, FLOAT, DATE, BOOLEAN }
    public static string[] dataTypeMap = { "string", "integer", "float", "date", "boolean" };

    [Tooltip("the Key for this progression data.")]
    public string key;

    [Tooltip("The current value for this progression data. The value should be of the type specified in the 'Data Type' field.")]
    public string value;

    [Tooltip("The type of the value.")]
    public DataType dataType;

    [Tooltip("format: [YYYY-MM-DD]")]
    public string LastUpdatedTime;

    public ProgressionData(string key, string value, DataType dataType, string LastUpdatedTime)
    {
      this.key = key;
      this.value = value;
      this.dataType = dataType;
      this.LastUpdatedTime = LastUpdatedTime;
    }
  }

  [Serializable]
  public class InGameItem
  {

    [Tooltip("The key for this in game item.")]
    public string key;

    [Tooltip("The value for in game item. This should be a positive integer.")]
    public string value;

    [Tooltip("The display name for the in game item.")]
    public string displayName;

    [Tooltip("Game IDs for games the item is availiable in.")]
    public List<string> gameIDs;

    [Tooltip("format: [YYYY-MM-DD]")]
    public string LastUpdatedTime;

    public InGameItem(string key, string value, string displayName, string LastUpdatedTime, List<String> gameIDs)
    {
      this.key = key;
      this.value = value;
      this.displayName = displayName;
      this.LastUpdatedTime = LastUpdatedTime;
      this.gameIDs = gameIDs;
    }
  }

}