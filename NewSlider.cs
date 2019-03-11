using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSlider : Slider
{
   public void SetWithCallback(float input, bool callback)
    {
        Set(input, callback);
    }
}
