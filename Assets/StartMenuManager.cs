using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{ 
  public string GameScene;
  public void PlayButtonPressed()
  {
    SkillzCrossPlatform.LaunchSkillz();
  }
}
