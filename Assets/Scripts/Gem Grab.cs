using System.Collections;
using UnityEngine;

public class GemGrab : MonoBehaviour
{
    [SerializeField] float timeToWake = 3;
    [SerializeField] int gemAmmount;
    GameObject player;
    GemBag gemBag;
    Collider2D gemTrigger;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gemBag = player.GetComponent<GemBag>();
        gemTrigger = GetComponent<Collider2D>();
        gemTrigger.enabled = false;

        StartCoroutine(startTrigger(timeToWake));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gemBag.getCurrentGems() < gemBag.getMaxGems() && collision.CompareTag("Player"))
        {
            gemBag.addGems(gemAmmount);
            Destroy(transform.parent.gameObject);
        }
    }

    IEnumerator startTrigger (float time)
    {
        yield return new WaitForSeconds(time);
        gemTrigger.enabled = true;
    }
}
