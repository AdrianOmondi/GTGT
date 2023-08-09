using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class BB_ScaleAnimation : MonoBehaviour
  {
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float animationTime;
    private Vector3 originalScale;
    private float animationStartTime = float.MinValue;

    private void Awake()
    {
      originalScale = transform.localScale;
    }

    public void Trigger()
    {
      animationStartTime = Time.time;
      
    }

    public void Update()
    {
      if (Time.time - animationStartTime > animationTime)
      {
        transform.localScale = originalScale;
      }
      else
      {
        transform.localScale = originalScale * animationCurve.Evaluate(Time.time - animationStartTime);
      }
    }
  }
}
