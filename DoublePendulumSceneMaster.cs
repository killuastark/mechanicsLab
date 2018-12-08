using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the UI and initial conditions of the double pendulum simulation

public class DoublePendulumSceneMaster : MonoBehaviour {

    public Material wireMaterial;
    public Material pathMaterial;
    public Transform hinge;
    public Transform pendulum1;
    public Transform pendulum2;

    private Vector3 prev_position1;
    private Vector3 prev_position2;

    // Use this for initialization
    void Start () {
        prev_position1 = pendulum1.position;
        prev_position2 = pendulum2.position;
    }
	
	// Update is called once per frame
	void Update () {
        //get the positions of the pendulums
        Vector3 current_pos1 = pendulum1.position;
        Vector3 current_pos2 = pendulum2.position;
        //Draw in the wire attaching each to the next (and hinge) but remember to destroy these after a frame
        DrawWire(hinge.position, current_pos1, true, wireMaterial);
        DrawWire(current_pos1, current_pos2, true, wireMaterial);

        //Draw path
        //DrawWire(prev_position1, current_pos1, false, pathMaterial);
        DrawWire(prev_position2, current_pos2, false, pathMaterial);
        prev_position1 = current_pos1;
        prev_position2 = current_pos2;
    }

    //method for drawing rays
    private void DrawWire(Vector3 startPosition, Vector3 endPosition, bool destroy, Material mat)
    {
        GameObject line = new GameObject();
        line.AddComponent<LineRenderer>();
        LineRenderer finalRay = line.GetComponent<LineRenderer>();
        finalRay.positionCount = 2;
        finalRay.SetPosition(0, startPosition);
        finalRay.SetPosition(1, endPosition);
        finalRay.material = new Material(mat);
        finalRay.startWidth = 0.05f;
        finalRay.endWidth = 0.05f;

        if (destroy)
        {
            Destroy(line, Time.deltaTime);
        }

    }
}
