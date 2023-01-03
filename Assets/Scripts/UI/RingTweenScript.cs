using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingTweenScript : MonoBehaviour
{
    [SerializeField] Image ring; 
    // Start is called before the first frame update
    void OnEnable()
    {
        ring.fillAmount = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        if (ring.fillAmount < 0.95f)
        {
            ring.fillAmount = Mathf.Lerp(ring.fillAmount, 1, Time.deltaTime * 2);
        }
    }
}
