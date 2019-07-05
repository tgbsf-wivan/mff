using UnityEngine;
using System.Collections;

public class BarcoEscape : MonoBehaviour {

	[Range(90.0f,180.0f)]
	public float escape_angle = 110.0f;

	public Camera camera_left;
	public Camera camera_center;
	public Camera camera_right;

	// Use this for initialization
	void Start () {

		camera_left.transform.localPosition = Vector3.zero;
		camera_center.transform.localPosition = Vector3.zero;
		camera_right.transform.localPosition = Vector3.zero;

		camera_left.transform.localRotation = Quaternion.identity;
		camera_center.transform.localRotation = Quaternion.identity;
		camera_right.transform.localRotation = Quaternion.identity;

		float hfov_angle = (180.0f - escape_angle);
		camera_left.transform.Rotate (0.0f, -hfov_angle, 0.0f);
		camera_right.transform.Rotate (0.0f, hfov_angle, 0.0f);

		camera_left.aspect = camera_center.aspect;
		camera_right.aspect = camera_center.aspect;

		float vfov_angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(Mathf.Tan((hfov_angle/2.0f) * Mathf.Deg2Rad) / camera_center.aspect);

		camera_left.fieldOfView = vfov_angle;
		camera_center.fieldOfView = vfov_angle;
		camera_right.fieldOfView = vfov_angle;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
