using UnityEngine;

public class Spinning : MonoBehaviour
{
    [SerializeField]
    private Vector3 speedRot;


    private void Update()
    {
        transform.Rotate(new Vector3(Time.deltaTime * speedRot.x, Time.deltaTime * speedRot.y, Time.deltaTime * speedRot.z));
    }
}