using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private float _gemFollowSpeed;
    [SerializeField] private float _gemIdleRange;
    [SerializeField] private Color _heldGemColor;
    [SerializeField] private Color _releasedGemColor;
    [SerializeField] private LayerMask _baseCollisionLayers;
    [SerializeField] private LayerMask _throwCollisionLayers;

    private bool isHeld;
    private GameObject objToFollow;

    private bool isWithinIdleRange;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;

    
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        col = gameObject.GetComponent<Collider2D>();
        col.excludeLayers = _baseCollisionLayers;
    }


    public void HoldGem(GameObject player)
    {
        isHeld = true;
        objToFollow = player;

        sr.color = _heldGemColor;
    }

    public void ReleaseGem()
    {
        isHeld = false;
        objToFollow = null;

        sr.color = _releasedGemColor;
        
        StartCoroutine(CollisionLayersTimer());
    }

    IEnumerator CollisionLayersTimer()
    {
        col.excludeLayers = _throwCollisionLayers;
        yield return new WaitForSeconds(0.5f);
        col.excludeLayers = _baseCollisionLayers;
    }
    
    void Update()
    {
        if (isHeld) isWithinIdleRange = Vector2.Distance(objToFollow.transform.position, transform.position) <= _gemIdleRange;

        if (isHeld && !isWithinIdleRange)
        {
            Vector2 followDirection = (objToFollow.transform.position - transform.position).normalized;

            rb.AddForce(followDirection * _gemFollowSpeed);
        }
    }
}
