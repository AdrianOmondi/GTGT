using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class BB_PaddleController : MonoBehaviour
  {
    public BB_ScaleAnimation scaleAnimation;
    Rigidbody2D rigidBody;

    private void Awake()
    {
      rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      ComputerControls();
      MobileControls();
    }

    private void MobileControls()
    {
      float targetPaddlePosition = transform.position.x;
      List<float> bottomTouches = new List<float>();

      for (int i = 0; i < Input.touchCount; i++)
      {
        Touch touch = Input.GetTouch(i);
        Vector2 touchPoint = Camera.main.ScreenToWorldPoint(touch.position);
        if (touchPoint.y < -16) //if touching at bottom of screen
        {
          targetPaddlePosition = touchPoint.x;
          bottomTouches.Add(targetPaddlePosition);
        }
      }

      //calculate target position as average of touches at bottom of the screen
      float total = 0f;
      foreach (float bt in bottomTouches)
      {
        total += bt;
      }
      if (bottomTouches.Count > 0)
      {
        targetPaddlePosition = total / bottomTouches.Count;
      }

      Vector2 paddleMovementVector = new Vector2(targetPaddlePosition - transform.position.x, 0);

      //stop paddle vibration when running into bounds
      if (AtLeftBounds(paddleMovementVector))
      {
        paddleMovementVector = Vector2.zero;
      }
      if (AtRightBounds(paddleMovementVector))
      {
        paddleMovementVector = Vector2.zero;
      }

      //move the paddle toward the target position
      rigidBody.AddForce((paddleMovementVector / 3) * 50000);


    }

    private bool AtRightBounds(Vector2 paddleMovementVector)
    {
      return transform.position.x > 11.7 && paddleMovementVector.x > 0;
    }

    private bool AtLeftBounds(Vector2 paddleMovementVector)
    {
      return transform.position.x < -11.7 && paddleMovementVector.x < 0;
    }

    private void ComputerControls()
    {
      if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
      {
        Vector2 paddleMovementVector = Vector2.left;
        if (!AtLeftBounds(paddleMovementVector))
        {
          rigidBody.AddForce(paddleMovementVector * 40000);
        }
      }
      if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
      {
        Vector2 paddleMovementVector = Vector2.right;
        if (!AtRightBounds(paddleMovementVector))
        {
          rigidBody.AddForce(paddleMovementVector * 40000);
        }
      }
    }

    //Stop the paddle from bouncing off the bounds
    void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.GetComponent<BB_BoundsController>())
      {
        rigidBody.velocity = Vector2.zero;
      }
      if (collision.gameObject.GetComponent<BB_BallController>())
      {
        scaleAnimation.Trigger();
      }
    }

  }
}
