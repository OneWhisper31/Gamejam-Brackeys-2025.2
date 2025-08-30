using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Deck
{
    private UIHandler uiHandler;
    
    int numberOfDecks, maxNumberOfCards, numberOfPowerUps;
    Queue<(int,int)> deck = new Queue<(int,int)>();//type of number, type of pica
    public List<(int, int)> DeckPlayed { get; private set; } = new List<(int, int)>();
    
    public List<(int, int)> UnshuffledDeck{ get; private set; } = new List<(int, int)>();

    public Deck(int _numberOfDecks, int _maxNumberOfCards, int _numberOfPowerUps,UIHandler _uiHandler)
    {
        numberOfDecks = _numberOfDecks;
        maxNumberOfCards = _maxNumberOfCards;
        numberOfPowerUps = _numberOfPowerUps;
        uiHandler = _uiHandler;
    }
    
    public int GetCard(int userIndex, bool canGetJoker = true)
    {
        if (deck.Count <= 0)
            ShuffleDeck();
        var card = deck.Dequeue();
        DeckPlayed.Add(card);

        if (!canGetJoker)
        {
            var jokers = new List<(int,int)>();
            
            while (card.Item1 == 0)
            {
                jokers.Add(card);
                card = deck.Dequeue();
                DeckPlayed.Add(card);
            }

            foreach (var item in jokers)
            {
                DeckPlayed.Remove(item);
                deck.Enqueue(item);
            }
        }
        
        uiHandler.UpdateDeck(deck.Count, card,userIndex);
        
        return Mathf.Clamp(card.Item1,0,10);
    }

    void ShuffleDeck()
    {
        if (UnshuffledDeck.Count <= 0)
            CreateUnshuffleDeck();
        
        deck = ShuffleCards(new List<(int,int)>(UnshuffledDeck));
    }

    void CreateUnshuffleDeck()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < numberOfDecks * maxNumberOfCards; i++)
            {
                int iParsed = i - i / maxNumberOfCards * maxNumberOfCards+1;
                UnshuffledDeck.Add((iParsed,j));
            }
        }
        
        for (int i = 0; i < numberOfPowerUps; i++)
            UnshuffledDeck.Add((0,-1));
    }
    Queue<(int,int)> ShuffleCards(List<(int,int)> cards)
    {
        var cardsTemp = new List<(int,int)>(cards);
        var output = new Queue<(int,int)>();
        
        while (cardsTemp.Count > 0)
        {
            var nChoose = Random.Range(0, cardsTemp.Count);
            output.Enqueue(cards[nChoose]);
            cardsTemp.RemoveAt(nChoose);
        }
        
        return output;
    }
}