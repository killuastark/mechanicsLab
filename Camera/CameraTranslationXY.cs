using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script should allow the camera to be moved in the x-y plane 
// WASD keys are used to move the camera around since GetAxis appears to be time.timescale dependent and won't work when timescale = 0

public class CameraTranslationXY : MonoBehaviour
{

    public Camera cam;                  // The main scene camera

    public float camShiftDelta = 0.3f;         //the amount to shift the camera by on each update when key pressed
    float cam_z;

    [SerializeField] private float max_x;
    [SerializeField] private float max_y;

    // Use this for initialization
    void Start()
    {
        cam_z = cam.transform.position.z;       //the height/z coord of the camera, which shouldn't be changed
    }

    // Update is called once per frame
    void Update()
    {
        //If any of the WASD directional keys are held then move the camera in the corresponding direction
        //Limit the movement
        //if (Input.GetAxis("Vertical") > 0)
        if (Input.GetKey(KeyCode.W))
        {
            float current_y = cam.transform.position.y; //get current y position
            float new_y = current_y + camShiftDelta;               // get new position
            if(Mathf.Abs(new_y) <= max_y)
            {
                cam.transform.position = new Vector3(cam.transform.position.x, new_y, cam_z);   //update camera position
            }
            
        }

        //if (Input.GetAxis("Vertical") < 0)
        if (Input.GetKey(KeyCode.S))
        {
            float current_y = cam.transform.position.y; //get current y position
            float new_y = current_y - camShiftDelta;               // get new position
            if (Mathf.Abs(new_y) <= max_y)
            {
                cam.transform.position = new Vector3(cam.transform.position.x, new_y, cam_z);   //update camera position
            }
                
        }

        //if (Input.GetAxis("Horizontal") < 0)
        if (Input.GetKey(KeyCode.A))
        {
            float current_x = cam.transform.position.x; //get current x position
            float new_x = current_x - camShiftDelta;               // get new position
            if (Mathf.Abs(new_x) <= max_x)
            {
                cam.transform.position = new Vector3(new_x, cam.transform.position.y, cam_z);   //update camera position
            }
                
        }

        //if (Input.GetAxis("Horizontal") > 0)
        if(Input.GetKey(KeyCode.D))
        {
            float current_x = cam.transform.position.x; //get current x position
            float new_x = current_x + camShiftDelta;               // get new position
            if (Mathf.Abs(new_x) <= max_x)
            {
                cam.transform.position = new Vector3(new_x, cam.transform.position.y, cam_z);   //update camera position
            }
                
        }
    }



}
