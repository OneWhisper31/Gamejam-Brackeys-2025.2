using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [field:SerializeField] public int BustValue { get; private set; } = 21;
    public int CurrentPlayerPlaying { get; private set; } = 21;

    [field: SerializeField] public int NumberOfDecks { get; private set; } = 3;
    [field: SerializeField] public int MaxNumberOfCards { get; private set; } = 12;
    [field:SerializeField] public int NumberOfPowerUps { get; private set; } = 5;
 
    [field:SerializeField] public Entity[] Players { get; private set; }
    
    [SerializeField] UIHandler uiHandler;
    
    public Deck Deck { get; private set; }
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        

        for (int i = 0; i < Players.Length; i++)
        {
            int userIndex = i;
            //set number of player
            Players[i].userIndex = userIndex;
            Players[i].OnStartRound += ()=>
                uiHandler.ClearDeck(userIndex);
        }
        
        Deck = new Deck(NumberOfDecks, MaxNumberOfCards, NumberOfPowerUps, uiHandler);
    }
    private void Start()
    {
        StartTurn();
    }
    public void StartTurn()
    {
        CurrentPlayerPlaying = Random.Range(0, Players.Length);
        Debug.Log("Start Turn");
        PassTurn(CurrentPlayerPlaying);
    }
    public void PassTurn(int i = -1, bool exeStartTurn = true, bool exeFinishTurn = true)
    {
        if(exeFinishTurn)
            Players[CurrentPlayerPlaying].OnFinishTurn?.Invoke();



        if (!IsEndOfRound())
        {
            if (i == -1)
            {
                do
                {
                    CurrentPlayerPlaying++;

                    if (CurrentPlayerPlaying >= Players.Length)
                        CurrentPlayerPlaying = 0;

                } while (Players[CurrentPlayerPlaying].StopTurn);
            }
            else
                CurrentPlayerPlaying = i;

            

            if (exeStartTurn)
                Players[CurrentPlayerPlaying].OnStartTurn?.Invoke();
        }

        
    }
    public bool IsEndOfRound()
    {
        if(Players.All(x => x.StopTurn))
        {
            Debug.Log("Stop turn");

            var damagedPlayers = Players
                .Where(x=> // if busted take damage
                {
                    bool busted = x.CurrentCount > BustValue;
                    
                    if(busted)
                        x.TakeDamage();
                    
                    return !busted;
                })
                .OrderBy(x => x.CurrentCount).ToArray();
            if (damagedPlayers.Length > 1)
            {
                damagedPlayers = damagedPlayers.Take(damagedPlayers.Length - 1).ToArray();//damage the players but the winner

                if(damagedPlayers.Length>0)
                    foreach (var x in damagedPlayers)
                        x.TakeDamage();
            }
            
            foreach (var x in Players)
                x.OnStartRound?.Invoke();
            StartTurn();

            return true;
        };
        return false;
    }

    public void UpdateCounter(string usersCounter, int userIndex)
        => uiHandler.UpdateCounter(usersCounter,userIndex);
}
