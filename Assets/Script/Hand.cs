using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] Image[] fingers;

    public void RemoveFingers()
    {
        var finger = fingers.Where(x => x.enabled).FirstOrDefault();

        if (finger != null)
            finger.enabled = false;
        else
            Debug.LogWarning("Finger not found");
    }
}
