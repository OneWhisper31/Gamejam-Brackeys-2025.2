using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    [SerializeField]Button getCardB, standB, doubleDownB;

    protected override void _Start()
    {
        OnStartTurn += () =>
        {
            getCardB.interactable = true;
            standB.interactable = true;
            doubleDownB.interactable = true;
        };
        OnFinishTurn += () =>
        {
            getCardB.interactable = false;
            standB.interactable = false;
            doubleDownB.interactable = false;
        };
    }

    public void GetCardB()
        => GetCard();
    public void StandB()
        => Stand();
    public void DoubleDownB()
        => DoubleDown();
}
