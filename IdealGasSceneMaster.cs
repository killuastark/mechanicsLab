using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using System.IO;

public class IdealGasSceneMaster : MonoBehaviour {

    public ParticleSystem IdealGasParticlesSystem;
    private ParticleSystem.Particle[] particles;
    private List<float> particle_speeds;
    private float slow_factor = 100;
    private float vp;
    private int N;              //the number of particles being simulated - not the number of particles in calculations
    private float n_mol;        //the number of moles - for calculating pressure          
    private float T;
    private float m;
    private int divs;
    private float max_temp = 10000;
    private float min_temp = 10;
    private float k = 1.38E-23f;         //m2 kg s-2 K-1, Boltzmann's constant
    private float amu = 1.66E-27f;

    private int count;                      //used to ensure that the particles get setup
    //TESTING/ DATA OUTPUT
    private List<string[]> data = new List<string[]>();

    //UI 
    public InputField temp_input;           //input fields are only used to output text
    public InputField volume_input;
    public InputField pressure_input;
    public InputField num_mol_input;
    public InputField particle_mass_input;
    public Slider particle_mass_slider;
    public NewSlider temp_slider;
    public NewSlider box_slider;
    public Slider num_mol_slider;
    public Dropdown graph_dropdown;
    public GameObject data_point_prefab;
    public Transform graph_origin;
    public Transform PV_origin;
    public Transform PT_origin;
    public Transform VT_origin;
    private List<GameObject> plotted_points;
    private List<GameObject> plotted_points_PVT;
    public Toggle isPressureConstant;               //whether calculations should be made with constant pressure
    public Toggle areTrailsOn;                      //whether to show trails on some particles or not

    public Camera plotCamera;
    private Vector3 plotCameraShift;

    //distribution plot UI
    public GameObject vp_text;
    public GameObject vrms_text;
    public GameObject vavg_text;
    private TextMeshProUGUI vp_text_pro;
    private TextMeshProUGUI vavg_text_pro;
    private TextMeshProUGUI vrms_text_pro;
    //box volume
    public GameObject box;          //the box within which the gas is trapped

    // Use this for initialization
    void Start () {
        plotted_points = new List<GameObject>();        //initialise lists of plotted points
        plotted_points_PVT = new List<GameObject>();
        count = 0;
        
        particle_speeds = new List<float>();
        
        //set initial conditions
        n_mol = 1f;
        T = 500;                        //temperature starting value
        temp_input.text = "500";
        m = 12*amu;             //carbon atoms mass 12amu - default setting
        vp = Mathf.Sqrt(2 * k * T / m);
        N = (int)(1000f * n_mol);                       //simulate 1000 particles for every mole of gas
        var ps = IdealGasParticlesSystem.main;
        //ps.maxParticles = (int)(num_mol_slider.maxValue*1000);                        //sets the max particles
        //particles = new ParticleSystem.Particle[IdealGasParticlesSystem.main.maxParticles];
        ps.maxParticles = (int)(n_mol * 1000);                        //sets the max particles - particle system was starting with too many particles, so reset the max particles when n_mol updates
        particles = new ParticleSystem.Particle[IdealGasParticlesSystem.main.maxParticles];
        //break the boltzmann distribution into divs number of speed ranges
        divs = 1000;

        CalculateAndSetSpeeds(m, T, N, divs, vp);

        

        //Add listeners
        //disable input_field input
        //temp_input.onEndEdit.AddListener(delegate { UpdateTemperature(0); });
        temp_slider.onValueChanged.AddListener(delegate { UpdateTemperature(1); });
        box_slider.onValueChanged.AddListener(delegate { UpdateBoxDimensions(); });
        num_mol_slider.onValueChanged.AddListener(delegate { UpdateNumberParticles(); });
        graph_dropdown.onValueChanged.AddListener(delegate { ChangePlot(); });
        areTrailsOn.onValueChanged.AddListener(delegate { ShowHideTrails(); });
        particle_mass_slider.onValueChanged.AddListener(delegate { UpdateParticleMass(); });

        //get the correct shift from the origin transform for the plot camera
        plotCameraShift.x = plotCamera.transform.position.x - PV_origin.position.x;
        plotCameraShift.y = plotCamera.transform.position.y - PV_origin.position.y;
        plotCameraShift.z = plotCamera.transform.position.z - PV_origin.position.z;

        //get UI components
        vp_text_pro = vp_text.GetComponent<TextMeshProUGUI>();
        vavg_text_pro = vavg_text.GetComponent<TextMeshProUGUI>();
        vrms_text_pro = vrms_text.GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
    //Only
	void Update () {
        //try a few times to ensure that particle speeds are setup properly, but then stop updating
        if (count < 5)
        {
            SetParticleSpeeds(N, vp);       //try just looping this 5 times in the start function?!
            count++;
        }
        //update should continually set the text field/input field values for volume and temperature based upon the slider values
        temp_input.text = temp_slider.value.ToString("F0");
        float l = box_slider.value;
        volume_input.text = (l * l * l).ToString("F2");

        //output maxwell-boltzmann distribution speeds - most probable, average, rms
        OutputDistributionSpeeds();
    }

    private void CalculateAndSetSpeeds(float m, float T, int N, int divs, float vp)
    {
        particle_speeds = MaxwellBoltzmannSpeeds.Speeds(m, T, N, divs, vp);
        SetParticleSpeeds(N, vp);
        PlotDistribution();
    }

    //assign the particles to the calculated particle speeds from the maxwell boltzmann dist.
    //If there are fewer speeds than particles, set the remaining particle speed to the most probable speed vp
    private void SetParticleSpeeds(int N, float vp)
    {
        //get all the particles in the particle system and add them to the array particles
        IdealGasParticlesSystem.GetParticles(particles);
       
        int num_particle_speeds = particle_speeds.Count;
        
        for (int i = 0; i < N; i++)
        {
            if (num_particle_speeds > i)
            {
                
                if (particle_speeds[i] != 0)
                {
                    Vector3 p_vel_orig = particles[i].velocity;         //get the current velocity
                    Vector3 p_direction = p_vel_orig.normalized;        //get the direction which we don't want to change
                    particles[i].velocity = p_direction * particle_speeds[i] / slow_factor;     //change the speed of the particle but maintain its direction
                    //Debug.Log("Speed = " + particle_speeds[i]);
                }
                else
                {
                    Vector3 p_vel_orig = particles[i].velocity;
                    Vector3 p_direction = p_vel_orig.normalized;
                    particles[i].velocity = p_direction * vp / slow_factor;           //set to the most probable speed
                    //Debug.Log("Speed = " + particle_speeds[i]);
                }

            }
            //return the updated particles to the particle system
            IdealGasParticlesSystem.SetParticles(particles, N);
        }
    }

    //Updates the temperature and then resets the particle speeds
    //if input field was used to change the temperature then run with input = 0; if slider, then input = 1
    //If pressure is set to be constant then when updating temperature, also need to update the volume accordingly.
    private void UpdateTemperature(int input)
    {
        //get the current V/T ratio, before the new temp is set
        float box_dim = box.transform.localScale.x;
        float VT = box_dim * box_dim * box_dim / T;

        float temp;
        if (input == 0)
        {
            if (float.TryParse(temp_input.text, out temp))
            {
                if (temp >= min_temp && temp <= max_temp)
                {
                    T = temp;
                    temp_slider.value = T;
                }
                else
                {
                    temp_input.text = T.ToString("F0");
                }
            }
        }
        //if input is not 0 then the slider was used to change temperature
        //the slider properties have been set in the Unity editor
        else
        {
            T = temp_slider.value;
            temp_input.text = T.ToString("F0");
        }

        //if pressure is constant then this temperature change should cause a volume change in proportion
        //The volume needs to change without calling the box_slider on value changed event since that would start an infinite loop
        if (isPressureConstant.isOn)
        {
            
            float new_V = VT * T;       //ideal gas law for constant pressure
            float new_scale = Mathf.Pow(new_V, (1 / 3f));
            //set the box slider to the new value but not not call the update volume function
            //only do this if the scale the box will be set to is within the min/max box scales
            //if it isn't then switch off pressure being constant
            if(new_scale >= box_slider.minValue && new_scale <= box_slider.maxValue)
            {
                box_slider.SetWithCallback(Mathf.Pow(new_V, (1 / 3f)), false);      //assign the new box dimension - this will call the update volume method

                //now set the new volume and update particles
                box.transform.localScale = new Vector3(new_scale, new_scale, new_scale);
                IdealGasParticlesSystem.Clear();
                var s = IdealGasParticlesSystem.shape;      //the shape module which should be a box
                s.scale = new Vector3(90f * new_scale, 90f * new_scale, 90f * new_scale);  //make the scale 90% of the box size but also multiply by 100
                IdealGasParticlesSystem.Emit(N);
                //reset the particles to the correct speeds
                SetParticleSpeeds(N, vp);
                //Update graphs
                //PlotPVTGraphPoint();
            }
            else
            {
                isPressureConstant.isOn = false;
            }
            
        }

        //update the most probable speed
        vp = Mathf.Sqrt(2 * k * T / m);
        //update the gas particles
        CalculateAndSetSpeeds(m, T, N, divs, vp);

        //Update graphs
        PlotPVTGraphPoint();

    }

    //Plot the Maxwell-Boltzmann distribution. The x axis should be speed and the y axis the number of particles at that speed.
    private void PlotDistribution()
    {
        //Delete all the previous plotted points
        foreach(GameObject p in plotted_points)
        {
            GameObject.Destroy(p);
        }
        plotted_points.Clear();

        List<float[]> fractions = MaxwellBoltzmannSpeeds.Fractions(m, T, N, divs, vp);
        foreach(float[] data in fractions)
        {
            GameObject point = Instantiate(data_point_prefab);
            Vector3 point_position = new Vector3(graph_origin.position.x + data[1]/25f, graph_origin.position.y + data[0]*10000f, graph_origin.position.z);
            point.transform.position = point_position;
            plotted_points.Add(point);
        }
    }

    //Box dimensions are integer values from 1 to 10
    //Box dimensions are updated with a slider, the properties of which are set in the editor
    private void UpdateBoxDimensions()
    {
        //get the old V/T ratio so that if pressure is constant the temperature can be updated
        float box_dim = box.transform.localScale.x;
        float VT = box_dim * box_dim * box_dim / T;

        float new_scale = box_slider.value;
        //box dimensions are limited, so constant pressure can only apply whilst the box is free to change volume
        if(new_scale == box_slider.minValue || new_scale == box_slider.maxValue)
        {
            isPressureConstant.isOn = false;
        }
        //check if this should be a constant pressure calculation - if so then also update the temperature
        if (isPressureConstant.isOn)
        {
            T = Mathf.Round((new_scale*new_scale*new_scale) / VT);       //ideal gas law for constant pressure
            temp_slider.SetWithCallback(T, false);                  //set the slider without calling the event handler

            //now set the new temperature and reset the particle system
            //update the most probable speed
            vp = Mathf.Sqrt(2 * k * T / m);
            //update the gas particles. since temp has changed need to recalculate the speeds and then set them
            CalculateAndSetSpeeds(m, T, N, divs, vp);

        }


        box.transform.localScale = new Vector3(new_scale, new_scale, new_scale);
        IdealGasParticlesSystem.Clear();
        var s = IdealGasParticlesSystem.shape;      //the shape module which should be a box
        s.scale = new Vector3(90f * new_scale, 90f * new_scale, 90f * new_scale);  //make the scale 90% of the box size but also multiply by 100
        IdealGasParticlesSystem.Emit(N);

        //reset the particles to the correct speeds - since this is only affecting volume, speeds do not change, so no
        //need to recalculate (if constant pressure then calculation is done in above if statement).
        SetParticleSpeeds(N, vp);

        //Update graphs
        PlotPVTGraphPoint();
    }

    private float CalculatePressure()
    {
        //float ideal_constant = 1 / 500f;        //the scaling constant used for the ideal gas law
        float ideal_constant = n_mol*8.314f;
        float box_dim = box.transform.localScale.x;     //the cube width
        float P = ideal_constant * T / (box_dim * box_dim * box_dim);
        //whenever P is calculated, output it to the UI
        pressure_input.text = P.ToString("F0");
        return P;
    }


    private void PlotPVTGraphPoint()
    {
        float pressure_scale = 1/1000f;     //in order to fit plots in the graph axes
        float volume_scale = 30f;
        float temp_scale = 1 / 50f;
        float pressure = CalculatePressure();
        float box_dim = box.transform.localScale.x;
        float volume = box_dim * box_dim * box_dim;
        PlotPoint(PV_origin.transform.position, volume_scale * volume, pressure_scale * pressure);
        PlotPoint(PT_origin.transform.position, temp_scale * T, pressure_scale * pressure);
        PlotPoint(VT_origin.transform.position, temp_scale * T, volume_scale * volume);
    }

    //plot a single point on a graph x and y away from the origin. 
    private void PlotPoint(Vector3 origin, float x, float y)
    {
        GameObject point = Instantiate(data_point_prefab);
        Vector3 plot_position = new Vector3(origin.x + x, origin.y + y, origin.z);
        point.transform.position = plot_position;
        plotted_points_PVT.Add(point);              //store all the plotted PVT positions so that they can be easily cleared
    }

    private void ChangePlot()
    {
        int graph_value = graph_dropdown.value;
        if(graph_value == 0)       //plot PV plot
        {
            plotCamera.transform.position = PV_origin.transform.position + plotCameraShift;
        }
        else if(graph_value == 1)       //PT plot
        {
            plotCamera.transform.position = PT_origin.transform.position + plotCameraShift;
        }
        else if(graph_value == 2)          //VT plot
        {
            plotCamera.transform.position = VT_origin.transform.position + plotCameraShift;
        }
    }

    private void TestDistribution(float T)
    {
        int divs = 10000;
        StringBuilder sb = new StringBuilder();
        float prev_value = 1f;
        float next_value = 100f;
        
        for (int i = 0; i < divs; i++)
        {
            string[] output = new string[1];
            float frac = MaxwellBoltzmannSpeeds.ProbabilityLowerToUpper(prev_value, next_value, T, 4*1.67E-27f);
            //Debug.Log("Frac = " + frac);
            output[0] = frac.ToString("F6");
            data.Add(output);
            prev_value = next_value;
            next_value += 10f;
        }

        for(int j=0; j < divs; j++)
        {
            sb.AppendLine(string.Join(",", data[j]));
        }

        StreamWriter outStream = System.IO.File.CreateText("C:/Users/dpryd/Desktop/TESTDATA/" + "MaxBoltzDist" + T.ToString("F1") + ".csv");
        outStream.WriteLine(sb);
        outStream.Close();
    }

    private void UpdateNumberParticles()
    {
        n_mol = num_mol_slider.value;
        N = (int)(1000 * n_mol);            //simulate 1000 particles per mol
        num_mol_input.text = n_mol.ToString("F2");
        //update the particle system
        float new_scale = box.transform.localScale.x;
        var ps = IdealGasParticlesSystem.main;
        ps.maxParticles = (int)(n_mol * 1000);                        //sets the max particles
        particles = new ParticleSystem.Particle[IdealGasParticlesSystem.main.maxParticles];
        IdealGasParticlesSystem.Clear();
        var s = IdealGasParticlesSystem.shape;      //the shape module which should be a box
        s.scale = new Vector3(90f * new_scale, 90f * new_scale, 90f * new_scale);  //make the scale 90% of the box size but also multiply by 100
        IdealGasParticlesSystem.Emit(N);        //emit the correct, new number of particles

        //set the correct speeds according to maxwell boltzmann
        CalculateAndSetSpeeds(m, T, N, divs, vp);

        PlotPVTGraphPoint();
    }

    private void ShowHideTrails()
    {
        var trails = IdealGasParticlesSystem.trails;
        if (trails.enabled)
        {
            trails.enabled = false;
        }
        else
        {
            trails.enabled = true;
        }
    }

    private void UpdateParticleMass()
    {
        m = particle_mass_slider.value * amu;
        particle_mass_input.text = particle_mass_slider.value.ToString("F0");

        //update the most probable speed
        vp = Mathf.Sqrt(2 * k * T / m);
        CalculateAndSetSpeeds(m, T, N, divs, vp);
        //PlotPVTGraphPoint();                      //pressure doesn't change with change in molecular mass
    }

    //outputs the most probable, average and rms speeds for the Maxwell-Boltzmann distribution with the current ideal gas properties
    private void OutputDistributionSpeeds()
    {
        //calculate vrms and vavg - vp is already calculated
        float vrms = Mathf.Sqrt(3f / 2f) * vp;
        float vavg = (2f / Mathf.Sqrt(Mathf.PI)) * vp;
        //output to UI
        vp_text_pro.text = vp.ToString("F2") + " m/s";
        vrms_text_pro.text = vrms.ToString("F2") + " m/s";
        vavg_text_pro.text = vavg.ToString("F2") + " m/s";
    }
}
