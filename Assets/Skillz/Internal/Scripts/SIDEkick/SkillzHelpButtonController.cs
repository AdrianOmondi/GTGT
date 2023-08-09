using UnityEngine;
using UnityEngine.UI;

namespace SkillzSDK
{
  public class SkillzHelpButtonController : MonoBehaviour
  {
    [SerializeField] Button helpButton;
    [SerializeField] SkillzHelpButtonModal helpModal;

    private void Awake()
    {
      if (helpButton)
      {
        helpButton.onClick.AddListener(HelpButtonClicked);
      }
    }

    public void HelpButtonClicked()
    {
      helpModal.gameObject.SetActive(true);
    }
  }
}
