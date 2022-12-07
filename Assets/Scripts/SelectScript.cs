using UnityEngine;
using UnityEngine.Events;


public class SelectScript : MonoBehaviour
{
    public enum objType
    {
        tile,
        structure,
        unit,
    }
    public enum SelectState
    {
        Unselected,
        Highlighted,
        Selected,
    }

    [Header("Object State Info")]
    public objType type;
    public SelectState currentSelectState;

    [SerializeField] UnityEvent selectObj; 
    [SerializeField] UnityEvent unselectObj;

    //RayCast for.....
    private Ray _ray;
    private RaycastHit _hit;

    public void HighlightObject()
    {
        // guard for if state is anything other than unselected  
        if(currentSelectState != SelectState.Unselected)
        {
            return;
        }
        // Set state to highlighted 
        currentSelectState = SelectState.Highlighted;

        // Check type and run function based on type for highlight
        if (type == objType.tile)
        {
            //Tile Change Materials Extras
            GetComponent<TileInfo>().ChangeMaterialAndScale(currentSelectState, 0.5f); 
        }
        if (type == objType.structure)
        {
            //Structure Change Material Extras 
        }
        if (type == objType.unit)
        {
            //Unit Change Material Extras
            GetComponent<UnitInfo>().ChangeMaterial(currentSelectState, 0.5f);
        }
    }

    public void UnhighlightObject()
    {
        // Guard for if object is selected 
        if (currentSelectState == SelectState.Selected)
        {
            return;
        }
        // if not selected then set state to unselected 
        currentSelectState = SelectState.Unselected;
        // Unselect Object
        unselectObj.Invoke();  

        // check type and run function for state change to unselected. 
        if (type == objType.tile)
        {
            //Tile Change Materials Extras
            GetComponent<TileInfo>().ChangeMaterialAndScale(currentSelectState, 0.5f);
        }
        if (type == objType.structure)
        {
            //Structure Change Material Extras 
        }
        if (type == objType.unit)
        {
            //Unit Change Material Extras
            GetComponent<UnitInfo>().ChangeMaterial(currentSelectState, 0.5f);
        }
    }

    public void SelectObject()
    {
        // Guard for if object is selected 
        if (currentSelectState == SelectState.Selected)
        {
            return;
        }
        // if not selected then set state to unselected 
        currentSelectState = SelectState.Selected;
        //Invoke Unity Event in Editor
        selectObj.Invoke();
        // check type and run function for state change to unselected. 
        if (type == objType.tile)
        {
            //Tile Change Materials Extras
            GetComponent<TileInfo>().ChangeMaterialAndScale(currentSelectState, 0.5f);
            //Add code for checking tile state and flipping if able to
        }
        if (type == objType.structure)
        {
            //Structure Change Material Extras 
        }
        if (type == objType.unit)
        {
            //Unit Change Material Extras
            // Add code for checking unit movement range and tracking movement path
            GetComponent<UnitInfo>().ChangeMaterial(currentSelectState, 0.5f);
            GetComponent<UnitInfo>().CalculateMovementRaduis(); 
            TileManager.Instance.CheckWalkableTiles(transform.position, GetComponent<UnitInfo>().moveRadius); 
        }
    }

    public void DeselectObject()
    {
        // Guard for if object is selected 
        if (currentSelectState != SelectState.Selected)
        {
            return;
        }
        // if not selected then set state to unselected 
        currentSelectState = SelectState.Unselected;
        // Unselect Object
        unselectObj.Invoke();
        // check type and run function for state change to unselected. 
        if (type == objType.tile)
        {
            //Tile Change Materials Extras
            GetComponent<TileInfo>().ChangeMaterialAndScale(currentSelectState, 0.5f);
        }
        if (type == objType.structure)
        {
            //Structure Change Material Extras 
        }
        if (type == objType.unit)
        {
            //Unit Change Material Extras
            GetComponent<UnitInfo>().ChangeMaterial(currentSelectState, 0.5f);
            TileManager.Instance.ClearWalkableTiles();
        }
    }

    public void ClearSelectInfo()
    {
        DeselectObject();
        UnhighlightObject();
    }
}
