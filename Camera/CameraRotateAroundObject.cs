using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script should be attached to a camera. It keeps an object in view and rotates around it when the WASD keys are used.
public class CameraRotateAroundObject : MonoBehaviour {

    private Camera cam;
    public Transform centered_object;
    private Vector3 origin;             //the position towards which the camera should always face
    public float y_shift;
    public float x_shift;

    private float max_angle = 1.5f;
    private float min_angle = -1.5f;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        origin = centered_object.transform.position;
        //initialise camera in correct direction
        Vector3 direction = origin - transform.position;
        cam.transform.forward = direction;              //aim the camera at the centre object
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = origin - transform.position;
        float angle = Mathf.Atan(direction.y / direction.z);
        //if left mouse button is held down
        if (Input.GetAxis("Vertical") > 0 && angle < max_angle)
        {
            Vector3 current_pos = cam.transform.position;
            Vector3 new_position = current_pos;
            //new_position.y += y_shift;
            new_position += cam.transform.up.normalized * y_shift;
            cam.transform.position = new_position;
            cam.transform.forward = direction;              //aim the camera at the centre object
            
        }
        if(Input.GetAxis("Vertical") < 0 & angle > min_angle)
        {
            Vector3 current_pos = cam.transform.position;
            Vector3 new_position = current_pos;
            //new_position.y -= y_shift;
            new_position -= cam.transform.up.normalized * y_shift;
            cam.transform.position = new_position;
            cam.transform.forward = direction;              //aim the camera at the centre object
        }
        if(Input.GetAxis("Horizontal") < 0)
        {
            Vector3 current_pos = cam.transform.position;
            Vector3 new_position = current_pos;
            //new_position.x += x_shift;
            new_position -= cam.transform.right.normalized * x_shift;
            cam.transform.position = new_position;
            cam.transform.forward = direction;              //aim the camera at the centre object
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            Vector3 current_pos = cam.transform.position;
            Vector3 new_position = current_pos;
            //new_position.x -= x_shift;
            new_position += cam.transform.right.normalized * x_shift;
            cam.transform.position = new_position;
            cam.transform.forward = direction;              //aim the camera at the centre object
        }
    }
}
