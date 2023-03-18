using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    // The original position of the camera.
    Vector3 originalPos;

    public IEnumerator Shake(float shakeAmount, float shakeDuration)
    {
        originalPos = transform.localPosition;

        GetComponent<CameraFollow>().enabled = false;


        // Set the shake timer to the shake duration.
        float shakeTimer = shakeDuration;

        // While the shake timer is greater than zero, we are shaking.
        while (shakeTimer > 0)
        {
            // Set the camera's position to a random point within a sphere with radius equal to shakeAmount.
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            // Decrement the timer.
            shakeTimer -= Time.deltaTime;

            yield return null;
        }

        // Reset the camera's position.
        transform.localPosition = originalPos;
        GetComponent<CameraFollow>().enabled = true;
    }
}
