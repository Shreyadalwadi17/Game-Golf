using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClubGrab : MonoBehaviour
{
    public Rigidbody rb;
    private FixedJoint fixedJoint;
    //private const String grabbedBatLayer = "GrabbedBat";
    //private float HapticsAmplitude = 0.5f;
    //private float HapticsDuration = 0.3f;

    private XRGrabInteractable xRGrabInteractable;
    private XRBaseController xRBaseController;

    public bool isGrabbed;


    void Start()
    {
        isGrabbed = true;
        xRGrabInteractable = GetComponent<XRGrabInteractable>();
        xRGrabInteractable.selectEntered.AddListener(OnGrab);

        rb = GetComponent<Rigidbody>();


    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        Debug.Log(arg0.interactorObject);
        Debug.Log("Club Grabbed");

        rb.isKinematic = false;

        XRBaseController xRController = arg0.interactorObject.transform.GetComponent<XRBaseController>();
        xRBaseController = xRController;

        //fixedJoint = arg0.interactorObject.transform.GetComponent<FixedJoint>();
        Invoke(nameof(AttachFixedJoint), 0.1f);


    }
    public void AttachFixedJoint()
    {
        if (fixedJoint != null && isGrabbed)
        {
            Debug.Log("Fixed Joint Found");
            fixedJoint.connectedBody = rb;
            isGrabbed = false;

            //movementType = MovementType.VelocityTracking;
        }
        else
        {
            Debug.Log("No Fixed Joint");
        }
    }

    //public void Triggerhaptics()
    //{
    //    if (xRBaseController != null)
    //    {
    //        Debug.Log("XRController Found");

    //        xRBaseController.SendHapticImpulse(HapticsAmplitude, HapticsDuration);
    //    }
    //    else
    //    {
    //        Debug.Log("XRController Not Found");
    //    }
    //}
}
