using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Instantiates an input_field or text field, updates its position to follow the transform that it is attached to.
/// This can be accessed from a different script to update its text.
/// INPUT: Either an InputFIeld gameobject or a Text gameobject
///         The canvas on which they should show.
/// </summary>
public class TextboxFollowGameObject : MonoBehaviour
{
    private GravityLab2DSceneMaster sm;
    [SerializeField] private GameObject text_prefab;            //the INPUTFIELD OR TEXT PREFAB
    private Canvas canvas;
    private GameObject new_text;            //the text gameobject to control the transform.
    private InputField input_field;                       //the inputfield component (if added)
    private Text text_field;                                //the text_field (if added)
    private float scale_position;                   //how far along the forward direction to place the textbox

    // Start is called before the first frame update
    void Awake()
    {
        sm = GameObject.FindGameObjectWithTag("sceneMaster").GetComponent<GravityLab2DSceneMaster>();
        canvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Canvas>();
        new_text = Instantiate(text_prefab);
        new_text.transform.SetParent(canvas.transform);         //set it to the canvas
        new_text.transform.position = Camera.main.WorldToScreenPoint(transform.position + scale_position * transform.forward - 1.5f*transform.up);
        if(text_prefab.GetComponent<InputField>() != null)
        {
            input_field = new_text.GetComponent<InputField>();
            input_field.GetComponentInParent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            //input_field.text = "TESTING";
        }
        else
        {
            text_field = new_text.GetComponent<Text>();
            //text_field.text = "TESTING";
        }

        scale_position = transform.root.localScale.x * 2f;

        sm.ShowHideVelocity();      //the show velocity toggle may be switched off, so run this from the scenemaster
    }


    // Update is called once per frame
    void Update()
    {
        //constantly update the position of the text object to correspond with the transform it is attached to.
        new_text.transform.position = Camera.main.WorldToScreenPoint(transform.position + scale_position*transform.forward - 1.5f*transform.up);
    }

    public string GetText()
    {
        if(input_field != null)
        {
            return input_field.text;
        }
        else
        {
            return text_field.text;
        }
        
    }

    public void SetText(string t)
    {
        if (input_field != null)
        {
            input_field.text = t;
        }
        else
        {
            text_field.text = t;
        }
    }

    public InputField GetInputField()
    {
        return input_field;
    }
}
