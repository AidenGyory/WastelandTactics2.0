using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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


        if (countdownToDestroy < 1.5)
        {
            amountText.color = Color.Lerp(amountText.color, Color.clear, Time.deltaTime * 2);
            icon.color = Color.Lerp(amountText.color, Color.clear, Time.deltaTime * 2);
            transform.position += Vector3.up * Time.deltaTime / 2;
        }

        transform.position += Vector3.up * Time.deltaTime/2;
        
    }
}
