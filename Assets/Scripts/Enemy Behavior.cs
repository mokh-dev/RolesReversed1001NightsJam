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
    [SerializeField] private float _detectionFovAngle;
    [SerializeField] private float _detectionRange;
    [SerializeField] private LayerMask _distractionDetectionLayers;
    [SerializeField] private LayerMask _playerDetectionLayers;
    [SerializeField] private Transform _visionStartPoint;

    //Basic Variables
    bool heardNoise = false;
    bool isLooping = false;
    float alertMeter = 0;
    float timer = 0;

    bool canSeePlayer;
    bool canSeeDistraction;
    
    //Complex Variables
    SpriteRenderer sr;
    GameObject player;
    GoldBag goldBag;

//Starts and Updates-----------------------------------------------------------------------------------------
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        goldBag = player.GetComponent<GoldBag>();
    }

    void Update()
    {

    }

    //Colliders and Triggers-------------------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            goldBag.removeGold(1);
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

    public void OnVisionColliderStay(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSeePlayer = RaycastCheck(collision.gameObject, _playerDetectionLayers);
        }

        if (collision.gameObject.CompareTag("Distraction"))
        {
            canSeeDistraction = RaycastCheck(collision.gameObject, _distractionDetectionLayers);
        }
    }

    public void OnVisionColliderExit(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSeePlayer = false;
        }
        if (collision.gameObject.CompareTag("Distraction"))
        {
            canSeeDistraction = false;
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

        return hit.collider.gameObject == objInVisionCollider;
    }
}
