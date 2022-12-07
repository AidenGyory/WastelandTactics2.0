using UnityEditor.Experimental;
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

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.8f);
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

    public void CreateScout()
    {
        TileManager.Instance.ShowPlaceableTiles(transform.position);
        SelectObjectScript select = FindObjectOfType<SelectObjectScript>();

        select.mode = SelectObjectScript.PointerMode.PlacementMode;
    }
    public void heal(int amount)
    {
        if(currentHealth + amount > maxHealth)
        {
            currentHealth = maxHealth; 
        }
        else
        {
            currentHealth += amount; 
        }
    }
}
