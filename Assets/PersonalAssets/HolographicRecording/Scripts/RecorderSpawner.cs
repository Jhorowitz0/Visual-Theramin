using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderSpawner : MonoBehaviour
{
    public Transform QR;
    public GameObject RecorderPrefab;
    private void OnEnable() {
        Transform recorder = GameObject.Instantiate(RecorderPrefab).transform;
        LeanTween.value(gameObject,0,1,3).setOnComplete(()=>{
            recorder.parent = transform;
            recorder.localScale = new Vector3(QR.localScale.x,QR.localScale.x,QR.localScale.x);
            recorder.localPosition = QR.localPosition;
            recorder.rotation = QR.rotation;
        });   
    }
}
