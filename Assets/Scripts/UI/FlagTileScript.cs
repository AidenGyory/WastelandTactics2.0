using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagTileScript : MonoBehaviour
{
    [SerializeField] PlayerInfo owner;
    [SerializeField] bool active;  
    [SerializeField] Transform targetLocation;
    [SerializeField] bool follow; 
    [SerializeField] float smoothSpeed; 
    [SerializeField] float distanceOffset; 

    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Image>().DOColor(GetComponent<Image>().color,1f);
        GetComponent<Image>().color = Color.clear;
        Invoke(nameof(SetFollow), 1f);
    }

    public void SetTarget(Transform _target)
    {
        targetLocation = _target;
        owner = GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn]; 
    }

    // Update is called once per frame
    void Update()
    {
        if(owner != null)
        {
            if (owner != GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn] && active)
            {
                active = false;
                
                transform.GetChild(0).GetComponent<Image>().enabled = false;
                GetComponent<Image>().enabled = false;
            }
            else if(owner == GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn] && !active)
            {
                active = true;
                GetComponent<Image>().enabled = true;
                transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
        }
        

        if(targetLocation != null && !follow)
        {
            transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(targetLocation.position) + Vector3.up * distanceOffset, Time.deltaTime + smoothSpeed);
            if (Vector3.Distance(transform.position, targetLocation.position + Vector3.up * distanceOffset) < 1f)
            {
                follow = true;
            }
        }

        if(follow)
        {
            transform.position = Camera.main.WorldToScreenPoint(targetLocation.position) + Vector3.up * distanceOffset; 
        }
    }
    public void SetFollow()
    {
        follow = true; 
    }
}
