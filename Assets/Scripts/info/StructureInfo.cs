using UnityEngine;

public class StructureInfo : MonoBehaviour
{
    [Header("Unit Info")]
    public string structureName;
    public int currentHealth;
    public int maxHealth;
    [Space]
    public PlayerProfileTemplateSO factionOwner;

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
        
    }

    // Start is called before the first frame update
    public void UpdatePlayerDetails()
    {
        for (int i = 0; i < baseColouredParts.Length; i++)
        {
            baseColouredParts[i].material = factionOwner.playerBaseColour; 
        } 
    }
}
