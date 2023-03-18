using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MetalProductionScript : MonoBehaviour
{
    public int productionAmount;
    [Space]
    [SerializeField] TMP_Text amountText;
    [SerializeField] Image icon;
    [Space]
    [SerializeField] float countdownToDestroy;
    public Color positive;
    public Color negative;

    void Start()
    {
        transform.parent = SelectObjectScript.Instance.CameraScreenCanvas;
        if (productionAmount > 0)
        {
            amountText.color = positive;
            icon.color = positive;
            TileAudioManager.instance.PlayTileAudio(tileAudioType.posflip); 
        }
        else
        {
            amountText.color = negative;
            icon.color = negative;
            TileAudioManager.instance.PlayTileAudio(tileAudioType.negflip);

        }
    }

    void Update()
    {
        amountText.text = "" + productionAmount; 

        if(countdownToDestroy > 0)
        {
            countdownToDestroy -= Time.deltaTime; 
        }
        else
        {
            Destroy(gameObject); 
        }

        transform.position += Vector3.up * Time.deltaTime;
    }
}
