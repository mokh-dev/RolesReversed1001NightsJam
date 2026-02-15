using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private float _gemFollowSpeed;
    [SerializeField] private float _gemIdleRange;
    [SerializeField] private Color _heldGemColor;
    [SerializeField] private Color _releasedGemColor;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _nothingLayer;

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
        col.excludeLayers = _nothingLayer;
        col.includeLayers = _playerLayer;
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
        col.includeLayers = _nothingLayer;
        col.excludeLayers = _playerLayer;
        yield return new WaitForSeconds(0.5f);
        col.includeLayers = _playerLayer;
        col.excludeLayers = _nothingLayer;
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
