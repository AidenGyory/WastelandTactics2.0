using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class ExplorationPointUI : MonoBehaviour
{
    [SerializeField] Image[] uiImageElements;
    [SerializeField] TMP_Text[] uiTextElements;

    [SerializeField] Vector3 moveDistance; 
    [SerializeField] float movetimer;

    [SerializeField] bool positive; 



    // Start is called before the first frame update
    void Start()
    {
        if(positive)
        {
            TIleAudioManager.instance.PlayTileAudio(tileAudioType.posflip);
        }
        else
        {
            TIleAudioManager.instance.PlayTileAudio(tileAudioType.negflip);
        }


        foreach (Image _ui in uiImageElements)
        {
            _ui.DOColor(_ui.color, 0.5f);
            _ui.color = Color.clear;
        }
        foreach (TMP_Text _text in uiTextElements)
        {
            _text.DOColor(_text.color, 0.5f);
            _text.color = Color.clear;
        }
        transform.DOScale(Vector3.one * 2,1f).OnComplete(FadeOut); 
        
    }

    public void FadeOut()
    {
        transform.DOScale(Vector3.one, 1f).OnComplete(FadeOut);
        foreach (Image _ui in uiImageElements)
        {
            _ui.DOColor(Color.clear, 1).OnComplete(DestroyUI);
        }
        foreach (TMP_Text _text in uiTextElements)
        {
            _text.DOColor(Color.clear, 1);
        }
    }
    public void DestroyUI()
    {
        Destroy(gameObject);
    }
}
