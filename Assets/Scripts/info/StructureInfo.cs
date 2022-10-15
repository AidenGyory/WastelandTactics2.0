using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StructureInfo : MonoBehaviour
{
    public Material playerColour;
    [SerializeField] Renderer[] baseColouredParts;
    [SerializeField] LayerMask tile;
    [SerializeField] bool clearTileonStart;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down); 
    }

    private void Start()
    {
        if(clearTileonStart)
        {
            //ClearTile(); 
        }
    }

    // Start is called before the first frame update
    public void UpdatePlayerDetails()
    {
        for (int i = 0; i < baseColouredParts.Length; i++)
        {
            baseColouredParts[i].material = playerColour; 
        } 
    }
    //public void ClearTile()
    //{
    //    RaycastHit hit;
    //    // Does the ray intersect any objects excluding the player layer
    //    if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f, tile))
    //    {
    //        Debug.DrawRay(transform.position + Vector3.up, Vector3.down, Color.yellow);

    //        Debug.Log("Hit " + hit.transform.name);
    //        hit.transform.GetComponent<TileInfo>().ClearTile(); 
    //    }
    //    else
    //    {
    //        Debug.DrawRay(transform.position + Vector3.up, Vector3.down, Color.yellow);
    //        Debug.Log("Did not Hit");
    //    }
    //}
}
