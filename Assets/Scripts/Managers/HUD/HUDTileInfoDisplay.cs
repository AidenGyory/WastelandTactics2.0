using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDTileInfoDisplay : MonoBehaviour
{
    [Header("Tile Components")]
    [SerializeField] TMP_Text tileNameText;
    [SerializeField] TMP_Text tileDescriptionText;

    [Header("Resource Components")]
    [SerializeField] GameObject resourceInfo; 
    [SerializeField] Image resourceIcon;
    [SerializeField] TMP_Text resourceAmount; 

    public void TileInfoUpdate(TileInfo _tile)
    {
        tileNameText.text = "" + _tile.tileName;
        tileDescriptionText.text = "" + _tile.tileDescription;  

        if(_tile.isResourceTile)
        {
            resourceInfo.SetActive(true);
            resourceIcon.sprite = _tile.resourceIcon;
            resourceAmount.text = "" + _tile.resourceAmountLeft; 
        }
        else
        {
            resourceInfo.SetActive(false); 
        }
    }

}
