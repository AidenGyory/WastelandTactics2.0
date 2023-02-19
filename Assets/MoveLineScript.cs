using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveLineScript : MonoBehaviour
{
    [SerializeField] LineRenderer _line;

    [SerializeField] Material inRangeMaterial; 
    [SerializeField] Material outOfRangeMaterial;


    public void SetMoveLine(UnitInfo _unit,TileInfo _endTile)
    {
        List<TileInfo> _path = PathfindingManager.Instance.GetPath(_unit.occuipedTile, _endTile, _unit.canFly);

        if(_endTile.state == TileInfo.TileState.walkable)
        {
            _line.material = inRangeMaterial;
        }
        else
        {
            _line.material = outOfRangeMaterial;
        }

        if (_path != null)
        {
            _line.positionCount = _path.Count;

            for (int i = 0; i < _path.Count; i++)
            {
                _line.SetPosition(i, _path[i].transform.position);
            }

        }
        
    }
}
