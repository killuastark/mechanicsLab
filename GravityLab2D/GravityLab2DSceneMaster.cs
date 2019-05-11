using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityLab2DSceneMaster : MonoBehaviour
{
    [SerializeField] private Slider time_rate_slider;       //properties set in editor
    public Toggle show_velocity;
    public Toggle ThreebodySol;                            //if toggled, then setup 3 body solution
    public Toggle EarthMoonSol;                             //if toggled, then earth moon system is setup
    public Toggle show_grid_toggle;

    [SerializeField] private GameObject grid_object;

    //Prefab planets
    [SerializeField] private GameObject earth_prefab;
    [SerializeField] private GameObject moon_prefab;

    // Start is called before the first frame update
    void Start()
    {
        ThreebodySol.isOn = false;
        EarthMoonSol.isOn = false;
        //time rate begins at 0 so that planets positions and velocities can be set.
        Time.timeScale = time_rate_slider.value;        //slider value begins at zero, but can change this if I want

        time_rate_slider.onValueChanged.AddListener(delegate { UpdateTimeRate(); });
        show_velocity.onValueChanged.AddListener(delegate { ShowHideVelocity(); });
        ThreebodySol.onValueChanged.AddListener(delegate { ThreeBodyFigureEight(); });
        EarthMoonSol.onValueChanged.AddListener(delegate { EarthMoonSetup(); });
        show_grid_toggle.onValueChanged.AddListener(delegate { ShowGrid(); });
    }

    /// <summary>
    /// Updates the time rate based on the UI toggle
    /// </summary>
    public void UpdateTimeRate()
    {
        Time.timeScale = time_rate_slider.value;
    }

    /// <summary>
    /// Shows and hides the background grid for distance measuring
    /// </summary>
    public void ShowGrid()
    {
        if (show_grid_toggle.isOn)
        {
            grid_object.SetActive(true);
        }
        else
        {
            grid_object.SetActive(false);
        }
    }

    /// <summary>
    /// Shows or hides the velocity input fields attached to the planets depending on the UI toggle switch
    /// </summary>
    public void ShowHideVelocity()
    {
        GameObject[] fields = GameObject.FindGameObjectsWithTag("velocity_field");

        if (show_velocity.isOn)
        {
            foreach(GameObject field in fields)
            {
                field.GetComponent<Image>().enabled = true;
                field.GetComponent<InputField>().textComponent.enabled = true;
                field.GetComponent<InputField>().enabled = true;
                
            }
        }
        else
        {
            foreach (GameObject field in fields)
            {
                field.GetComponent<Image>().enabled = false;
                field.GetComponent<InputField>().textComponent.enabled = false;
                field.GetComponent<InputField>().enabled = false;
                
                

            }
        }
    }


    public void DeletePlanets()
    {
        GameObject[] old_planets = GameObject.FindGameObjectsWithTag("gravitating");
        foreach (GameObject planet in old_planets)
        {
            planet.tag = "Untagged";
            Destroy(planet.gameObject);
        }
    }

    public void DeleteVelocityInputs()
    {
        GameObject[] old_vels = GameObject.FindGameObjectsWithTag("velocity_field");
        foreach (GameObject vel in old_vels)
        {
            vel.tag = "Untagged";
            Destroy(vel.gameObject);
        }
    }
    /// <summary>
    /// A preset 3 body system: stable figure of 8
    /// </summary>
    public void ThreeBodyFigureEight()
    {
        //only if we have toggled the solution on
        if (ThreebodySol.isOn)
        {
            //Remove current trail particles
            GameObject[] trail = GameObject.FindGameObjectsWithTag("trail_particle");
            foreach(GameObject particle in trail)
            {
                Destroy(particle);
            }
            //Find and delete any planets currently in the system
            DeletePlanets();
            DeleteVelocityInputs();

            //switch off other toggles - if more are added then make a List
            EarthMoonSol.isOn = false;

            //Now add in 3 earth bodies.
            GameObject[] bodies = new GameObject[3];
            bodies[0] = Instantiate(earth_prefab);
            bodies[1] = Instantiate(earth_prefab);
            bodies[2] = Instantiate(earth_prefab);
            //bodies[0].GetComponent<NBodyGravScript>().SearchForPlanets();
            //bodies[1].GetComponent<NBodyGravScript>().SearchForPlanets();
            //bodies[2].GetComponent<NBodyGravScript>().SearchForPlanets();
            float scale = 10f;
            //scaling up the solution
            float start_x = scale * 0.9700436f;
            float start_y = scale * -0.24308753f;
            float start_vx = scale * 0.466203685f;
            float start_vy = scale * 0.43236573f;
            //initialise positions
            bodies[0].transform.position = new Vector3(start_x, start_y, 0f);
            bodies[1].transform.position = new Vector3(-start_x, -start_y, 0f);
            bodies[2].transform.position = new Vector3(0f, 0f, 0f);
            //initialise velocities
            Rigidbody rb0 = bodies[0].GetComponent<Rigidbody>();
            Rigidbody rb1 = bodies[1].GetComponent<Rigidbody>();
            Rigidbody rb2 = bodies[2].GetComponent<Rigidbody>();
            //set the gravitating body mass
            bodies[0].GetComponent<GravitatingBodyInfo>().SetMass(1000f);
            bodies[1].GetComponent<GravitatingBodyInfo>().SetMass(1000f);
            bodies[2].GetComponent<GravitatingBodyInfo>().SetMass(1000f);
            rb0.mass = 1000f;       //solution is for m=1 but have scaled r and v by 10 so mass is r*v*v -> 1000
            rb1.mass = 1000f;
            rb2.mass = 1000f;
            rb0.velocity = new Vector3(start_vx, start_vy, 0f);
            rb1.velocity = new Vector3(start_vx, start_vy, 0f);
            rb2.velocity = new Vector3(-2 * start_vx, -2 * start_vy, 0f);
        }
    }


    /// <summary>
    /// A preset Earth Moon setup
    /// </summary>
    public void EarthMoonSetup()
    {
        if (EarthMoonSol.isOn)
        {
            //Remove current trail particles
            GameObject[] trail = GameObject.FindGameObjectsWithTag("trail_particle");
            foreach (GameObject particle in trail)
            {
                Destroy(particle);
            }
            //get rid of any existing planets
            DeletePlanets();
            DeleteVelocityInputs();

            //switch off other toggles - if more presets are added then make a List object
            ThreebodySol.isOn = false;

            //create the earth and the moon
            GameObject earth = Instantiate(earth_prefab);
            GameObject moon = Instantiate(moon_prefab);

            //set their positions
            earth.transform.position = new Vector3(-5f, 0, 0);
            moon.transform.position = new Vector3(0, 0, 0);

            //set their velocities
            earth.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.0547f, 0);
            moon.GetComponent<Rigidbody>().velocity = new Vector3(0, -4.445f, 0);
        }
    }
}
