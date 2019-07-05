using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMove : MonoBehaviour {

    public float translationSpeed = 1.0f;
    public float rotationSpeed = 10.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * translationSpeed);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * translationSpeed);
        transform.Translate(Vector3.up * Input.GetAxis("LeftTrigger") * translationSpeed);
        transform.Translate(Vector3.down * Input.GetAxis("RightTrigger") * translationSpeed);
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal2") * rotationSpeed, Space.World);
        transform.Rotate(Vector3.right, Input.GetAxis("Vertical2") * rotationSpeed, Space.Self);
    }
}
