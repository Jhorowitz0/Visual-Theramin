using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class NoiseManipulatorManager : MonoBehaviour
{
    public VisualEffect noiseFX;

    public GameObject FrontShell;
    private Material _frontMaterial;
    private Material frontMaterial{
        get{
            if(_frontMaterial == null) _frontMaterial = FrontShell.GetComponent<MeshRenderer>().material;
            return _frontMaterial;
        }
    }

    public GameObject BackShell;
    private Material _backMaterial;
    private Material backMaterial{
        get{
            if(_backMaterial == null) _backMaterial = BackShell.GetComponent<MeshRenderer>().material;
            return _backMaterial;
        }
    }

    private float scale;

    private bool isGrabbed;

    private Vector3 offset;
    
    public void OnGrab(){
        isGrabbed = true;
        scale = noiseFX.GetFloat("Scale");
        offset = noiseFX.GetVector3("Offset");
    }
    public void OnRelease(){
        isGrabbed = false;
        transform.localPosition = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = new Vector3(1f,1f,1f);
    }

    private void Update() {
        noiseFX.SetVector3("PositionalOffset",transform.parent.position);
        if(isGrabbed){
            noiseFX.SetFloat("Scale",scale + transform.localScale.x);
            noiseFX.SetVector3("Offset",offset + transform.localPosition);
        }
    }
}
