using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20;
    [SerializeField] float sprintSpeed = 30;
    [SerializeField] float sneakSpeed = 10;
    float currentMovementSpeed = 0;
    Vector2 moveDirection;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMovementSpeed = moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(moveDirection * currentMovementSpeed * 10);
        Debug.Log(rb.linearVelocity.x);
    }

    void OnMove(InputValue moveValue)
    {
        moveDirection = moveValue.Get<Vector2>();
    }

    void OnSprint(InputValue sprintValue)
    {
        if(sprintValue.isPressed)
        {
            currentMovementSpeed = sprintSpeed;
        }
        else
        {
            currentMovementSpeed = moveSpeed;
        }
    }

    void OnSneak(InputValue sneakValue)
    {
        if(sneakValue.isPressed)
        {
            currentMovementSpeed = sneakSpeed;
        }
        else
        {
            currentMovementSpeed = moveSpeed;
        }
    }
}
