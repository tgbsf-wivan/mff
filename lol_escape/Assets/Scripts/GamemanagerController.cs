using UnityEngine;
using System.Collections;

public class GamemanagerController : MonoBehaviour {


    #region Values

    public GameObject[] turretscaos = new GameObject[11];
    public GameObject[] turretscaoscolliders = new GameObject[11];
    public GameObject[] turretsorder = new GameObject[11];
    public GameObject Menu;
    public GameObject player;
    public SpawnerController[] spawn = new SpawnerController[3];
    public CamaraScript maincamera;
    public int skin;

    #endregion


    #region Start

    // Use this for initialization
    void Start () {
        skin = 0;
        //turretscaos[0].tag = ("TurretCaos0");
        //turretscaos[3].tag = ("TurretOrder1");
        //turretscaoscolliders[3].tag = ("TurretOrder1Collider");
        //turretscaos[6].tag = ("TurretCaos2");

    }

    #endregion


    #region Update

    // Update is called once per frame
    void Update () {
        /*
        if (turretscaos[3].CompareTag("Death"))
        {
            turretscaoscolliders[3].tag = ("Death");
            turretscaos[4].tag = ("TurretOrder1");
            turretscaoscolliders[4].tag = ("TurretOrder1Collider");
            spawn[1].SetTurretObjective(turretscaos[4]);
            

        }
        */
    }

    #endregion


    #region Functions

    /// <summary>
    /// Shows the menu
    /// </summary>

    public void ShowMenu()
    {
        Time.timeScale = 0;
        Menu.SetActive(true);

    }

    /// <summary>
    /// Closes the menu
    /// </summary>

    public void CloseMenu()
    {
        Time.timeScale = 1;
        Menu.SetActive(false);

    }

    /// <summary>
    /// Changues the player skin
    /// </summary>

    public void ChangueSkin(int number)
    {
        if (skin != number)
        {
            skin = number;
            Destroy(player);
            this.player = Instantiate(Resources.Load("Prefabs/Garen/FinalPrefabs/GarenSkin" + number), player.transform.position, player.transform.rotation) as GameObject;
            maincamera.Settarget(this.player);
        }
        
    }

    #endregion



}
