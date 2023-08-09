using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;

namespace SkillzSDK
{
  public static class SkillzEvents
  {
    public static event SkillzEventsDelegates.OnMatchWillBeginDelegate OnMatchWillBegin;
    public static event SkillzEventsDelegates.OnSkillzWillExitDelegate OnSkillzWillExit;
    public static event SkillzEventsDelegates.OnProgressionRoomEnterDelegate OnProgressionRoomEnter;
    public static event SkillzEventsDelegates.OnEventReceivedDelegate OnEventReceived;
    
    public static void RaiseOnMatchWillBegin(Match match)
    {
      if (OnMatchWillBegin != null)
      {
        OnMatchWillBegin(match);
      }
    }

    public static void RaiseOnSkillzWillExit()
    {
      if (OnSkillzWillExit != null)
      {
        OnSkillzWillExit();
      }
    }

    public static void RaiseOnProgressionRoomEnter()
    {
      if (OnProgressionRoomEnter != null)
      {
        OnProgressionRoomEnter();
      }
    }
    
    public static void RaiseOnEventReceived(string eventName, Dictionary<string, string> eventData)
    {
      if (OnEventReceived != null)
      {
        OnEventReceived(eventName, eventData);
      }
    }
  }
}
