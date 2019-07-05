using UnityEngine;
using System.Collections;

public class CamaraScript : MonoBehaviour {

    #region Values

    public HUDController hud;

    public GameObject target;

    private Vector3 distance = new Vector3(0,10.2f,8.6f);

    private float zoom;

    #endregion


    #region Start

    // Use this for initialization
    void Start () {

        //distance = target.transform.position - this.transform.position;
	}

    #endregion


    #region Update
    // Update is called once per frame
    void Update () {

        this.transform.position = new Vector3(target.transform.position.x + distance.x,10.2f + zoom, target.transform.position.z + distance.z + zoom/2);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(this.transform.position.y > 6)
            {
                zoom -= 0.2f;
            }
        }else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (this.transform.position.y < 10.2f)
            {
                zoom += 0.2f;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Shop"))
            {

            }else if (hit.collider.CompareTag("AllyTurret"))
            {
                //hit.collider.gameObject;
            }
        }
    }

    #endregion


    #region Functions

    public void Settarget(GameObject target)
    {
        this.target = target;
    }

    #endregion


}
