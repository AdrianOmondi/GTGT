using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkillzExamples
{
  public class BB_TutorialMenu : MonoBehaviour
  {
    [SerializeField] private List<Button> tabButtons;
    [SerializeField] private List<GameObject> tabs;
    [SerializeField] Button closeMenuButton;

    [SerializeField] Color tabButtonColorSelected;
    [SerializeField] Color tabButtonColorUnselected;

    private void Awake()
    {
      SetupTabs();
      closeMenuButton.onClick.AddListener(delegate { ShowMenu(false); });
    }

    private void SetupTabs()
    {
      if (tabButtons != null)
      {
        for (int i = 0; i < tabButtons.Count; i++)
        {
          int x = i;
          tabButtons[i].onClick.AddListener(delegate { SwitchTab(x); });
        }
      }
      SwitchTab(0);
    }

    public void ShowMenu(bool show)
    {
      this.gameObject.SetActive(show);
      if (!show)
      {
        SwitchTab(0);
      }
    }

    public void SwitchTab(int tabIndex)
    {
      for(int i = 0; i < tabs.Count; i++)
      {
        if(i == tabIndex)
        {
          tabs[i].SetActive(true);
        }
        else
        {
          tabs[i].SetActive(false);
        }
      }

      for(int i = 0; i < tabButtons.Count; i++)
      {
        if(i == tabIndex)
        {
          tabButtons[i].GetComponent<Image>().color = tabButtonColorSelected;
        }
        else
        {
          tabButtons[i].GetComponent<Image>().color = tabButtonColorUnselected;
        }
      }
    }

  }
}
