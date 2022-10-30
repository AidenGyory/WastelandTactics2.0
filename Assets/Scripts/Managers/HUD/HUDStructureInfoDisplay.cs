using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDStructureInfoDisplay : MonoBehaviour
{
    [Header("Structure Components")]
    [SerializeField] TMP_Text unitName;
    [SerializeField] Image healthBarFill;
    [SerializeField] TMP_Text healthBarAmount;
    [Space]
    [SerializeField] Image factionIcon;

    public void StructureDisplayUpdate(StructureInfo _structure)
    {
        unitName.text = _structure.structureName;
        healthBarFill.fillAmount = _structure.currentHealth / _structure.maxHealth; 
        healthBarAmount.text = "" + _structure.currentHealth + "/" + _structure.maxHealth;

        factionIcon.sprite = _structure.factionOwner.factionFlag;
    }
}
