using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class CameraFollow : MonoBehaviour
{
    
    public Transform followTarget;
    public Vector3 targetPosition; 
    public bool follow;

    public float smoothSpeed;
    
    public Vector3 cameraOffset;

    public float topOffsetClamp; 
    public float bottomOffsetClamp;

    public bool canScroll; 

    private Vector3 _smoothVel;

    void Start()
    {
        GetComponent<CameraController>().SetOffset(0);
    }

    void LateUpdate()
    {
        targetPosition = followTarget.position + cameraOffset;

        if (follow)
        {

            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _smoothVel, smoothSpeed);

            transform.position = smoothPosition;

            transform.LookAt(followTarget);
        }

        if (canScroll)
        {
            if (Input.mouseScrollDelta.magnitude > 0.1f)
            {
                cameraOffset.y = cameraOffset.y + Input.mouseScrollDelta.y;
                if (cameraOffset.y > topOffsetClamp)
                {
                    cameraOffset.y = topOffsetClamp;
                }
                if (cameraOffset.y < bottomOffsetClamp)
                {
                    cameraOffset.y = bottomOffsetClamp;
                }
                if (Vector3.Distance(transform.position, followTarget.position + cameraOffset) >= 0.05f)
                {
                    Vector3 targetPosition = followTarget.position + cameraOffset;
                    Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _smoothVel, smoothSpeed);

                    transform.position = smoothPosition;

                    transform.LookAt(followTarget);
                }
            }
        }
    }
}
