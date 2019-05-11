using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideGameObject : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    
    public void ShowHideObject()
    {
        if (obj.activeInHierarchy)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }
}
