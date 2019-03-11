using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideImageScript : MonoBehaviour
{
    public Image image;             //the image to be shown/hid

   public void ShowHideImage()
    {
        if (image.IsActive())
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }
}
