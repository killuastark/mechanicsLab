using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gravitational force code added to a GameObject with a rigidbody component. Searches for other objects tagged with
/// "gravitating" and calculates the cumulative affect of all those bodies
/// </summary>
public class NBodyGravScript : MonoBehaviour
{
    private GameObject[] gravitating_bodies;
    private Vector3 acceleration;
    private float gravity_constant;             //gravitational scalar constant
    private Rigidbody rb;                       //this masses rigidbody

    private float delta_r = 0.00001f;             //a small additional distance so as not to calculate infinities if planets reach 0 separation
    // Start is called before the first frame update
    void Start()
    {
        gravity_constant = 1f;
        Time.fixedDeltaTime = 0.01f;
        rb = transform.GetComponent<Rigidbody>();
        //Search for all gravitating bodies and add them to a list.
        gravitating_bodies = GameObject.FindGameObjectsWithTag("gravitating");
        
    }

    void OnEnable()
    {
        GravityLabEventHandler.OnPlanetAdded += SearchForPlanets;       //add SearchForPlanets to the event handler
    }

    private void OnDisable()
    {
        GravityLabEventHandler.OnPlanetAdded -= SearchForPlanets;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        acceleration = new Vector3(0, 0, 0);
        foreach(GameObject body in gravitating_bodies)
        {
            //NO SELF GRAVITATION.
            if(body.gameObject != transform.gameObject)
            {
                //the vector direction from this mass towards the gravitating mass
                Vector3 displacement = body.transform.position - transform.position;

                float sqr_distance = displacement.sqrMagnitude;
                Vector3 direction = displacement.normalized;

                float scalar_acceleration = gravity_constant * body.GetComponent<GravitatingBodyInfo>().GetMass() / (sqr_distance + delta_r);
                acceleration += (scalar_acceleration * direction);
            }
            
        }
        Vector3 force = acceleration * rb.mass;
        rb.AddForce(force, ForceMode.Force);
    }

    /// <summary>
    /// Repeats the search for gravitating bodies
    /// </summary>
    public void SearchForPlanets()
    {
        gravitating_bodies = GameObject.FindGameObjectsWithTag("gravitating");
    }

}
