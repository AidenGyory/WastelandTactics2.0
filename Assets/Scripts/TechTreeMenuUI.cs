using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TechTreeMenuUI : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] PlayerInfo _currentPlayer; 
    private int track1Index; 
    private int track2Index;
    private int track3Index;

    [Header("Ui Components")]
    [SerializeField] TMP_Text researchPointsTotal;
    [Space]
    [SerializeField] TMP_Text option1_UpgradeName; 
    [SerializeField] TMP_Text option1_UpgradeDescription;
    [SerializeField] TMP_Text option1_UpgradeCost;
    [Space]
    [SerializeField] TMP_Text option2_UpgradeName;
    [SerializeField] TMP_Text option2_UpgradeDescription;
    [SerializeField] TMP_Text option2_UpgradeCost;
    [Space]
    [SerializeField] TMP_Text option3_UpgradeName;
    [SerializeField] TMP_Text option3_UpgradeDescription;
    [SerializeField] TMP_Text option3_UpgradeCost;

    public void OpenUI()
    {
        UI.SetActive(true); 
        UpdateUI();
    }

    public void CloseUI()
    {
        UI.SetActive(false);
        SelectObjectScript.Instance.SetModeToSelect(); 
    }

    public void UpdateUI()
    {
        _currentPlayer = GameManager.Instance.currentPlayerTurn; 

        researchPointsTotal.text = "" + _currentPlayer.ResearchPoints;

        for (int i = 4; i >= 0; i--)
        {
            if (!_currentPlayer.agressiveUpgradeTrack[i].alreadyResearched)
            {
                track1Index = i;
                 
            }
        }

        option1_UpgradeName.text = "" + _currentPlayer.agressiveUpgradeTrack[track1Index].upgradeName;
        option1_UpgradeDescription.text = "" + _currentPlayer.agressiveUpgradeTrack[track1Index].description;
        option1_UpgradeCost.text = "" + _currentPlayer.agressiveUpgradeTrack[track1Index].pointCost;

        for (int i = 4; i >= 0; i--)
        {
            if (!_currentPlayer.defensiveUpgradeTrack[i].alreadyResearched)
            {
                track2Index = i;
                continue; 
            }
        }
        option2_UpgradeName.text = "" + _currentPlayer.defensiveUpgradeTrack[track2Index].upgradeName;
        option2_UpgradeDescription.text = "" + _currentPlayer.defensiveUpgradeTrack[track2Index].description;
        option2_UpgradeCost.text = "" + _currentPlayer.defensiveUpgradeTrack[track2Index].pointCost;

        for (int i = 4; i >= 0; i--)
        {
            if (!_currentPlayer.explotiveUpgradeTrack[i].alreadyResearched)
            {
                track3Index = i;
                continue;
            }
        }

        option3_UpgradeName.text = "" + _currentPlayer.explotiveUpgradeTrack[track3Index].upgradeName;
        option3_UpgradeDescription.text = "" + _currentPlayer.explotiveUpgradeTrack[track3Index].description;
        option3_UpgradeCost.text = "" + _currentPlayer.explotiveUpgradeTrack[track3Index].pointCost;
    }


    public void BuyOption1()
    {
        if(_currentPlayer.ResearchPoints >= _currentPlayer.agressiveUpgradeTrack[track1Index].pointCost)
        {
            _currentPlayer.ResearchPoints -= _currentPlayer.agressiveUpgradeTrack[track1Index].pointCost;
            _currentPlayer.PurchaseTrack1Upgrade(track1Index);
        }
        UpdateUI();
    }

    public void BuyOption2()
    {
        if (_currentPlayer.ResearchPoints >= _currentPlayer.defensiveUpgradeTrack[track2Index].pointCost)
        {
            _currentPlayer.ResearchPoints -= _currentPlayer.defensiveUpgradeTrack[track2Index].pointCost;
            _currentPlayer.PurchaseTrack2Upgrade(track2Index);
        }
        UpdateUI();
    }

    public void BuyOption3()
    {
        if (_currentPlayer.ResearchPoints >= _currentPlayer.explotiveUpgradeTrack[track3Index].pointCost)
        {
            _currentPlayer.ResearchPoints -= _currentPlayer.explotiveUpgradeTrack[track3Index].pointCost;
            _currentPlayer.PurchaseTrack3Upgrade(track3Index);
        }
        UpdateUI();
    }
}
