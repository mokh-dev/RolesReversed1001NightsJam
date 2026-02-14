using UnityEngine;
using UnityEngine.UI;

public class GoldBag : MonoBehaviour
{
    Slider slider;
    [SerializeField] Vector3 Offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
}
