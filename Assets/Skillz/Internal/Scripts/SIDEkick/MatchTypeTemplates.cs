using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzSDK.Settings
{
  [CreateAssetMenu(fileName = "MatchTypes", menuName = "Skillz/SIDEkick/Match Type Templates", order = 1)]
  public class MatchTypeTemplates : ScriptableObject
  {
    public List<MatchType> templates;

    private void Reset()
    {
      templates = new List<MatchType>();
      
    }
  }

  [Serializable]
  public class MatchType
  {
    public MatchType()
    {
      connectionInfo = new CustomServerConnectionInfo();
    }

    [Tooltip("The name of this tournament type.")]
    public string name;

    [Tooltip("The description of this tournament type.")]
    public string matchDescription;

    [Tooltip("The unique ID for this match. (ulong)")]
    public ulong id;

    [Tooltip("The unique ID for the tournament template this match is based on. (int)")]
    public int templateId;

    [Tooltip("If this game supports 'Automatic Difficulty' this value represents the difficulty this game. (uint)")]
    public uint skillzDifficulty;

    [Tooltip("Is this match being played for real cash or for Z?")]
    public bool isCash;

    [Tooltip("If this tournament is being played for Z, this is the amount of Z required to enter. (int)")]
    public int entryPoints;

    [Tooltip("If this tournament is being played for real cash, this is the amount of cash required to enter. (float)")]
    public float entryCash;

    [Tooltip("If this tournament is Synchronous or Asynchronous?")]
    public bool isSynchronous;

    [Tooltip("Is the match a tie breaker match?")]
    public bool isTieBreaker;

    [Tooltip("Is the matche a bracket event match?")]
    public bool isBracket;

    [Tooltip("If the tournament is a bracketed match, this is the current round. (int)")]
    public int bracketRound;

    [Tooltip("Is the match as a video add entry match?")]
    public bool isVideoAdEntry;

    [Tooltip("Is the match a custom syncronous match? If this is false then the 'Custom Server Connection Info' will be ignored.")]
    public bool isCustomSynchronousMatch;

    [Tooltip("The connection info to a custom server that coordinates a real-time match. Will be ignored if 'Is Custom Synchronous Match' is false.")]
    public CustomServerConnectionInfo connectionInfo;

    [HideInInspector]
    public List<PlayerTemplate> players;

    [HideInInspector]
    public List<MatchParameter> gameParameters;
  }

  [Serializable]
  public class CustomServerConnectionInfo
  {
    [Tooltip("The ID of the real-time match.")]
    public string matchId;

    [Tooltip("The address of the custom server.")]
    public string serverIP;

    [Tooltip("The port of the custom server.")]
    public string serverPort;

    [Tooltip("The token for entering the match. This is encrypted.")]
    public string matchToken;

    [Tooltip("Whether or not this game should be played against a synchronous gameplay bot. This should be used as part of the synchronous gameplay on-boarding experience.")]
    public string isBotMatch;
  }

}
