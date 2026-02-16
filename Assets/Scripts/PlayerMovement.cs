using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] float moveSpeed = 20;
    [SerializeField] float sprintSpeed = 30;
    [SerializeField] float sneakSpeed = 10;

    [Header("Noise")]
    [SerializeField] float movingRadius = 5;
    [SerializeField] float sprintingRadius = 7;
    [SerializeField] float sneakingRaius = 3;
    [SerializeField] float timeToDropGold = 3;

    [Header("Animation")]
    [SerializeField] float _baseAnimSpeed;
    [SerializeField] float _sprintAnimSpeed;
    [SerializeField] float _sneakAnimSpeed;


    bool isMoving = false;
    bool isSprinting = false;
    bool isSneaking = false;
    float currentMovementSpeed = 0;
    float goldDropTimer;
    Vector2 moveDirection;
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;
    Transform noiseRadius;
    PlayerInventory goldBag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        noiseRadius = transform.GetChild(0).GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentMovementSpeed = moveSpeed;
        goldBag = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        SpriteAndAnim();

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
                goldDropTimer += Time.deltaTime;
                if(goldDropTimer >= timeToDropGold)
                {
                    goldBag.removeGold(1);
                    goldDropTimer = 0;
                }
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

    void SpriteAndAnim()
    {
        if (rb.linearVelocityX > 0) sr.flipX = false;
        else if (rb.linearVelocityX < 0) sr.flipX = true;

        anim.SetBool("IsWalking", isMoving);

        if (currentMovementSpeed == moveSpeed) anim.SetFloat("WalkAnimSpeed", _baseAnimSpeed);
        else if (currentMovementSpeed == sprintSpeed) anim.SetFloat("WalkAnimSpeed", _sprintAnimSpeed);
        else if (currentMovementSpeed == sneakSpeed) anim.SetFloat("WalkAnimSpeed", _sneakAnimSpeed);       
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
