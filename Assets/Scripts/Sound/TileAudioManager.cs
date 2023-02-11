using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tileAudioType
{
    click,
    swipe,
    flip,
    sandstorm,
    posflip,
    negflip,
    ping,
    select,
    shoot,
    damage,
}

public class TileAudioManager : MonoBehaviour
{
    public static TileAudioManager instance;

    [SerializeField] AudioClip tileClick;
    [SerializeField] AudioClip tileSwipe; 
    [SerializeField] AudioClip TileFlip;
    [SerializeField] AudioClip sandstorm;
    [SerializeField] AudioClip positiveFlip;
    [SerializeField] AudioClip negativeFlip;
    [SerializeField] AudioClip pingTile;
    [SerializeField] AudioClip select;
    [SerializeField] AudioClip shoot;
    [SerializeField] AudioClip destroy; 

    private AudioSource _audio; 

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayTileAudio(tileAudioType type)
    {
        switch (type)
        {
            case tileAudioType.click:
                _audio.PlayOneShot(tileClick); 
                break;
            case tileAudioType.swipe:
                _audio.PlayOneShot(tileSwipe);

                break;
            case tileAudioType.flip:
                _audio.PlayOneShot(TileFlip);

                break;
            case tileAudioType.sandstorm:
                _audio.PlayOneShot(sandstorm);
                break;
            case tileAudioType.posflip:
                _audio.PlayOneShot(positiveFlip);
                break;
            case tileAudioType.negflip:
                _audio.PlayOneShot(negativeFlip);
                break;
            case tileAudioType.ping:
                _audio.PlayOneShot(pingTile);
                break;
            case tileAudioType.select:
                _audio.PlayOneShot(select);
                break;
            case tileAudioType.shoot:
                _audio.PlayOneShot(shoot);
                break;
            case tileAudioType.damage:
                _audio.PlayOneShot(destroy);
                break; 
            default:
                break;
        }
    }

}
