using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SkillzSDK;

namespace SkillzSDK
{
  public class SkillzDebugController : MonoBehaviour
  {

    [Header("Debug Panel UI")]

    [SerializeField] private Button submitButton;

    [SerializeField] private TMP_Dropdown debugDropdown;

    [SerializeField] private TextMeshProUGUI outputText;

    [Header("Progression UI")]

    [SerializeField] private GameObject progressionPanel;

    [SerializeField] private TMP_Dropdown progressionDropdown;

    [SerializeField] private TMP_InputField progressionKeyInput;

    [Header("Season UI")]

    [SerializeField] private GameObject seasonPanel;

    [SerializeField] private TMP_Dropdown seasonsDropdown;

    [SerializeField] private Slider seasonsSlider;

    [SerializeField] private TextMeshProUGUI seasonsSliderValueDisplay;

    private enum debugOptions
    {
      MATCH_PARAMETERS,
      CURRENT_PLAYER_DATA,
      AUDIO_VOLUME,
      PROGRESSION_DATA,
      SEASON_DATA,
      RANDOM_DATA
    }

    private enum progressionOptions
    {
      DEFAULT_PLAYER_DATA,
      PLAYER_DATA,
      IN_GAME_ITEMS
    }

    private enum seasonsOptions
    {
      CURRENT_SEASON,
      PREVIOUS_SEASONS,
      NEXT_SEASONS
    }

    //Used to ensure other threads can write to output successfully
    private string outputString;

    private void Awake()
    {

      if (submitButton != null)
      {
        submitButton.onClick.AddListener(SubmitPressed);
      }

      if (debugDropdown != null)
      {
        debugDropdown.onValueChanged.AddListener(OnDropdownChanged);
      }

      if (progressionDropdown != null)
      {
        progressionDropdown.onValueChanged.AddListener(OnProgressionDropdownChanged);
      }

      if (seasonsDropdown != null)
      {
        seasonsDropdown.onValueChanged.AddListener(OnSeasonsDropdownChanged);
      }

      if (seasonsSlider != null)
      {
        seasonsSlider.onValueChanged.AddListener(OnSeasonsSliderChanged);
      }

      DeactiveAllSubPanels();
    }


    private void Update()
    {
      outputText.text = outputString;
    }

    //Display the correct panel when dropdown value is changed
    private void OnDropdownChanged(int val)
    {
      DeactiveAllSubPanels();

      if (val == (int)debugOptions.PROGRESSION_DATA)
      {
        progressionPanel.SetActive(true);
      }

      if (val == (int)debugOptions.SEASON_DATA)
      {
        seasonPanel.SetActive(true);
      }
    }

    //Display the selected data in the Debug Output text box
    private void SubmitPressed()
    {
      if (debugDropdown.value == (int)debugOptions.MATCH_PARAMETERS)
      {
        DisplayMatchParameters();
      }

      if (debugDropdown.value == (int)debugOptions.CURRENT_PLAYER_DATA)
      {
        DisplayCurrentPlayerData();
      }

      if (debugDropdown.value == (int)debugOptions.AUDIO_VOLUME)
      {
        DisplayAudioVolume();
      }

      if (debugDropdown.value == (int)debugOptions.PROGRESSION_DATA)
      {
        DisplayProgressionData();
      }

      if (debugDropdown.value == (int)debugOptions.SEASON_DATA)
      {
        DisplaySeasonData();
      }

      if (debugDropdown.value == (int)debugOptions.RANDOM_DATA)
      {
        DisplayRandomData();
      }
    }

    private void DeactiveAllSubPanels()
    {
      progressionPanel.SetActive(false);
      seasonPanel.SetActive(false);
    }

    private void DisplayMatchParameters()
    {
      Match match = SkillzCrossPlatform.GetMatchInfo();
      if (match != null)
      {
        string matchString = "---Match Info---" +
              "\nID: [" + match.ID + "]" +
              "\nName: [" + match.Name + "]" +
              "\nDescription: [" + match.Description + "]" +
              "\nTemplateID: [" + match.TemplateID + "]" +
              "\nSkillzDifficulty: [" + match.SkillzDifficulty + "]" +
              "\nIsCash: [" + match.IsCash + "]" +
              "\nIsSynchronous: [" + match.IsSynchronous + "]" +
              "\nIsTieBreaker: [" + match.IsTieBreaker + "]" +
              "\nIsBracket: [" + match.IsBracket + "]" +
              "\nBracketRound: [" + match.BracketRound + "]" +
              "\nIsVideoAdEntry: [" + match.IsVideoAdEntry + "]" +
              "\nIsCustomSynchronousMatch: [" + match.IsCustomSynchronousMatch + "]" +
              "\nEntryPoints: [" + match.EntryPoints + "]" +
              "\nEntryCash: [" + match.EntryCash + "]";

        string playersString = "\nPlayers: {";
        foreach (Player player in match.Players)
        {
          if (player != null)
          {
            playersString += player.DisplayName + ",";
          }
        }
        playersString = playersString.Remove(playersString.Length - 1, 1); //remove last comma
        playersString += "}\n\n";

        string gameParamsString = "---Game Parameters---";
        foreach (KeyValuePair<string, string> entry in match.GameParams)
        {
          gameParamsString += "\n" + entry.Key + ": [" + entry.Value + "]";
        }

        string customServerInfoString = "";
        if (match.CustomServerConnectionInfo != null)
        {
          customServerInfoString += "\n\n---Custom Server Connection Info---";
          customServerInfoString += "\nMatchId: [" + match.CustomServerConnectionInfo.MatchId + "]";
          customServerInfoString += "\nServerIp: [" + match.CustomServerConnectionInfo.ServerIp + "]";
          customServerInfoString += "\nServerPort: [" + match.CustomServerConnectionInfo.ServerPort + "]";
          customServerInfoString += "\nMatchToken: [" + match.CustomServerConnectionInfo.MatchToken + "]";
          customServerInfoString += "\nIsBotMatch: [" + match.CustomServerConnectionInfo.IsBotMatch + "]";
        }

        SetOutput(matchString + playersString + gameParamsString + customServerInfoString);
      }
      else
      {
        SetOutput("Current Match is null");
      }
    }

    private void DisplayCurrentPlayerData()
    {
      Player player = SkillzCrossPlatform.GetPlayer();
      SetOutput("---Current Player info---\n");
      if (player != null)
      {
        string playerString = "ID: [" + player.ID + "]" +
                              "\nDisplayName: [" + player.DisplayName + "]" +
                              "\nTournamentPlayerID: [" + player.TournamentPlayerID + "]" +
                              "\nAvatarURL: [" + player.AvatarURL + "]" +
                              "\nFlagURL: [" + player.FlagURL + "]";
        AddOutput(playerString);
      }
      else
      {
        AddOutput("Current Player is null");
      }
    }

    private void DisplayAudioVolume()
    {
      SetOutput("---Audio Info---\n");
      AddOutput("Skillz Music Volume: " + SkillzCrossPlatform.getSkillzMusicVolume() + "\n");
      AddOutput("Skillz SXF Volume: " + SkillzCrossPlatform.getSFXVolume());
    }

    #region Progression

    private void OnProgressionDropdownChanged(int val)
    {
      if (val == (int)progressionOptions.DEFAULT_PLAYER_DATA)
      {
        progressionKeyInput.gameObject.SetActive(false);
      }
      else
      {
        progressionKeyInput.gameObject.SetActive(true);
      }
    }

    private void DisplayProgressionData()
    {
      string progressionNamespace = "";
      List<string> userDataKeys = new List<string>();

      if (progressionDropdown.value == (int)progressionOptions.DEFAULT_PLAYER_DATA)
      {
        progressionNamespace = ProgressionNamespace.DEFAULT_PLAYER_DATA;
        userDataKeys = new List<string>()
      {
        "games_played",
        "cash_games_played",
        "games_won",
        "cash_games_won",
        "best_score_lifetime",
        "average_score",
        "player_level",
        "skillz_level",
        "install_date"
      };

        SetOutput("---Getting Default Progression Data---\n");
      }

      if (progressionDropdown.value == (int)progressionOptions.PLAYER_DATA)
      {
        progressionNamespace = ProgressionNamespace.PLAYER_DATA;
        userDataKeys.Add(progressionKeyInput.text);

        SetOutput("---Getting Progression Player Data: " + progressionKeyInput.text + "---\n");
      }

      if (progressionDropdown.value == (int)progressionOptions.IN_GAME_ITEMS)
      {
        progressionNamespace = ProgressionNamespace.IN_GAME_ITEMS;
        userDataKeys.Add(progressionKeyInput.text);
        SetOutput("---Getting In Game Item: " + progressionKeyInput.text + "---\n");
      }

      SkillzCrossPlatform.GetProgressionUserData(progressionNamespace,
                                                userDataKeys,
                                                ProgressionSuccessCallback,
                                                ProgressionFailureCallback);
    }

    private void ProgressionSuccessCallback(Dictionary<string, ProgressionValue> progressionData)
    {
      AddOutput("Success\n");
      foreach (string key in progressionData.Keys)
      {
        AddOutput("Key: [" + key + "]\n");
        AddOutput("Value: [" + progressionData[key].Value + "]\n");
        AddOutput("DataType: [" + progressionData[key].DataType + "]\n");
        AddOutput("DisplayName: [" + progressionData[key].DisplayName + "]\n");
        AddOutput("LastUpdatedTime: [" + progressionData[key].LastUpdatedTime + "]\n");
        if (progressionData[key].Metadata != null)
        {
          AddOutput(progressionData[key].Metadata.ToString());
        }
        else
        {
          AddOutput("ProgressionMetadata: null");
        }
        AddOutput("\n\n");
      }
      if (progressionData.Count == 0)
      {
        AddOutput("No Progression Data Returned\n\n");
      }
    }

    private void ProgressionFailureCallback(string message)
    {
      AddOutput("Failure\n");
      AddOutput(message);
    }

    #endregion

    #region Seasons

    private void OnSeasonsDropdownChanged(int val)
    {
      if (val == (int)seasonsOptions.CURRENT_SEASON)
      {
        seasonsSlider.gameObject.SetActive(false);
        seasonsSliderValueDisplay.gameObject.SetActive(false);
      }
      else
      {
        seasonsSlider.gameObject.SetActive(true);
        seasonsSliderValueDisplay.gameObject.SetActive(true);
      }
    }

    private void OnSeasonsSliderChanged(float val)
    {
      seasonsSliderValueDisplay.text = ((int)val).ToString();
    }

    private void DisplaySeasonData()
    {
      if (seasonsDropdown.value == (int)seasonsOptions.CURRENT_SEASON)
      {
        SetOutput("---Getting Current Season---\n");
        SkillzCrossPlatform.GetCurrentSeason(CurrentSeasonSuccessCallback, SeasonsFailureCallback);
      }

      if (seasonsDropdown.value == (int)seasonsOptions.NEXT_SEASONS)
      {
        int seasonsCount = (int)seasonsSlider.value;
        SetOutput("---Getting " + seasonsCount + " Next Seasons---\n");
        SkillzCrossPlatform.GetNextSeasons(seasonsCount, SeasonsSuccessCallback, SeasonsFailureCallback);
      }

      if (seasonsDropdown.value == (int)seasonsOptions.PREVIOUS_SEASONS)
      {
        int seasonsCount = (int)seasonsSlider.value;
        SetOutput("---Getting " + seasonsCount + " Previous Seasons---\n");
        SkillzCrossPlatform.GetPreviousSeasons(seasonsCount, SeasonsSuccessCallback, SeasonsFailureCallback);
      }
    }

    private void CurrentSeasonSuccessCallback(Season season)
    {
      AddOutput("Success\n");
      AddOutput(SeasonToString(season));
    }

    private void SeasonsSuccessCallback(List<Season> seasons)
    {
      AddOutput("Success\n");
      if (seasons.Count == 0)
      {
        AddOutput("No Seasons");
      }
      foreach (Season season in seasons)
      {
        AddOutput(SeasonToString(season));
      }
    }

    private void SeasonsFailureCallback(string message)
    {
      AddOutput("Failure\n");
      AddOutput(message);
    }

    private string SeasonToString(Season season)
    {
      if (season == null)
      {
        return "Season is null\n\n";
      }

      string seasonString = "\nID: [" + season.ID + "]" +
                            "\nName: [" + season.Name + "]" +
                            "\nDescription: [" + season.Description + "]" +
                            "\nStartTime: [" + season.StartTime.ToString() + "]" +
                            "\nEndTime: [" + season.EndTime.ToString() + "]" +
                            "\nIsActive: [" + season.IsActive + "]" +
                            "\nIndex: [" + season.SeasonIndex + "]";

      string paramsString = "\n-SeasonParams-";
      foreach (string key in season.SeasonParams.Keys)
      {
        paramsString += ("\n" + key + ": [" + season.SeasonParams[key] + "]");
      }
      if (season.SeasonParams.Count == 0)
      {
        paramsString += "No Params";
      }
      paramsString += "\n\n";

      return seasonString + paramsString;
    }

    private void DisplayRandomData()
    {
      SetOutput("---Getting Skillz Random Numbers---\n");
      for (int i = 0; i < 10; i++)
      {
        AddOutput(SkillzCrossPlatform.Random.Value().ToString() + "\n");
      }
    }


    #endregion

    private void SetOutput(string message)
    {
      if (message == null)
      {
        outputString = "null";
      }
      else
      {
        outputString = message;
      }
    }

    private void AddOutput(string message)
    {
      outputString += message;
    }
  }
}