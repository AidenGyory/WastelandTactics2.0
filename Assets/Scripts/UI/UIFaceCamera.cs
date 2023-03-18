using UnityEngine;
using UnityEngine.UI;

public class UIFaceCamera : MonoBehaviour
{
    private Canvas canvas;

    void Start()
    {
        //canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
