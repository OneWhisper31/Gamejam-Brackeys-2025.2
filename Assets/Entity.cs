using UnityEngine;

public class Entity : MonoBehaviour
{
    [Range(0,10),SerializeField]private int initialHealth;
    
    public int CurrentHealth { get; private set; }
    
    private int currentCount,rewardMultiply;

    private void Start()
    {
        CurrentHealth = initialHealth;
    }

    protected virtual void GetCard()
    {
        if(GameManager.instance==null)
            return;
        
        currentCount += GameManager.instance.Deck.GetCard();

        if (GameManager.instance.BustValue >= currentCount)
            TakeDamage(Mathf.Clamp(rewardMultiply,1,10));
    }

    protected virtual void Stand()
    {
        
    }

    protected virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
            Dead();
    }

    protected virtual void Dead()
    {
        Debug.Log($"<color=red>{gameObject.name} dead</color>");
    }
}
