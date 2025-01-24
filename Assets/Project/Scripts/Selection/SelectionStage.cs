using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionStage : MonoBehaviour
{
    private Transform trans;

    private void Awake()
    {
        trans = GameObject.Find("Transition").transform;    
    }

    void Start()
    {
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
