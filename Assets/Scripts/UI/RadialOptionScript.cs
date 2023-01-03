using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class RadialOptionScript : MonoBehaviour
{
    Image segmentBackgroundImage;
    public string unitName;
    public int resourceCost; 

    public Image icon;
    [SerializeField] Image iconSpotlight; 

    private void Start()
    {
        segmentBackgroundImage = GetComponent<Image>(); 
    }
    public void HighlightSegment(Color _color)
    {
        transform.DOScale(1.1f, 0.5f); 
        segmentBackgroundImage.DOColor(_color, 0.5f);
        icon.DOColor(Color.white, 0.5f);
        iconSpotlight.DOColor(new Color(0.8f, 0.8f, 0.8f, 0.5f), 0.5f); 

    }

    public void UnHighlightSegment(Color _color)
    {
        transform.DOScale(1f, 0.5f);
        segmentBackgroundImage.DOColor(_color, 0.5f);
        icon.DOColor(new Color(1,1,1,0.5f), 0.5f);
        iconSpotlight.DOColor(Color.clear, 0.5f);




    }
}
