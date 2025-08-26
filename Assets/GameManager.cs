using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [field:SerializeField] public int BustValue { get; private set; } = 21;
    public int CurrentPlayerPlaying { get; private set; } = 21;

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
        CurrentPlayerPlaying = Random.Range(0, players.Length);
        PassTurn(CurrentPlayerPlaying,true,false);
    }

    public void PassTurn(int i = -1, bool exeStartTurn = true, bool exeFinishTurn = true)
    {
        if(exeFinishTurn)
            players[CurrentPlayerPlaying].OnFinishTurn?.Invoke();

        if (i == -1)
            CurrentPlayerPlaying++;
        else
            CurrentPlayerPlaying = i;

        if (CurrentPlayerPlaying >= players.Length)
            CurrentPlayerPlaying = 0;

        if(exeStartTurn)
            players[CurrentPlayerPlaying].OnStartTurn?.Invoke();
    }
}
