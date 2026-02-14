using UnityEngine;

public class GoldAddTEST : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<GoldBag>().addGold(1);
        }
    }
}
