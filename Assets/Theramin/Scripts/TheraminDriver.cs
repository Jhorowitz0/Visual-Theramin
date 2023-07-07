using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TheraminDriver : MonoBehaviour
{
    public Transform driver1;
    public VisualEffect FX;

    public float rotationRate;

    private void Start() {
    }
    // Update is called once per frame
    void Update()
    {
        float rot = FX.transform.eulerAngles.y;
        rot += rotationRate;
        FX.transform.eulerAngles = new Vector3(0,rot,90);
        FX.SetFloat("Driver 0",Vector3.Angle(transform.up,driver1.localPosition)/180);
        FX.SetFloat("Driver 1",Vector3.Angle(transform.forward,driver1.localPosition)/180);
        FX.SetFloat("Driver 2",Vector3.Angle(transform.right,driver1.localPosition)/180);
        FX.SetFloat("Driver 3",Vector3.Angle(transform.up,driver1.up)/180);
        FX.SetFloat("Driver 4",Vector3.Angle(transform.forward,driver1.forward)/180);
    }
}
