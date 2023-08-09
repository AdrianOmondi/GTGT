using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;
using SkillzSDK.Settings;

public static class SkillzDebug
{

  public enum Type { SKILLZ, SIDEKICK, SETTINGS }

  public static readonly Dictionary<Type, string> logMessageFormat
    = new Dictionary<Type, string>
    {
      { Type.SKILLZ, "[Skillz] {0}" },
      { Type.SIDEKICK, "[Skillz SIDEkick] {0}"},
      { Type.SETTINGS, "[Skillz Editor Settings] {0}"}
    };

  public static void Log(SkillzDebug.Type logMessageType ,object message, bool forceLog = false)
  {
    if (forceLog || SkillzSettings.Instance.IsDebugMode)
    {
      Debug.Log(System.String.Format(logMessageFormat[logMessageType], message));
    }
  }

  public static void Log(SkillzDebug.Type logMessageType, object message, Object context, bool forceLog = false)
  {
    if (forceLog || SkillzSettings.Instance.IsDebugMode)
    {
      Debug.Log(System.String.Format(logMessageFormat[logMessageType], message));
    }
  }

  public static void LogWarning(SkillzDebug.Type logMessageType, object message)
  {
    Debug.LogWarning(System.String.Format(logMessageFormat[logMessageType], message));
  }

  public static void LogWarning(SkillzDebug.Type logMessageType, object message, Object context)
  {
    Debug.LogWarning(System.String.Format(logMessageFormat[logMessageType], message), context);
  }

  public static void LogError(SkillzDebug.Type logMessageType, string message)
  {
    Debug.LogError(System.String.Format(logMessageFormat[logMessageType], message));
  }

  public static void LogError(SkillzDebug.Type logMessageType, string message, Object context)
  {
    Debug.LogError(System.String.Format(logMessageFormat[logMessageType], message), context);
  }

  public static void LogErrorFormat(SkillzDebug.Type logMessageType, string format, params object[] args)
  {
    Debug.LogErrorFormat(System.String.Format(logMessageFormat[logMessageType], format), args);
  }


  public static string Format(List<string> list)
  {
    try
    {
      if (list == null)
      {
        return "null";
      }
      return "{" + string.Join(", ", list) + "}";
    }
    catch
    {
      return "failed to convert keys list to string";
    }
  }

  public static string Format(Dictionary<string, object> dict)
  {
    //try and catch in case running on old versions of .net
    try
    {
      if (dict == null)
      {
        return "null";
      }
      return "{" + string.Join(", ", dict) + "}";
    }
    catch
    {
      return "failed to convert data updates dictionary to string";
    }
  }



  public static string Format(List<Season> seasons)
  {
    try
    {
      if(seasons == null)
      {
        return "null";
      }
      List<string> seasonsStringList = new List<string>();
      foreach (Season season in seasons)
      {
        seasonsStringList.Add(Format(season));
      }
      return "{" + string.Join(", ", seasonsStringList) + "}";
    }
    catch
    {
      return "Error converting season list to string";
    }
  }

  public static string Format(Season season)
  {
    try
    {
      if(season == null)
      {
        return "null";
      }
      return season.ToString();
    }
    catch
    {
      return "Error converting season to string";
    }
  }

  public static string Format(Dictionary<string, ProgressionValue> dict)
  {
    try
    {
      if(dict == null)
      {
        return "null";
      }
      List<string> dictStringList = new List<string>();
      foreach(string key in dict.Keys)
      {
        ProgressionValue value = dict[key];
        dictStringList.Add($"(key: {key}, value: {value.Value})");
      }
      return "{" + string.Join(", ", dictStringList) + "}";
    }
    catch
    {
      return "Error converting progression value map to string";
    }
  }
}
