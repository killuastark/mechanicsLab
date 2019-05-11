using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLabEventHandler : MonoBehaviour
{
    public delegate void PlanetAdded();
    public static event PlanetAdded OnPlanetAdded;

    public static void PlanetAddedTriggerEvent()
    {
        if (OnPlanetAdded != null)
        {
            OnPlanetAdded();
        }
    }

}
