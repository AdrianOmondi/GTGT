using SkillzSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SkillzManager : SkillzEventsHandler
{
  [Header("On Match Will Begin")]
  [Tooltip("Scene that will load when the match begins")]
  public SceneField gameScene;
  public UnityEvent<SkillzSDK.Match> onMatchWillBegin; 

  [Header("On Skillz Will Exit")]
  [Tooltip("Scene that will load when the user returns to the start menu")]
  public SceneField startMenuScene;
  public UnityEvent onSkillzWillExit;

  [Header("On Progression Room Enter")]
  [Tooltip("Scene that will launch when the user clicks on a progression entry point")]
  public SceneField progressionRoomScene;
  public UnityEvent onProgressionRoomEnter;

  static SkillzManager instance;

  void Awake()
  {
    if (instance == null)
    {
      instance = this; // In first scene, make us the singleton.
      DontDestroyOnLoad(gameObject);
    }
    else if (instance != this) {
      Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
    }
  }

  public static bool ExistsInProject()
  {
    return (instance != null);
  }

  public static void LaunchSkillz()
  {
    SkillzCrossPlatform.LaunchSkillz();
  }

  protected override void OnMatchWillBegin(SkillzSDK.Match match)
  {
    onMatchWillBegin.Invoke(match);

    if (gameScene != null)
    {
      SceneManager.LoadScene(gameScene);
    }
  }

  protected override void OnSkillzWillExit()
  {
    onSkillzWillExit.Invoke();

    if (startMenuScene != null)
    {
      SceneManager.LoadScene(startMenuScene);
    }
    
  }

  protected override void OnProgressionRoomEnter()
  {
    onProgressionRoomEnter.Invoke();

    if (progressionRoomScene != null)
    {
      SceneManager.LoadScene(progressionRoomScene);
    }
  }
}
