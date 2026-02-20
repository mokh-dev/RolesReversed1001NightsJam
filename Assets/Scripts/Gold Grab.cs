using System.Collections;
using UnityEngine;

public class GoldGrab : MonoBehaviour
{
    [SerializeField] float repeatCheckTime;
    [SerializeField] int goldAmmount;
    GameObject player;
    PlayerInventory goldBag;
    Collider2D goldTrigger;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        goldBag = player.GetComponent<PlayerInventory>();
        goldTrigger = GetComponent<Collider2D>();
        goldTrigger.enabled = false;
        StartCoroutine(startTrigger(repeatCheckTime));
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
        if (transform.parent.GetComponent<Rigidbody2D>().linearVelocity.magnitude <= (new Vector2(0.3f, 0.3f)).magnitude)
            goldTrigger.enabled = true;
        else
            StartCoroutine(startTrigger(repeatCheckTime));
    }
}
