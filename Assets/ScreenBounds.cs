using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public float speed = 10f;
    private float minX, maxX, minY, maxY;

    void Start()
    {
        // Calculate screen bounds based on the current resolution
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        minX = -screenBounds.x;
        maxX = screenBounds.x;
        minY = -screenBounds.y;
        maxY = screenBounds.y;
    }

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

                // Calculate the movement based on touch delta position
                horizontalMovement = touchDeltaPosition.x;
                verticalMovement = touchDeltaPosition.y;
            }
        }

        // Calculate the movement vector
        Vector2 movement = new Vector2(horizontalMovement, verticalMovement) * speed * Time.fixedDeltaTime;

        // Calculate the new position
        Vector2 newPosition = (Vector2)transform.position + movement;

        // Clamp the position within the screen bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Apply the clamped position to the player's position
        GetComponent<Rigidbody2D>().MovePosition(newPosition);
    }
}

