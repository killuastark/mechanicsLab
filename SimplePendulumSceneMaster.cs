using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimplePendulumSceneMaster : MonoBehaviour {

    public Transform pendulum1;     //the 11 pendulum masses
    public Transform pendulum2;
    public Transform pendulum3;
    public Transform pendulum4;
    public Transform pendulum5;
    public Transform pendulum6;
    public Transform pendulum7;
    public Transform pendulum8;
    public Transform pendulum9;
    public Transform pendulum10;
    public Transform pendulum11;

    private List<Transform> pendulums;
    
    private float[] lengths = new float[11] { 8.9456f, 6.2123f, 4.5641f, 3.4944f, 2.7610f, 2.2364f, 1.8483f, 1.5531f,1.3233f,1.1410f,0.99396f };

    // Use this for initialization
    void Start() {
       
        pendulums = new List<Transform>();
        pendulums.Add(pendulum1);
        pendulums.Add(pendulum2);
        pendulums.Add(pendulum3);
        pendulums.Add(pendulum4);
        pendulums.Add(pendulum5);
        pendulums.Add(pendulum6);
        pendulums.Add(pendulum7);
        pendulums.Add(pendulum8);
        pendulums.Add(pendulum9);
        pendulums.Add(pendulum10);
        pendulums.Add(pendulum11);


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(0);
        }
	}

    //Function that allows the user to click a button and the correct lengths are input in order to 
    // get the desired repetition in oscillations.
    void SetPrecalculatedLengths()
    {
        int i = 0;
        foreach(Transform pendulum in pendulums)
        {
            pendulum.GetComponent<SimplePendulumMotionScript>().length = lengths[i];
            i++;
        }

    }
}
