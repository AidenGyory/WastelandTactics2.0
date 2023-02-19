using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellInfo : MonoBehaviour
{
    public enum PowerState
    {
        unused,
        used,
        deficit,
    }

    [SerializeField] Color unused; 
    [SerializeField] Color used; 
    [SerializeField] Color defict;

    public void ChangeColor(PowerState _state)
    {
        switch (_state)
        {
            case PowerState.unused:
                GetComponent<Image>().color = unused; 
                break;
            case PowerState.used:
                GetComponent<Image>().color = used;

                break;
            case PowerState.deficit:
                GetComponent<Image>().color = defict;

                break;
            default:
                break;
        }
    }
}
