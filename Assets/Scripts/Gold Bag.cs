using UnityEngine;
using UnityEngine.UI;

public class GoldBag : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Vector3 Offset;
    [SerializeField] int maxGold = 5;
    int currentGold;

    void Awake()
    {
        currentGold = maxGold;
        slider.maxValue = maxGold;
        slider.value = currentGold;
    }

    public void SetGold(int gold)
    {
        if(gold <= maxGold && gold >= 0)
        {
            currentGold = gold;
            slider.value = currentGold;
        }
        else if(gold > maxGold)
        {
            currentGold = maxGold;
            slider.value = currentGold;
        }
        else if (gold < 0)
        {
            currentGold = 0;
            slider.value = currentGold;
        }
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
}
