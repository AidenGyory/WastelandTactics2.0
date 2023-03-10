using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject[] placeholderModel;
    [SerializeField] bool Active; 
    [SerializeField] bool canSpawn;

    [SerializeField] Vector3 positionOffset;

    public int cost;

    bool spawned = false;

    private void Start()
    {
        foreach (GameObject model in placeholderModel)
        {
            model.SetActive(false); 
        }

        placeholderModel[(int)type].SetActive(true);

        Invoke(nameof(SetAsPlaceable), 0.5f); 
    }

    public void SetAsPlaceable()
    {
        Active = true; 
    }

    void Update()
    {
        if(Active)
        {
            SelectScript _highlighted = SelectObjectScript.Instance.highlightedObject;

            if (_highlighted == null) { return; }

            if (_highlighted.objectType == SelectScript.objType.tile)
            {
                if (_highlighted.GetComponent<TileInfo>().state == TileInfo.TileState.CanPlace)
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

            if (Input.GetMouseButton(0))
            {
                if (canSpawn && !spawned)
                {
                    //Get TileInfo reference  
                    TileInfo _tileInfo = _highlighted.GetComponent<TileInfo>();

                    // Instantiate Unit at position
                    GameObject _unit = Instantiate(unitType[(int)type]);
                    _unit.transform.position = transform.position;

                    //Set player owner and initalise setup
                    UnitInfo _unitInfo = _unit.GetComponent<UnitInfo>();
                    _unitInfo.owner = GameManager.Instance.currentPlayerTurn;
                    _unitInfo.UpdatePlayerDetails();
                    _unitInfo.occuipedTile = _tileInfo;
                    _tileInfo.isOccupied = true;

                    //Remove Player Resources 
                    GameManager.Instance.currentPlayerTurn.AddMetalScrap(-cost); 

                    //Reset Camera and OjectSelectionManager
                    TileManager.instance.ClearPlaceableStateOnAllTiles();
                    SelectObjectScript.Instance.SetModeToSelect();

                    //spawned = true;
                    Destroy(gameObject);

                }
            }
        }
    }
}
