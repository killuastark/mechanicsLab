﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Equations of motion for the double pendula
//Calculates the new angle of the pendulum at the next time step t + h
//Motion of each pendulum depends on the other
public class DoublePendulumMotion : MonoBehaviour {

    public RungeKutta4thIntegrator rk4;

    public Transform pendulum1;
    public Transform pendulum2;
    public Transform hinge;         //the origin of the coordinate system around which the pendulums are rotating
    private Vector3 hinge_position;

    //stored positions of the pendulums for the trail
    private List<Vector3> pendulum1_positions;
    private List<Vector3> pendulum2_positions;
    
    
    private float l1;       //properties of the pendula.
    private float l2;
    private float m1;
    private float m2;

    private float h;            //the time step
    private Vector4 x;             //the current state of the pendulums Vector4(theta1, theta2, omega1, omega2)

	// Use this for initialization
	void Start () {
        hinge_position = hinge.position;        //set the hinge position, but it should be 0,0 (+z)
        pendulum1_positions = new List<Vector3>();
        pendulum2_positions = new List<Vector3>();
        

        l1 = 10;
        l2 = 10;
        m1 = 1f;
        m2 = 1f;
        x = new Vector4(2f, 2f, 0.2f, 0.3f);          //TEST initial conditions for double pendulum

        Time.fixedDeltaTime = 0.01f;
        h = Time.fixedDeltaTime;
	}
	
	// FixedUpdate is called every time step h
    //change the Time.fixedDeltaTime to change h
	void FixedUpdate () {
        x = rk4.RK4(Time.time, x, h, l1, l2, m1, m2);
        //Debug.Log("X = " + x);
        //Store the current position
        //pendulum1_positions.Add(pendulum1.position);
        //pendulum2_positions.Add(pendulum2.position);
        //Set the position of each of the pendulums
        SetPendulumPosition(pendulum1, hinge_position, x[0], l1);
        SetPendulumPosition(pendulum2, pendulum1.transform.position, x[1], l2);
	}

    //INPUT: the pendulum transform to set, the position from which the angle is set, the angle the pendulum is at and the length of the pendulum string.
    //OUTPUT: Sets the transform with no output
    private void SetPendulumPosition(Transform pendulum, Vector3 from, float angle, float l)
    {
        float new_x = from[0] + l * Mathf.Sin(angle);
        float new_y = from[1] - l * Mathf.Cos(angle);

        pendulum.position = new Vector3(new_x, new_y, pendulum.position.z);
    }
}
