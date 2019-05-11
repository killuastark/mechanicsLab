using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Attach to each gravitating mass in the scene. These gameobjects should be tagged with "gravitating".
/// </summary>
public class GravitatingBodyInfo : MonoBehaviour
{
    [SerializeField] private float mass;

    public float GetMass()
    {
        return mass;
    }

    public void SetMass(float m)
    {
        mass = m;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
    }
}
