using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandstormParticleScript : MonoBehaviour
{
    [SerializeField] float rounds;
    [SerializeField] Transform particle; 
    // Start is called before the first frame update
    void Start()
    {
        TIleAudioManager.instance.PlayTileAudio(tileAudioType.sandstorm);
        transform.DORotate(new Vector3(0,360*rounds, 0), 2f);
        particle.DOScaleZ(-3, 2f); 
    }

}
