using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class AnimDownTextClass : MonoBehaviour
{
    
    public float StartY = 208f;
    public float EndY = 886f;

    public float Heght = 1518.6f;
    
    public RectTransform rectTransform;
    
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x, EndY, 0), 0.5f);
            rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, Heght), 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x, StartY, 0), 0.5f);
            rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, 0), 0.5f);
        }
    }
}
