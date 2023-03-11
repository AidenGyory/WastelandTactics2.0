using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTileScript : MonoBehaviour
{
    [SerializeField] GameObject scanLayer;

    public bool scanned; 

    public void TurnOnScanLayer()
    {
        
        scanned = true; 
        scanLayer.SetActive(true);
        Color _color = Color.white; 
        scanLayer.GetComponent<Renderer>().material.color = new Color(_color.r, _color.g, _color.b, 0.4f);   
    }

    public void TurnOffScanLayer()
    {
        scanned = false; 
        scanLayer.SetActive(false);

    }
}
