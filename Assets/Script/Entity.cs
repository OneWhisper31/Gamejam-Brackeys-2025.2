using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [HideInInspector] public int userIndex;  
    [Range(0,10),SerializeField]private int initialHealth;
    public int CurrentHealth { get; private set; }
    public int CardsDealt { get; private set; }
    private int rewardMultiply;
    
    public bool IsPlaying { get; protected set; }
    
    public bool HasAnAs { get; protected set; }
    public bool HasTwoAs { get; protected set; }
    public bool StopTurn { get; protected set; }

    [SerializeField] private Hand hand;
    
    public int CurrentCount
    {
        get
        {
            if (HasAnAs && _currentCount <= 11)//as mechanic
                return _currentCount+10;
            return _currentCount;
        } 
        private set=>_currentCount = value;
    } //valueCounter
    private int _currentCount;

    public Action OnStartTurn, OnFinishTurn, OnStartRound, OnFinishRound;

    private void Awake()
    {
        CurrentHealth = initialHealth;

        OnStartTurn += () => IsPlaying = true;

        OnFinishTurn += () => IsPlaying = false;
        OnStartRound += () =>
        {
            CurrentCount = 0;
            CardsDealt = 0;
            StopTurn = false;
            IsPlaying = false;
            HasAnAs = false;
            HasTwoAs = false;
            GetCard(canGetJoker:false);
            GetCard(canGetJoker:false);
        };
        OnFinishRound += () =>
        {
            StopTurn = true;
        };
    }
    private void Start()
    {
        OnStartRound?.Invoke();
        _Start();
    }
    protected virtual void _Start()
    {

    }

    protected virtual void GetCard(bool canGetJoker = true)
    {
        if(GameManager.instance==null)
            return;
        
        var nextCard = GameManager.instance.Deck.GetCard(userIndex,canGetJoker);
        Debug.Log($"Cart Dealt {nextCard}");

        if (nextCard == 0)
        {
            Debug.Log("Hability");
            return;
        }
        CurrentCount += nextCard;
        CardsDealt++;
        
        /*As Mechanic*/
        if (nextCard == 1)
        {
            if(!HasAnAs)
                HasAnAs = true;
            else
                HasTwoAs = true;
        }
            
        if(HasAnAs && !HasTwoAs && CurrentCount-10 <= 9)
            GameManager.instance.UpdateCounter($"{CurrentCount-10 }/{CurrentCount/*As mechanic*/}",userIndex);
        else
            GameManager.instance.UpdateCounter($"{CurrentCount}",userIndex);
        
        if(CurrentCount == GameManager.instance.BustValue)
            Stand();
        else if (CurrentCount > GameManager.instance.BustValue)
        {
            Debug.Log("Bust");
            Stand();//TakeDamage(Mathf.Clamp(rewardMultiply,1,10));
        }
        else
            GameManager.instance?.PassTurn();

    }

    protected virtual void Stand()
    {
        Debug.Log($"{gameObject.name} stand, Value {CurrentCount}");
        OnFinishRound?.Invoke();
        GameManager.instance?.PassTurn();
    }
    protected virtual void DoubleDown(){

        if(CardsDealt>3)
            return;
        
        Debug.Log($"{gameObject.name} double down, Value {CurrentCount}");
        rewardMultiply++;
	    GetCard();
	    Stand();

    }

    public virtual void TakeDamage(int damage = -1, bool exeFinishRound = true)
    {
        CurrentHealth-= damage!=-1?
            damage:
            1+rewardMultiply;

        Debug.Log($"<color=blue>{gameObject.name} take damage, currentHealth {CurrentHealth} dead</color>");
        hand?.RemoveFingers();

        if (CurrentHealth <= 0)
            Dead();
        else if(exeFinishRound)
            OnFinishRound?.Invoke();
        
    }

    protected virtual void Dead()
    {
        Debug.Log($"<color=red>{gameObject.name} dead</color>");
        
    }
}
