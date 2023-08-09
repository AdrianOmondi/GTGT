using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class BB_ParticleSystemManager : MonoBehaviour
  {
    [SerializeField] GameObject ballCollideParticleSystem;
    [SerializeField] GameObject brickBreakParticleSystem;
    BB_Pool ballCollidePool;
    BB_Pool brickBreakPool;

    private void Awake()
    {
      BB_Managers.particleSystemManager = this;
    }

    private void Start()
    {
      GenerateParticleSystemPools();
    }

    private void GenerateParticleSystemPools()
    {
      ballCollidePool = new BB_Pool();
      ballCollidePool.GeneratePool(20, ballCollideParticleSystem, this.transform);

      brickBreakPool = new BB_Pool();
      brickBreakPool.GeneratePool(20, brickBreakParticleSystem, this.transform);
    }

    public void BallCollide(Vector2 position)
    {
      GameObject ballCollideParticles =  ballCollidePool.GetNext();
      ballCollideParticles.transform.position = position;
      ballCollideParticles.SetActive(true);
    }

    public void BrickBreak(BB_BrickController brick)
    {
      GameObject brickBreakParticles = brickBreakPool.GetNext();
      brickBreakParticles.transform.position = brick.transform.position + Vector3.forward * -1f;

      //set particle system color to color of brick
      ParticleSystem.MainModule settings = brickBreakParticles.GetComponent<ParticleSystem>().main;
      settings.startColor = new ParticleSystem.MinMaxGradient(brick.spriteRenderer.color);

      brickBreakParticles.GetComponentInChildren<BB_BrickBreakScore>().SetScore(brick.ScoreWhenBroken * brick.ScoreMultiplier);

      brickBreakParticles.SetActive(true);
    }
  }
}
