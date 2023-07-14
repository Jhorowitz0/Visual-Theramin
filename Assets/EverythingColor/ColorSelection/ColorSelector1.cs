using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector1 : MonoBehaviour
{
    public Transform node;

    public Transform CenterDisc;
    private Material _discMaterial;
    bool isGrabbed = false;


    public void setSelectedNode(Transform newNode){
        node = newNode;
    }
    private Material discMaterial{
        get{
            if(_discMaterial == null) _discMaterial = CenterDisc.GetComponent<MeshRenderer>().material;
            return _discMaterial;
        }
    }

    private void Update() {
        if(isGrabbed && node != null){
            CenterDisc.localPosition = new Vector3(0,node.localPosition.y,0);
            discMaterial.SetFloat("_Value",CenterDisc.localPosition.y+0.5f);
            node.GetComponent<MeshRenderer>().material.SetColor("_Color",PosToColor(node.localPosition));
        }
    }

    public void OnGrab(){
        isGrabbed = true;
        LeanTween.value(gameObject,0,0.99f,0.2f).setOnUpdate((float v) =>{
            discMaterial.SetFloat("_Radius",v);
            node.GetComponent<MeshRenderer>().material.SetFloat("_Fill",v);
        });
    }

    public void OnRelease(){
        isGrabbed = false;
        LeanTween.value(gameObject,0.99f,0f,0.2f).setOnUpdate((float v) =>{
            discMaterial.SetFloat("_Radius",v);
            node.GetComponent<MeshRenderer>().material.SetFloat("_Fill",v);
        });
    }

    private Color PosToColor(Vector3 pos){
        Vector3 HSV = new Vector3();
        HSV.x = (Mathf.Atan2(pos.x,pos.z)+Mathf.PI)/(Mathf.PI*2);
        HSV.x = (HSV.x + 0.5f)%1;
        HSV.y = Vector2.Distance(Vector2.zero,new Vector2(pos.x,pos.z))*2;
        HSV.z = pos.y + 0.5f;
        return Color.HSVToRGB(HSV.x,HSV.y,HSV.z);
    }
}
