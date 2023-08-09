#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using SkillzSDK.Settings;

[InitializeOnLoadAttribute]
public static class DefaultSceneLoader
{
  static DefaultSceneLoader(){
    EditorApplication.playModeStateChanged += LoadDefaultScene;
  }
  static void LoadDefaultScene(PlayModeStateChange state){
    if (state == PlayModeStateChange.EnteredPlayMode && SkillzSettings.Instance.IsLaunching) {
      SkillzSettings.Instance.IsLaunching = false;
      SkillzDebug.Log(SkillzDebug.Type.SIDEKICK, "Loading Scene at index 0");
      EditorSceneManager.LoadScene (0);
    }
  }
}
#endif
