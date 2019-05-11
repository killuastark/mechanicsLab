using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderKeyboardController : MonoBehaviour
{
    private Slider slider;
    [Tooltip("The interval by which the slider will change value on key press")]
    [SerializeField] private float slider_delta;        //the rate to increase the slider value by when key pressed

    // Start is called before the first frame update
    void Start()
    {
        slider = transform.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            slider.value += slider_delta;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            slider.value -= slider_delta;
        }
    }
}
