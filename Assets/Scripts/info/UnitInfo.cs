using UnityEngine;

public class UnitInfo: MonoBehaviour
{
    [Header("Unit Info")]
    public string unitName;
    public int currentHealth;
    public int maxHealth;
    [Space]
    public int attack;
    public int defense;
    public int range;
    public int currentMovement;
    public int maxMovement;
    [Space]
    public PlayerProfileTemplateSO factionOwner; 

    [SerializeField] Renderer[] baseColouredParts;
    [SerializeField] LayerMask isTile;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down); 
    }

    private void Awake()
    {
        UpdatePlayerDetails();
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
