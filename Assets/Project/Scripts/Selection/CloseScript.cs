using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseScript : MonoBehaviour, IPointerClickHandler
{
    private SelectionStage stageScript;

    private void Awake()
    {
        stageScript = FindObjectOfType<SelectionStage>();    
    }
    public void OnPointerClick(PointerEventData data)
    {
        AudioManager.PlaySound(stageScript.NodeAppearSound, 0.4f, -0.6f);
        transform.parent.DOScale(new Vector3(1, 0, 1), 0.5f);
    }
}
