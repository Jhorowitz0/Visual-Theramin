using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderTester : MonoBehaviour
{
    public ComputeShader shader;
    private RenderTexture texture;
    public GameObject imagePlane;
    // Start is called before the first frame update
    void Start()
    {
        texture = new RenderTexture(255,255,24);
        texture.enableRandomWrite = true;
        texture.Create();

        shader.SetTexture(0,"Result",texture);
        shader.SetFloat("Resolution",texture.width);
        shader.Dispatch(0,texture.width,texture.height,1);

        Material imgMat = imagePlane.GetComponent<MeshRenderer>().material;
        imgMat.SetTexture("_img",texture);
    }
}
