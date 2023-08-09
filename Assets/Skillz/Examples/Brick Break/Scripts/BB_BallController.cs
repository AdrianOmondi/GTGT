using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class BB_BallController : MonoBehaviour
  {
    
    public float maxVelocity;
    public float fadeInDuration = 1;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rigidBody;
    private CircleCollider2D circleCollider;
    private Color originalColor;
    private float fadeInStartTime;
    private bool isFadingIn;

    private void Awake()
    {
      rigidBody = gameObject.GetComponent<Rigidbody2D>();
      originalColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b);
      spriteRenderer.color = Color.clear;
    }

    private void OnEnable()
    {
      StartFadeIn();
    }

    private void FixedUpdate()
    {
      SoftLimitVelocity();
      ProcessFadeIn();
      CheckIfFarOutOfBounds();
    }

    private void CheckIfFarOutOfBounds()
    {
      if(transform.position.x < -25 || transform.position.x > 25 || transform.position.y > 35 || transform.position.y < -50)
      {
        BB_Managers.ballManager.DestroyBall(this);
      }
    }

    private void SoftLimitVelocity()
    {
      //soft limit ball velocity
      if (rigidBody.velocity.magnitude > maxVelocity)
      {
        rigidBody.velocity = rigidBody.velocity * .9f;
      }
    }

    private void ProcessFadeIn()
    {
      if (isFadingIn)
      {
        float time = Time.time - fadeInStartTime;
        if (time > fadeInDuration)
        {
          EndFadeIn();
        }
        if (spriteRenderer)
        {
          spriteRenderer.color = Color.Lerp(Color.clear, originalColor, time / fadeInDuration);
        }
      }
    }

    private void StartFadeIn()
    {
      circleCollider = gameObject.GetComponent<CircleCollider2D>();
      circleCollider.enabled = false;
      rigidBody.bodyType = RigidbodyType2D.Static;
      fadeInStartTime = Time.time;
      isFadingIn = true;
    }

    private void EndFadeIn()
    {
      isFadingIn = false;
      spriteRenderer.color = originalColor;
      rigidBody.bodyType = RigidbodyType2D.Dynamic;
      circleCollider.enabled = true;

      AddRandomPerturbance();
    }

    private void AddRandomPerturbance()
    {
      rigidBody.velocity = BB_Fairness.GetNextBallPerturbation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.GetComponent<BB_PaddleController>())
      {
        Vector2 paddleHitDirection = gameObject.transform.position - collision.gameObject.transform.position;
        rigidBody.AddForce(paddleHitDirection * 250);
      }
      BB_Managers.particleSystemManager.BallCollide(collision.contacts[0].point);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.gameObject.GetComponent<BB_FloorController>())
      {
        BB_Managers.ballManager.DestroyBall(this);
      }
    }
  }
}

