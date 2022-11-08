using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    public bool follow;
    [SerializeField] float smoothSpeed;

    private Vector3 _smoothVel;

    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, followTarget.position, ref _smoothVel, smoothSpeed);

            transform.position = smoothPosition;
        }
    }

    public void SetTarget(Transform _target)
    {
        followTarget = _target;
        transform.position = _target.position; 
    }
}
