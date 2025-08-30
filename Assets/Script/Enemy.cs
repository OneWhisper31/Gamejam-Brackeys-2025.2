using System.Collections;
using UnityEngine;
using System.Linq;

public class Enemy : Entity
{
	protected override void _Start()
	{
		StartCoroutine(_Update());
	}
    IEnumerator _Update()
    {
	    while (true)
	    {
		    if (IsPlaying)
		    {
			    yield return new WaitForSeconds(1f);

			    if (GameManager.instance == null)
			    {
				    Debug.LogWarning(name+" - Game Manager is null!");
				    yield break;
			    }

			    var enemy = GameManager.instance.Players.Where(x => x != this)
				    .OrderByDescending(x=>x.CurrentCount).FirstOrDefault();
			    
			    if(enemy !=null && enemy.CurrentCount > CurrentCount && enemy.CurrentCount < GameManager.instance.BustValue)
				    GetCard();
			    else if (CurrentCount >= 19)
				    Stand();
			    else
			    {
				    float bustProbability = CalculateBustProbability();
				    int random = Random.Range(0, 100);
				    
				    if (random > bustProbability)
					    GetCard();
				    else
					    Stand();
			    }
		    }
		    yield return new WaitForEndOfFrame();
	    }
    }

    float CalculateBustProbability()
    {
	    if (GameManager.instance == null)
		    return -1;
	    
	    var currentShuffle = GameManager.instance.Deck.UnshuffledDeck.Except(GameManager.instance.Deck.DeckPlayed).ToArray();

	    string currentShuffleString = "";
	    foreach (var x in currentShuffle)
	    {
		    currentShuffleString += x.ToString();
	    }
	    Debug.Log(currentShuffle.Length + currentShuffleString);
	    
	    float bustCount = 0;
	    foreach (var x in currentShuffle)
	    {
		    if(x.Item1+CurrentCount > GameManager.instance.BustValue)
			    bustCount++;
	    }
	    return bustCount/currentShuffle.Length*100;
    }
}
