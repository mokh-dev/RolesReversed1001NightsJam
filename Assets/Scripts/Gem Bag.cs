using UnityEngine;

public class GemBag : MonoBehaviour
{
    [SerializeField] GameObject[] gemIcons;
    [SerializeField] GameObject gemContainer;
    [SerializeField] int maxGem = 3;
    int currentGems = 0;

    void Awake()
    {
        maxGem = gemIcons.Length;
    }

    public void SetGems(int gems)
    {
        if(gems <= maxGem && gems >= 0)
        {
            currentGems = gems;
        }
        else if(gems > maxGem)
        {
            currentGems = maxGem;
        }
        else if (gems < 0)
        {
            currentGems = 0;
        }
        for (int i = 0; i < gemIcons.Length; i++)
        {
            if (i < currentGems)
            {
                gemIcons[i].SetActive(true);
            }
            else
            {
                gemIcons[i].SetActive(false);
            }
        }
        gemContainer.SetActive(currentGems > 0);
    }

    public void addGems(int gems)
    {
        SetGems(currentGems + gems);
    }

    public void removeGems(int gems)
    {
        SetGems(currentGems - gems);
    }

    public int getCurrentGems()
    {
        return currentGems;
    }

    public int getMaxGems()
    {
        return maxGem;
    }
}
