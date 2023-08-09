using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class BB_BallManager : MonoBehaviour
  {
    public Vector2 startPosition = new Vector2(0, -5);
    public List<float> spawnExtraBallsTimes;
    [SerializeField] private BB_BallController ballControllerPrefab;

    private List<BB_BallController> balls;

    private void Awake()
    {
      BB_Managers.ballManager = this;
      balls = new List<BB_BallController>();
    }

    public void SpawnBallStartLocation()
    {
      SpawnBallAt(startPosition);
    }

    public void SpawnBallAt(Vector2 posistion)
    {
      BB_BallController ball = SpawnBall();
      ball.transform.position = posistion;
    }

    public BB_BallController SpawnBall()
    {
      BB_BallController newBall = Instantiate(ballControllerPrefab, this.transform);
      balls.Add(newBall);
      return newBall;
    }

    public void DestroyBall(BB_BallController ball)
    {
      balls.Remove(ball);
      Destroy(ball.gameObject);
      if(balls.Count == 0)
      {
        SpawnBallStartLocation();
      }
      BB_Managers.progressionManager.ballLost = true;
    }

    public void SpawnExtraBalls()
    {
      float time = BB_Managers.matchManager.GetTime();
      for(int i = 0; i < spawnExtraBallsTimes.Count; i++)
      {
        float ballSpawnTime = spawnExtraBallsTimes[i];
        if(time > ballSpawnTime)
        {
          Debug.Log("Spawning Extra Ball");
          SpawnBallStartLocation();
          spawnExtraBallsTimes.RemoveAt(i);
          i--;
        }
      }
    }
  }
}
