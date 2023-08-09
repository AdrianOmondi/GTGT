using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzSDK.Settings
{
  [CreateAssetMenu(fileName = "Seasons", menuName = "Skillz/SIDEkick/Seasons Template", order = 5)]
  public class SeasonsTemplate : ScriptableObject
  {
    public CompanionSeason currentSeason;
    public List<CompanionSeason> previousSeasons;
    public List<CompanionSeason> nextSeasons;

    private void Reset()
    {

    }
  }

  [Serializable]
  public class CompanionSeason
  {
    [Tooltip("The name for this Season.")]
    public string Name;

    [Tooltip("A unique identifier for this season on the Skillz platform.")]
    public string ID;

    [Tooltip("The description for this season.")]
    public string Descritpion;

    [Tooltip("Whether or not this season was currently active when the data was retrieved from Skillz.")]
    public bool IsActive;

    [Tooltip("when the season starts, format: [YYYY-MM-DD]")]
    public string StartTime;

    [Tooltip("When the season ends, format: [YYYY-MM-DD]")]
    public string EndTime;

    [Tooltip("The index of this season relative to the current season. 0 indicates this is the current season.")]
    public int SeasonIndex;

    [Tooltip("List of parameters for this Season.")]
    public List<SeasonParameter> SeasonParams;
  }

  [Serializable]
  public class SeasonParameter
  {
    [Tooltip("The name for this season parameter.")]
    public string name;

    [Tooltip("The value for this season parameter.")]
    public string value;
  }
}