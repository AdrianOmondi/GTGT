using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;

namespace SkillzSDK
{
  public class SkillzEventsHandler : MonoBehaviour
  {

    private void OnEnable()
    {
      SkillzEvents.OnMatchWillBegin += OnMatchWillBegin;
      SkillzEvents.OnSkillzWillExit += OnSkillzWillExit;
      SkillzEvents.OnProgressionRoomEnter += OnProgressionRoomEnter;
      SkillzEvents.OnEventReceived += OnEventReceived;
    }

    private void OnDisable()
    {
      SkillzEvents.OnMatchWillBegin -= OnMatchWillBegin;
      SkillzEvents.OnSkillzWillExit -= OnSkillzWillExit;
      SkillzEvents.OnProgressionRoomEnter -= OnProgressionRoomEnter;
      SkillzEvents.OnEventReceived -= OnEventReceived;
    }

    protected virtual void OnMatchWillBegin(Match match) { }
    protected virtual void OnSkillzWillExit() { }
    protected virtual void OnProgressionRoomEnter() { }
    protected virtual void OnEventReceived(string eventName, Dictionary<string, string> eventData) { }
  }
}
