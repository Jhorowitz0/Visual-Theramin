using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

public class ColorCloudManipulator : MonoBehaviour
{
    private Transform[] nodes;
    private float[] offsetStrength;
    private Vector3 startPosition;
    private Vector3[] nodeStartingPositions;

    public float radius = 0.1f;

    public bool isGrabbed = false;

    public Transform Manipulator;
    public Transform IndexTracker;
    public Transform ThumbTracker;

    public GameObject nodePrefab;

    private Transform nodeStorage;

    private ColorPalletExtractor image;


    private void Start() {
        image = gameObject.GetComponent<ColorPalletExtractor>();
        nodeStorage = new GameObject().transform;
        nodeStorage.gameObject.name = "Nodes";
        nodeStorage.parent = transform;
        nodeStorage.localScale = new Vector3(1,1,1);
        nodeStorage.eulerAngles = Vector3.zero;
        nodeStorage.localPosition = Vector3.zero;
    }

    private void Update() {
        if(isGrabbed){
            Vector3 offsetVector = Manipulator.position - startPosition;
            for(int i = 0; i < nodes.Length; i++){
                nodes[i].position = nodeStartingPositions[i] + offsetVector*offsetStrength[i];
                clampNodePosition(nodes[i]);
                Color c = PosToColor(nodes[i].localPosition);
                image.colors[i] = new Vector3(c.r,c.g,c.b);
                nodes[i].GetComponent<MeshRenderer>().material.SetColor("_Color",PosToColor(nodes[i].localPosition));
            }
        } else {
            Manipulator.position = (IndexTracker.position + ThumbTracker.position)/2f;
        }
    }

    public void clearNodes(){
        if(nodes==null)return;
        foreach(Transform node in nodes){
            GameObject.Destroy(node.gameObject);
        }
        nodes = null;
    }


    public void InitializeColorNodes(Vector3[] colors){
        clearNodes();
        nodes = new Transform[colors.Length];
        offsetStrength = new float[colors.Length];
        nodeStartingPositions = new Vector3[colors.Length];
        for(int i = 0; i < colors.Length; i++){
            Transform node = GameObject.Instantiate(nodePrefab).transform;
            nodes[i] = node;
            node.parent = nodeStorage;
            Vector3 c = colors[i];
            Color color = new Color(c.x,c.y,c.z);
            node.GetComponent<MeshRenderer>().material.SetColor("_Color",color);
            setNodePos(node,color);
        }
    }

    public void OnGrab(){
        isGrabbed = true;
        startPosition = Manipulator.position;
        for(int i = 0; i < nodes.Length; i++){
            nodeStartingPositions[i] = nodes[i].position;
            offsetStrength[i] = calculateOffsetStrength(startPosition,nodes[i].position);
        }
    }

    public void OnRelease(){
        isGrabbed = false;
    }

    private float calculateOffsetStrength(Vector3 pos1, Vector3 pos2){
        float distance = Vector3.Distance(pos1,pos2);
        distance = Mathf.Clamp(distance,0,radius)/radius;
        return 1-distance;
    }

    public void setRadius(float r){
        radius = r;
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

    private Color PosToColor(Vector3 pos){
        Vector3 HSV = new Vector3();
        HSV.x = (Mathf.Atan2(pos.x,pos.z)+Mathf.PI)/(Mathf.PI*2);
        HSV.x = (HSV.x + 0.5f)%1;
        HSV.y = Vector2.Distance(Vector2.zero,new Vector2(pos.x,pos.z))*2;
        HSV.z = pos.y + 0.5f;
        return Color.HSVToRGB(HSV.x,HSV.y,HSV.z);
    }

    public void clampNodePosition(Transform node){
        Vector2 flatPos = new Vector2(node.localPosition.x,node.localPosition.z);
        flatPos = Vector2.ClampMagnitude(flatPos,0.5f);
        node.localPosition = new Vector3(flatPos.x,Mathf.Clamp(node.localPosition.y,-0.5f,0.5f),flatPos.y);
    }

}
