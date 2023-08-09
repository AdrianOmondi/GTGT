using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkillzExamples
{
  public class BB_PreMatchMenuController : MonoBehaviour
  {
    [SerializeField] private Button beginMatchButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private BB_TutorialMenu tutorialMenu;

    private void Awake()
    {
      if (beginMatchButton)
      {
        beginMatchButton.onClick.AddListener(BeginMatchButtonClicked);
      }
      if (howToPlayButton)
      {
        howToPlayButton.onClick.AddListener(HowToPlayButtonClicked);
      }
    }

    public void Start()
    {
      if (!PlayerPrefs.HasKey("FTUE"))
      {
        tutorialMenu.ShowMenu(true);
      }
    }

    private void BeginMatchButtonClicked()
    {
      BB_Managers.matchManager.StartMatch();

      gameObject.SetActive(false);

      PlayerPrefs.SetInt("FTUE", 1);
    }

    private void HowToPlayButtonClicked()
    {
      tutorialMenu.ShowMenu(true);
    }
  }
}
