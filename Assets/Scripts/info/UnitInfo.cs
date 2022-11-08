using DG.Tweening;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
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
                    TileManager.Instance.ClearWalkableTiles(); 
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


}
