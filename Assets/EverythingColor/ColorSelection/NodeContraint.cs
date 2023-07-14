using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContraint : MonoBehaviour
{
    private Transform Grabbable;
    private bool isGrabbed = false;
    
    private void Start() {
        Grabbable = transform.GetChild(0);
    }

    private void Update() {
        if(!isGrabbed) return;
        Vector2 flatPos = new Vector2(Grabbable.localPosition.x,Grabbable.localPosition.z);
        flatPos = Vector2.ClampMagnitude(flatPos,0.5f);
        transform.localPosition = new Vector3(flatPos.x,Mathf.Clamp(Grabbable.localPosition.y,-0.5f,0.5f),flatPos.y);
    }

    public void OnGrab(){
        isGrabbed = true;
        Grabbable.parent = transform.parent;
    }

    public void OnRelease(){
        isGrabbed = false;
        Grabbable.parent = transform;
        Grabbable.position = transform.position;
        Grabbable.localScale = new Vector3(1.7f,1.7f,1.7f);
    }

    public void setSize(float s){
        LeanTween.scale(gameObject, new Vector3(s,s,s),0.2f).setEase(LeanTweenType.easeSpring);
    }
}
