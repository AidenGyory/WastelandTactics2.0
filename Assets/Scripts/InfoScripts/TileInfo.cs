
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TileInfo: MonoBehaviour
{
    public PlayerInfo Owner; 
    public enum TileState
    {
        CanFlip,
        CannotFlip,
        IsFlipped, 
    }
    [SerializeField] bool isFlagged;
    [SerializeField] GameObject flagPrefab;
    GameObject flag; 
    
    public TileState state; 

    [SerializeField] string tileName;
    [TextArea]
    [SerializeField] string tileDescription;

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



        //set up original colour renderers for each model 
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            originalColour.Add(modelMaterials[i].material.color); 
        }
    }
    //bool to check if you can flip this tile 
    public bool CheckIfTileCanFlip()
    {
        //Check if the player has enough EP and the tilestate is "canFlip"
        bool _canFlip = GameManager.Instance.CheckExplorationPoints() && state == TileState.CanFlip;

        return _canFlip;         
    }

    public void TryToFlipTile()
    {
        if (CheckIfTileCanFlip())
        {
            TIleAudioManager.instance.PlayTileAudio(tileAudioType.swipe);

            SelectObjectScript.Instance.canSelect = false;
            int currentPlayerTurn = (int)GameManager.Instance.currentPlayerTurn;

            var playerInfo = GameManager.Instance.playerInfo[currentPlayerTurn];

            playerInfo.AddPoints(ResourcesType.ExplorationPoints, -1);
            Owner = GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn]; 
            transform.DOJump(transform.position, 0.25f, 1, 0.2f); 
            transform.DORotate(new Vector3(0f, 0, 0), 0.25f).OnComplete(TriggerTileHasFlipped);
            state = TileState.IsFlipped;
            UnselectTile();
            TileManager.instance.FindPlayerOwnedTilesForFlipCheck(Owner);
            
            if (isFlagged)
            {
                ToggleFlagState();
            }
            return;
            
        }
        
    }
    public void FlipTileBack()
    {
        TIleAudioManager.instance.PlayTileAudio(tileAudioType.swipe);
        Owner = null; 
        SelectObjectScript.Instance.canSelect = false; 
        DOTween.Kill(transform); 
        transform.DOJump(transform.position, 0.25f, 1, 0.2f);
        transform.DORotate(new Vector3(180f, 0, 0), 0.25f).OnComplete(TriggerTileHasFlippedBack);
        state = TileState.CannotFlip;
    }

    public void ToggleFlagState()
    {
        isFlagged = !isFlagged;

        TIleAudioManager.instance.PlayTileAudio(tileAudioType.ping); 

        if(isFlagged)
        {
            if(flag == null)
            {
                GameObject _flag = Instantiate(flagPrefab, SelectObjectScript.Instance.CameraScreenCanvas);
                _flag.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 3); 
                _flag.GetComponent<FlagTileScript>().SetTarget(transform);
                flag = _flag;
            }
            //Debug.Log("Flag Tile"); 
        }
        else
        {
            if(flag != null)
            {
                flag.GetComponent<Image>().DOColor(Color.clear, 0.3f).OnComplete(DestroyFlag);
                flag.transform.GetComponentInChildren<Image>().DOColor(Color.clear, 0.3f); 

            }
            //Debug.Log("UnFlag Tile"); 
        }
    }

    public void DestroyFlag()
    {
        TIleAudioManager.instance.PlayTileAudio(tileAudioType.ping);

        Destroy(flag);
        flag = null;
    }

    [Tooltip("Add Actions that are in scripts INSIDE the prefab")] 
    public void TriggerTileHasFlipped()
    {
        TIleAudioManager.instance.PlayTileAudio(tileAudioType.flip);

        SelectObjectScript.Instance.canSelect = true;
        TileHasFlipped.Invoke(); 
    }
    [Tooltip("Add Actions that are in scripts INSIDE the prefab")]
    public void TriggerTileHasFlippedBack()
    {
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
            default:
                break;
        }
    }


    //Used to clear tiles once used. 
    public void SetToClearTile()
    {
        GameObject _replacementTile = Instantiate(TileManager.instance.clearTilePrefab, transform.parent);
        _replacementTile.name = gameObject.name;
        _replacementTile.transform.position = transform.position;
        _replacementTile.GetComponent<TileInfo>().state = state;
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

}
