using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script attached to the upward force objects for the force input script
public class MomentsUpwardForceUIScript : MonoBehaviour {

    public InputField inputField;
    public float offset_x;
    public float offset_y;
    public float offset_z;

    private float weight;           //the weight of the force object itself

    // Use this for initialization
    void Start () {

        weight = Mathf.Abs(transform.GetComponent<Rigidbody>().mass * Physics.gravity.y);   //the set force need to also balance out the weight of the force object itself
        //Add a listener for changes in the mass value
        inputField.onEndEdit.AddListener(delegate { UpdateForce(); });
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = transform.position;
        newPosition.z += offset_z;
        newPosition.y += offset_y;
        newPosition.x += offset_x;
        inputField.transform.position = Camera.main.WorldToScreenPoint(newPosition);
    }


    private void UpdateForce()
    {
        float newValue;
        if (float.TryParse(inputField.text, out newValue))
        {
            //force will always act vertically upwards
            transform.GetComponent<ConstantForce>().force = new Vector3(0, Mathf.Abs(newValue) + weight, 0);
            //increase the y position fractionally in order to stop the collision and then it will recalculate
            //check what the behaviour will be
            //transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        }
    }
}
