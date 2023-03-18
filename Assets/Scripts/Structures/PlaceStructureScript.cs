using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceStructureScript : MonoBehaviour
{
    public enum BuildingTypes
    {
        //New Eden Units
        Outpost_NewEden,
        Factory_NewEden,
        Power_NewEden,
        Research_NewEden,
    }

    [SerializeField] GameObject[] placeable;
    [SerializeField] GameObject[] structurePrefab; 

    public int cost;
    public BuildingTypes type;
    bool canSpawn;
    bool active;  
    [SerializeField] Vector3 positionOffset; 
    private void Start()
    {
        placeable[(int)type].SetActive(true);
        Invoke("SetActive", 0.5f); 
    }

    public void SetActive()
    {
        active = true;
    }

    private void Update()
    {
        SelectScript _highlighted = SelectObjectScript.Instance.highlightedObject;

        if (_highlighted == null) { return; }

        if (_highlighted.objectType == SelectScript.objType.tile)
        {
            if (_highlighted.GetComponent<TileInfo>().state == TileInfo.TileState.CanPlace)
            {
                placeable[(int)type].SetActive(true);
                canSpawn = true;
                transform.position = SelectObjectScript.Instance.highlightedObject.transform.position + positionOffset;
            }
            else
            {
                canSpawn = false;
                placeable[(int)type].SetActive(false);
            }
        }
        else
        {
            canSpawn = false;
            placeable[(int)type].SetActive(false);
        }

        if (Input.GetMouseButton(0) && active)
        {
            if (canSpawn)
            {
                //Get TileInfo reference  
                TileInfo _tileInfo = _highlighted.GetComponent<TileInfo>();

                // Instantiate Unit at position
                GameObject _structure = Instantiate(structurePrefab[(int)type]);
                _structure.transform.position = transform.position;

                //Set player owner and initalise setup
                StructureInfo _structureInfo = _structure.GetComponent<StructureInfo>();
                _structureInfo.owner = GameManager.Instance.currentPlayerTurn;
                
                _structureInfo.occupiedTile = _tileInfo;
                _tileInfo.isOccupied = true;

                _structureInfo.UpdatePlayerDetails();

                //Remove Player Resources 
                GameManager.Instance.currentPlayerTurn.AddMetalScrap(-cost);

                //Reset Camera and OjectSelectionManager
                TileManager.instance.ClearPlaceableStateOnAllTiles();
                SelectObjectScript.Instance.SetModeToSelect();

                TileManager.instance.SetBorderTileOwnership();
                Destroy(gameObject);

            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Reset Camera and OjectSelectionManager
            TileManager.instance.ClearPlaceableStateOnAllTiles();
            SelectObjectScript.Instance.SetModeToSelect();
            Destroy(gameObject);
        }
        if (Input.GetMouseButton(1))
        {
            //Reset Camera and OjectSelectionManager
            TileManager.instance.ClearPlaceableStateOnAllTiles();
            SelectObjectScript.Instance.SetModeToSelect();
            Destroy(gameObject);
        }
    }
}
