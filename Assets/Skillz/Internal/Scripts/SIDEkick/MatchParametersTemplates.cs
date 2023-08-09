using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzSDK.Settings
{
  [CreateAssetMenu(fileName = "Match Parameter Templates", menuName = "Skillz/SIDEkick/Match Parameter Templates", order = 2)]
  public class MatchParametersTemplates : ScriptableObject
  {
    public List<MatchParametersTemplate> templates;

    private void Reset()
    {
      templates = new List<MatchParametersTemplate>();
    }
  }

  [Serializable]
  public class MatchParametersTemplate
  {
    [Tooltip("Displayed name of this template.")]
    public string name;

    [Tooltip("List of match parameters for this template.")]
    public List<MatchParameter> parameters;
  }

  [Serializable]
  public class MatchParameter
  {
    public string key;
    public string value;
  }
}
