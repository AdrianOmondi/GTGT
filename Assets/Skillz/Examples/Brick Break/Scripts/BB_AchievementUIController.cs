using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SkillzExamples
{
  public class BB_AchievementUIController : MonoBehaviour
  {
    public string progressionDataKey;
    public ProgressionNamespace progressionNamespace;
    public int barMax;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image barFill;
    [SerializeField] private TextMeshProUGUI barText;

    public enum ProgressionNamespace { DEFAULT, CUSTOM };

    public void SetBar(int barValue)
    {
      if(barValue > barMax)
      {
        barValue = barMax;
      }

      if(barMax == 0)
      {
        barFill.fillAmount = 1;
        return;
      }
      barFill.fillAmount = (float)barValue / (float)barMax;
      barText.text = barValue.ToString() + "/" + barMax.ToString();
    }

    public void SetTitle(string title)
    {
      this.title.text = title;
    }

    public void SetDescription(string description)
    {
      this.description.text = description;
    }
  }
}
