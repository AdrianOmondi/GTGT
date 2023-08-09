using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillzSDK;

public class ExtraLogic : MonoBehaviour
{
  public void OnMatchWillBegin(Match match)
  {
    Debug.Log("Beginning Match...");
  }
}
