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
        Vector3 rotation = FX.transform.eulerAngles;
        rotation.y += rotationRate;
        FX.transform.eulerAngles = rotation;
        FX.SetFloat("Driver 0",Input.GetAxis("LeftX")*0.5f+0.5f);
        FX.SetFloat("Driver 1",Input.GetAxis("LeftY")*0.5f+0.5f);
        FX.SetFloat("Driver 2",Input.GetAxis("RightX")*0.5f+0.5f);
        FX.SetFloat("Driver 3",Input.GetAxis("RightY")*0.5f+0.5f);
    }
}
