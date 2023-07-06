using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public SliderManager[] sliders;
    public GameObject ColorTab;
    private Material _colorTab;

    public TextMeshPro Hex;
    public TextMeshPro Title;

    private Color savedColor;

    private Material colorTab{
        get{
            if(_colorTab == null) _colorTab = ColorTab.GetComponent<MeshRenderer>().material;
            return _colorTab;
        }
    }

    private void Start() {
        updateColor(Color.HSVToRGB(0,0,0));
    }

    public ColorPickerManager ColorPicker;


    public void updateColor(Color newColor){

        savedColor = newColor;
        colorTab.SetColor("_Color",newColor);
        Hex.text = "#" + ColorUtility.ToHtmlStringRGB(newColor);
        Vector3 HSV = Vector3.zero;
        Color.RGBToHSV(newColor,out HSV.x,out HSV.y,out HSV.z);
        sliders[0].setValue(HSV.x);
        sliders[1].setValue(HSV.y);
        sliders[2].setValue(HSV.z);
        sliders[3].setValue(newColor.r);
        sliders[4].setValue(newColor.g);
        sliders[5].setValue(newColor.b);
    }

    public void setTitle(string name){
        Title.text = name;
    }

    public void UpdateHue(float value){
        Vector3 HSV = Vector3.zero;
        Color.RGBToHSV(savedColor,out HSV.x,out HSV.y,out HSV.z);
        updateColor(Color.HSVToRGB(value,HSV.y,HSV.z));
        ColorPicker.setColor(Color.HSVToRGB(value,HSV.y,HSV.z));
    }

    public void UpdateSat(float value){
        Vector3 HSV = Vector3.zero;
        Color.RGBToHSV(savedColor,out HSV.x,out HSV.y,out HSV.z);
        updateColor(Color.HSVToRGB(HSV.x,value,HSV.z));
        ColorPicker.setColor(Color.HSVToRGB(HSV.x,value,HSV.z));
    }

    public void UpdateVal(float value){
        Vector3 HSV = Vector3.zero;
        Color.RGBToHSV(savedColor,out HSV.x,out HSV.y,out HSV.z);
        updateColor(Color.HSVToRGB(HSV.x,HSV.y,value));
        ColorPicker.setColor(Color.HSVToRGB(HSV.x,HSV.y,value));
    }

    public void UpdateRed(float value){
        updateColor(new Color(value,savedColor.g,savedColor.b));
        ColorPicker.setColor(new Color(value,savedColor.g,savedColor.b));
    }

    public void UpdateBlue(float value){
        updateColor(new Color(savedColor.r,savedColor.g,value));
        ColorPicker.setColor(new Color(savedColor.r,savedColor.g,value));
    }

    public void UpdateGreen(float value){
        ColorPicker.setColor(new Color(savedColor.r,value,savedColor.b));
        updateColor(new Color(savedColor.r,value,savedColor.b));
    }
}
