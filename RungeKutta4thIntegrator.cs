using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Performs a Runge-Kutta 4th order integration for the double pendulum

public class RungeKutta4thIntegrator: MonoBehaviour{

    public DoublePendulumEquations function;

    //Takes in the values of the function x at present time and outputs the values at t + h
    //equations are of form dx/dt = f(t,x)
	public Vector4 RK4(float t, Vector4 x, float h, float l1, float l2, float m1, float m2)
    {
        Vector4 new_x = new Vector4();
      
        Vector4 k1 = function.func(t, x, h, l1, l2, m1, m2);        //the k1 values for all 4 functions
        //Debug.Log("k1 = " + k1);
        Vector4 k2 = function.func(t + (h / 3f), x + (h * k1 / 3f), h, l1, l2, m1, m2);
        Vector4 k3 = function.func(t + (2 * h / 3f), x - (h * k1 / 3f) + (h * k2), h, l1, l2, m1, m2);
        Vector4 k4 = function.func(t + h, x + (h * k1) - (h * k2) + (h * k3), h, l1, l2, m1, m2);

        new_x = x + ((k1 + (3 * k2) + (3 * k3) + k4)*h / 8f);

        return new_x;
            

    }

    
}
