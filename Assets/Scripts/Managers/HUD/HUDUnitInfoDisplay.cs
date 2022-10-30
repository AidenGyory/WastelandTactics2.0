using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUnitInfoDisplay : MonoBehaviour
{
    [Header("Unit Components")]
    [SerializeField] TMP_Text unitName;
    [SerializeField] Image healthBarFill;
    [SerializeField] TMP_Text healthBarAmount;
    [Header("Unit Stats")]
    [SerializeField] TMP_Text attackAmount; 
    [SerializeField] TMP_Text defenseAmount;
    [SerializeField] TMP_Text rangeAmount;
    [SerializeField] TMP_Text movementAmount;
    [Header("Player Components")]
    [SerializeField] Image factionIcon; 

    public void UnitDisplayUpdate(UnitInfo _unit)
    {
        unitName.text = "" + _unit.unitName;

        healthBarFill.fillAmount = _unit.currentHealth / _unit.maxHealth;
        healthBarAmount.text = "" + _unit.currentHealth + "/" + _unit.maxHealth;

        attackAmount.text = "" + _unit.attack;
        defenseAmount.text = "" + _unit.defense;
        rangeAmount.text = "" + _unit.range;
        movementAmount.text = "" + _unit.currentMovement + "/" + _unit.maxMovement;

        factionIcon.sprite = _unit.factionOwner.factionFlag; 
    }

}
