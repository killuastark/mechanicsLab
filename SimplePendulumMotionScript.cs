using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scripted attached to the simple pendulum in order to calculate its position each time frame.
//Pendulums will oscillate in the x-y plane
public class SimplePendulumMotionScript : MonoBehaviour {

    public Transform hinge;             //the position from which the pendulum is swinging

    [SerializeField] private float angle;                //the angle from the vertical which the pendulum is at
    private float omega;                //the pendulum's angular velocity
    public float length;               //the length of the pendulum
    private float z_pos;                 //the z_pos of this pendulum
    private float origin_x;             //the x and y position of the hinge point, z_pos is the same for pendulum and hinge
    private float origin_y;
    private Vector3 attach_position;    //the position on the hinge that this pendulum attaches to

    private float delta_t;              //the time interval between calculations

    public Material wire_material;      //the wire material that attaches the pendulum bob to the hinge

	// Use this for initialization
	void Start () {
        //to test, I will simply start the pendulum at a specific angle, but will incorporate user interactivity later
        angle = 0.35f;         //initial angle in radians
        omega = 0f;     //pendulum starts at rest
        //length = 10f;       //set for testing
        z_pos = transform.position.z;

        origin_x = hinge.position.x;
        origin_y = hinge.position.y;

        Time.fixedDeltaTime = 0.005f;        //this is the time between each fixed update calculation below, may allow user to change
        delta_t = Time.fixedDeltaTime;

        attach_position = new Vector3(origin_x, origin_y, z_pos);
    }

    //for drawing in the pendulum wire
    private void Update()
    {
        DrawWire(attach_position, transform.position, true);
    }


    // FixedUpdate is called once per fixedDeltaTime: it is the small interval for the calculations
    void FixedUpdate () {
        CalculateAngularVelocity();         //calculates and sets the new omega value using old angle
        CalculateAngle();                   //calculates and sets the new angle using new omega and old angle
        //calculate new x and y position
        float x_pos = origin_x + length * Mathf.Sin(angle);
        float y_pos = origin_y - length * Mathf.Cos(angle);
        //set new position
        transform.position = new Vector3(x_pos, y_pos, z_pos);
        
	}


    private void CalculateAngularVelocity()
    {
        omega += (Physics.gravity.y / length) * Mathf.Sin(angle) * delta_t;
    }

    //OUTPUT: the angle in radians!
    private void CalculateAngle()
    {
        angle += omega * delta_t;
    }


    //method for drawing rays
    private void DrawWire(Vector3 startPosition, Vector3 endPosition, bool destroy)
    {
        GameObject line = new GameObject();
        line.AddComponent<LineRenderer>();
        LineRenderer finalRay = line.GetComponent<LineRenderer>();
        finalRay.positionCount = 2;
        finalRay.SetPosition(0, startPosition);
        finalRay.SetPosition(1, endPosition);
        finalRay.material = new Material(wire_material);
        finalRay.startWidth = 0.05f;
        finalRay.endWidth = 0.05f;

        if (destroy)
        {
            Destroy(line, 0.02f);
        }

    }


}
