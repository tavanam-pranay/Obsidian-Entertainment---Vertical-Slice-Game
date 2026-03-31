using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public AnimationCurve curve;

    // This code taken from my previous 2300 project - Etienne
    public void gunShake()
    {
       StartCoroutine(ShakeCamera(0.15f, 0.1f));
    }

    public void bigShake()
    {
        StartCoroutine(ShakeCamera(1f, 0.5f));
    }

    public IEnumerator ShakeCamera(float duration, float power) //This is a coroutine
    {
        Vector3 startPos = transform.position;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            startPos = transform.position; // Return to pos while moving
            elapsed += Time.deltaTime;
            float strength = curve.Evaluate(elapsed / duration);
            transform.position = startPos + Random.insideUnitSphere * strength * power;
            yield return null;
        }

        transform.localPosition = startPos;
    }
}
