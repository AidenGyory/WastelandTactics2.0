using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellInfo : MonoBehaviour
{
    public enum PowerState
    {
        none,
        collected,
        used,
        inDeficit,
        notCollected,
    }

    [SerializeField] Color none; 
    [SerializeField] Color collected; 
    [SerializeField] Color used;
    [SerializeField] Color inDeficit;
    [SerializeField] Color inDeficitNotCollected;

    public void ChangeColor(PowerState _state)
    {
        switch (_state)
        {
            case PowerState.none:
                GetComponent<Image>().color = none; 
                break;
            case PowerState.collected:
                GetComponent<Image>().color = collected;
                break;
            case PowerState.used:
                GetComponent<Image>().color = used;
                break;
            case PowerState.inDeficit:
                GetComponent<Image>().color = inDeficit;

                break;
            case PowerState.notCollected:
                GetComponent<Image>().color = inDeficitNotCollected;

                break;
        }
    }
}
