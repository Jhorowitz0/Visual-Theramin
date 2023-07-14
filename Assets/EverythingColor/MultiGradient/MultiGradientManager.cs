using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MultiGradientManager : MonoBehaviour
{
    public Gradient gradient;
    public List<Transform> nodes;
    public VisualEffect blend;
    // Start is called before the first frame update

    // Update is called once per frame


    void Update()
    {
        GradientColorKey[] keys = new GradientColorKey[5];
        for(int i = 0; i < 5; i++){
            Color c = PosToColor(nodes[i].localPosition);
            keys[i] = new GradientColorKey(c,i*0.25f);
        }
        gradient.SetKeys(keys,gradient.alphaKeys);
        blend.SetGradient("grad",gradient);
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
