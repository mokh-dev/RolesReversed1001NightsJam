using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] int maxGold = 5;
    [SerializeField] float _gemThrowForce;
    [SerializeField] float _coinThrowForce;
    [SerializeField] GameObject _coinToSummon;

    Vector2 worldPos;
    GameObject thrownCoin;
    int currentGold;

    Camera cam;

    List<Gem> heldGems = new List<Gem>();

    void Awake()
    {
        currentGold = maxGold;
        slider.maxValue = maxGold;
        slider.value = currentGold;

        cam = Camera.main;
    }

    public void SetGold(int gold)
    {
        if(gold <= maxGold && gold >= 0)
        {
            currentGold = gold;
        }
        else if(gold > maxGold)
        {
            currentGold = maxGold;
        }
        else if (gold < 0)
        {
            currentGold = 0;
        }
        
        slider.value = currentGold;
        slider.gameObject.SetActive(gold < maxGold);
    }

    public void addGold(int gold)
    {
        SetGold(currentGold + gold);
    }

    public void removeGold(int gold)
    {
        SetGold(currentGold - gold);
    }

    public int getCurrentGold()
    {
        return currentGold;
    }

    public int getMaxGold()
    {
        return maxGold;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gem"))
        {
            if (heldGems.Contains(collision.gameObject.GetComponent<Gem>())) return;
            
            collision.gameObject.GetComponent<Gem>().HoldGem(this.gameObject);
            heldGems.Add(collision.gameObject.GetComponent<Gem>());
        }
    }

    void OnThrowGem(InputValue input)
    {
        if (heldGems.Count <= 0) return;
      

        Vector2 throwDirection = (cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - heldGems[0].transform.position).normalized;

        heldGems[0].ReleaseGem();
        heldGems[0].GetComponent<Rigidbody2D>().AddForce(throwDirection * _gemThrowForce, ForceMode2D.Impulse);

        heldGems.RemoveAt(0);
    }

    void OnThrowCoin(InputValue input)
    {
        if (currentGold <= 0) return;

        thrownCoin = Instantiate(_coinToSummon, gameObject.transform.position, quaternion.identity);
        thrownCoin.GetComponent<Rigidbody2D>().linearVelocity = gameObject.GetComponent<Rigidbody2D>().linearVelocity;
        Vector2 throwDirection = (cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - thrownCoin.transform.position).normalized;
        thrownCoin.GetComponent<Rigidbody2D>().AddForce(throwDirection * _coinThrowForce, ForceMode2D.Impulse);
        removeGold(1);
    }
}
