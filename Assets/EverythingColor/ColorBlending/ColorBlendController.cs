using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ColorBlendController : MonoBehaviour
{
    public Transform node1;
    public Transform node2;

    public VisualEffect blend;

    // Update is called once per frame
    void Update()
    {
        blend.SetVector4("Color1",PosToColor(node1.localPosition));
        blend.SetVector4("Color2",PosToColor(node2.localPosition));
    }



    private Color PosToColor(Vector3 pos){
        Vector3 HSV = new Vector3();
        HSV.x = (Mathf.Atan2(pos.x,pos.z)+Mathf.PI)/(Mathf.PI*2);
        HSV.x = (HSV.x + 0.5f)%1;
        HSV.y = Vector2.Distance(Vector2.zero,new Vector2(pos.x,pos.z))*2;
        HSV.z = pos.y + 0.5f;
        return Color.HSVToRGB(HSV.x,HSV.y,HSV.z);
    }

    public void toggleSplit(){
        if(blend.GetFloat("isSplit")>0.5f){
            LeanTween.value(gameObject,1,0,0.5f).setEase(LeanTweenType.easeInQuad).setOnUpdate((float v)=>{
                blend.SetFloat("isSplit",v);
                node2.gameObject.SetActive(true);
            });
        } else {
            LeanTween.value(gameObject,0,1,0.5f).setEase(LeanTweenType.easeInQuad).setOnUpdate((float v)=>{
                blend.SetFloat("isSplit",v);
                node2.gameObject.SetActive(false);
            });
        }
    }

    public void toggleSag(){
        if(blend.GetFloat("isStraight")>0.5f){
            LeanTween.value(gameObject,1,0,0.5f).setEase(LeanTweenType.easeInQuad).setOnUpdate((float v)=>{
                blend.SetFloat("isStraight",v);
            });
        } else {
            LeanTween.value(gameObject,0,1,0.5f).setEase(LeanTweenType.easeInQuad).setOnUpdate((float v)=>{
                blend.SetFloat("isStraight",v);
            });
        }
    }
}
