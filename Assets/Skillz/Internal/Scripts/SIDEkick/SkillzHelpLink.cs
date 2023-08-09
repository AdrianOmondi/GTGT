using UnityEngine;
using UnityEngine.UI;

namespace SkillzSDK
{
  public class SkillzHelpLink : MonoBehaviour
  {
    [SerializeField] Button linkButton;
    [SerializeField] string url;

    private void Awake()
    {
      if (linkButton)
      {
        linkButton.onClick.AddListener(LinkButtonClicked);
      }
    }

    public void LinkButtonClicked()
    {
      Application.OpenURL(url);
    }
  }
}
