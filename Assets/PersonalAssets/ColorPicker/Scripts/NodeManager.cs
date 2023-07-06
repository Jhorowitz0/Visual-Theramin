using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class NodeManager : MonoBehaviour
{
    private ObjectManipulator _manipulator;
    private ObjectManipulator manipulator{
        get{
            if(_manipulator == null)_manipulator = gameObject.GetComponent<ObjectManipulator>();
            return _manipulator;
        }
    }

    private Material _mat;
    private Material mat{
        get{
            if(_mat == null) _mat = gameObject.GetComponent<MeshRenderer>().material;
            return _mat;
        }
    }

    private Transform _target;
    private Transform target{
        get{
            if(_target == null){
                _target = new GameObject().transform;
                _target.transform.parent = transform;
                _target.transform.localPosition = Vector3.zero;
            } return _target;
        }
    }

    public HueLamp lamp = null;


    public bool isGrabbed = false;

    private void Start() {
        manipulator.OnManipulationStarted.AddListener(OnGrab);
        manipulator.OnManipulationEnded.AddListener(OnRelease);
        manipulator.OnHoverEntered.AddListener(OnHover);
        manipulator.OnHoverExited.AddListener(OffHover);
        manipulator.HostTransform = target;
        mat.SetVector("_Pos",transform.localPosition + new Vector3(0.5f, 0.5f,0.5f));
        setLampColor();
    }

    private void Update() {
        if(isGrabbed){
            Debug.Log(gameObject.name + "::: " + lamp);
            transform.position = target.position;
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x,-0.5f,0.5f),Mathf.Clamp(transform.localPosition.y,-0.5f,0.5f),Mathf.Clamp(transform.localPosition.z,-0.5f,0.5f));
            mat.SetVector("_Pos",transform.localPosition + new Vector3(0.5f, 0.5f,0.5f));
            adjustLamp();
        }
    }

    public void updateColor(){
        mat.SetVector("_Pos",transform.localPosition + new Vector3(0.5f, 0.5f,0.5f));
        Debug.Log("updated");
        adjustLamp();
    }

    private void OnHover(ManipulationEventData data){
        LeanTween.value(gameObject,0.1f,0.2f,0.2f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float v)=>{
            mat.SetFloat("_Thickness",v);
        });
    }

    private void OffHover(ManipulationEventData data){
        LeanTween.value(gameObject,0.2f,0.1f,0.2f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float v)=>{
            mat.SetFloat("_Thickness",v);
        });
    }

    private void OnGrab(ManipulationEventData data){
        // Debug.Log("grabbed");z
        transform.parent.gameObject.GetComponent<ColorPickerManager>().onNodeDrag(this);
        target.parent = null;
        isGrabbed = true;
        mat.SetInt("_IsGrabbed",1);
        LeanTween.value(gameObject,0.2f,0.05f,0.2f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float v)=>{
            mat.SetFloat("_Thickness",v);
        });
    }

    private void OnRelease(ManipulationEventData data){
        target.parent = transform;
        target.localPosition = Vector3.zero;
        transform.parent.gameObject.GetComponent<ColorPickerManager>().onNodeRelease();
        isGrabbed = false;
        mat.SetInt("_IsGrabbed",0);
        LeanTween.value(gameObject,0.1f,0.2f,0.2f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float v)=>{
            mat.SetFloat("_Thickness",v);
        });
    }

    public void adjustLamp(){
        // if(lamp == null) return;
        // if(!canLampUpdate) return;
        // canLampUpdate = false;
        // Vector3 HSV = transform.localPosition + new Vector3(0.5f,0.5f,0.5f);
        // lamp.updateColor(Color.HSVToRGB(HSV.z,HSV.x,HSV.y));
        // // lamp.color = Color.HSVToRGB(HSV.z,HSV.x,HSV.y);
        // LeanTween.value(gameObject,0,1,1f).setOnComplete(()=>{canLampUpdate = true;});
    }

    public void setLampColor(){
        // if(lamp != null){
        //     Vector3 HSV = transform.localPosition + new Vector3(0.5f,0.5f,0.5f);
        //     // lamp.updateColor(Color.HSVToRGB(HSV.z,HSV.x,HSV.y));
        //     lamp.color = Color.HSVToRGB(HSV.z,HSV.x,HSV.y);
        //     Debug.Log("lamp update??");
        // }

        // LeanTween.value(gameObject,0,1,1f).setOnComplete(()=>{setLampColor();});
    }
}
