using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For use in the GravityLab2D scene.  Allows for the control of the position and velocity vector for the attached planet.
/// Controls: Left click on planet to move it around. Right click on planet and drag vector into position.
/// </summary>
public class PlanetPositionAndVelocityController : MonoBehaviour
{
    private bool isHovering;            //is the mouse cursor over the planet
    [SerializeField] private GameObject velocity_vector;
    private float rotation_speed = 10f;

    private float initial_speed;           //the initial speed of the planet
    private TextboxFollowGameObject speed_io;               //the speed input field
    private InputField input_field;                         //the input field for velocity
    private Rigidbody rb;

    private Vector3 planet_mouse_diff;

    // Start is called before the first frame update
    void Start()
    {
        initial_speed = 0f;     //by default
        rb = transform.GetComponent<Rigidbody>();
        speed_io = transform.GetComponentInChildren<TextboxFollowGameObject>();     //get access to the input field
        input_field = speed_io.GetInputField();
        //initialise the velocity vector in an appropriate position to see the input box
        Vector3 startPos = new Vector3(0f, 1f, 0f);
        velocity_vector.transform.LookAt(startPos);
        //Add listener for change in intial_speed
        input_field.onValueChanged.AddListener(delegate { SetInitialSpeed(); });
    }

    // Update is called once per frame
    void Update()
    {
        //only check which button has been clicked if the appropriate planet has been clicked.
        
        if (isHovering)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            //Change the position of the planet
            if (Input.GetMouseButton(0))
            {
                transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                //mousePos.z = 0f;        //working in 2D 
                //move the planet to the position of the mouse - but maintain the location of where the mouse actually clicked the planet
                transform.position = new Vector3(mousePos.x + planet_mouse_diff.x, mousePos.y + planet_mouse_diff.y, 0f);       //2D
            }
            //only allow for the velocity to be changed if the simulation is paused
            else if (Input.GetMouseButton(1) && Time.timeScale == 0f)
            {
                Vector3 mousePos2D = new Vector3(mousePos.x, mousePos.y, 0f);
                velocity_vector.transform.LookAt(mousePos2D);
                rb.velocity = initial_speed * (velocity_vector.transform.forward.normalized);        //planet should have a velocity in the direction of the velocity vector
            }
        }

        //whilst not clicked, the velocity vector should aim in the direction of the velocity of the planet
        velocity_vector.transform.forward = rb.velocity.normalized;

        //if the simulation is running, output current speed to the input field
        if (Time.timeScale > 0)
        {
            input_field.text = rb.velocity.magnitude.ToString("F2");
        }
        
    }

    //Sets the initial speed based upon the input field value
    private void SetInitialSpeed()
    {
        float value;
        if(float.TryParse(input_field.text, out value))
        {
            initial_speed = value;
        }
        
    }

    //when the mouse is first clicked, get the location of the click
    private void OnMouseDown()
    {
        planet_mouse_diff = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);              //get the initial difference when first clicked
    }
    private void OnMouseOver()
    {
        isHovering = true;
    }

    private void OnMouseExit()
    {
        isHovering = false;
    }
}
