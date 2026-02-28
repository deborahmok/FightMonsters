using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Read input (old Input system — works because you have "Both")
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D
        float moveY = Input.GetAxisRaw("Vertical");   // W/S

        movement = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}