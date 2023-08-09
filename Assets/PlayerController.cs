using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.5f;
    public float touchSensitivity = 0.05f; // Adjust this value to reduce touch sensitivity

    void FixedUpdate()
    {
        float horizontalMovement = 0f;
        float verticalMovement = 0f;

        // Check for PC arrow key input
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Get the touch delta position
                Vector2 touchDeltaPosition = touch.deltaPosition;

                // Calculate the movement based on touch delta position with reduced sensitivity
                horizontalMovement = touchDeltaPosition.x * touchSensitivity;
                verticalMovement = touchDeltaPosition.y * touchSensitivity;
            }
        }

        // Calculate the movement vector
        Vector2 movement = new Vector2(horizontalMovement, verticalMovement) * speed * Time.fixedDeltaTime;

        // Apply the movement to the player's position
        GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + movement);
    }
}

