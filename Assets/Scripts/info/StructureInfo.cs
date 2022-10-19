using UnityEngine;

public class StructureInfo : MonoBehaviour
{
    public Material playerColour;
    [SerializeField] Renderer[] baseColouredParts;
    [SerializeField] LayerMask isTile;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down); 
    }

    private void Start()
    {
        UpdateTileDetails();
    }

    // Start is called before the first frame update
    public void UpdatePlayerDetails()
    {
        for (int i = 0; i < baseColouredParts.Length; i++)
        {
            baseColouredParts[i].material = playerColour; 
        } 
    }

    public void UpdateTileDetails()
    {
        
    }
}
