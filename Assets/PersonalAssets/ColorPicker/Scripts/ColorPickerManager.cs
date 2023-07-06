using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerManager : MonoBehaviour
{
    private Transform _plane;
    private Transform plane{
        get{
            if(_plane == null) _plane = transform.Find("ColorPlane");
            return _plane;
        }
    }

    public MenuManager Menu;

    private Material _planeMat;

    private Material planeMat{
        get{
            if(_planeMat == null) _planeMat = plane.gameObject.GetComponent<MeshRenderer>().material;
            return _planeMat;
        }
    }
    
    private NodeManager currentNode = null;

    private NodeManager lastNode = null;
    private void Update() {
        if(currentNode != null){
            plane.localPosition = new Vector3(plane.localPosition.x,plane.localPosition.y,currentNode.transform.localPosition.z);
            planeMat.SetVector("_Pos",currentNode.transform.localPosition + new Vector3(0,0,0.5f));

            Vector3 nodePos = currentNode.transform.localPosition;
            nodePos += new Vector3(0.5f,0.5f,0.5f);
            Color nodeColor = Color.HSVToRGB(nodePos.z,nodePos.x,nodePos.y);
            Menu.updateColor(nodeColor);
        }
    }

    private void Start() {
        updateLamp();
    }

    public void setColor(Color c){
        if(lastNode != null && !lastNode.isGrabbed){
            Vector3 pos = Vector3.zero;
            Color.RGBToHSV(c,out pos.z,out pos.x,out pos.y);
            pos -= new Vector3(0.5f,0.5f,0.5f);
            lastNode.transform.localPosition = pos;
            lastNode.updateColor();
        }
    }

    public void onNodeDrag(NodeManager node){
        currentNode = node;
        lastNode = node;
        plane.GetChild(0).gameObject.SetActive(true);
        LeanTween.value(gameObject,0,1f,0.2f).setOnUpdate((float v)=>{
            planeMat.SetFloat("_Radius",v);
        }).setOnComplete(()=>{
            planeMat.SetFloat("_Radius",1.5f);
        });
        Menu.setTitle(node.gameObject.name);
    }

    public void onNodeRelease(){
        currentNode = null;
        plane.GetChild(0).gameObject.SetActive(false);
        LeanTween.value(gameObject,1f,0f,0.6f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float v)=>{
            planeMat.SetFloat("_Radius",v);
        });
    }

    private void updateLamp(){
        Debug.Log(lastNode);
        if(lastNode != null) Debug.Log(lastNode.lamp);
        if(lastNode != null && lastNode.lamp != null){
            Vector3 HSV = lastNode.transform.localPosition + new Vector3(0.5f,0.5f,0.5f);
            lastNode.lamp.updateColor(Color.HSVToRGB(HSV.z,HSV.x,HSV.y));
            Debug.Log("lamp update??");
        }

        LeanTween.value(gameObject,0,1,1f).setOnComplete(()=>{updateLamp();});
    }


}
