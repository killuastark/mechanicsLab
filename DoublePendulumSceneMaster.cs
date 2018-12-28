using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Controls the UI and initial conditions of the double pendulum simulation

public class DoublePendulumSceneMaster : MonoBehaviour {

    public Material wireMaterial;
    public Material pathMaterial;
    public Transform hinge;
    public Transform pendulum1;
    public Transform pendulum2;

    //UI inputs
    public InputField mass1_input;
    public InputField mass2_input;
    public Slider mass1_slider;
    public Slider mass2_slider;
    public InputField length1_input;
    public InputField length2_input;
    public Slider length1_slider;
    public Slider length2_slider;
    public Toggle trail_toggle;
    public InputField gravity_input;
    public InputField timestep_input;


    private Vector3 prev_position1;
    private Vector3 prev_position2;

    private DoublePendulumMotion dp_motion;          //the function that calculates and sets the position

    //limiting values
    float max_mass = 10f;
    float max_length = 50f;
    [SerializeField] private float minTimeStep = 0.001f;
    [SerializeField] private float maxTimeStep = 0.5f;

    //lighting and trails/paths
    private bool trailOn;           //if a trail should be drawn

    // Use this for initialization
    void Start() {
        trailOn = false;                //start with no trail
        trail_toggle.isOn = false;

        prev_position1 = pendulum1.position;
        prev_position2 = pendulum2.position;

        //get the dp_motion class
        dp_motion = hinge.GetComponent<DoublePendulumMotion>();

        //set listeners
        mass1_input.onEndEdit.AddListener(delegate { UpdateMass(1, "input"); });
        mass2_input.onEndEdit.AddListener(delegate { UpdateMass(2, "input"); });
        mass1_slider.onValueChanged.AddListener(delegate { UpdateMass(1, "slider"); });
        mass2_slider.onValueChanged.AddListener(delegate { UpdateMass(2, "slider"); });
        length1_input.onEndEdit.AddListener(delegate { UpdateLength(1, "input"); });
        length2_input.onEndEdit.AddListener(delegate { UpdateLength(2, "input"); });
        length1_slider.onValueChanged.AddListener(delegate { UpdateLength(1, "slider"); });
        length2_slider.onValueChanged.AddListener(delegate { UpdateLength(2, "slider"); });
        trail_toggle.onValueChanged.AddListener(delegate { UpdateTrail(); });
        gravity_input.onEndEdit.AddListener(delegate { UpdateGravity(); });
        timestep_input.onEndEdit.AddListener(delegate { UpdateTimeStep(); });

        //Set values for the pendulum properties
        mass1_input.text = dp_motion.GetMass1().ToString("F2");
        mass2_input.text = dp_motion.GetMass2().ToString("F2");
        length1_input.text = dp_motion.GetLength1().ToString("F2");
        length2_input.text = dp_motion.GetLength2().ToString("F2");
        gravity_input.text = Mathf.Abs(Physics.gravity.y).ToString("F2");
        timestep_input.text = Time.fixedDeltaTime.ToString("F2");

    }

    // Update is called once per frame
    void Update() {
        //get the positions of the pendulums
        Vector3 current_pos1 = pendulum1.position;
        Vector3 current_pos2 = pendulum2.position;
        //Draw in the wire attaching each to the next (and hinge) but remember to destroy these after a frame
        DrawWire(hinge.position, current_pos1, true, wireMaterial);
        DrawWire(current_pos1, current_pos2, true, wireMaterial);

        //Draw path
        if (trailOn)
        {
            DrawWire(prev_position2, current_pos2, false, pathMaterial);
        }
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

    //a function to update the mass of the pendulum
    //INPUT: the mass that is being changed: 1 or 2; whether the InputField ("input") or Slider ("slider") is being used
    private void UpdateMass(int mass, string field)
    {
        float new_mass;
        if (field.Equals("input"))
        {
            if (mass == 1)
            {
                if (float.TryParse(mass1_input.text, out new_mass))
                {
                    if (new_mass != 0 && new_mass <= max_mass)
                    {
                        dp_motion.SetMass1(new_mass);
                        mass1_slider.value = new_mass;
                    }
                }

            }
            else
            {
                if (float.TryParse(mass2_input.text, out new_mass))
                {
                    if (new_mass != 0 && new_mass <= max_mass)
                    {
                        dp_motion.SetMass2(new_mass);
                        mass2_slider.value = new_mass;
                    }
                }
            }
        }
        else
        {
            if (mass == 1)
            {
                new_mass = mass1_slider.value;
                mass1_input.text = new_mass.ToString("F2");
                dp_motion.SetMass1(new_mass);
            }
            else
            {
                new_mass = mass2_slider.value;
                mass2_input.text = new_mass.ToString("F2");
                dp_motion.SetMass2(new_mass);
            }
        }
    }

    private void UpdateLength(int length, string field)
    {
        float new_length;
        if (field.Equals("input"))
        {
            if (length == 1)
            {
                if (float.TryParse(length1_input.text, out new_length))
                {
                    if (new_length != 0 && new_length <= max_length)
                    {
                        dp_motion.SetLength1(new_length);
                        length1_slider.value = new_length;
                    }

                }

            }
            else
            {
                if (float.TryParse(length2_input.text, out new_length))
                {
                    if (new_length != 0 && new_length <= max_length)
                    {
                        dp_motion.SetLength2(new_length);
                        length2_slider.value = new_length;
                    }
                }
            }
        }
        else
        {
            if (length == 1)
            {
                new_length = length1_slider.value;
                length1_input.text = new_length.ToString("F2");
                dp_motion.SetLength1(new_length);
            }
            else
            {
                new_length = length2_slider.value;
                length2_input.text = new_length.ToString("F2");
                dp_motion.SetLength2(new_length);
            }
        }
    }


    public void UpdateTrail()
    {
        if (trailOn)
        {
            trailOn = false;
        }
        else
        {
            trailOn = true;
        }

    }

    private void UpdateGravity()
    {
        float grav;
        if (float.TryParse(gravity_input.text, out grav))
        {
            if (grav > 0 && grav < 20)
            {
                Physics.gravity = new Vector3(0, -grav, 0);
            }
            else
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
                gravity_input.text = "9.81";
            }
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
            gravity_input.text = "9.81";
        }
    }

    private void UpdateTimeStep()
    {
        float delta_t;
        if(float.TryParse(timestep_input.text,out delta_t))
        {
            if(delta_t >= minTimeStep && delta_t < maxTimeStep)
            {
                Time.fixedDeltaTime = delta_t;
            }
            else
            {
                Time.fixedDeltaTime = 0.01f;
                timestep_input.text = "0.01";
            }
        }
        else
        {
            Time.fixedDeltaTime = 0.01f;
            timestep_input.text = "0.01";
        }
    }
}
