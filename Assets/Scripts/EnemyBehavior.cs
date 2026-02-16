using System.Collections;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Noise Detection")]
    [SerializeField] float _noiseAlertGain = 0.4f;
    [SerializeField] float _alertThreshhold = 15;

    [Header("Vision Detection")]
    [SerializeField] private float _detectionRange;
    [SerializeField] private LayerMask _distractionDetectionLayers;
    [SerializeField] private LayerMask _playerDetectionLayers;
    [SerializeField] private Transform _visionStartPoint;

    [Header("Enemy Movement")]
    [SerializeField] private float _patrolMovementSpeed;
    [SerializeField] private float _distractionMovementSpeed;
    [SerializeField] private float _chasingMovementSpeed;
    [SerializeField] private float _distractionCalmDownTime;
    [SerializeField] private float _chaseCalmDownTime;
    [SerializeField] private float _patrolPointStopTime;
    [SerializeField] private Vector2[] _patrolPathPoints;


    //Basic Variables
    bool heardNoise = false;
    bool isLooping = false;
    
    float alertMeter = 0;
    float timer = 0;
    float ySmoothVelo = 0.0f;

    //serialized for easier debugging
    [Header("Debugging")]
    [SerializeField] bool canSeePlayer;
    [SerializeField] bool canSeeDistraction;

    [SerializeField] bool isDistracted;
    [SerializeField] bool onPatrol = true;
    [SerializeField] bool onCalmDownTimer;
    [SerializeField] bool onDistractionTimer;
    [SerializeField] bool onPatrolPointStopCooldown;
    [SerializeField] int currentTargetPatrolPoint;


    GameObject distractionObject;
    Vector2 lastSeenDistractionPosition;
    
    //Complex Variables
    SpriteRenderer sr;
    Rigidbody2D rb;
    GameObject player;



//Starts and Updates-----------------------------------------------------------------------------------------
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        SpriteFlip();

        if (canSeePlayer)
        {
            Chasing();
        } 

        if (canSeeDistraction && distractionObject != null && !canSeePlayer)
        {
            isDistracted = true;
        }
        else
        {
            isDistracted = false;
        }


        if (!canSeeDistraction && !canSeePlayer)
        {
            onPatrol = true;
        }
        else
        {
            onPatrol = false;
        }


        if (isDistracted) Distracted();
        if (onPatrol && !onCalmDownTimer && !onDistractionTimer) Patrolling();

    }

    void SpriteFlip()
    {
        if (rb.linearVelocityX > 0) sr.flipX = false;
        else if (rb.linearVelocityX < 0) sr.flipX = true;
    }

    void Patrolling()
    {
        if (_patrolPathPoints.Length <= 0) return;
        if (onPatrolPointStopCooldown == true) return;

        bool withinMarginOfPatrolPoint = Vector2.Distance(transform.position, _patrolPathPoints[currentTargetPatrolPoint]) <= 0.05f;
        
        if (withinMarginOfPatrolPoint == true && onPatrolPointStopCooldown == false)
        {
            StartCoroutine(PatrolPointStopCooldown());
            return;
        }

        Vector2 targetPatrolPointDirection = (_patrolPathPoints[currentTargetPatrolPoint] - (Vector2)transform.position).normalized;
        rb.AddForce(targetPatrolPointDirection * _patrolMovementSpeed);
    }

    IEnumerator PatrolPointStopCooldown()
    {
        onPatrolPointStopCooldown = true;
        yield return new WaitForSeconds(_patrolPointStopTime);

        currentTargetPatrolPoint = (currentTargetPatrolPoint == _patrolPathPoints.Length-1) ? 0 : currentTargetPatrolPoint+1;
        onPatrolPointStopCooldown = false;
    }

    void Chasing()
    {   
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        rb.AddForce(playerDirection * _chasingMovementSpeed);
    }

    void Distracted()
    {
        Vector2 distractionDirection;
        
        if (canSeeDistraction)
        {
            distractionDirection = (distractionObject.transform.position - transform.position).normalized;
        }
        else
        {
            distractionDirection = (lastSeenDistractionPosition - (Vector2)transform.position).normalized;
        }
        
        rb.AddForce(distractionDirection * _distractionMovementSpeed);
    }
    IEnumerator ChaseCalmDownTimer()
    {
        onCalmDownTimer = true;
        yield return new WaitForSeconds(_chaseCalmDownTime);

        onCalmDownTimer = false;
    }

    IEnumerator DistractionLastSeenPos()
    {
        bool withinMarginOfLastSeenPos = Vector2.Distance(transform.position, lastSeenDistractionPosition) <= 0.05f;
        yield return new WaitUntil(() => withinMarginOfLastSeenPos == true);

        StartCoroutine(DistractionCalmDownTimer());
    }
    IEnumerator DistractionCalmDownTimer()
    {
        onDistractionTimer = true;

        canSeeDistraction = false;
        distractionObject = null;

        yield return new WaitForSeconds(_distractionCalmDownTime);
        onDistractionTimer = false;
    }


    //Colliders and Triggers-------------------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //player.GetComponent<PlayerInventory>().removeGold(1);
        }

        if ((distractionObject != null) && (collision.gameObject == distractionObject))
        {
            StartCoroutine(DistractionCalmDownTimer());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Noise"))
        {
            heardNoise = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Noise"))
        {
            addAlert(_noiseAlertGain);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Noise"))
        {
            heardNoise = false;
            StartCoroutine(RemoveAlert(_noiseAlertGain));
        }
    }


    public void OnVisionColliderEnter(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Distraction") || collision.gameObject.CompareTag("Gem"))
        {
            if (RaycastCheck(collision.gameObject, _distractionDetectionLayers)) canSeeDistraction = true;
            distractionObject = collision.gameObject;
        }
    }

    public void OnVisionColliderExit(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSeePlayer = false;
            StartCoroutine(ChaseCalmDownTimer());
        }

        if (collision.gameObject.CompareTag("Distraction") || collision.gameObject.CompareTag("Gem"))
        {
            lastSeenDistractionPosition = collision.gameObject.transform.position;
            canSeeDistraction = false;
            StartCoroutine(DistractionLastSeenPos());
        }
    }
    public void OnVisionColliderStay(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSeePlayer = RaycastCheck(collision.gameObject, _playerDetectionLayers);
        }
    }


//Custom Methods---------------------------------------------------------------------------------------------
    void addAlert(float alert)
    {
        if (alertMeter < _alertThreshhold)
        {
            timer += Time.deltaTime;
            if (timer >= 0.25f)
            {
                alertMeter += alert;
                timer = 0;
            }
        }
        else if(alertMeter > _alertThreshhold)
        {
            alertMeter = _alertThreshhold + (_noiseAlertGain * 20);
        }

        AlertBehavior();
    }

    IEnumerator RemoveAlert(float alert)
    {
        if(!isLooping)
        {
            isLooping = true;
            while(!heardNoise && alertMeter > 0)
            {
                if (alertMeter > 0)
                {
                    timer += Time.deltaTime;
                    if (timer >= 0.25f)
                    {
                        alertMeter -= alert;
                        timer = 0;
                    }
                }
                if(alertMeter < 0)
                {
                    alertMeter = 0;
                }

                AlertBehavior();
                yield return null;
            }
            isLooping = false;
        }
    }

    void AlertBehavior()
    {
        if(alertMeter >= _alertThreshhold)
        {
            if(sr.color != Color.red)
            {
                sr.color = Color.red;
            }
        }
        else if(alertMeter > _alertThreshhold/2)
        {
            if(sr.color != Color.orange)
            {
                sr.color = Color.orange;
            }
        }
        else if(alertMeter > _alertThreshhold/4)
        {
            if(sr.color != Color.yellow)
            {
                sr.color = Color.yellow;
            }
        }
        else if(alertMeter < _alertThreshhold/4 && sr.color != Color.green)
        {
            sr.color = Color.green;
        }
    }

    //checks if walls or vision blocking objects are in the way
    private bool RaycastCheck(GameObject objInVisionCollider, LayerMask detectionLayers)
    {
        Vector2 objDirection = (objInVisionCollider.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(_visionStartPoint.position, objDirection, _detectionRange, detectionLayers);

        if (hit.collider == null) return false;
        
        return hit.collider.gameObject == objInVisionCollider;
    }
}
