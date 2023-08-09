using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkillzExamples
{
  public class BB_ButtonAnimator : MonoBehaviour
  {
    [SerializeField] Button button;
    public float pulseSpeed = 1f;
    public float pulseMagnitude = .05f;

    private Vector3 orignalScale;

    private void Awake()
    {
      orignalScale = button.transform.localScale;
    }

    private void Update()
    {
      button.transform.localScale = orignalScale + Vector3.one * (Mathf.Sin(Time.realtimeSinceStartup * pulseSpeed) * pulseMagnitude);
    }
  }
}
