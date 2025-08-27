using System.Collections;
using UnityEngine;

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
			    
			    if(Random.Range(0, 100) <= 50)
				    GetCard();
			    else
				    Stand(); 
		    }
		    yield return new WaitForEndOfFrame();
	    }
    }
}
