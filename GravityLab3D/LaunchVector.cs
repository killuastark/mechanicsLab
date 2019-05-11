using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Aims the launch vector and fires a mass.
public class LaunchVector : MonoBehaviour
{
    [Tooltip("The mass to be launched")]
    [SerializeField] private GameObject launch_mass_prefab;
    [Tooltip("The maximum number of masses allowed in the scene")]
    [SerializeField] int num_launch_masses;

    private List<GameObject> launch_masses = new List<GameObject>();
    private int num_launched;
    private float rotation_speed = 4f;

    [Tooltip("How fast to project the mass. Set in UI")]
    private float launch_speed;
    [SerializeField] private Slider launch_slider;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i< num_launch_masses; i++)
        {
            GameObject new_mass = Instantiate(launch_mass_prefab);
            new_mass.SetActive(false);
            launch_masses.Add(new_mass);
        }
        //haven't launched any yet
        num_launched = 0;

        //set listeners
        launch_slider.onValueChanged.AddListener(delegate { SetLaunchSpeed(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && num_launched < num_launch_masses)
        {
            GameObject launch_mass = launch_masses[num_launched];
            launch_mass.SetActive(true);
            launch_mass.transform.position = transform.position + transform.forward;    //start it along the transform forward direction
            launch_mass.GetComponent<Rigidbody>().velocity = launch_speed*transform.forward;
            num_launched++;
        }


    }

    public void SetLaunchSpeed()
    {
        launch_speed = launch_slider.value;
    }


    //void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        RaycastHit hit;
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //        {
    //            Vector3 targetPoint = hit.point;
    //            //targetPoint.z = 0f;
    //            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
    //            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotation_speed * Time.deltaTime);

    //            launch_speed = (targetPoint - transform.position).magnitude;
    //            Debug.Log("Launch speed = " + launch_speed);
    //        }

    //    }
    //    if (Input.GetMouseButtonDown(1) && num_launched < num_launch_masses)
    //    {
    //        GameObject launch_mass = launch_masses[num_launched];
    //        launch_mass.SetActive(true);
    //        launch_mass.transform.position = transform.position + transform.forward;    //start it along the transform forward direction
    //        //launch_mass.GetComponent<Rigidbody>().velocity = launch_speed*direction_vector.transform.forward;
    //        num_launched++;
    //    }


    //}
}
