using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//When the moments scene is loaded, this script should do the following:
//          randomly set the hinge point on the plank
//          set the time frame to zero
//          create either 1 or 2 boxes on the plank
//          create 1 or 2 upward forces at certain positions along the plank??
//          Update should check if the plank remains within a small horizontal angle when timeframe > 0
//          Load a new random level? Count points for completed scenes? Get harder if more points?

public class MomentsSceneMaster : MonoBehaviour {

    public Transform plank;         //the plank object that needs to be balanced
    public Transform hinge;         //the hinge object around which the plank will pivot

    //prefab for making random boxes
    public GameObject mass_prefab;
    public GameObject mass_input_prefab;
   

    public Canvas canvas;

    //for storing the positions of the initial box and plank
    private float[] box_pos = new float[3];              //the z coord of the box along the plank
    private float[] box_mass = new float[3];
    private Vector3 plank_position;
    private float plank_mass;
    private int num_boxes;              //the number of initial boxes that should be added to the simulation
    //UI
    public Button reset_button;
    public Button start_button;
    public Text balanced_text;          //text to display if the user balances the system
    public Text balance_streak_text;
    public Text plank_mass_text;
    //private float balance_streak;
    public Text time_left_text;
    //angle to remain within - can be changed for different difficulties
    public float delta_angle;       //degrees
    private float delta_time = 5;   //the time that the beam has to remain within delta_angle
    private float balance_time;

    private bool levelComplete;

	// Use this for initialization
	void Awake () {
        float num_points = MomentsDataScript.GetStreakPoints();
        //testing 
        //num_points = 6;
        balance_streak_text.text = num_points.ToString("F0");      //display the streak points

        levelComplete = false;
        Time.timeScale = 0f;        //freeze time for the setup of masses and forces before the unpause button is clicked
        float hinge_z = Random.Range(-0.5f, 0.5f);     //random position along the plank length
        plank_position = plank.position;
        hinge.position = new Vector3(plank_position.x, plank_position.y, hinge_z*(plank.localScale.z));
        //plank.GetComponent<HingeJoint>().connectedBody = hinge.GetComponent<Rigidbody>();
        plank.GetComponent<HingeJoint>().anchor = new Vector3(0,0,hinge_z);    //anchor position along the plank
        //set random mass of plank between 1 - 10kg.
        plank_mass = plank.GetComponent<Rigidbody>().mass = Random.Range(1, 10);
        plank_mass_text.text = plank_mass.ToString("F0");

        num_boxes = 0;          //the number of boxes that should be added to the simulation
        //create the positions and masses for the boxes that will be added
        //get a random position along the plank for a box to start
        box_pos[0] = Random.Range(-plank.transform.localScale.z / 2, plank.transform.localScale.z / 2);
        box_mass[0] = Random.Range(1, 10);       //random range for the box between 1 and 10
        num_boxes = 1;

        //if the user has fewer that 3 points, only produce 1 box on start
        if(num_points < 3)
        {
            //get a random position along the plank for a box to start
            box_pos[0] = Random.Range(-plank.transform.localScale.z / 2, plank.transform.localScale.z / 2);
            box_mass[0] = Random.Range(1, 10);       //random range for the box between 1 and 10
            num_boxes = 1;
        }

        else if (num_points >= 3 && num_points < 5)
        {
            //get a random position along the plank for a box to start
            box_pos[0] = Random.Range(-plank.transform.localScale.z / 2, plank.transform.localScale.z / 2);
            box_mass[0] = Random.Range(1, 10);       //random range for the box between 1 and 10
            box_pos[1] = Random.Range(-plank.transform.localScale.z / 2, plank.transform.localScale.z / 2);
            box_mass[1] = Random.Range(1, 10);       //random range for the box between 1 and 10
            num_boxes = 2;
        }
        else
        {
            //get a random position along the plank for a box to start
            box_pos[0] = Random.Range(-plank.transform.localScale.z / 2, plank.transform.localScale.z / 2);
            box_mass[0] = Random.Range(1, 10);       //random range for the box between 1 and 10
            box_pos[1] = Random.Range(-plank.transform.localScale.z / 2, plank.transform.localScale.z / 2);
            box_mass[1] = Random.Range(1, 10);       //random range for the box between 1 and 10
            box_pos[2] = Random.Range(-plank.transform.localScale.z / 2, plank.transform.localScale.z / 2);
            box_mass[2] = Random.Range(1, 10);       //random range for the box between 1 and 10
            num_boxes = 3;
        }
        //randomly position boxes on the plank
        CreateMasses();
        

        //listeners
        reset_button.onClick.AddListener(delegate { ResetScene(); });
        start_button.onClick.AddListener(delegate { StartTiming(); });
    }
	
	// Update is called once per frame
    //Update should check if the scene has been unpaused and if so count for 5 seconds whilst checking if the beam remains 
    //within a small horizontal angle. If so, it should output some message to say so.
	void Update () {
		if(Time.timeScale != 0)
        {
            if(Quaternion.Angle(plank.rotation,Quaternion.identity) < delta_angle)
            {
                balance_time += Time.deltaTime;     //add the frame time onto the balance time
                if(balance_time > delta_time && !levelComplete)       //if equilibrium has been held for long enough then you have completed it
                {
                    balanced_text.enabled = true;       //reveal the BALANCED! text
                    MomentsDataScript.AddStreakPoint();
                    balance_streak_text.text = MomentsDataScript.GetStreakPoints().ToString("F0");
                    levelComplete = true;
                    time_left_text.text = "0.00";
                }
                else
                {
                    if(balance_time < delta_time)
                    {
                        time_left_text.text = (delta_time - balance_time).ToString("F2");
                    }
                }
            }
            else
            {
                balance_time = 0;       //if equilibrium is lost then reset the balance time
            }
        }
	}

    private void CreateMasses()
    {
        for (int i = 0; i < num_boxes; i++)
        {

            //instantiate a new box and set its mass
            GameObject new_box = Instantiate(mass_prefab);
            new_box.transform.position = new Vector3(plank_position.x, plank_position.y + new_box.transform.localScale.y, box_pos[i]);
            new_box.GetComponent<Rigidbody>().mass = box_mass[i];
            //Instantiate the inputfield prefab and position it where the mass is.
            //It is not updated here.
            Vector3 input_position = new_box.transform.position;
            GameObject massInput = Instantiate(mass_input_prefab) as GameObject;
            massInput.transform.SetParent(canvas.transform);
            input_position = new Vector3(input_position.x, input_position.y, input_position.z + 1);
            massInput.transform.position = Camera.main.WorldToScreenPoint(input_position);
            massInput.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);   //scale appropriately on different resolutions.
            massInput.GetComponent<InputField>().text = box_mass[i].ToString("F0");        //set the text box to the mass of the box

            //Attach the input field to the new mass
            new_box.GetComponent<MomentsMassUIScript>().inputField = massInput.GetComponent<InputField>();

        }
    }

    //resets the scene to its original orientation so that another attempt can be made.
    //As opposed to creatin a new scene by loading the scene again - which will randomly assign box position on Awake
    public void ResetScene()
    {
        //don't reset the level complete boolean as repeating this scene should not get you another balance streak point
        //get rid of the balanced_text
        balanced_text.enabled = false;
        balance_time = 0;       //reset balance time
        //find and delete all the moment_force_objects in the scene (all boxes and upward forces)
        GameObject[] objects = GameObject.FindGameObjectsWithTag("moment_force_object");
        foreach(GameObject obj in objects)
        {
            GameObject.Destroy(obj);
        }
        //reset the rotation of the plank
        plank.rotation = Quaternion.identity;
        plank.position = new Vector3(0, 0, 0);
        //recreate the original mass at the same position
        CreateMasses();
    }

    private void StartTiming()
    {
        //time = Time.time;
        balance_time = 0;
    }
}
