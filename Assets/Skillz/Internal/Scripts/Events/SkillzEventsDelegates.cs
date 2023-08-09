using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;

namespace SkillzSDK
{
  public static class SkillzEventsDelegates
  {
    public delegate void OnMatchWillBeginDelegate(Match match);
    public delegate void OnSkillzWillExitDelegate();
    public delegate void OnProgressionRoomEnterDelegate();
    public delegate void OnEventReceivedDelegate(string eventName, Dictionary<string,string> eventData);
  }
}
