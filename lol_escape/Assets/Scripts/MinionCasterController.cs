using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinionCasterController : MobController {

    #region Values

    public Image LifeBar;

    public Canvas LifeBarCanvas;

    public float radius;

    public GameObject Turretobjective;

    public GameObject gun;   

    private GameObject objective;

    private GameObject playerObject;

    private GameObject proyectile;

    private bool aggro;

    private bool move;

    private bool objectiveDetected;

    private string enemyteam;

    private UnityEngine.AI.NavMeshAgent agent;

    private Transform myTransform;

    private int lane;

    private Quaternion targetRotation;

    private Animator Minionanim;

    private float proyectilespeed;

    #endregion


    #region Start

    void Start()
    {
        this.agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        this.objectiveDetected = false;
        this.Minionanim = this.GetComponent<Animator>();
        this.myTransform = this.transform;
        this.statsvalues.team = this.gameObject.GetComponentInParent<SpawnerController>().team;
        this.enemyteam = this.gameObject.GetComponentInParent<SpawnerController>().enemyteam;
        this.lane = this.gameObject.GetComponentInParent<SpawnerController>().lane;
        this.Turretobjective = GameObject.FindGameObjectWithTag("Turret" + enemyteam + lane);
        this.agent.destination = Turretobjective.transform.position;  
        this.statsvalues.life = 290;
        this.statsvalues.totallife = 290;
        this.statsvalues.ad = 23;
        this.statsvalues.attackrange = 550 / 122.22f;
        this.statsvalues.attackspeed = 1.25f;
        this.radius = 3f;
        this.proyectilespeed = 8;

        this.objective = null;
        aggro = false;


    }

    #endregion


    #region Update

    void Update()
    {
        if (this.statsvalues.life > 0)
        {
            if (objectiveDetected == false)
            {
                Collider[] hitColliders = Physics.OverlapSphere(this.myTransform.position, radius);

                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].gameObject.CompareTag("Turret" + enemyteam + lane))
                    {
                        this.objective = GameObject.FindGameObjectWithTag("Turret" + enemyteam + lane);
                        this.agent.destination = objective.transform.position;
                        this.objectiveDetected = true;
                    }
                    else if (hitColliders[i].gameObject.tag.Contains(enemyteam))
                    {
                        if (this.objective)
                        {
                            if (!this.objective.CompareTag("Turret" + enemyteam + lane))
                            {
                                this.objective = hitColliders[i].gameObject;
                                this.agent.destination = objective.transform.position;
                                this.objectiveDetected = true;
                            }
                        }
                        else
                        {
                            this.objective = hitColliders[i].gameObject;
                            this.agent.destination = objective.transform.position;
                            this.objectiveDetected = true;
                        }

                    }
                }
            }
            else if (objectiveDetected == true)
            {
                if (move == false)
                {
                    StartCoroutine(CoroutineMovement());
                }

            }


        }
        else
        {
            StartCoroutine(Destroy());
        }

        LifeBar.fillAmount = this.statsvalues.life / this.statsvalues.totallife;
        LifeBarCanvas.transform.rotation = FindObjectOfType<CamaraScript>().transform.rotation;


    }

    #endregion


    #region Functions

    /// <summary>
    /// Draws a Gizmo when you select it
    /// </summary>

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.myTransform.position, this.radius);

    }

    /// <summary>
    /// Destroys the minion in 2 seconds
    /// </summary>

    private IEnumerator Destroy()
    {
        this.tag = "Death";
        this.Minionanim.SetBool("death", true);
        LifeBarCanvas.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Instantiates a proyectile
    /// </summary>

    private void Shoot()
    {
        if (objective.GetComponent<MobController>().GetLife() > 0)
        {
            //shoot = true;
            proyectile = Instantiate(Resources.Load("Prefabs/Proyectiles/Proyectile2"+this.statsvalues.team), gun.transform.position, gun.transform.rotation) as GameObject;
            proyectile.GetComponent<ProyectileController>().SetObjective(this.objective);
            proyectile.GetComponent<ProyectileController>().Setdamage(this.statsvalues.ad);
            proyectile.GetComponent<ProyectileController>().Setspeed(this.proyectilespeed);          
            //shoot = false;
        }
    }


    #region Cosis

    //clase que hace que se pegue algo al player de forma muy gitanesca

    //   private Vector3 player;

    //   private GameObject playerObject;

    //   private Transform tr;

    //   private Rigidbody rb;

    //   private float veloc = 500;

    //// Use this for initialization
    //void Start ()
    //   {
    //       this.playerObject = GameObject.FindGameObjectWithTag("Player");
    //       this.player = playerObject.transform.position;

    //       this.tr = this.transform;
    //       this.rb = this.GetComponent<Rigidbody>();
    //}

    //// Update is called once per frame
    //void Update () {

    //       this.player = playerObject.transform.position;
    //       this.tr.LookAt(player);
    //       this.rb.velocity = veloc * this.tr.forward * Time.deltaTime;
    //}

    #endregion


    /// <summary>
    /// Manages the minions movement
    /// </summary>

    private IEnumerator CoroutineMovement()
    {
        move = true;
        float distance;
        distance = Vector3.Distance(this.transform.position, objective.transform.position);
        distance = distance - objective.transform.lossyScale.x / 2;
        if (distance <= this.statsvalues.attackrange)
        {
            if (!this.objective.CompareTag("Death"))
            {
                this.agent.speed = 0;
                Minionanim.SetBool("autoattack", true);
            }
            else
            {
                Quaternion rot = Quaternion.LookRotation(objective.transform.position - this.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
                this.agent.destination = Turretobjective.transform.position;
                Minionanim.SetBool("autoattack", false);
                this.agent.speed = 3;
                objectiveDetected = false;
            }

        }
        else if (distance > this.radius)
        {
            this.agent.destination = Turretobjective.transform.position;
            Minionanim.SetBool("autoattack", false);
            this.agent.speed = 3;
            objectiveDetected = false;


        }
        else if (distance > this.statsvalues.attackrange && distance <= this.radius)
        {
            this.agent.speed = 3;
            this.agent.destination = objective.transform.position;
            Minionanim.SetBool("autoattack", false);
        }
        yield return new WaitForSeconds(0.5f);
        move = false;
    }

    #endregion


}
