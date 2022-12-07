using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SelectScript))]
public class UnitInfo : MonoBehaviour
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
    public float moveRadius;
    [Header("Owner Info")]
    public PlayerProfileTemplateSO factionOwner;

    [SerializeField] Renderer[] modelRenderer;
    Color[] unselected;
    [SerializeField] LayerMask isTile;
    [SerializeField] float tileSize; 

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, moveRadius);
    //}

    public void AddMovement(int amount)
    {
        if(currentMovement + amount > maxMovement)
        {
            currentMovement = maxMovement;
        }
        else
        {
            currentMovement += amount;
        }
    }

    public void ScanArea()
    {
        Debug.Log("Scanning Terrain");
        TileManager.Instance.FlipTiles(transform.position,tileSize); 
    }

    private void Awake()
    {
        UpdatePlayerDetails();
    }

    // Start is called before the first frame update
    private void Start()
    {
        unselected = new Color[modelRenderer.Length];

        for (int i = 0; i < modelRenderer.Length; i++)
        {
            unselected[i] = modelRenderer[i].material.color;
        }
    }
    public void UpdatePlayerDetails()
    {
        for (int i = 0; i < modelRenderer.Length; i++)
        {
            modelRenderer[i].material = factionOwner.playerBaseColour;
        }
    }

    public void ChangeMaterial(SelectScript.SelectState currentSelectState, float _timer)
    {
        switch (currentSelectState)
        {
            case SelectScript.SelectState.Unselected:
                for (int i = 0; i < modelRenderer.Length; i++)
                {
                    DOTween.Kill(modelRenderer[i].material);
                    modelRenderer[i].material.DOColor(unselected[i], _timer); 
                }
                break;
            case SelectScript.SelectState.Highlighted:
                for (int i = 0; i < modelRenderer.Length; i++)
                {
                    DOTween.Kill(modelRenderer[i].material);
                    modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.brightness, 0.5f);
                }
                break;
            case SelectScript.SelectState.Selected:
                for (int i = 0; i < modelRenderer.Length; i++)
                {
                    DOTween.Kill(modelRenderer[i].material);
                    modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.selected * TileManager.Instance.brightness, 0.5f).SetLoops(-1, LoopType.Yoyo);

                }
                break;
        }
    }
    public void MoveUnit(Vector3 position)
    {
        //Calculate Movement 
        float temp_distance = Vector3.Distance(transform.position, position);

        moveRadius -= temp_distance;
        //Debug.Log("Movement left is " + moveRadius);

        temp_distance /= tileSize;
        //Debug.Log("Distance is " + temp_distance);

        currentMovement -= Mathf.RoundToInt(temp_distance);
        
        //Debug.Log("Rounded to " + Mathf.RoundToInt(temp_distance) + "Tiles"); 
        //Debug.Log("Current Moves Left = " + currentMovement);

        transform.DOLookAt(position, 0.2f).OnComplete(Move);
        void Move()
        {
            transform.DOMove(position, 0.5f).OnComplete(CheckTiles);
            void CheckTiles()
            {
                TileManager.Instance.CheckWalkableTiles(transform.position, moveRadius);
            }
        }
    }

    public void CalculateMovementRaduis()
    {
        moveRadius = currentMovement * tileSize; 
        

    }


}
