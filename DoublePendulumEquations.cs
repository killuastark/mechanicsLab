using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Outputs the function f(x,y) of the differential equation dy/dx = f(x,y) defined for the double pendulum simulation
public class DoublePendulumEquations: MonoBehaviour{

    //private float g = 9.81f;

    //INPUT: Vector4 x which defines the quantities at this time step
    //      x[0] = theta1, x[1] = theta2, x[2] = omega1, x[3] = omega2      theta = angular displacement in radians, omega = angular velocity
    public Vector4 func(float t, Vector4 x, float h, float l1, float l2, float m1, float m2)
    {
        float g = Mathf.Abs(Physics.gravity.y);
        Vector4 dxdt = new Vector4();
    
        dxdt[0] = x[2];        //d(theta)/dt = omega1
        //Debug.Log("dxdt[0] = " + dxdt[0]);
        dxdt[1] = x[3];
        //Debug.Log("dxdt[1] = " + dxdt[1]);
        dxdt[2] = (-g * (2 * m1 + m2) * Mathf.Sin(x[0]) - m2 * g * Mathf.Sin(x[0] - 2 * x[1]) - 2 * Mathf.Sin(x[0] - x[1]) * m2 * (x[3] * x[3] * l2 + x[2] * x[2] * l1 * Mathf.Cos(x[0] - x[1]))) / (l1 * (2 * m1 + m2 - m2 * Mathf.Cos(2 * x[0] - 2 * x[1])));
        //Debug.Log("dxdt[2] = " + dxdt[2]);
        dxdt[3] = 2 * Mathf.Sin(x[0] - x[1]) * (x[2] * x[2] * l1 * (m1 + m2) + g * (m1 + m2) * Mathf.Cos(x[0]) + x[3] * x[3] * l2 * m2 * Mathf.Cos(x[0] - x[1])) / (l2 * (2 * m1 + m2 - m2 * Mathf.Cos(2 * x[0] - 2 * x[1])));
        //Debug.Log("dxdt[3] = " + dxdt[3]);
        return dxdt;

    }

    //variable output tells us which of the 4 functions we want to calculate
    //public float func(float t, float[] x, float h, float l1, float l2, float m1, float m2, int output)
    //{
    //    float dxdt;

    //    if(output == 0)
    //    {
    //        dxdt = x[2];        //d(theta)/dt = omega1
    //    } else if(output == 1)
    //    {
    //        dxdt = x[3];
    //    } else if(output == 2)
    //    {
    //        dxdt = (-g * (2 * m1 + m2) * Mathf.Sin(x[0]) - m2 * g * Mathf.Sin(x[0] - 2 * x[1]) - 2 * Mathf.Sin(x[0] - x[1]) * m2 * (x[3] * x[3] * l2 + x[2] * x[2] * l1 * Mathf.Cos(x[0] - x[1]))) / (l1 * (2 * m1 + m2 - m2 * Mathf.Cos(2 * x[0] - 2 * x[1])));
    //    }
    //    else
    //    {
    //        dxdt = 2 * Mathf.Sin(x[0] - x[1]) * (x[2] * x[2] * l1 * (m1 + m2) + g * (m1 + m2) * Mathf.Cos(x[0]) + x[3] * x[3] * l2 * m2 * Mathf.Cos(x[0] - x[1])) / (l2 * (2 * m1 + m2 - m2 * Mathf.Cos(2 * x[0] - 2 * x[1])));
    //    }

    //    return dxdt;

    //}


}
