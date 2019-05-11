using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a pool of trail particles on start. Adds a new trail particle on update at the attached transform's position.
/// The trail particles fade with time and are then readded to the pool.
/// </summary>
public class OrbitalTrailScript : MonoBehaviour
{
    [SerializeField] private GameObject trail_prefab;
    [SerializeField] private int num_particles;             //number of particles in the pool

    private List<GameObject> trail_particles = new List<GameObject>();      //the object pool
    private float trail_lifetime_factor = 4f;      //scale factor for trail particle lifetime
    private int trail_delta_frames = 10;      //how long between trail particles
    private int frame_count;
    //Trail colours
    [SerializeField] private Color orig_colour;
    [SerializeField] private Color final_colour;


    // Start is called before the first frame update
    void Start()
    {
        frame_count = 0;
        //create the pool
        for(int i=0; i< num_particles; i++)
        {
            GameObject new_trail = Instantiate(trail_prefab);
            new_trail.gameObject.SetActive(false);          //objects are picked from the pool based on activity in scene
            trail_particles.Add(new_trail);
        }
    }

    // FixedUpdate is called every fixed delta time
    void FixedUpdate()
    {
        frame_count++;
        //only create trail particles after a specific amount of time/frames
        if(frame_count % trail_delta_frames == 0)
        {
            GameObject next_particle = GetObjectInPool();
            //if there was actually an object left in the pool
            if(next_particle != null)
            {
                next_particle.transform.position = transform.position;      //place it where the transform currently is
                next_particle.SetActive(true);
                StartCoroutine(Fade(next_particle));                                        //start the coroutine to fade and then deactivate.
            }
           
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="particle">The particle from the object pool that is being created</param>
    /// <param name="time">The time when the particle was created.</param>
    /// <returns></returns>
    private IEnumerator Fade(GameObject particle)
    {
        float lerp_time = 0f;
        while (lerp_time <= 1)
        {
            //Debug.Log("Lerping");
            lerp_time += Time.deltaTime/trail_lifetime_factor;
            particle.GetComponent<Renderer>().material.color = Color.Lerp(orig_colour, final_colour, lerp_time);
            yield return null;
        }
        //Debug.Log("Fade ended");
        particle.SetActive(false);      //return to the object pool
        
    }

    private GameObject GetObjectInPool()
    {
        //return the first particle that is inactive in the pool
        for(int i=0; i < trail_particles.Count; i++)
        {
            if (!trail_particles[i].activeInHierarchy)
            {
                return trail_particles[i];
            }
        }
        return null;        //if no particles are available return null
    }


    private void ReturnToPool(GameObject particle)
    {
        particle.SetActive(false);      //making the particle inactive will allow it to be selected from the pool again
    }

    private void OnDestroy()
    {
        //destroy the object pool particles for this planet
        foreach(GameObject particle in trail_particles)
        {
            Destroy(particle);
        }

        //make sure to free up the memory
        Resources.UnloadUnusedAssets();
    }
}
