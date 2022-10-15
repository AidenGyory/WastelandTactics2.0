using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 _originalPosition = transform.position;

        float _elapsed = 0f;

        GetComponent<CameraFollow>().follow = false;

        while (_elapsed < duration)
        {
            float x = Random.Range(-1f,1f) * magnitude;
            float y = Random.Range(-1f,1f) * magnitude;

            transform.position = new Vector3(_originalPosition.x + x, _originalPosition.y + y, _originalPosition.z);

            _elapsed += Time.deltaTime;

            yield return null;
        }

        GetComponent<CameraFollow>().follow = true;
    }
}
