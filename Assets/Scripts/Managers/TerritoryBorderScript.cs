using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerritoryBorderScript : MonoBehaviour
{

    public List<LineRenderer> _borders = new List<LineRenderer>();
    private TileInfo _tileinfo;

    private void Start()
    {
        _tileinfo = GetComponent<TileInfo>();
    }
    public void UpdateBorders()
    {
        if(_tileinfo != null)
        {
            foreach (LineRenderer _line in _borders)
            {
                _line.enabled = true;
                _line.material = _tileinfo.Owner.settings.baseMaterial;
            }
            for (int i = 0; i < _borders.Count; i++)
            {
                if(i < _tileinfo.neighbours.Count)
                {
                    if (_tileinfo.neighbours[i].BorderOwner == _tileinfo.BorderOwner)
                    {
                        _borders[i].enabled = false;
                    }
                }
            }
        }
        
    }
}
