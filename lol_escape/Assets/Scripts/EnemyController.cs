using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MobController {

    #region Values

    public Image LifeBar;

    public Canvas LifeBarCanvas;

    public float radius;

    public GameObject Turretobjective;

    private bool objectiveDetected;

    private bool aggro;

    private bool move;

    private GameObject objective;

    private GameObject playerObject;

    private string team;

    private string enemyteam;

    private Animator Minionanim;

    private int lane;

    private Quaternion targetRotation;

    private UnityEngine.AI.NavMeshAgent agent;

    private Transform myTransform;


    #endregion


    #region Start

    private void Start()
    {
        this.agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        this.objectiveDetected = false;
        this.Minionanim = this.GetComponent<Animator>();
        this.myTransform = this.transform;
        this.team = this.gameObject.GetComponentInParent<SpawnerController>().team;
        this.enemyteam = this.gameObject.GetComponentInParent<SpawnerController>().enemyteam;
        this.lane = this.gameObject.GetComponentInParent<SpawnerController>().lane;
        this.Turretobjective = GameObject.FindGameObjectWithTag("Turret"+ enemyteam + lane);
        this.agent.destination = Turretobjective.transform.position;

        this.statsvalues.life = 455;
        this.statsvalues.totallife = 455;
        this.statsvalues.ad = 12;
        this.statsvalues.attackrange = 110 / 122.22f;
        this.statsvalues.attackspeed = 1.25f;
        this.radius = 3f;

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


    #region Function

    /// <summary>
    /// Draws a Gizmo when you select it
    /// </summary>

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.myTransform.position, this.radius);

        var nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nav == null || nav.path == null)
            return;

        var line = this.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.blue };
            line.SetWidth(0.5f, 0.5f);
            line.SetColors(Color.blue, Color.blue);
        }

        var path = nav.path;

        line.SetVertexCount(path.corners.Length);

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]);
        }

    }

    /// <summary>
    /// Destroys the minion after 2 seconds
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
    /// Deals damage to the enemy
    /// </summary>

    private void Attack()
    {            
            objective.GetComponent<MobController>().DealDamage(this.statsvalues.ad,1);
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
        distance = distance - (objective.transform.lossyScale.x);
        if (distance <= this.statsvalues.attackrange)
        {
            if (!this.objective.CompareTag("Death"))
            {
                Quaternion rot = Quaternion.LookRotation(objective.transform.position - this.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 40);
                this.agent.speed = 0;
                Minionanim.SetBool("autoattack", true);
            }
            else
            {
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
