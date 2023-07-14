using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GradientManager : MonoBehaviour
{
    public VisualEffect Curve;

    public Gradient grad;

    public GameObject nodePrefab;

    private List<Transform> nodes;

    public Transform CenterDisc;
    private Material _discMaterial;

    private Material discMaterial{
        get{
            if(_discMaterial == null) _discMaterial = CenterDisc.GetComponent<MeshRenderer>().material;
            return _discMaterial;
        }
    }

    // Update is called once per frame
    private void Start() {
        Curve.SetGradient("Grad",grad);
        InitializeNodes();
    }

    public void OnGrab(){
        LeanTween.value(gameObject,0,1f,0.2f).setOnUpdate((float v) =>{
            discMaterial.SetFloat("_Radius",v);
        });
    }

    public void OnRelease(){
        LeanTween.value(gameObject,1,0f,0.2f).setOnUpdate((float v) =>{
            discMaterial.SetFloat("_Radius",v);
        });
    }


    public void setNodePos(Transform node, Color c){
        Vector3 HSV = Vector3.zero;
        Color.RGBToHSV(c, out HSV.x, out HSV.y, out HSV.z);
        Vector2 rotation = Vector2.zero;
        rotation.x = Mathf.Sin(HSV.x*Mathf.PI*2);
        rotation.y = Mathf.Cos(HSV.x*Mathf.PI*2);
        rotation *= HSV.y * 0.5f;
        node.localPosition = new Vector3(rotation.x,HSV.z - 0.5f, rotation.y);
    }

    private void ClearNodes(){
        if(nodes != null){
            foreach(Transform node in nodes){
                GameObject.Destroy(node.gameObject);
            }
        }
        nodes = new List<Transform>();
    }

    private Color PosToColor(Vector3 pos){
        Vector3 HSV = new Vector3();
        HSV.x = (Mathf.Atan2(pos.x,pos.z)+Mathf.PI)/(Mathf.PI*2);
        HSV.x = (HSV.x + 0.5f)%1;
        HSV.y = Vector2.Distance(Vector2.zero,new Vector2(pos.x,pos.z))*2;
        HSV.z = pos.y + 0.5f;
        return Color.HSVToRGB(HSV.x,HSV.y,HSV.z);
    }

    public void UpdateNodePos(Transform node){
        GradientColorKey[] gck = grad.colorKeys;
        for(int i = 0; i < gck.Length; i++){
            if(nodes[i] == node){
                Debug.Log(i);
                gck[i] = new GradientColorKey(PosToColor(node.localPosition),gck[i].time);
                grad.SetKeys(gck,grad.alphaKeys);
                CenterDisc.localPosition = new Vector3(0,node.localPosition.y,0);
                discMaterial.SetFloat("_Value",CenterDisc.localPosition.y+0.5f);
                break;
            }
        }
        Curve.SetGradient("Grad",grad);
    }

    private void InitializeNodes(){
        ClearNodes();
        for(int i = 0; i < grad.colorKeys.Length; i++){
            Transform node = GameObject.Instantiate(nodePrefab).transform;
            nodes.Add(node);
            node.gameObject.name = "Node_" + i;
            node.parent = transform;
            setNodePos(node,grad.colorKeys[i].color);
            node.localScale = new Vector3(0.03f,0.03f,0.03f);
            node.eulerAngles = Vector3.zero;
        }
    }


}
