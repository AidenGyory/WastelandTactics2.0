using System.Collections.Generic;
using UnityEngine;

public class SandstormTileScript : TileInfo
{
    [SerializeField] bool debug; 
    [SerializeField] float flipRadius;
    [SerializeField] LayerMask isTiles;
    [SerializeField] List<TileInfo> tilesToFlipBack;
    [SerializeField] bool canSandstorm;
    [SerializeField] GameObject sandstormMesh;
    [SerializeField] GameObject emptyMesh; 

    [Header("Prefab Element")]
    [SerializeField] GameObject sandstormUIPrefab;
    [SerializeField] float distanceOffset;
    [SerializeField] GameObject sandstormParticlePrefab;

    public void UnFlipTiles()
    {
        if(canSandstorm)
        {
            GameObject _ui = Instantiate(sandstormUIPrefab, SelectObjectScript.Instance.CameraScreenCanvas);
            _ui.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * distanceOffset;

            //// find the tiles within unit movement radius 
            //Collider[] _tiles = Physics.OverlapSphere(transform.position, flipRadius, isTiles);

            //for (int i = 0; i < _tiles.Length; i++)
            //{
            //    TileInfo _info = _tiles[i].GetComponent<TileInfo>();

            //    if (_info.state == TileState.IsFlipped)
            //    {
            //        tilesToFlipBack.Add(_info);
            //    }

            //}
            CreateSandstormParticle();
            ClearSandstorm(); 
            //Invoke("WaitAndFlip", 0.2f);
        }

        
    }
    public void ClearSandstorm()
    {
        canSandstorm = false;
        sandstormMesh.SetActive(false);
        emptyMesh.SetActive(true); 
    }

    public void WaitAndFlip()
    {
        //Once Flip Back has finished 
        if(tilesToFlipBack.Count < 1)
        {
            //Find tiles that can be flipped again
            TileManager.instance.FindPlayerOwnedTilesForFlipCheck(GameManager.Instance.currentPlayerTurn);
            return; 
        }

        int rand = Random.Range(0, tilesToFlipBack.Count);

        tilesToFlipBack[rand].FlipTileBack();

        
        tilesToFlipBack.Remove(tilesToFlipBack[rand]);

        Invoke("WaitAndFlip", 0.05f);
    }

    public void CreateSandstormParticle()
    {
        TileAudioManager.instance.PlayTileAudio(tileAudioType.sandstorm);
        GameObject _dust = Instantiate(sandstormParticlePrefab);
        _dust.transform.position = transform.position;
        //ShakeCamera(); 
    }

}
