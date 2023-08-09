using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class SkillzExampleManagerLogic : MonoBehaviour
  {
    public void OnMatchWillBeginLogic(SkillzSDK.Match match)
    {
      Debug.Log("Loading Game Scene");
    }

    public void OnSkillzWillExitLogic()
    {
      Debug.Log("Loading Start Menu Scene");
    }

    public void OnProgressionRoomEnterLogic()
    {
      Debug.Log("Loading Progression Room Scene");
    }
  }
}
