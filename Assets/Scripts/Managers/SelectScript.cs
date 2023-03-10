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
    public objType objectType;
    public SelectState currentSelectState;

    [SerializeField] UnityEvent selectObj; 
    [SerializeField] UnityEvent unselectObj;
    [SerializeField] UnityEvent highlightObj;
    [SerializeField] UnityEvent unhighlightObj;

    public void HighlightObject()
    {
        if(objectType == objType.tile)
        {
            TileAudioManager.instance.PlayTileAudio(tileAudioType.click);
        }
        // guard for if state is anything other than unselected  
        if(currentSelectState != SelectState.Unselected) { return; }

        // Set state to highlighted 
        
        currentSelectState = SelectState.Highlighted;
        highlightObj.Invoke();
    }

    public void UnhighlightObject()
    {
        // Guard for if object is selected 
        if (currentSelectState == SelectState.Selected) {return; }
        // if not selected then set state to unselected 
        currentSelectState = SelectState.Unselected;

        // Unhighlight Object
        if(unhighlightObj != null)
        {
            unhighlightObj.Invoke();

        }
    }

    public void SelectObject()
    {
        // Guard for if object is selected 
        if (currentSelectState == SelectState.Selected) { return; }

        TileAudioManager.instance.PlayTileAudio(tileAudioType.select);

        // if not selected then set state to unselected 
        currentSelectState = SelectState.Selected;

        //Invoke "SelectObj()" Event in Editor
        selectObj.Invoke();
        
    }

    public void DeselectObject()
    {
        // Guard for if object is selected 
        if (currentSelectState != SelectState.Selected) { return; }

        // if not selected then set state to unselected 
        currentSelectState = SelectState.Unselected;

        // Unselect Object
        unselectObj.Invoke();
    }

    public void ClearSelectInfo()
    {
        DeselectObject();
        UnhighlightObject();
    }
}
