using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class SliderManager : MonoBehaviour
{
    public Transform grabNode;
    public Transform displayNode;

    public UnityEvent<float> onValueUpdate;

    private bool isGrabbed;

    public float displayScale = 0.04f;
    public float displayScaleLarge = 0.07f;

    private float value = 0;
    private void Update() {
        if(isGrabbed){
            onValueUpdate.Invoke(value);
            displayNode.localPosition = new Vector3(Mathf.Clamp(grabNode.localPosition.x,0,1),0,0);
            value = displayNode.localPosition.x;
        }
    }

    public void setIsGrabbed(bool state){
        isGrabbed = state;
        if(!state) grabNode.position = displayNode.position;
    }

    public void GrowDisplay(){
        LeanTween.scale(displayNode.gameObject,new Vector3(displayScaleLarge,displayScaleLarge,displayScaleLarge),0.2f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void shrinkDisplay(){
        LeanTween.scale(displayNode.gameObject,new Vector3(displayScale,displayScale,displayScale),0.2f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void setValue(float v){
        if(isGrabbed)return;
        displayNode.localPosition = new Vector3(v, 0,0);
        grabNode.localPosition = displayNode.localPosition;
    }
}
