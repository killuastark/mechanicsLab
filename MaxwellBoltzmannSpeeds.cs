using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This static script should output speeds of n particles according to the Maxwell-Boltzmann distribution.
// METHOD: Split the distribution into d elements between speeds vn and v(n+1) with a maximum cutoff of 
// v(cutoff) = i*vp where i is some integer and vp is the most probable speed.
//Using an approximation to the error function, erf(x), find the probability of particles having speeds in 
//element at the specified conditions (m and T) and multiply by the total number of particles.
//That number of particles will then be given a random speed between the limits of that element.
//INPUT: n the number of particles, m the mass of the particles, T the temperature in K (which are set in the scenemaster)
//OUTPUT: An array of length n storing the speeds of all n particles in order.
//This output will then be applied to all n particle speeds in the scenemaster.
public static class MaxwellBoltzmannSpeeds {

    private static float k = 1.38E-23f;         //m2 kg s-2 K-1, Boltzmann's constant
    private static float pi = Mathf.PI;
    private static float v_max = 10000f;        //the maximum particle speed to calculate

    //OUTPUT: A List<float> of the n speeds of the (num_particles) particles
    public static List<float> Speeds(float mass, float temp, int num_particles, int divs, float vp)
    {
        //float[] speeds = new float[num_particles];
        List<float> speeds = new List<float>();
        float v_interval = v_max / divs;        //the range of speeds in a single interval
        float v_prev = 0;
        float v_next = v_interval;
        //float actual_N = 0;
        for(int i=0; i < divs; i++)
        {
            float fraction = ProbabilityLowerToUpper(v_prev, v_next, temp, mass);
            //Debug.Log("fraction = " + fraction);
            int num_particles_interval = (int)Mathf.Round(num_particles * fraction);
            //actual_N += num_particles_interval;
            for (int j = 0; j < num_particles_interval; j++)
            {
                float new_speed = Random.Range(v_prev, v_next);
                speeds.Add(new_speed);
                //Debug.Log("N interval = " + num_particles_interval);
                //Debug.Log("speed = " + new_speed);
            }
            
            v_prev = v_next;
            v_next += v_interval;
        }
        return speeds;

    }

    public static List<float[]> Fractions(float mass, float temp, int num_particles, int divs, float vp)
    {
        List<float[]> fractions = new List<float[]>();
        //float v_max = upper_factor * vp;
        float v_interval = v_max / divs;        //the range of speeds in a single interval
        float v_prev = 0;
        float v_next = v_interval;

        for (int i = 0; i < divs; i++)
        {
            float fraction = ProbabilityLowerToUpper(v_prev, v_next, temp, mass);
            float avg_speed = (v_prev + v_next) / 2f;
            float[] data = new float[2];
            data[0] = fraction;
            data[1] = avg_speed;
            fractions.Add(data);

            v_prev = v_next;
            v_next += v_interval;
        }

        return fractions;
    }


    //Outputs the fraction of particles between lower limit a and upper limit b.
    public static float ProbabilityLowerToUpper(float a, float b, float T, float m)
    {
        float prob_lower = ProbabilityZeroToLimit(a, T, m);
        float prob_upper = ProbabilityZeroToLimit(b, T, m);
        return prob_upper - prob_lower;
    }

    //OUTPUT: the fraction of particles that have speed between speed 0 and a at temp T and mass m.
    //Uses the solution to the gaussian integral x^2exp(-bx^2) with the erf(x)
    private static float ProbabilityZeroToLimit(float a, float T, float m)
    {
        float b = m / (2 * k * T);
        float c = Mathf.Pow((2 / pi), 0.5f) * Mathf.Pow((m / (k * T)), 1.5f);
        float integral = (Mathf.Pow(pi, 0.5f) * ERFApprox(a * Mathf.Pow(b, 0.5f))) / (4f * Mathf.Pow(b, 1.5f)) - (a * Mathf.Exp(-a * a * b) / (2f * b));
        float prob = integral * c;
        return prob;
    }
    
    
    //An approximation to the error function.
    public static float ERFApprox(float x)
    {
        float a = (8f / (3f * Mathf.PI)) * ((pi - 3) / (4 - pi));
        float erf = Mathf.Pow((1 - Mathf.Exp(-x * x * ((4f / pi) + (a * x * x)) / (1 + (a * x * x)))), 0.5f);

        return erf;
    }
}
