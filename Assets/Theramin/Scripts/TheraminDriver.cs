using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TheraminDriver : MonoBehaviour
{
    public Transform driver1;
    public Transform driver2;
    public VisualEffect FX;

    public float rotationRate;

    private void Start() {
    }
    // Update is called once per frame
    void Update()
    {
        float rot = FX.transform.eulerAngles.y;
        rot += rotationRate;
        // FX.transform.eulerAngles = new Vector3(0,rot,90);
        Vector3 pos = driver1.localPosition;

        FX.SetFloat("Driver 0",Vector3.Angle(transform.up,pos)/180f);
        float azimuth = Mathf.Atan(Mathf.Sqrt((Mathf.Pow(pos.x,2) + Mathf.Pow(pos.y,2)))/pos.z)*Mathf.Rad2Deg;
        if(azimuth < 0) azimuth += 180;
        FX.SetFloat("Driver 1",azimuth/180);

        pos = driver2.localPosition;
        FX.SetFloat("Driver 2",Vector3.Angle(transform.up,pos)/180f);
        azimuth = Mathf.Atan(Mathf.Sqrt((Mathf.Pow(pos.x,2) + Mathf.Pow(pos.y,2)))/pos.z)*Mathf.Rad2Deg;
        if(azimuth < 0) azimuth += 180;
        FX.SetFloat("Driver 3",azimuth/180);
    }
}
