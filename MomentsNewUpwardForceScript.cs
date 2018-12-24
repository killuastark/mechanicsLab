using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MomentsNewUpwardForceScript : MonoBehaviour {

    public Canvas canvas;                   //The canvas to which the UI label should be set.
    
    public GameObject force_prefab;          //The prefab force object that will be created.
    public GameObject force_input_prefab;     //The inputfield prefab
                                          
    void Start () {
		
	}
	
	public void CreateNewUpwardForce()
    {
        //Create the mass and position it above the field of view so that it falls into place
        GameObject newForce = Instantiate(force_prefab) as GameObject;
        newForce.transform.position = new Vector3(0, 2, 0);

        //Instantiate the inputfield prefab and position it where the force object is.
        //It is not updated here.
        Vector3 input_position = newForce.transform.position;
        GameObject forceInput = Instantiate(force_input_prefab) as GameObject;
        forceInput.transform.SetParent(canvas.transform);
        //input_position = new Vector3(input_position.x, input_position.y, input_position.z + 1);
        forceInput.transform.position = Camera.main.WorldToScreenPoint(input_position);

        //Attach the input field to the new force object
        newForce.GetComponent<MomentsUpwardForceUIScript>().inputField = forceInput.GetComponent<InputField>();
    }
}
