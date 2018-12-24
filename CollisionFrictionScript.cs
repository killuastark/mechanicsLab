using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Attached to the mass, whenever it has a collision it detects the collider and finds the coefficient of friction for it
// A public getter then allows other scripts to access it.

//Unity 2018 apparently has a better friction calculations - perhaps think of updating
//Because this is built in Unity 2017 I have had to fix that problem by just freezing the block until the calculated resultant is greater than 0

public class CollisionFrictionScript : MonoBehaviour {

    private float frictionCoefficient;
    public Text coefficientText;
    public Text accelerationText;
    private float mass;
    private Rigidbody rb;       //rigidbody of the mass

    private float resultant;
    private Collider col;

    private CalculateForcesScript forcesScript;
	// Use this for initialization
	void Start () {
        rb = transform.GetComponent<Rigidbody>();
        forcesScript = transform.GetComponentInParent<CalculateForcesScript>(); //allows access to the forces and acceleration
        mass = rb.mass;
    }

    //private void OnCollisionEnter(Collision collision)
    void Update()
    {
        //col = collision.collider;
        //if the mass is stationary then static friction coefficient used, else dynamic.
        if (col != null)
        {
            //Debug.Log("Vel = " + rb.velocity.z);
            if (resultant <= 0)
            {
                frictionCoefficient = col.material.staticFriction;
                
            }
            else
            {
                frictionCoefficient = col.material.dynamicFriction;
            }
            OutputToTextBox(frictionCoefficient);   //output to UI
            UpdateAcceleration();
        }
    }

    //called from calculate forces script in order to get the friction force
    public float GetFrictionCoefficient()
    {
        return frictionCoefficient;
    }

    //acceleration of the rigidbody not calculated in Unity - calculate using components of forces along slope
    private void UpdateAcceleration()
    {
        resultant = forcesScript.GetParallelForce() - forcesScript.GetFriction();     //resultant along slope
        //Debug.Log("Fp = " + forcesScript.GetParallelForce());
        //Debug.Log("Ff = " + forcesScript.GetFriction());
        Debug.Log("F = " + resultant);
        if(resultant < 0)     
        {
            accelerationText.text = "0";
            //set the velocity to 0 because it shouldn't be moving
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);     //Unity 2017 does not have accurate friction calculation, so mass slides before it should

        }
        else
        {
            float acceleration = resultant / mass;
            accelerationText.text = acceleration.ToString("F2");

        }
    }

    private void OutputToTextBox(float frictionCoefficient)
    {
        coefficientText.text = frictionCoefficient.ToString();
    }

    //when mass collides with a new object it resets the collider
    private void OnCollisionEnter(Collision collision)
    {
        col = collision.collider;
    }
}
