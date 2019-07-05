using UnityEngine;
using System.Collections;

public class DamageTextScript : MonoBehaviour {

	// Use this for initialization
	void Start () {       
        Invoke("CallDestroy", 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = this.transform.position + Vector3.up / 100 + Vector3.forward/200;
	}

    void CallDestroy()
    {
        Destroy(this.gameObject);
    }

    public void Settext(string value)
    {
        this.GetComponent<TextMesh>().text = value;
    }
}
