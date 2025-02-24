using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    public static IEnumerator ShakeScreen(float duration, float intensity)
    {
        float newIntensity = intensity;
        for (float i = 0; i < duration; i += 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            Camera.main.transform.DOLocalMove(new Vector3(0, 0,-10) + new Vector3((float)Random.Range(-100, 100) / 1000 * newIntensity, (float)Random.Range(-100, 100) / 1000 * newIntensity, 0), 0.01f);
            newIntensity = intensity * i / duration;
        }
        Camera.main.transform.DOLocalMove(new Vector3(0, 0, -10), 0.5f);
    }

    public static IEnumerator ShakeScreen(float duration, float intensity, Vector3 originalPos)
    {
        float newIntensity = intensity;
        for (float i = 0; i < duration; i += 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            Camera.main.transform.DOLocalMove(originalPos + new Vector3((float)Random.Range(-100, 100) / 1000 * newIntensity, (float)Random.Range(-100, 100) / 1000 * newIntensity, 0), 0.01f);
            newIntensity = intensity * i / duration;
        }
        Camera.main.transform.DOLocalMove(originalPos, 0.5f);
    }
}
