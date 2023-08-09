using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkillzExamples
{
  public class BB_UIAnimator : MonoBehaviour
  {
    [Header("Fade")]
    [SerializeField] CanvasGroup fadeCanvasGroup;
    [SerializeField] float fadeInDuration = 1;
    [SerializeField] float fadeOutDuration = 1;

    bool isFadingIn;
    bool isFadingOut;

    [Header("Scale")]
    [SerializeField] RectTransform scaleTransform;
    [SerializeField] float scaleDuration = 1;
    [SerializeField] AnimationCurve scaleCurve;


    Vector3 originalScale;
    float timeOfScaleStarted = float.MinValue;
    bool isScaling;
    

    private void OnEnable()
    {
      FadeIn();
      TriggerScale();
    }

    private void Update()
    {

      if (fadeCanvasGroup)
      {
        ProcessFade();
      }

      if (scaleTransform)
      {
        ProcessScale();
      }

    }

    public void TriggerScale()
    {
      if (scaleTransform)
      {
        originalScale = scaleTransform.localScale;
        timeOfScaleStarted = Time.realtimeSinceStartup;
        isScaling = true;
      }
    }

    private void ProcessScale()
    {
      if (isScaling)
      {
        float time = Time.realtimeSinceStartup - timeOfScaleStarted;
        if(time > scaleDuration)
        {
          isScaling = false;
          scaleTransform.localScale = originalScale;
        }
        else
        {
          scaleTransform.localScale = originalScale * scaleCurve.Evaluate(time);
        }
      }
    }

    private void ProcessFade()
    {
      if (isFadingIn)
      {
        if (fadeCanvasGroup.alpha < 1)
        {
          fadeCanvasGroup.alpha += (Time.unscaledDeltaTime / fadeInDuration);
          if (fadeCanvasGroup.alpha >= 1)
          {
            isFadingIn = false;
            fadeCanvasGroup.alpha = 1;
          }
        }
      }

      if (isFadingOut)
      {
        if (fadeCanvasGroup.alpha > 0)
        {
          fadeCanvasGroup.alpha -= (Time.unscaledDeltaTime / fadeOutDuration);
          if (fadeCanvasGroup.alpha <= 0)
          {
            isFadingOut = false;
            fadeCanvasGroup.alpha = 0;
          }
        }
      }
    }

    public void FadeIn()
    {
      isFadingIn = true;
      isFadingOut = false;
      fadeCanvasGroup.alpha = 0;
    }
    public void FadeOut()
    {
      isFadingOut = true;
      isFadingIn = false;
    }

    

  }
}
