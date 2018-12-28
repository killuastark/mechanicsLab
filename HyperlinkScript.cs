using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperlinkScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void MechanicsLabTutorialSiteLink()
    {
        Application.OpenURL("https://physics-labs.com/mechanics-lab");
    }

    public void PhysicsLabsSiteLink()
    {
        Application.OpenURL("https://physics-labs.com");
    }
}
