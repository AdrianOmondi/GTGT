using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzSDK.Settings
{
  [CreateAssetMenu(fileName = "Player Templates", menuName = "Skillz/SIDEkick/Player Templates", order = 3)]
  public class PlayerTemplates : ScriptableObject
  {
    public List<PlayerTemplate> templates;

    public void Reset()
    {
      templates = new List<PlayerTemplate>();
    }

  }

  [Serializable]
  public class PlayerTemplate
  {
    [Tooltip("The user's display name.")]
    public string displayName;

    [Tooltip("An ID unique to this user. (UInt64)")]
    public UInt64 id;

    [Tooltip("A Tournament Player ID unique to this user. (UInt64)")]
    public UInt64 tournamentPlayerId;

    [Tooltip("A link to the user's avatar image.")]
    public string avatarURL;

    [Tooltip("A link to the user's coutry's flag image.")]
    public string flagURL;

    [Tooltip("Does this player represent the current user?")]
    public bool isCurrentPlayer;
  }

}
