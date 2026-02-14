using System.Collections;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //serialize Fields
    [SerializeField] float noiseAlertGain = 0.4f;
    [SerializeField] float AlertThreshhold = 15;

    //Basic Variables
    bool heardNoise = false;
    bool isLooping = false;
    float alertMeter = 0;
    float timer = 0;
    
    //Complex Variables
    SpriteRenderer sr;

//Starts and Updates-----------------------------------------------------------------------------------------
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Debug.Log(alertMeter + "/" + AlertThreshhold);
    }

//Colliders and Triggers-------------------------------------------------------------------------------------
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
            addAlert(noiseAlertGain);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Noise"))
        {
            heardNoise = false;
            StartCoroutine(RemoveAlert(noiseAlertGain));
        }
    }

//Custom Methods---------------------------------------------------------------------------------------------
    void addAlert(float alert)
    {
        if (alertMeter < AlertThreshhold)
        {
            timer += Time.deltaTime;
            if (timer >= 0.25f)
            {
                alertMeter += alert;
                timer = 0;
            }
        }
        else if(alertMeter > AlertThreshhold)
        {
            alertMeter = AlertThreshhold + (noiseAlertGain * 20);
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
        if(alertMeter >= AlertThreshhold)
        {
            if(sr.color != Color.red)
            {
                sr.color = Color.red;
            }
        }
        else if(alertMeter > AlertThreshhold/2)
        {
            if(sr.color != Color.orange)
            {
                sr.color = Color.orange;
            }
        }
        else if(alertMeter > AlertThreshhold/4)
        {
            if(sr.color != Color.yellow)
            {
                sr.color = Color.yellow;
            }
        }
        else if(alertMeter < AlertThreshhold/4 && sr.color != Color.green)
        {
            sr.color = Color.green;
        }
    }
}
