using Unity.VisualScripting;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    private EnemyBehavior enemyParent;

    void Awake()
    {
        enemyParent = transform.parent.gameObject.GetComponent<EnemyBehavior>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        enemyParent.OnVisionColliderEnter(collision);
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        enemyParent.OnVisionColliderStay(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        enemyParent.OnVisionColliderExit(collision);
    }
}
