using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Short script for addition to all scenes (except the main Lab which contains it within the scenemaster)
// The script stops a memory leak due to the number of rays that are drawn, destroyed but not removed from memory.
public class UnloadUnusedResources : MonoBehaviour
{
    public int delta_frames;

    // Update is called once per frame
    void Update()
    {
        //every 20 frames unload the destroyed gameobject assets from memory.
        //This is normally only done when a new scene is loaded and doesn't appear to even be done during garbage collection.
        if (Time.frameCount % delta_frames == 0)
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
