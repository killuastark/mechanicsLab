using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add this component to a button. Clicking the button will then add the attached GameObject (planet) to the scene
/// </summary>
public class ClickButtonAddGameObject : MonoBehaviour
{
    [SerializeField] private GameObject gameobject_to_add;
    
    [SerializeField] private Vector3 add_location;          //if above is false, then what is the location that the object should instantiate at

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddObject()
    {
        GameObject new_object = Instantiate(gameobject_to_add);
        new_object.transform.position = add_location;
        GravityLabEventHandler.PlanetAddedTriggerEvent();           //trigger the planet added event
    }  

    
}
