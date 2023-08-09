using UnityEngine;
using UnityEngine.UI;

namespace SkillzSDK
{
  public class SkillzHelpButtonModal : MonoBehaviour
  {
    [SerializeField] Button closeButton;

    private void Awake()
    {
      if (closeButton)
      {
        closeButton.onClick.AddListener(CloseButtonClicked);
      }
    }

    public void CloseButtonClicked()
    {
      gameObject.SetActive(false);
    }
  }
}
