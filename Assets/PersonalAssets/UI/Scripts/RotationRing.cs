using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class RotationRing : MonoBehaviour
{
    private float startTheta = 0f; //the current angle on each grab
    private float contentTheta = 0f; //the current content angle on each grab

    private Transform _rotationContent; 
    private Transform rotationContent{ //the content to rotate when the slider is turned
        get{
            if(_rotationContent == null) _rotationContent = transform.Find("Content");
            return _rotationContent;
        }
    }

    private Transform _blank; 
    private Transform blank{ //used as a placeholder for MRTK's movement on object manipulation
        get{
            if(_blank == null){
                _blank = new GameObject("blank").transform;
                _blank.parent = transform;
                _blank.localPosition = Vector3.zero;
                _blank.eulerAngles = Vector3.zero;
            }
            return _blank;
        }
    }

    private Transform _node;
    private Transform node{
        get{
            if(_node == null) _node = transform.Find("Visuals").Find("NodeWrapper");
            return _node;
        }
    }

    private GrabNodeManager _grabber;
    private GrabNodeManager grabber{
        get{
            if(_grabber == null) _grabber = node.Find("GrabNode").gameObject.GetComponent<GrabNodeManager>();
            return _grabber;
        }
    }

    private void Start() {
        grabber.GetManipulator().OnManipulationStarted.AddListener((ManipulationEventData data)=>{
            rotationContent.parent = node;
        });
        grabber.GetManipulator().OnManipulationEnded.AddListener((ManipulationEventData data)=>{
            rotationContent.parent = transform;
        });
        grabber.GetManipulator().HostTransform = blank;
    }

    private void Update() {
        rotateTowardsNearestFingers();
    }

     void rotateTowardsNearestFingers(){ //rotates towards the closest grab point to make the handle very easy to grab
        Handedness closestHand = getClosestHand();
        var handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        Vector3 TargetPos = (handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, closestHand).position +
                            handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, closestHand).position )/2;
        Vector3 TargetDir = node.position - TargetPos;
        if(Vector3.RotateTowards(node.forward,TargetDir,5f,5f) == Vector3.zero)return;
        node.rotation = Quaternion.LookRotation(Vector3.RotateTowards(node.forward,TargetDir,5f,5f));
        node.eulerAngles = new Vector3(0,node.eulerAngles.y,0f);
    }

        Handedness getClosestHand(){ //gets the closest hand
        var handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        Vector3 rightHandPos = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right).position;
        Vector3 leftHandPos = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left).position;

        Vector3 sliderPos = Vector3.zero;
        try {sliderPos = node.GetChild(0).position;}
        catch(Exception e){
            Debug.LogError("node incorrectly set up, make sure it has a child with object manipulator");
            Debug.LogError(node);
        }

        if(Vector3.Distance(rightHandPos,sliderPos) < Vector3.Distance(leftHandPos,sliderPos)) return Handedness.Right;
        else return Handedness.Left;
    }


}
