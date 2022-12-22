using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        Unfocused,
        Focused,
        Topdown,
        Zoomed,
    }

    public CameraMode mode;

    public bool debug;
    [Space]
    public Vector3[] cameraPosition;

    public int cameraIndex; 

    public void SetOffset(int index)
    {
        GetComponent<CameraFollow>().cameraOffset = cameraPosition[index];
        cameraIndex = index; 
    }

    private void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) SetOffset(0);
            if (Input.GetKeyDown(KeyCode.Alpha1)) SetOffset(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SetOffset(2);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SetOffset(3);
            if (Input.GetKeyDown(KeyCode.Alpha4)) SetOffset(4);
            if (Input.GetKeyDown(KeyCode.Alpha5)) SetOffset(5);
            if (Input.GetKeyDown(KeyCode.Alpha6)) SetOffset(6);
            if (Input.GetKeyDown(KeyCode.Alpha7)) SetOffset(7);
            if (Input.GetKeyDown(KeyCode.Alpha8)) SetOffset(8);
            if (Input.GetKeyDown(KeyCode.Alpha9)) SetOffset(9);
        }
    }

    public void SetCameraMode(CameraMode _cameraMode)
    {
        mode = _cameraMode;
        SetOffset((int)mode); 
    }
}
