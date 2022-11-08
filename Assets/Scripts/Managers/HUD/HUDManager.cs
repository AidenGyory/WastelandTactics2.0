using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Display Info HUD Components")]
    [SerializeField] HUDTileInfoDisplay tileInfo;
    [SerializeField] HUDUnitInfoDisplay unitInfo;
    [SerializeField] HUDStructureInfoDisplay structureInfo;

    [Header("Display Model on HUD References")]
    [SerializeField] GameObject renderObjectCamera;
    [SerializeField] GameObject renderedTexture; 
    public void DisplayObjectInfoHUD(SelectScript _selected)
    {
        
        renderObjectCamera.SetActive(true);
        renderObjectCamera.GetComponent<FollowScript>().SetTarget(_selected.transform); 
        renderedTexture.SetActive(true); 

        tileInfo.gameObject.SetActive(false); 
        unitInfo.gameObject.SetActive(false);
        structureInfo.gameObject.SetActive(false); 

        switch(_selected.type)
        {
            case SelectScript.objType.tile:
                tileInfo.gameObject.SetActive(true);
                tileInfo.TileInfoUpdate(_selected.GetComponent<TileInfo>()); 
            
                break;
            case SelectScript.objType.unit:
                unitInfo.gameObject.SetActive(true);
                unitInfo.UnitDisplayUpdate(_selected.GetComponent<UnitInfo>()); 
                
                break;
            case SelectScript.objType.structure:
                structureInfo.gameObject.SetActive(true);
                structureInfo.StructureDisplayUpdate(_selected.GetComponent<StructureInfo>()); 
                break; 
        }
    }
    
    public void ClearObjectInfoHUD()
    {
        renderObjectCamera.SetActive(false); 
        renderedTexture.SetActive(false);
        tileInfo.gameObject.SetActive(false);
        unitInfo.gameObject.SetActive(false);
        structureInfo.gameObject.SetActive(false);



    }
}
