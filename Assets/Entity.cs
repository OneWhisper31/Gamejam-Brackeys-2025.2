using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [Range(0,10),SerializeField]private int initialHealth;
    
    public int CurrentHealth { get; private set; }
    private int currentCount,rewardMultiply;
    
    public bool IsPlaying { get; protected set; }

    public Action OnStartTurn, OnFinishTurn;


    private void Start()
    {
        CurrentHealth = initialHealth;

        OnStartTurn += ()=> IsPlaying = true;
        OnFinishTurn += () => IsPlaying = false;
    }

    protected virtual void GetCard()
    {
        if(GameManager.instance==null)
            return;
        
        currentCount = Mathf.Clamp(currentCount+GameManager.instance.Deck.GetCard(),0,GameManager.instance.BustValue);

        if (GameManager.instance.BustValue >= currentCount)
            TakeDamage(Mathf.Clamp(rewardMultiply,1,10));
    }

    protected virtual void Stand()
    {
        GameManager.instance?.PassTurn();
    }

    protected virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        GameManager.instance?.PassTurn();

        if (CurrentHealth <= 0)
            Dead();
    }

    protected virtual void Dead()
    {
        Debug.Log($"<color=red>{gameObject.name} dead</color>");
    }
}
