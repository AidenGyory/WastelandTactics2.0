using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveLineScript : MonoBehaviour
{
    [SerializeField] LineRenderer _line;
    [SerializeField] Vector3 _lineOffset;

    public void SetLinePoints(TileInfo _targetTile, UnitInfo _selectedUnit)
    {
        _line.positionCount = 1;
        _line.SetPosition(0, _selectedUnit.transform.localPosition + _lineOffset);

        List<TileInfo> _tileList = TileManager.instance.SetTileList(_selectedUnit.transform.position, 0);

        if (_tileList.Count < 1)
        {
            return;
        }

        TileInfo currentTile = _tileList[0];

        while (currentTile != _targetTile)
        {
            List<TileInfo> nextTiles = TileManager.instance.SetTileList(currentTile.transform.position, 1);
            List<float> tileDistances = new List<float>();

            if (nextTiles.Count == 0)
            {
                break;
            }

            for (int i = 0; i < nextTiles.Count; i++)
            {
                tileDistances.Add(Vector3.Distance(nextTiles[i].transform.localPosition, _targetTile.transform.position));
            }

            float minDistance = tileDistances.Min();
            int minDistanceIndex = tileDistances.IndexOf(minDistance);

            TileInfo nextTile = nextTiles[minDistanceIndex];
            _line.positionCount++;
            _line.SetPosition(_line.positionCount - 1, nextTile.transform.localPosition + _lineOffset);
            Debug.Log("set new point");

            currentTile = nextTile;
        }
    }
}
