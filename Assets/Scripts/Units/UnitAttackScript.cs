using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthScript))]
public class UnitAttackScript : MonoBehaviour
{
    public enum UnitTypes
    {
        Scout,
        Soldier,
        Tank,
        AntiGun, 
        none,
    }

    public bool hasAttacked;
    [Space]
    public UnitTypes type;
    public int attackLevel;
    public int damageRangeMin; 
    public int damageRangeMax;
    public int attackRange; 
    [Space]
    [SerializeField] UnitTypes strongAgainst; 
    [SerializeField] UnitTypes weakAgainst;

    private UnitInfo _unitInfo;

    private void Start()
    {
        _unitInfo = GetComponent<UnitInfo>();
    }
    public void ReplenishAttack()
    {
        hasAttacked = false;
    }

    public void CheckAttackRadius()
    {
        //Guard against already attacked and units that are not yours to control. 
        if (hasAttacked || GameManager.Instance.currentPlayerTurn != GetComponent<UnitInfo>().owner) { return; }

        //Establish Variables 
        float tileSize = TileManager.instance.tileSize;
        LayerMask _isUnit = TileManager.instance.isUnit; 
        LayerMask _isStructure = TileManager.instance.isStructure;

        //Find targets
        Collider[] _units = Physics.OverlapSphere(_unitInfo.occuipedTile.transform.position, attackRange * tileSize, _isUnit);

        foreach (Collider _unit in _units)
        {
            UnitInfo _info = _unit.transform.GetComponent<UnitInfo>();

            if (_info != null)
            {
                if(_info.owner != _unitInfo.owner)
                {
                    _info.GetComponent<HealthScript>().targetIcon.SetActive(true);
                    _info.GetComponent<HealthScript>().isTarget = true;
                    Debug.Log("Enemy unit: " + _info.name + " can be attacked!"); 
                }
            }
        }

        Collider[] _structures = Physics.OverlapSphere(_unitInfo.occuipedTile.transform.position, attackRange * tileSize, _isStructure);

        foreach (Collider _structure in _structures)
        {
            StructureInfo _info = _structure.transform.GetComponent<StructureInfo>();

            if (_info != null)
            {
                if (_info.owner != _unitInfo.owner)
                {
                    _info.GetComponent<HealthScript>().targetIcon.SetActive(true);
                    _info.GetComponent<HealthScript>().isTarget = true;
                    Debug.Log("Enemy unit: " + _info.name + " can be attacked!");
                }
            }
        }

    }

    public int GetDamage(HealthScript _target)
    {
        float _multiplier = 1;
        
        //Check Advantage
        if(_target.GetComponent<UnitInfo>() != null)
        {
            if(_target.GetComponent<UnitAttackScript>().type == strongAgainst)
            {
                //Advantage Multiplier
                _multiplier = 1.5f; 
            }
            if(_target.GetComponent<UnitAttackScript>().type == weakAgainst)
            {
                //Disadvantage Multiplier
                _multiplier = 0.5f; 
            }
        }

        int _damage = Random.Range(damageRangeMin, damageRangeMax);

        _multiplier *= (float)_damage;

        _damage = Mathf.RoundToInt(_multiplier);

        return _damage; 
    }
}
