using System.Collections;
using UnityEngine;

public class GoldGrab : MonoBehaviour
{
    [SerializeField] float timeToWake;
    [SerializeField] int goldAmmount;
    GameObject player;
    GoldBag goldBag;
    Collider2D goldTrigger;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        goldBag = player.GetComponent<GoldBag>();
        goldTrigger = GetComponent<Collider2D>();
        goldTrigger.enabled = false;

        StartCoroutine(startTrigger(timeToWake));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (goldBag.getCurrentGold() < goldBag.getMaxGold() && collision.CompareTag("Player"))
        {
            goldBag.addGold(goldAmmount);
            Destroy(transform.parent.gameObject);
        }
    }

    IEnumerator startTrigger (float time)
    {
        yield return new WaitForSeconds(time);
        goldTrigger.enabled = true;
    }
}
