using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float _timeToOpenDoor = 0.3f;
    GameObject door;
    bool isOpen = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Gem") && !isOpen)
        {
            StartCoroutine(openDoor());
        }
    }

    IEnumerator openDoor()
    {
        yield return new WaitForSeconds(_timeToOpenDoor);

        Color color = door.GetComponent<SpriteRenderer>().color;
        color.a = 0.5f;
        door.GetComponent<SpriteRenderer>().color = color;
        door.GetComponent<Collider2D>().enabled = false;
        isOpen = true;
    }
}
