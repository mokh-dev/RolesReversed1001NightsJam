using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 20;
    [SerializeField] float sprintSpeed = 30;
    [SerializeField] float sneakSpeed = 10;

    [Header("Noise Settings")]
    [SerializeField] float movingRadius = 5;
    [SerializeField] float sprintingRadius = 7;
    [SerializeField] float sneakingRaius = 3;


    bool isMoving = false;
    bool isSprinting = false;
    bool isSneaking = false;
    float currentMovementSpeed = 0;
    Vector2 moveDirection;
    Rigidbody2D rb;
    Transform noiseRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        noiseRadius = transform.GetChild(0).GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        currentMovementSpeed = moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(moveDirection * currentMovementSpeed * 10);

        if(isMoving)
        {
            if(isSneaking)
            {
               noiseRadius.localScale = new(sneakingRaius, sneakingRaius, sneakingRaius);
            }
            else if(isSprinting)
            {
                noiseRadius.localScale = new(sprintingRadius, sprintingRadius, sprintingRadius);
            }
            else
            {
                noiseRadius.localScale = new(movingRadius, movingRadius, movingRadius);
            }
        }
        else
        {
            noiseRadius.localScale = Vector3.zero;
        }
    }

    void OnMove(InputValue moveValue)
    {
        moveDirection = moveValue.Get<Vector2>();
        isMoving = moveDirection != Vector2.zero;
    }

    void OnSprint(InputValue sprintValue)
    {

        if(!isSneaking)
        {
            isSprinting = sprintValue.isPressed;
            if(isSprinting)
            {
                currentMovementSpeed = sprintSpeed;
                
            }
            else
            {
                currentMovementSpeed = moveSpeed;
            }
        }
    }

    void OnSneak(InputValue sneakValue)
    {
        if(!isSprinting)
        {
            isSneaking = sneakValue.isPressed;
            if(isSneaking)
            {
                currentMovementSpeed = sneakSpeed;
            }
            else
            {
                currentMovementSpeed = moveSpeed;
            }
        }
    }
}
