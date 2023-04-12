using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SwitchHandsController : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public Transform leftHandAttach;
    public Transform rightHandAttach;
    public GameObject leftHandGrabInteractor;
    public GameObject rightHandGrabInteractor;

    public GameObject leftHandRayInteractor;
    public GameObject rightHandRayInteractor;

    // Start is called before the first frame update
    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void ResetHands()
    {
        Debug.Log("Reset Grab");
        grabInteractable.attachTransform = rightHandAttach;
    }

    public void SwapHands()
    {
        IXRSelectInteractor interactor = grabInteractable.GetOldestInteractorSelecting();
        Debug.Log(interactor.transform.gameObject.name);
        if(interactor.transform.gameObject.name == leftHandGrabInteractor.name|| interactor.transform.gameObject.name == leftHandRayInteractor.name)
        {
            Debug.Log("Left Grab");
            grabInteractable.attachTransform = leftHandAttach;
        }

        if (interactor.transform.gameObject.name == rightHandGrabInteractor.name || interactor.transform.gameObject.name == rightHandRayInteractor.name)
        {
            Debug.Log("Right Grab");
            grabInteractable.attachTransform = rightHandAttach;
        }
    }
}
