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

        foreach (var x in players)
        {
            x.OnFinishRound += () => IsEndOfRound();
        }
    }
    private void Start()
    {
        StartTurn();
    }
    public void StartTurn()
    {
        CurrentPlayerPlaying = Random.Range(0, players.Length);
        Debug.Log("Start Turn");
        PassTurn(CurrentPlayerPlaying);
    }
    public void PassTurn(int i = -1, bool exeStartTurn = true, bool exeFinishTurn = true)
    {
        if(exeFinishTurn)
            players[CurrentPlayerPlaying].OnFinishTurn?.Invoke();



        if (!IsEndOfRound())
        {
            if (i == -1)
            {
                do
                {
                    CurrentPlayerPlaying++;

                    if (CurrentPlayerPlaying >= players.Length)
                        CurrentPlayerPlaying = 0;

                } while (players[CurrentPlayerPlaying].StopTurn);
            }
            else
                CurrentPlayerPlaying = i;

            

            if (exeStartTurn)
                players[CurrentPlayerPlaying].OnStartTurn?.Invoke();
        }

        
    }
    public bool IsEndOfRound()
    {
        if(players.All(x => x.StopTurn))
        {
            Debug.Log("Stop turn");

            var damagedPlayers = players.Where(x=>x.CurrentCount<=BustValue).OrderBy(x => x.CurrentCount).ToArray();
            damagedPlayers = damagedPlayers.Take(damagedPlayers.Count() - 1).ToArray();//damage the players minus the winner

            if(damagedPlayers.Length>0)
                foreach (var x in damagedPlayers)
                    x.TakeDamage();

            foreach (var x in players)
                x.OnStartRound?.Invoke();
            StartTurn();

            return true;
        };
        return false;
    }
}
