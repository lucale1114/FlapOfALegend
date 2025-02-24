using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CloudMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject cloud;
    [SerializeField]
    private float rate = 1;
    [SerializeField]
    private float dist = 25;
    void Start()
    {
        StartCoroutine(PaintClouds());
    }
    IEnumerator PaintClouds()
    {
        GameObject floatingCloud = Instantiate(cloud);
        floatingCloud.transform.position = transform.position + new Vector3(0, 2 + (float)Random.Range(-dist, dist) / 10);
        yield return new WaitForSeconds(Random.Range(4 / rate, 10 / rate));
        StartCoroutine(PaintClouds());
    }
}
