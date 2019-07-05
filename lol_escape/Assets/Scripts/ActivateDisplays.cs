using UnityEngine;
using System.Collections;

public class ActivateDisplays : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        for (int i = 1; i < Display.displays.Length; ++i)
        {
            Display.displays[i].Activate();
        }
    }

}
