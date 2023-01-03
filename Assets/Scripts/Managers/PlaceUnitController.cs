using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlaceUnitController : MonoBehaviour
{
    public enum UnitTypes
    {
        //New Eden Units
        Scout_NewEden,
        Worker_NewEden,
        Soldier_NewEden,
        Tank_NewEden,
        AntiTank_NewEden,
    }

    public UnitTypes type; 

    [SerializeField] GameObject[] unitType;

    [SerializeField] bool canSpawn;

    [SerializeField] Vector3 positionOffset;

    public int cost;

    bool spawned = false; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (canSpawn && !spawned)
            {
                spawned = true ;
                //Spawn Unit and set position
                Debug.Log("Spawn Unit");
                GameObject _unit = Instantiate(unitType[(int)type]);
                _unit.transform.position = transform.position;

                //Set Player Owner
                _unit.GetComponent<UnitInfo>().owner = GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn];
                _unit.GetComponent<UnitInfo>().UpdatePlayerDetails();

                //Remove Resources 
                GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn].MetalScrapAmount -= cost;

                //Reset back to Select Mode 
                SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.SelectMode;
                Camera.main.GetComponent<CameraController>().SetCameraMode(CameraController.CameraMode.Unfocused);

                //Reset Tiles 
                TileManager.instance.ClearPlaceableStateOnAllTiles();
                TileManager.instance.FindPlayerOwnedTilesForFlipCheck(GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn]);  

                //Set Tile as not empty 
                SelectScript _tile = SelectObjectScript.Instance.highlightedObject;
                if (_tile == null) { return; }
                _tile.GetComponent<TileInfo>().isEmpty = false;

                Destroy(gameObject);
            }

        }

        SelectScript _highlighted = SelectObjectScript.Instance.highlightedObject; 

        if(_highlighted == null) { return;  }

        if(_highlighted.objectType == SelectScript.objType.tile)
        {
            if(_highlighted.GetComponent<TileInfo>().state == TileInfo.TileState.CanPlace)
            {
                canSpawn = true; 
                transform.position = SelectObjectScript.Instance.highlightedObject.transform.position + positionOffset;
            }
            else
            {
                canSpawn = false;
                transform.position = Vector3.zero; 
            }
        }
        else
        {
            canSpawn = false;
            transform.position = Vector3.zero;
        }


        
    }
}
