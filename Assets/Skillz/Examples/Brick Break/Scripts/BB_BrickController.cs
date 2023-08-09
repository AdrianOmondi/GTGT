using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SkillzExamples
{
  public class BB_BrickController : MonoBehaviour
  {
    
    public enum BrickType { bronze, Silver, Gold, platinum }
    public BrickType brickType;
    [SerializeField] TextMeshPro multiplierText;

    public SpriteRenderer spriteRenderer;
    public int ScoreWhenBroken = 100;
    public int ScoreMultiplier = 1;

    private BB_GameBoard gameBoard;

    private void Awake()
    {
      if(brickType == BrickType.bronze)
      {
        spriteRenderer.color = new Color(.69f, .55f, .34f);
        ScoreWhenBroken = 100;
      }
      if (brickType == BrickType.Silver)
      {
        spriteRenderer.color = new Color(.75f, .75f, .75f);
        ScoreWhenBroken = 200;
      }
      if (brickType == BrickType.Gold)
      {
        spriteRenderer.color = new Color(.83f, .69f, .22f);
        ScoreWhenBroken = 400;
      }
      if (brickType == BrickType.platinum)
      {
        spriteRenderer.color = new Color(.898f, .894f, .886f);
        ScoreWhenBroken = 800;
      }

      gameBoard = transform.GetComponentInParent<BB_GameBoard>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      Debug.Log("Brick hit: " + gameObject.name);
      gameBoard.BreakBrick(this);
      BB_Managers.particleSystemManager.BrickBreak(this);
    }

    public void SetMultiplier(int multiplier)
    {
      ScoreMultiplier = multiplier;
      if (multiplierText)
      {
        multiplierText.gameObject.SetActive(true);
        multiplierText.text = "X" + multiplier.ToString();
      }
    }
  }
}
