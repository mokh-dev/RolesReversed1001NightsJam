using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float _timeToOpenDoor = 0.3f;
    [SerializeField] OpenMecahnism openMecahnism; 
    [SerializeField] ConsumeItem consumeItem;
    GameObject door;
    bool isOpen = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door = transform.parent.gameObject;

        if(openMecahnism == OpenMecahnism.Gem)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.lightSkyBlue;
        }
        else if(openMecahnism == OpenMecahnism.Coin)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if(openMecahnism == OpenMecahnism.Both)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.orangeRed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(openMecahnism != OpenMecahnism.Both)
        {
            if(collision.collider.CompareTag(openMecahnism.ToString()) && !isOpen)
            {
                StartCoroutine(openDoor(collision.gameObject));
            }
        }
        else
        {
            if((collision.collider.CompareTag("Coin") || collision.collider.CompareTag("Gem"))&& !isOpen)
            {
                StartCoroutine(openDoor(collision.gameObject));
            }
        }
    }

    IEnumerator openDoor(GameObject item)
    {
        if(consumeItem == ConsumeItem.Yes)
        {
            Destroy(item);
        }
        
        yield return new WaitForSeconds(_timeToOpenDoor);

        Color color = door.GetComponent<SpriteRenderer>().color;
        color.a = 0.5f;
        door.GetComponent<SpriteRenderer>().color = color;
        door.GetComponent<Collider2D>().enabled = false;
        isOpen = true;
    }

    enum OpenMecahnism
    {
        Gem,
        Coin,
        Both
    }

    enum ConsumeItem
    {
        Yes,
        No
    }
}
