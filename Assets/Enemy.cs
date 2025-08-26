using UnityEngine;

public class Enemy : Entity
{
    

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying && Random.Range(0, 100) <= 50)
            GetCard();
        else
            Stand();
            
    }
}
