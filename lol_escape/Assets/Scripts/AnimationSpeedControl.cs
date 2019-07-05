using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedControl : MonoBehaviour {

    public float speed = 0.5f;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Animator>().speed = speed;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
