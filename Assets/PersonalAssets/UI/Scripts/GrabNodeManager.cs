using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class GrabNodeManager : MonoBehaviour
{
    private ObjectManipulator _manipulator;
    private ObjectManipulator manipulator{
        get{
            if(_manipulator == null)_manipulator = gameObject.GetComponent<ObjectManipulator>();
            return _manipulator;
        }
    }

    private GameObject _outerVisuals;
    private GameObject outerVisuals{
        get{
            if(_outerVisuals == null) _outerVisuals = transform.Find("OuterVisuals").gameObject;
            return _outerVisuals;
        }
    }

    private GameObject _innerVisuals;
    private GameObject innerVisuals{
        get{
            if(_innerVisuals == null) _innerVisuals = transform.Find("InnerVisuals").gameObject;
            return _innerVisuals;
        }
    }

    private Material _outerMaterial;
    private Material outerMaterial{
        get{
            if(_outerMaterial == null){
                if(outerVisuals == null) Debug.LogError("NodeGrabber cannot find outer visuals gameobject");
                _outerMaterial = outerVisuals.GetComponent<MeshRenderer>().material;
            }
            return _outerMaterial;
        }
    }
    private bool isGrabbed = false;
    private void Start() {
        manipulator.OnManipulationStarted.AddListener(OnGrab);
        manipulator.OnManipulationEnded.AddListener(OnRelease);
        manipulator.OnHoverEntered.AddListener(OnHover);
        manipulator.OnHoverExited.AddListener(OffHover);
        outerMaterial.SetFloat("_Thickness",0);
    }

    public bool GetIsGrabbed(){
        return isGrabbed;
    }

    public ObjectManipulator GetManipulator(){
        return manipulator;
    }
    private void OnGrab(ManipulationEventData data){
        isGrabbed = true;
        LeanTween.scale(innerVisuals,new Vector3(1,1,1),0.2f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.scale(outerVisuals, new Vector3(0.5f,0.5f,0.5f),0.2f).setEase(LeanTweenType.easeInOutQuad);
    }

    private void OnRelease(ManipulationEventData data){
        isGrabbed = false;
        LeanTween.scale(innerVisuals,new Vector3(1.5f,1.5f,1.5f),0.2f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.scale(outerVisuals, new Vector3(1f,1f,1f),0.2f).setEase(LeanTweenType.easeInOutQuad);
    }

    private void OnHover(ManipulationEventData data){
        LeanTween.value(gameObject,0,0.3f,0.2f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float v)=>{
            outerMaterial.SetFloat("_Thickness",v);
        });
        LeanTween.scale(innerVisuals,new Vector3(1.5f,1.5f,1.5f),0.2f).setEase(LeanTweenType.easeInOutQuad);
    }

    private void OffHover(ManipulationEventData data){
        LeanTween.value(gameObject,0.3f,0,0.2f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float v)=>{
            outerMaterial.SetFloat("_Thickness",v);
        });
        LeanTween.scale(innerVisuals,new Vector3(1,1,1),0.1f).setEase(LeanTweenType.easeInOutQuad);
    }
}
