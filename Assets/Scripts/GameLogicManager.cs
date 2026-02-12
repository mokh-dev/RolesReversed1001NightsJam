using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{

    private static GameLogicManager instance;
    public static GameLogicManager Instance { get { return instance; } }

    public List<CardScriptableObject> Deck = new List<CardScriptableObject>();

    public Player TestPlayer1;

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


    void Start()
    {
        CardScriptableObject randomCard = Deck[Random.Range(0, Deck.Count)];

        Deck.Remove(randomCard);
        TestPlayer1.PlayerHand.Add(randomCard);
    }

    void Update()
    {
        
    }
}
