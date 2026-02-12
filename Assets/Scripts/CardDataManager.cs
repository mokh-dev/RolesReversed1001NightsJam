using Unity.VisualScripting;
using UnityEngine;

public class CardDataManager : MonoBehaviour
{

    private static CardDataManager instance;
    public static CardDataManager Instance { get { return instance; } }




    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            instance = this;
        }
    }




    void Update()
    {
        
    }


}


