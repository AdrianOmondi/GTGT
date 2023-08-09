using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkillzExamples
{
  public class BB_PauseMenuController : MonoBehaviour
  {
    [SerializeField] Button resumeButton;
    [SerializeField] Button endButton;

    private void Awake()
    {
      resumeButton.onClick.AddListener(ResumeButtonClicked);
      endButton.onClick.AddListener(EndButtonClicked);
    }

    private void ResumeButtonClicked()
    {
      BB_Managers.matchManager.ResumeMatch();
      this.gameObject.SetActive(false);
    }

    private void EndButtonClicked()
    {
      BB_Managers.matchManager.StopMatch();
      this.gameObject.SetActive(false);
    }
  }
}
