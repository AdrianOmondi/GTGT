using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public static class BB_Fairness
  {

    private static List<Vector2> randomBallPerturbations;
    private static int nextPerturbationIndex;

    public static BB_GameBoard GenerateFairGame(List<BB_GameBoard> possibleGameBoards, bool isDeluxMatch, Transform gameBoardParent)
    {
      //Choose Game Board
      int gameBoardIndex = SkillzCrossPlatform.Random.Range(0,possibleGameBoards.Count);
      BB_GameBoard chosenGameBoard = possibleGameBoards[gameBoardIndex];

      //Create the game board
      BB_GameBoard gameBoard = GameObject.Instantiate(chosenGameBoard, gameBoardParent);

      //Choose Brick Multiplier Locations
      if (isDeluxMatch)
      {
        for (int i = 0; i < 6; i++)
        {
          int brickChoice = SkillzCrossPlatform.Random.Range(0, gameBoard.NumberOfBricks());
          gameBoard.GetBrick(brickChoice).SetMultiplier(2);
        }
      }

      //Generate List of ball perturbations
      randomBallPerturbations = new List<Vector2>();
      for (int i = 0; i < 50; i++)
      {
        Vector2 randomPerturbation = SkillzCrossPlatform.Random.InsideUnitCircle() * .1f;
        randomBallPerturbations.Add(randomPerturbation);
      }

      return gameBoard;
    }

    public static Vector2 GetNextBallPerturbation()
    {
      Vector2 nextRandomPerturbation = randomBallPerturbations[nextPerturbationIndex];

      nextPerturbationIndex++;
      if(nextPerturbationIndex >= randomBallPerturbations.Count)
      {
        nextPerturbationIndex = 0;
      }

      return nextRandomPerturbation;
    }
  }
}
