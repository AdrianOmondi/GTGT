using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SkillzExamples
{
  public class BB_SceneChangeFade : MonoBehaviour
  {
    [SerializeField] private bool isFadeIn;
    [SerializeField] private bool fadeOnEnable;
    [SerializeField] private float fadeDuration;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private UnityEvent onFadeFinished;
    
    private bool isfading;
    private float fadeStartTime;

    public void StartFade()
    {
      if (isFadeIn)
      {
        canvasGroup.alpha = 1f;
      }
      else
      {
        canvasGroup.alpha = 0;
      }

      isfading = true;
      fadeStartTime = Time.realtimeSinceStartup;

    }
    public void EndFade()
    {
      isfading = false;
      if (isFadeIn)
      {
        canvasGroup.alpha = 0f;
      }
      else
      {
        canvasGroup.alpha = 1f;
      }
      onFadeFinished.Invoke();
    }

    public void OnFadeFinished()
    {
      SkillzManager.LaunchSkillz();
    }

    private void OnEnable()
    {
      if (fadeOnEnable)
      {
        StartFade();
      }
    }

    private void Update()
    {
      if (isfading)
      {
        if(Time.realtimeSinceStartup - fadeStartTime > fadeDuration || fadeDuration == 0)
        {
          EndFade();
          return;
        }
        canvasGroup.alpha = (Time.realtimeSinceStartup - fadeStartTime) / fadeDuration;
      }
    }
  }
}
