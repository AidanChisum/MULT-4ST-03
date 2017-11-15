using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairMovementManager : MonoBehaviour {

    public GameObject leftController;
    public GameObject rightController;

    private Vector3 leftPosition;
    private Vector3 rightPosition;
    private Vector3 tempLeftPosition;
    private Vector3 tempRightPosition;
    private float leftVelocity;
    private float rightVelocity;
    

    // Use this for initialization
    void Start () {
        leftPosition = leftController.transform.localPosition;
        rightPosition = rightController.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        tempLeftPosition = leftPosition;
        tempRightPosition = rightPosition;
        leftPosition = leftController.transform.localPosition;
        rightPosition = rightController.transform.localPosition;

	}

    public float GetLeftVelocity()
    {
        return leftVelocity;
    }
    public float GetRightVelocity()
    {
        return rightVelocity;
    }
}
