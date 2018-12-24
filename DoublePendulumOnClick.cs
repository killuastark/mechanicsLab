using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to a pendulum mass
//When clicked it should disable the pendulum motion function
//It should then follow the mouse position whilst clicked
//When unclicked, pendulum motion should begin again
public class DoublePendulumOnClick : MonoBehaviour {

    public Transform hinge;         //gameobject on which the pendulum motion function is attached
    private DoublePendulumMotion dp_motion;
    public Transform otherPendulum;

    private float length;

    private Vector3 relativePosition;
    private float z_pos;


    private void OnMouseDown()
    {
        dp_motion = hinge.GetComponent<DoublePendulumMotion>();
        //only pendulum 1 can have its position moved
        length = dp_motion.GetLength1();
        //get the fixed z position of the pendulum
        z_pos = transform.position.z;

        //disable the pendulum motion
        dp_motion.enabled = false;
        relativePosition = otherPendulum.position - transform.position;
    }

    private void OnMouseDrag()
    {
        //set the position of pendulum 1 but ensure that it sticks to the same length of pendulum that it is set to
        float new_x_pos = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, -length, length);
        float new_y_mag = Mathf.Sqrt(length * length - new_x_pos * new_x_pos);
        
        //if the mouse is below the hinge (0,0) then the new y position should be below the hinge 
        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).y < hinge.position.y)
        {
            transform.position = new Vector3(new_x_pos, -new_y_mag, z_pos);
        }
        //else it is above it
        else
        {
            transform.position = new Vector3(new_x_pos, new_y_mag, z_pos);
        }
        
        otherPendulum.position = transform.position + relativePosition;
    }

    private void OnMouseUp()
    {
        //when mouse released, reset the initial conditions to the current position and then start updating again
        dp_motion.SetInitialConditions();
        dp_motion.enabled = true;
    }
}
