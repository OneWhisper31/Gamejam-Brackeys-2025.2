using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deckLeft;
    [SerializeField] private Image cardPrefab;
    [SerializeField] private Transform cardHolder;
    [SerializeField] private Transform[] usersCardHolder;
    [SerializeField] private TextMeshProUGUI[] usersCount;
    [SerializeField] private int maxCardHolder;
    
    [Header("Cards Sprites")]
    [SerializeField] private Sprite[] cardHSprites;
    [SerializeField] private Sprite[] cardDSprites;
    [SerializeField] private Sprite[] cardSSprites;
    [SerializeField] private Sprite[] cardCSprites;

    private void Start()
    {
        StartCoroutine(DeckGarbageCollector());
    }

    public void UpdateDeck(int text, (int,int) card, int userIndex)
    {
        deckLeft?.SetText(text.ToString());
        
        if(card.Item1 == 0)
            return;
        
        var cardSprite = Instantiate(cardPrefab, cardHolder);
        
        Image userCardSprite;
        if (usersCardHolder.Length > userIndex)
            userCardSprite = Instantiate(cardPrefab, usersCardHolder[userIndex]);
        else
            userCardSprite = cardSprite;
        
        var cardS =cardHSprites[card.Item1-1];
        
        switch (card.Item2)
        {
            case 0:
                cardSprite.sprite = cardS;
                userCardSprite.sprite = cardS;
                break;
            case 1:
                cardSprite.sprite = cardS;
                userCardSprite.sprite = cardS;
                break;
            case 2:
                cardSprite.sprite = cardS;
                userCardSprite.sprite = cardS;
                break;
            case 3:
                cardSprite.sprite = cardS;
                userCardSprite.sprite = cardS;
                break;
            default:
                break;
        }
    }

    public void UpdateCounter(string usersCounter,int userIndex)
    {
        if(usersCount.Length > userIndex)
            usersCount[userIndex]?.SetText(usersCounter);
    }
    
    public void ClearDeck(int userIndex)
    {
        for (int i = usersCardHolder[userIndex].childCount - 1; i >= 0; i--)
            Destroy(usersCardHolder[userIndex].GetChild(i).gameObject);
    }
    IEnumerator DeckGarbageCollector()
    {
        while (true)
        {
            if(cardHolder.childCount > maxCardHolder)
                Destroy(cardHolder.GetChild(0).gameObject);
            
            yield return new WaitForSeconds(0.01f);
        }
    }
}
