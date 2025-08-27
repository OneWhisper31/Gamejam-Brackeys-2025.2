using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Deck
{
    int numberOfDecks, maxNumberOfCards;
    Queue<int> deck = new Queue<int>();

    public Deck(int _numberOfDecks, int _maxNumberOfCards)
    {
        numberOfDecks = _numberOfDecks;
        maxNumberOfCards = _maxNumberOfCards;
    }
    
    public int GetCard()
    {
        if (deck.Count <= 0)
            ShuffleDeck();
        return deck.Dequeue();
    }

    void ShuffleDeck()
    {
        List<int> deckUnshuffled = new List<int>();
        for (int i = 0; i < numberOfDecks * maxNumberOfCards; i++)
        {
            int iParsed = Mathf.Clamp(i - (i / maxNumberOfCards * maxNumberOfCards) + 1,1,10);
            deckUnshuffled.Add(iParsed);
        }
        deck = ShuffleCards(deckUnshuffled);
    }
    Queue<int> ShuffleCards(List<int> cards)
    {
        var output = new Queue<int>();
        
        while (cards.Count > 0)
        {
            var nChoose = Random.Range(0, cards.Count);
            output.Enqueue(cards[nChoose]);
            cards.RemoveAt(nChoose);
        }
        
        return output;
    }
}