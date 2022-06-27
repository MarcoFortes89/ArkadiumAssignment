using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public void OnClick() 
    {
        Transform container=GameObject.Find("MenuContainer").transform;
        if (container.childCount < 1)
            Instantiate(Resources.Load("Prefabs/Settings"), container);
    }
}