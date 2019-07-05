using UnityEngine;
using System.Collections;

public class MinimapIconScript : MonoBehaviour {

    #region Values

    public MobController myMob;
    public Vector3 values;
    string myMobtag = "";

    #endregion


    #region Start

    // Use this for initialization
    void Start () {
        values = myMob.transform.position - this.transform.position;
        myMobtag = myMob.tag;
	}

    #endregion


    #region Update

    // Update is called once per frame
    void Update () {
        if (myMob != null)
        {
            this.transform.position = new Vector3(myMob.transform.position.x - values.x, myMob.transform.position.y - values.y, myMob.transform.position.z - values.z);
        }
        else
        {
            myMob = GameObject.FindGameObjectWithTag(myMobtag).gameObject.GetComponent<MobController>();
        }
    }

    #endregion

}
