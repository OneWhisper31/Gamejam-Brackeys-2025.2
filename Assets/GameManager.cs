using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field:SerializeField] public int BustValue { get; private set; } = 21;
    
    [SerializeField] int numberOfDecks = 3, maxNumberOfCards = 12;
    
    [SerializeField] Entity[] players;
    
    public Deck Deck { get; private set; }
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        Deck = new Deck(numberOfDecks, maxNumberOfCards);
    }

    private void Start()
    {
        
    }

    IEnumerator GameLoop()
    {
        while (players.Where(x=>x.CurrentHealth<=0).Count()<=1)
        {
            int turn = UnityEngine.Random.Range(0, players.Length);
            WaitUntil(()=>)
        }
    }
}
