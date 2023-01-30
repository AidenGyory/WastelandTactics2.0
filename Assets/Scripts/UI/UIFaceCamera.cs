using UnityEngine;
using UnityEngine.UI;

public class UIFaceCamera : MonoBehaviour
{
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - Camera.main.transform.position);
    }
}
