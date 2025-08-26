using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [Range(0,10),SerializeField]private int initialHealth;
    
    public int CurrentHealth { get; private set; }
    public int CurrentCount { get; private set; }
    private int rewardMultiply;
    
    public bool IsPlaying { get; protected set; }
    public bool StopTurn { get; protected set; }

    public Action OnStartTurn, OnFinishTurn, OnStartRound, OnFinishRound;

    private void Awake()
    {
        CurrentHealth = initialHealth;

        OnStartTurn += () => IsPlaying = true;

        OnFinishTurn += () => IsPlaying = false;
        OnStartRound += () =>
        {
            StopTurn = false;
            CurrentCount = 0;
        };
        OnFinishRound += () => StopTurn = true;
    }
    private void Start()
    {
        OnStartRound?.Invoke();
        _Start();
    }
    protected virtual void _Start()
    {

    }

    protected virtual void GetCard()
    {
        if(GameManager.instance==null)
            return;

        CurrentCount += GameManager.instance.Deck.GetCard();
        Debug.Log($"{gameObject.name} hit, Value {CurrentCount}");

        GameManager.instance?.PassTurn();

        if (CurrentCount > GameManager.instance.BustValue)
            TakeDamage(Mathf.Clamp(rewardMultiply,1,10));
    }

    protected virtual void Stand()
    {
        Debug.Log($"{gameObject.name} stand, Value {CurrentCount}");
        OnFinishRound?.Invoke();
        GameManager.instance?.PassTurn();
    }
    protected virtual void DoubleDown(){

        Debug.Log($"{gameObject.name} double down, Value {CurrentCount}");
        rewardMultiply++;
	    GetCard();
	    if (CurrentCount < GameManager.instance.BustValue)
	    	Stand();

    }

    public virtual void TakeDamage(int damage = -1, bool exeFinishRound = true)
    {
        CurrentHealth-= damage!=-1?
            damage:
            1+rewardMultiply;

        if(exeFinishRound)
            OnFinishRound?.Invoke();

        Debug.Log($"{gameObject.name} take damage, currentHealth {CurrentHealth}");

        if (CurrentHealth <= 0)
            Dead();
    }

    protected virtual void Dead()
    {
        Debug.Log($"<color=red>{gameObject.name} dead</color>");
        
    }
}
