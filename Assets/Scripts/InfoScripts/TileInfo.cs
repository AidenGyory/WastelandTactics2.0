
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileInfo: MonoBehaviour
{
    public enum TileType
    {
        Empty,
        Reward,
        Sandstorm,
        Mountain,
        MetalMine,
        Unhexium,
        Vantage,
        Exhaust,
        Oasis,
    }
    public PlayerInfo Owner; 
    public PlayerInfo BorderOwner;
    public GameObject border; 
    public enum TileState
    {
        CanFlip,
        CannotFlip,
        IsFlipped, 
        CanPlace, 
        walkable,
        unwalkable,
    }
    public GameObject tileMesh; 
    public TileState state;
    public bool unwalkable; 
    public bool isOccupied;
    public List<TileInfo> neighbours; 
    public bool Checkable;

    public string tileName;
    public TileType type; 
    public Sprite tileImage;
    [TextArea]
    public string tileDescription;

    [SerializeField] UnityEvent TileHasFlipped;
    [SerializeField] UnityEvent TileHasFlippedBack;

    public Renderer[] modelMaterials;

    public List<Color> originalColour;

    [Header("dust prefab")]
    [SerializeField] GameObject dustPrefab;
    [SerializeField] Vector3 particlePrefabOffset;
    private void Start()
    {
        //flip tile upside down if tile is not in flipped state.
        if(state != TileState.IsFlipped)
        {
            transform.rotation = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            tileMesh.SetActive(true);
        }

        //set up original colour renderers for each model 
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            originalColour.Add(modelMaterials[i].material.color); 
        }
    }
    public void EstablishNeighbours()
    {
        if (neighbours.Count > 0) return; 

        // degree angles for clock-wise rotation from transform.forward / Vector3.forward
        float[] angleInDeg = { 90f, 30f, -30f, -90f, -150f, 150f, 90f };
        float m_maxDistance = 1;
        Vector3 m_direction;
        bool m_HitDetected = false;
        List<GameObject> list = new List<GameObject>();

        Vector3 origin = this.transform.position;
        RaycastHit raycastHit;

        foreach (float a in angleInDeg)
        {
            float angleInRad = a * Mathf.Deg2Rad;
            m_direction = new Vector3(Mathf.Cos(angleInRad), 0f, Mathf.Sin(angleInRad));

            m_HitDetected = Physics.Raycast(origin, m_direction, out raycastHit, m_maxDistance, TileManager.instance.isTiles);
            if (m_HitDetected)
            {
                if (raycastHit.transform.GetComponent<TileInfo>())
                {
                    // Give information about raycast box
                    //Debug.Log($"Hit{raycastHit.collider.name}");
                    
                    neighbours.Add(raycastHit.transform.GetComponent<TileInfo>());
                }
            }
        }

        foreach(TileInfo _tile in neighbours)
        {
            _tile.EstablishNeighbours(); 
        }

    }
    public void CheckIfCanFlipNeighbours()
    {
        if (Checkable && state == TileState.IsFlipped)
        {
            bool canFlipAdjacentTiles = false;

            for (int i = 0; i < neighbours.Count -1; i++)
            {
                if (neighbours[i].state != TileState.IsFlipped)
                {
                    canFlipAdjacentTiles = true;
                }
            }

            Checkable = canFlipAdjacentTiles;
        }

    }
    public bool CheckIfTileCanFlip()
    {
        //Check if the player has enough EP and the tilestate is "canFlip"
        bool _canFlip = GameManager.Instance.currentPlayerTurn.ExplorationPointsLeft > 0 && state == TileState.CanFlip;

        return _canFlip;         
    }

    public void TryToFlipTile()
    {
        TileAudioManager.instance.PlayTileAudio(tileAudioType.swipe);

        SelectObjectScript.Instance.canSelect = false;

        GameManager.Instance.currentPlayerTurn.AddExplorationPoints(-1);

        Owner = GameManager.Instance.currentPlayerTurn;

        tileMesh.SetActive(true);
        transform.DOJump(transform.position, 0.25f, 1, 0.2f);
        transform.DORotate(new Vector3(0f, 0, 0), 0.25f).OnComplete(TriggerTileHasFlipped);
        state = TileState.IsFlipped;
        Checkable = true;

        foreach (TileInfo _tile in neighbours)
        {
            Checkable = true;
            _tile.CheckIfCanFlipNeighbours();
        }

        UnselectTile();

        TileManager.instance.FindPlayerOwnedTilesForFlipCheck(Owner);

        TileManager.instance.SetBorderTileOwnership();

        //ShakeCamera(); 

        return;

    }
    public void FlipTileBack()
    {
        TileAudioManager.instance.PlayTileAudio(tileAudioType.swipe);
        Owner = null; 
        SelectObjectScript.Instance.canSelect = false; 
        DOTween.Kill(transform); 
        transform.DOJump(transform.position, 0.25f, 1, 0.2f);
        transform.DORotate(new Vector3(180f, 0, 0), 0.25f).OnComplete(TriggerTileHasFlippedBack);
        Checkable = false;
        foreach(TileInfo _tile in neighbours)
        {
            _tile.Checkable = true; 
            _tile.CheckIfCanFlipNeighbours();
        }
        state = TileState.CannotFlip;
    }

    [Tooltip("Add Actions that are in scripts INSIDE the prefab")] 
    public void TriggerTileHasFlipped()
    {
        foreach (TileInfo _tile in neighbours)
        {
            _tile.CheckIfCanFlipNeighbours();
        }
        TileAudioManager.instance.PlayTileAudio(tileAudioType.flip);
        SelectObjectScript.Instance.canSelect = true;
        TileHasFlipped.Invoke(); 
    }
    [Tooltip("Add Actions that are in scripts INSIDE the prefab")]
    public void TriggerTileHasFlippedBack()
    {
        tileMesh.SetActive(false);
        SelectObjectScript.Instance.canSelect = true;
        TileHasFlippedBack.Invoke();
    }

    public void HighlightTile()
    {
        foreach(Renderer _model in modelMaterials)
        {
            
            Color _highlight = _model.material.color * TileManager.instance.brightness;
            DOTween.Kill(_model);
            _model.material.DOColor(_highlight, 0.3f); 
        }
        if(state == TileState.IsFlipped)
        {
            DOTween.Kill(transform.localScale); 
            transform.DOScaleY(1.2f, 0.3f);
             
        }
    }

    public void SetasCanFlip()
    {
        state = TileState.CanFlip;

        foreach (Renderer _model in modelMaterials)
        {
            _model.material.DOColor(TileManager.instance.flippable, 0.3f); 
        }
    }

    public void SelectTile()
    {
        if(CheckIfTileCanFlip())
        {
            TryToFlipTile(); 
        }
        else
        {
            foreach (Renderer _model in modelMaterials)
            {
                _model.material.DOColor(TileManager.instance.selected, TileManager.instance.flashSpeed).SetLoops(-1, LoopType.Yoyo);

            }

            if (state == TileState.IsFlipped)
            {
                DOTween.Kill(transform.localScale);
                transform.DOScaleY(1.5f, 0.3f);
            }
            else
            {
                UnselectTile();
            }
        }
    }

    //When the tile is unselected, unhighlighted etc. 
    public void UnselectTile()
    {
        //switch case for each state as they can vary. 
        switch (state)
        {
            case TileState.CanFlip:
                for (int i = 0; i < modelMaterials.Length; i++)
                {
                    DOTween.Kill(modelMaterials[i].material);
                    modelMaterials[i].material.DOColor(TileManager.instance.flippable, 0.3f);
                }
                break;
            case TileState.CannotFlip:
                for (int i = 0; i < modelMaterials.Length; i++)
                {
                    DOTween.Kill(modelMaterials[i].material);
                    modelMaterials[i].material.DOColor(originalColour[i], 0.3f);
                }
                break;
            case TileState.IsFlipped:
                for (int i = 0; i < modelMaterials.Length; i++)
                {
                    DOTween.Kill(modelMaterials[i].material);
                    modelMaterials[i].material.DOColor(originalColour[i], 0.3f);
                    DOTween.Kill(transform.localScale);
                    transform.DOScaleY(1f, 0.3f);
                }
                break;
            case TileState.CanPlace:
                for (int i = 0; i < modelMaterials.Length; i++)
                {
                    DOTween.Kill(modelMaterials[i].material);
                    modelMaterials[i].material.DOColor(TileManager.instance.placeable, 0.3f);
                }
                break;
            case TileState.walkable:
                for (int i = 0; i < modelMaterials.Length; i++)
                {
                    DOTween.Kill(modelMaterials[i].material);
                    modelMaterials[i].material.DOColor(TileManager.instance.walkable, 0.3f);
                }
                break;
            case TileState.unwalkable:
                for (int i = 0; i < modelMaterials.Length; i++)
                {
                    DOTween.Kill(modelMaterials[i].material);
                    modelMaterials[i].material.DOColor(TileManager.instance.unwalkable, 0.3f);
                }
                break;
        }
    }

    public void SetTileToPlaceable()
    {
        if(!isOccupied && state == TileState.IsFlipped)
        {
            state = TileState.CanPlace;

            foreach (Renderer _model in modelMaterials)
            {
                _model.material.DOColor(TileManager.instance.placeable, 0.3f);
            }
        }
    }

    public void SetTileToWalkable()
    {
        if(!isOccupied)
        {
            state = TileState.walkable;
            for (int i = 0; i < modelMaterials.Length; i++)
            {
                DOTween.Kill(modelMaterials[i].material);
                modelMaterials[i].material.DOColor(TileManager.instance.walkable, 0.3f);
            }
        }
        else
        {
            SetTileToUnwalkable(); 
        }
        
    }

    public void SetTileToUnwalkable()
    {
        state = TileState.unwalkable;
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            DOTween.Kill(modelMaterials[i].material);
            modelMaterials[i].material.DOColor(TileManager.instance.unwalkable, 0.3f);
        }
    }
   

    //Used to clear tiles once used. 
    public void SetToClearTile()
    {
        GameObject _replacementTile = Instantiate(TileManager.instance.clearTilePrefab, transform.parent);
        _replacementTile.name = gameObject.name;
        _replacementTile.transform.position = transform.position;
        _replacementTile.GetComponent<TileInfo>().state = state;
        _replacementTile.GetComponent<TileInfo>().isOccupied = isOccupied;
        _replacementTile.GetComponent<TileInfo>().Checkable = Checkable; 
       TileManager.instance.InitialiseAllTiles(); 

        Destroy(gameObject); 

    }

    public void ShakeCamera()
    {
        Camera.main.GetComponent<CameraFollow>().StartShake(0.1f, 0.1f); 
    }

    public void CreateDustParticle()
    {
        GameObject _dust = Instantiate(dustPrefab); 
        _dust.transform.position = transform.position;
    }

    public GameObject scanIcon; 
    public void ShowScanIcon(bool visible)
    {
        if(scanIcon != null)
        {
            scanIcon.SetActive(visible);
        }
    }

    public void FlipTile()
    {
        TileAudioManager.instance.PlayTileAudio(tileAudioType.swipe);

        SelectObjectScript.Instance.canSelect = false;

        Owner = GameManager.Instance.currentPlayerTurn;
        tileMesh.SetActive(true);
        transform.DOJump(transform.position, 0.25f, 1, 0.2f);
        transform.DORotate(new Vector3(0f, 0, 0), 0.25f).OnComplete(TriggerTileHasFlipped);
        state = TileState.IsFlipped;
        Checkable = true;

        foreach (TileInfo _tile in neighbours)
        {
            Checkable = true;
            _tile.CheckIfCanFlipNeighbours();
        }

        UnselectTile();

        TileManager.instance.FindPlayerOwnedTilesForFlipCheck(Owner);
        TileManager.instance.SetBorderTileOwnership();

        return;

    }

}
