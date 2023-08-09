using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillzSDK.Internal.API.UnityEditor;
using SkillzSDK.Settings;
using System;

namespace SkillzSDK
{
  public class TournamentSelectionController : MonoBehaviour
  {

    [SerializeField]
    private GameObject interactableUI;

    [SerializeField]
    private Button enterMatchButton;

    [SerializeField]
    private Button startMenuButton;

    [SerializeField]
    private Button progressionRoomButton;

    [SerializeField]
    private Dropdown matchTypeDropdown;

    [SerializeField]
    private Dropdown matchParametersDropdown;

    [SerializeField]
    private Dropdown playerDropdown;

    [SerializeField]
    private Toggle seedToggle;

    [SerializeField]
    private InputField seedInput;

    private void Awake()
    {
      InitButtons();
      InitRandom();
      InvokeRepeating("UpdateDropdowns", 0f, 1f); //update dropdowns every 1 second
    }

    private void InitRandom()
    {
      if (seedToggle)
      {
        seedToggle.onValueChanged.AddListener(OnSeedToggleChanged);
      }
    }

    private void OnSeedToggleChanged(bool isChecked)
    {
      if (isChecked)
      {
        seedInput.interactable = true;
      }
      else
      {
        seedInput.interactable = false;
      }
    }

    private void InitButtons()
    {
      if (enterMatchButton)
      {
        enterMatchButton.onClick.AddListener(StartMatch);
      }

      if (progressionRoomButton)
      {
        progressionRoomButton.onClick.AddListener(SkillzState.NotifyProgressionRoomEnter);
      }

      if (startMenuButton)
      {
        startMenuButton.onClick.AddListener(SkillzState.NotifySkillzWillExit);
      }
    }

    private void UpdateDropdowns()
    {
      if (matchTypeDropdown && SkillzSettings.Instance.MatchTypeTemplates)
      {
        int currentSelectedOption = matchTypeDropdown.value;
        matchTypeDropdown.ClearOptions();
        foreach (MatchType template in SkillzSettings.Instance.MatchTypeTemplates.templates)
        {
          matchTypeDropdown.options.Add(new Dropdown.OptionData() { text = template.name });
        }     
        matchTypeDropdown.value = currentSelectedOption;
        matchTypeDropdown.RefreshShownValue();
      }

      if (matchParametersDropdown && SkillzSettings.Instance.MatchParametersTemplates)
      {
        int currentSelectedOption = matchParametersDropdown.value;
        matchParametersDropdown.ClearOptions();
        foreach (MatchParametersTemplate template in SkillzSettings.Instance.MatchParametersTemplates.templates)
        {
          matchParametersDropdown.options.Add(new Dropdown.OptionData() { text = template.name });
        }
        matchParametersDropdown.value = currentSelectedOption;
        matchParametersDropdown.RefreshShownValue();
      }

      if (playerDropdown && SkillzSettings.Instance.PlayerTemplates)
      {
        int currentSelectedOption = playerDropdown.value;
        playerDropdown.ClearOptions();
        foreach (PlayerTemplate template in SkillzSettings.Instance.PlayerTemplates.templates)
        {
          playerDropdown.options.Add(new Dropdown.OptionData() { text = template.displayName });
        }
        playerDropdown.value = currentSelectedOption;
        playerDropdown.RefreshShownValue();
      }
    }

    

    private void StartMatch()
    {
      int randomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue - 1);

      if (seedToggle.isOn)
      {
        try
        {
          randomSeed = int.Parse(seedInput.text);
        }
        catch
        {
          randomSeed = 0;
        }
      }

      APIResponseSimulator.InitializeSimulatedMatch(matchTypeDropdown.value, matchParametersDropdown.value, playerDropdown.value, randomSeed);
    }
  }
}
