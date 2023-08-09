using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;

namespace SkillzExamples
{
  public class BB_ProgressionManager : MonoBehaviour
  {

    private Dictionary<string, SkillzSDK.ProgressionValue> defaultProgressionData;
    private Dictionary<string, SkillzSDK.ProgressionValue> customProgressionData;

    private Dictionary<string, object> customProgressionUpdates;

    private List<string> customDataKeys;
    private List<string> defualtDataKeys;

    private bool hasRevievedDefaultData;
    private bool hasRecievedCustomData;

    public bool ballLost = false;

    private void Awake()
    {
      defaultProgressionData = new Dictionary<string, ProgressionValue>();
      customProgressionData = new Dictionary<string, ProgressionValue>();
      customProgressionUpdates = new Dictionary<string, object>();

      BB_Managers.progressionManager = this;
    }

    private void Start()
    {
      defualtDataKeys = new List<string>();
      defualtDataKeys.Add("games_played");
      defualtDataKeys.Add("cash_games_played");
      defualtDataKeys.Add("games_won");
      defualtDataKeys.Add("cash_games_won");
      defualtDataKeys.Add("best_score_lifetime");
      defualtDataKeys.Add("average_score");
      defualtDataKeys.Add("player_level");
      defualtDataKeys.Add("skillz_level");
      defualtDataKeys.Add("install_date");

      customDataKeys = new List<string>();
      customDataKeys.Add("bricks_broken");
      customDataKeys.Add("bricks_cleared");
      customDataKeys.Add("only_bronze_remaining");
      customDataKeys.Add("15_time_remaining");
      customDataKeys.Add("no_ball_lost");

      //Retrieve the progression data from Skillz
      GetProgressionData();
    }

    private void GetProgressionData()
    {
      SkillzCrossPlatform.GetProgressionUserData(ProgressionNamespace.DEFAULT_PLAYER_DATA,
                                                 defualtDataKeys,
                                                 OnSuccessDefault,
                                                 OnFailure);
      SkillzCrossPlatform.GetProgressionUserData(ProgressionNamespace.PLAYER_DATA,
                                                 customDataKeys,
                                                 OnSuccessCustom,
                                                 OnFailure);
    }

    //Save the progression data to Skillz
    public void UpdateProgressionData()
    {

      SkillzCrossPlatform.UpdateProgressionUserData(ProgressionNamespace.PLAYER_DATA,
                                                    customProgressionUpdates,
                                                    OnSuccessUpdate,
                                                    OnFailure);
    }

    //Store the default progression data locally
    private void OnSuccessDefault(Dictionary<string, ProgressionValue> data)
    {
      foreach (string key in data.Keys)
      {
        defaultProgressionData.Add(key, data[key]);
      }
      hasRevievedDefaultData = true;
    }

    //Store the custom progression data locally
    private void OnSuccessCustom(Dictionary<string, ProgressionValue> data)
    {
      foreach (string key in data.Keys)
      {
        customProgressionData.Add(key, data[key]);
      }
      hasRecievedCustomData = true;
    }


    private void OnSuccessUpdate()
    {
      Debug.Log("Successfully updated custom progression data");
      customProgressionUpdates.Clear();
    }

    private void OnFailure(string message)
    {
      Debug.Log(message);
    }

    public bool HasRecievedProgressionData(string progressionNamespace)
    {
      if(progressionNamespace == ProgressionNamespace.DEFAULT_PLAYER_DATA && hasRevievedDefaultData)
      {
        return true;
      }
      if(progressionNamespace == ProgressionNamespace.PLAYER_DATA && hasRecievedCustomData)
      {
        return true;
      }
      return false;
    }

    public string GetData(string progressionNamespace, string dataKey)
    {
      if (HasRecievedProgressionData(progressionNamespace))
      {
        if(progressionNamespace == ProgressionNamespace.DEFAULT_PLAYER_DATA && defaultProgressionData.ContainsKey(dataKey))
        {
          return defaultProgressionData[dataKey].Value;
        }
        if(progressionNamespace == ProgressionNamespace.PLAYER_DATA && customProgressionData.ContainsKey(dataKey))
        {
          return customProgressionData[dataKey].Value;
        } 
      }
      return null;
    }

    public void SetData(string dataKey, object value)
    {
      if (customProgressionUpdates.ContainsKey(dataKey))
      {
        customProgressionUpdates[dataKey] = value;
      }
      else
      {
        customProgressionUpdates.Add(dataKey, value);
      }
    }

    public void IncrementProgressionInt(string dataKey, int incrementAmount)
    {
      string dataValueString = GetData(ProgressionNamespace.PLAYER_DATA, dataKey);
      if (dataValueString != null)
      {
        int newValue = int.Parse(dataValueString) + incrementAmount;
        SetData(dataKey, newValue);
      }
    }

  }
}
