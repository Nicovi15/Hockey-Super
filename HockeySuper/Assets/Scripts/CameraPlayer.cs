using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [Header("Shaking settings")]
    Vector3 initPos;
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    float duration;

    Coroutine currentShake = null;

    private void Awake()
    {
        initPos = transform.position;
    }

    public void shake(float strength, float durationMultiplier = 1.0f)
    {
        if (currentShake != null)
            StopCoroutine(currentShake);
        currentShake = StartCoroutine(shaking(strength, durationMultiplier));
    }

    IEnumerator shaking(float strength, float durationMultiplier = 1.0f)
    {
        float t = 0;
        while(t < duration * durationMultiplier)
        {
            t += Time.deltaTime;
            float curvePos = curve.Evaluate(t / (duration * durationMultiplier));
            transform.position = initPos + Random.insideUnitSphere * curvePos * strength;
            yield return null;
        }
        transform.position = initPos;
    }

}
