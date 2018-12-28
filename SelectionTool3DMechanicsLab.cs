using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Selection tool uses raycasting to identify the object selected.
//It displays information on the HUD/Screen UI depending on the object selected.
//This script is attached to the FPSCharacter so raycasts originate from the character model.
//Raycast needs to emit along the z axis of the local fpscharacter transform
//The selectable layer contains all the objects that can be hit by the raycast

public class SelectionTool3DMechanicsLab: MonoBehaviour {

    //InputFields for the pendulums
    public InputField pendulum_input_1;
    public InputField pendulum_input_2;
    public InputField pendulum_input_3;
    public InputField pendulum_input_4;
    public InputField pendulum_input_5;
    public InputField pendulum_input_6;
    public InputField pendulum_input_7;
    public InputField pendulum_input_8;
    public InputField pendulum_input_9;
    public InputField pendulum_input_10;
    public InputField pendulum_input_11;

    //scenemaster
    public SimplePendulumSceneMaster sm;
    //the camera object from which to fire raycasts
    private Camera fps_cam;


    //private Vector3 prev_forward_direction;
	// Use this for initialization
	void Start () {
        fps_cam = transform.GetComponentInChildren<Camera>();

        //Add listeners for all the input fields
        pendulum_input_1.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_1, 0); });
        pendulum_input_2.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_2, 1); });
        pendulum_input_3.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_3, 2); });
        pendulum_input_4.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_4, 3); });
        pendulum_input_5.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_5, 4); });
        pendulum_input_6.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_6, 5); });
        pendulum_input_7.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_7, 6); });
        pendulum_input_8.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_8, 7); });
        pendulum_input_9.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_9, 8); });
        pendulum_input_10.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_10, 9); });
        pendulum_input_11.onEndEdit.AddListener(delegate { UpdateLength(pendulum_input_11, 10); });

        //set initial lengths to show in input fields
        pendulum_input_1.text = sm.pendulums[0].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_2.text = sm.pendulums[1].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_3.text = sm.pendulums[2].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_4.text = sm.pendulums[3].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_5.text = sm.pendulums[4].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_6.text = sm.pendulums[5].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_7.text = sm.pendulums[6].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_8.text = sm.pendulums[7].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_9.text = sm.pendulums[8].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_10.text = sm.pendulums[9].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
        pendulum_input_11.text = sm.pendulums[10].GetComponent<SimplePendulumMotionScript>().length.ToString("F2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            
            RaycastHit hit;
            //fire a ray from the fpscharacter in the direction it is looking and check if it hits any selectable objects
            if (Physics.Raycast(fps_cam.transform.position, fps_cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100,Color.red);
                InteractWithObject(hit.collider);
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.white);
                
            }
            //prev_forward_direction = transform.forward;
        }
            
        
    }

    //================= UPDATE WITH THE OUTPUT FOR EACH SELECTABLE OBJECT IN THE SCENE=============================
    private void InteractWithObject(Collider hit)
    {
        if(hit.GetComponent<InputField>() != null)
        {
            hit.GetComponent<InputField>().Select();
        }
        
        
    }


    private void UpdateLength(InputField input, int pendulum_num)
    {
        float l;
        if(float.TryParse(input.text, out l))
        {
            if(l >= 1 && l < 20)
            {
                sm.pendulums[pendulum_num].GetComponent<SimplePendulumMotionScript>().length = l;
            }
        }
        
    }
}
