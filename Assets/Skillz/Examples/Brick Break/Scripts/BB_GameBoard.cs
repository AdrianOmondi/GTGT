using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class BB_GameBoard : MonoBehaviour
  {
    public List<BB_BrickController> bricks;
    private int initialNumberOfBricks;

    private void Awake()
    {
      bricks = new List<BB_BrickController>(GetComponentsInChildren<BB_BrickController>(false));
      initialNumberOfBricks = NumberOfBricks();
    }

    public BB_BrickController GetBrick(int index)
    {
      if(bricks.Count > index)
      {
        return bricks[index];
      }
      return null;
    }

    public int NumberOfBricks()
    {
      return bricks.Count;
    }

    public int NumberOfBricksBroken()
    {
      return initialNumberOfBricks - NumberOfBricks();
    }

    public void BreakBrick(BB_BrickController brick)
    {
      bricks.Remove(brick);
      BB_Managers.matchManager.AddScore(brick.ScoreWhenBroken * brick.ScoreMultiplier);
      brick.gameObject.SetActive(false);
    }

    public bool AllBricksBroken()
    {
      return bricks.Count == 0;
    }

    public bool OnlyBronzeRemaining()
    {
      foreach(BB_BrickController brick in bricks)
      {
        if(brick.gameObject.activeInHierarchy && brick.brickType != BB_BrickController.BrickType.bronze)
        {
          return false;
        }
      }
      return true;
    }
  }
}
