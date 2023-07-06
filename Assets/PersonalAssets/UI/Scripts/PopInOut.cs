using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopInOut : MonoBehaviour
{
    private Vector3 scale = Vector3.zero;
    public float animationDelay = 0f;
    public float animationDuration = 0.2f;
    private void OnEnable() {
        if(scale == Vector3.zero) scale = transform.localScale;
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject,scale,animationDuration).setDelay(animationDelay);
    }

    public void Hide(){
        scale = transform.localScale;
        LeanTween.scale(gameObject,Vector3.zero,animationDuration).setDelay(animationDelay).setOnComplete(()=>{
            gameObject.SetActive(false);
        });
    }
}
