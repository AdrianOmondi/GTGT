using UnityEngine;
using UnityEngine.SceneManagement;

using SkillzSDK;

public class SkillzExampleMatchDelegate : SkillzMatchDelegate
{
  private string ProgressionRoomScene = "";
  private string StartMenuScene = "";
  private string GameScene = "";

  // Called when a player chooses a tournament and the match countdown expires
  public void OnMatchWillBegin(Match matchInfo)
  {
    // This is where you launch into your competitive gameplay
    Debug.Log("Loading Game Scene: " + GameScene);
    SceneManager.LoadScene(GameScene);
  }

  // Called when a player clicks the Progression entry point or side menu
  public void OnProgressionRoomEnter()
  {
    Debug.Log("Loading Progression Room Scene: " + ProgressionRoomScene);
    SceneManager.LoadScene(ProgressionRoomScene);
  }

  // Called when a player chooses Exit Skillz from the side menu
  public void OnSkillzWillExit()
  {
    Debug.Log("Loading Start Menu Scene: " + StartMenuScene);
    SceneManager.LoadScene(StartMenuScene);
  }

  public void SetScenes(string startMenuScene, string gameScene, string progressionRoomScene)
  {
    this.StartMenuScene = startMenuScene;
    this.GameScene = gameScene;
    this.ProgressionRoomScene = progressionRoomScene;
  }
}
