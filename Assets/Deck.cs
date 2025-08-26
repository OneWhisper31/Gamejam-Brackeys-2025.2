using UnityEngine;
using System.Collections.Generic;

public class Deck
{
    int numberOfDecks, maxNumberOfCards;
    Queue<int> deck;

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
            int iParsed = i - (i / 12 * 12) + 1;
            deckUnshuffled.Add(iParsed);
        }
        foreach (var item in deckUnshuffled)
        {
            Debug.Log(item);
        }
        deck = ShuffleCards(deckUnshuffled);
    }
    Queue<int> ShuffleCards(List<int> cards)
    {
        var output = new Queue<int>();
        int cardsL = cards.Count;
        for (int i = cardsL; i > 0; i--)
        {
            var nChoose = Random.Range(0, i);
            output.Enqueue(cards[nChoose]);
            cards.RemoveAt(nChoose);
        }
        return output;
    }
}