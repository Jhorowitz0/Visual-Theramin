using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject[] list1;
    public GameObject[] list2;
    private bool state = true;
    public void Toggle(){
        state = !state;
        foreach(GameObject obj in list1){
            obj.SetActive(state);
        }
        foreach(GameObject obj in list2){
            obj.SetActive(!state);
        }
    }
}
