using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class ColorPalletExtractor : MonoBehaviour
{
    private int MAX_DEPTH = 5;
    public Texture2D img;
    public ComputeBuffer colorBuffer;

    public ComputeBuffer newColorBuffer;

    public RenderTexture outputImage;

    public ComputeShader closestColorIndexShader;
    public ComputeShader imageRecreator;

    private RenderTexture inputImage;
    public GameObject imagePlane;
    private Material _imgMat;

    public VisualEffect pointCloud;

    private ColorCloudManipulator manipulator;
    private Material imgMat{
        get{
            if(_imgMat == null) _imgMat = imagePlane.GetComponent<MeshRenderer>().material;
            return _imgMat;
        }
    }

    public Vector3[] colors;
    void Start()
    {
        Vector3[] photo = getColorArray(img,100,100); //convert photo to array of colors
        colors = getColorPallete(photo,0); //get the colors from a photo

        InitializeEditor();
        imgMat.SetTexture("_img",outputImage);

        manipulator = GetComponent<ColorCloudManipulator>();
        manipulator.InitializeColorNodes(colors);
        pointCloud.SetTexture("Source",outputImage);
    }

    private void Update() {
        if(manipulator.isGrabbed)updateColors();
    }

    private void updateColors(){
        newColorBuffer.SetData(colors);
        closestColorIndexShader.SetBuffer(0,"NewColors",newColorBuffer);

        closestColorIndexShader.Dispatch(0,inputImage.width,inputImage.height,1);
    }

    private void InitializeEditor(){
        //initialize a render texture that is a copy of our image
        inputImage = new RenderTexture(img.width,img.height,24);
        inputImage.enableRandomWrite = true;
        inputImage.Create();
        RenderTexture.active=inputImage;
        Graphics.Blit(img,inputImage);

        //initialize output buffer
        outputImage = new RenderTexture(img.width,img.height,24);
        outputImage.enableRandomWrite = true;
        outputImage.Create();

        //pass the color pallete into the shader
        colorBuffer = new ComputeBuffer(colors.Length,sizeof(float)*3);
        colorBuffer.SetData(colors);
        closestColorIndexShader.SetBuffer(0,"Colors",colorBuffer);

        //pass the color pallete into the shader
        newColorBuffer = new ComputeBuffer(colors.Length,sizeof(float)*3);
        newColorBuffer.SetData(colors);
        closestColorIndexShader.SetBuffer(0,"NewColors",newColorBuffer);

        closestColorIndexShader.SetFloat("PalleteSize",colors.Length);

        //pass the image into the shader
        closestColorIndexShader.SetTexture(0,"Input",inputImage);
        closestColorIndexShader.SetFloat("ResolutionX",inputImage.width);
        closestColorIndexShader.SetFloat("ResolutionY",inputImage.height);

        //stage output buffer
        closestColorIndexShader.SetTexture(0,"Output",outputImage);



        closestColorIndexShader.Dispatch(0,inputImage.width,inputImage.height,1);
    }

    private void mergeColor(Vector3[] array, int left, int mid, int right,int ValueToSortBy){
        int subArrayOne = mid-left+1;
        int subArrayTwo = right - mid;

        //create temp arrays
        Vector3[] leftArray = new Vector3[subArrayOne];
        Vector3[] rightArray = new Vector3[subArrayTwo];

        //copy data to temp sub arrays
        for(int i = 0; i < subArrayOne; i++){
            leftArray[i] = array[left+i];
        }
        for(int i = 0; i<subArrayTwo; i++){
            rightArray[i] = array[mid + 1 + i];
        }

        // Merge the temp arrays back into array
        int indexOne = 0;
        int indexTwo = 0;
        int indexOfMergedArray = left;

        while(indexOne < subArrayOne && indexTwo < subArrayTwo){
            if(leftArray[indexOne][ValueToSortBy] <= rightArray[indexTwo][ValueToSortBy]){
                array[indexOfMergedArray] = leftArray[indexOne];
                indexOne++;
            } 
            else{
                array[indexOfMergedArray] = rightArray[indexTwo];
                indexTwo++;
            }
            indexOfMergedArray++;
        }

        //copy remaining elements of left[] if there are any
        while(indexOne < subArrayOne){
            array[indexOfMergedArray] = leftArray[indexOne];
            indexOne++;
            indexOfMergedArray++;
        }

        //same for right array
        while(indexTwo < subArrayTwo){
            array[indexOfMergedArray] = rightArray[indexTwo];
            indexTwo++;
            indexOfMergedArray++;
        } 
    }

    private void mergeSortColor(Vector3[] array, int start, int end,int ValueToSortBy){
        if(start >= end) return;

        int mid = start + (end - start)/2;
        mergeSortColor(array, start, mid, ValueToSortBy);
        mergeSortColor(array,mid+1, end,ValueToSortBy);
        mergeColor(array,start,mid,end,ValueToSortBy);
    }

    private void printArray(Vector3[] array){
        Debug.Log("////////////////////////////////////////////////////////////////////////////");
        for(int i = 0; i < array.Length; i++){
            Debug.Log(array[i]);
        }
    }

    private Vector3[] getColorArray(Texture2D img, int xRes, int yRes){
        Vector3[] colorArray = new Vector3[xRes*yRes];
        int index = 0;
        for(int x = 0; x < xRes; x++){
            for(int y = 0; y < yRes; y++){
                Color c = img.GetPixel((int)((x/(float)xRes)*img.width),(int)((y/(float)yRes)*img.height));
                colorArray[index] = new Vector3(c.r,c.g,c.b);
                index++;
            }
        }
        return colorArray;
    }

    private int getLargestColorRange(Vector3[] colors){
        Vector3 minValues = new Vector3(1,1,1);
        Vector3 maxValues = Vector3.zero;

        for(int i = 0; i < colors.Length; i++){
            minValues[0] = Mathf.Min(minValues[0],colors[i][0]);
            minValues[1] = Mathf.Min(minValues[1],colors[i][1]);
            minValues[2] = Mathf.Min(minValues[2],colors[i][2]);

            maxValues[0] = Mathf.Max(maxValues[0],colors[i][0]);
            maxValues[1] = Mathf.Max(maxValues[1],colors[i][1]);
            maxValues[2] = Mathf.Max(maxValues[2],colors[i][2]);
        }

        Vector3 delta = new Vector3(maxValues[0] - minValues[0],maxValues[1]-minValues[1],maxValues[2]-minValues[2]);
        float largestRange = Mathf.Max(delta[0],delta[1],delta[2]);
        for(int i = 0; i < 3; i++){
            if(largestRange == delta[i]) return i;
        }
        return 0;
    }

    private Vector3[] getColorPallete(Vector3[] colors, int depth){
        Vector3[] colorPallete = new Vector3[1];
        if(depth >= MAX_DEPTH || colors.Length == 0){
            for(int i = 0; i < colors.Length; i++){
                colorPallete[0][0] += colors[i][0];
                colorPallete[0][1] += colors[i][1];
                colorPallete[0][2] += colors[i][2];
            }
            colorPallete[0][0] = colorPallete[0][0]/colors.Length;
            colorPallete[0][1] = colorPallete[0][1]/colors.Length;
            colorPallete[0][2] = colorPallete[0][2]/colors.Length;

            return colorPallete;
        }

        int largestColorRange = getLargestColorRange(colors);

        mergeSortColor(colors,0,colors.Length-1,largestColorRange);

        //create temp arrays
        Vector3[] leftArray = new Vector3[(int)colors.Length/2];
        Vector3[] rightArray = new Vector3[(int)colors.Length/2];

        for(int i = 0; i < colors.Length/2; i++){
            leftArray[i] = colors[i];
            rightArray[i] = colors[((int)colors.Length/2)+i];
        }

        leftArray = getColorPallete(leftArray,depth+1);
        rightArray = getColorPallete(rightArray,depth+1);

        colorPallete = new Vector3[leftArray.Length + rightArray.Length];
        int index = 0;
        for(int i = 0; i < leftArray.Length; i ++){
            colorPallete[index] = leftArray[i];
            index++;
        }
        for(int i = 0; i < rightArray.Length; i++){
            colorPallete[index] = rightArray[i];
            index++;
        }
        return colorPallete;

    }
}
