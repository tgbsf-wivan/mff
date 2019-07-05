using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChampionController : MobController {

    #region Values

    protected Vector3 targetPosition;

    protected bool isMoving;

    protected bool isAttacking;

    private Quaternion newRotation;
    
    public UnityEngine.AI.NavMeshAgent navagent;

    private Collider colliderchampion;
    
    /// <summary>
    /// LifeBar of the Character
    /// </summary>
    [SerializeField]
    [Header("HUD Life Bar")]
    private Image LifeBar;
    /// <summary>
    /// LifeBar Text of the Character
    /// </summary>
    [SerializeField]
    private Text Lifetext;

    /// <summary>
    /// Is the character life lees than 0
    /// </summary>
    protected bool death;

    /// <summary>
    /// Can the character be rotated
    /// </summary>
    private bool rotate;

    /// <summary>
    /// What is the character objective
    /// </summary>
    private MobController objective;

    /// <summary>
    /// Where did the player click
    /// </summary>
    private Vector2 clickposition;

    /// <summary>
    /// Minimaps Camera
    /// </summary>
    private Camera MinimapCamera;

    /// <summary>
    /// Character Icon
    /// </summary>
    private GameObject Icon;

    private LineRenderer line ;
    #endregion


    #region Start

    // Use this for initialization
    protected virtual void Start () {
        Icon = GameObject.FindGameObjectWithTag("IconLine");
        this.statsvalues.team = "Caos"; // Gitanesco por favor luego quitar
        LifeBar = GameObject.FindGameObjectWithTag("LifeBar").GetComponent<Image>(); 
        Lifetext = GameObject.FindGameObjectWithTag("LifeText").GetComponent<Text>();
        navagent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        navagent.speed = this.statsvalues.speed/70;
        LifeBar.fillAmount = this.statsvalues.life / this.statsvalues.totallife;
        Lifetext.text = (int)this.statsvalues.life + " / " + (int)this.statsvalues.totallife + "                      +" + this.statsvalues.regen;
        this.colliderchampion = this.GetComponent<Collider>();
        navagent.SetAreaCost(4, 5);
        line = this.Icon.GetComponent<LineRenderer>();
        MinimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Camera>();
    }

    #endregion


    #region Update

    // Update is called once per frame
    protected virtual void Update () {
        LineUpdater();
        navagent.speed = this.statsvalues.speed / 70;
        if (this.statsvalues.life <= 0)
        {
            this.statsvalues.life = 0;
            LifeBar.fillAmount = this.statsvalues.life / this.statsvalues.totallife;
            Lifetext.text = (int)this.statsvalues.life + " / " + (int)this.statsvalues.totallife + "                      +" + this.statsvalues.regen;
            Kill();       
        }else
        {
            base.UpdateLife();
            LifeBar.fillAmount = this.statsvalues.life / this.statsvalues.totallife;
            Lifetext.text = (int)this.statsvalues.life + " / " + (int)this.statsvalues.totallife + "                      +" + this.statsvalues.regen;

            if (Input.GetMouseButtonDown(1))
            {
                clickposition = Input.mousePosition;
                SetTargetPosition();
            }

            if (this.transform.position.x < targetPosition.x + .5 && this.transform.position.x > targetPosition.x - .5)
            {
                if (this.transform.position.z < targetPosition.z + .5 && this.transform.position.z > targetPosition.z - .5)
                {
                    if (!this.objective)
                    {
                        isMoving = false;
                    }
                }
            }

            if (isMoving == true)
            {
                MovePlayer();
            }

            if (this.objective)
            {
                if (this.objective.GetLife() > 0)
                {
                    float distance;
                    distance = Vector3.Distance(this.transform.position, objective.transform.position);
                    distance = distance - (objective.transform.lossyScale.x);

                    if (distance <= this.statsvalues.attackrange)
                    {
                        isAttacking = true;                        
                    }
                    else
                    {
                        isAttacking = false;
                        targetPosition = this.objective.transform.position;
                        MovePlayer();
                    }
                }
                else
                {
                    isAttacking = false;
                    this.objective = null;
                    targetPosition = this.transform.position;
                }
            }
        }

        if(rotate == true)
        {
            RotatePlayer();
        }
    }

    #endregion


    #region Functions

    private void Attack()
    {
        if (objective)
        {
            objective.GetComponent<MobController>().DealDamage(this.statsvalues.ad, 1);
        }
    }

    /// <summary>
    /// Sets a new position when the user clicks
    /// </summary>

    public void SetTargetPosition()
    {
        //Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray;
        if (clickposition.x > 1600 && clickposition.y < 320)
        {
            ray = MinimapCamera.ScreenPointToRay(Input.mousePosition);
        }
        else
        { 
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);           
        }
        //float point = 0f;
        RaycastHit hite;
        if(Physics.Raycast(ray,out hite))
        {
            if (!hite.collider.gameObject.tag.Contains(this.statsvalues.team) && !hite.collider.gameObject.tag.Contains("Obstacle"))
            {
                print(hite.collider.gameObject.tag + ": " + hite.point);
                if (objective != hite.collider.gameObject.GetComponent<MobController>())
                {
                    isAttacking = false;
                    objective = hite.collider.gameObject.GetComponent<MobController>();
                }

            }
            else
            {
                this.objective = null;
                isAttacking = false;
            }

            targetPosition = hite.point;
            

        }
        

        isMoving = true;
            

    }

    /// <summary>
    /// Kills the champion
    /// </summary>


    public void Kill()
    {
        death = true;
        this.tag = "Death";
        navagent.speed = 0;
        //colliderchampion.enabled = false;      
    }

    /// <summary>
    /// Revives the champion
    /// </summary>

    private void Revive()
    {
        death = false;
        this.tag = "Champion";
        navagent.speed = this.statsvalues.speed / 70;
        this.transform.position = new Vector3(-0.045f, -0.24f, -0.79f);
        this.transform.rotation.SetEulerAngles(new Vector3(0, 0, 0));
        colliderchampion.enabled = true;
    }

    /// <summary>
    /// Sets a new location
    /// </summary>

    protected void MovePlayer()
    {
        navagent.SetDestination(targetPosition);
        rotate = true;
        //this.transform.LookAt(targetPosition);
        
    }

    /// <summary>
    /// Updates the line on the minimap
    /// </summary>

    void LineUpdater()
    {

        if (navagent != null && navagent.path != null)
        {

            UnityEngine.AI.NavMeshPath path = navagent.path;

            line.SetVertexCount(path.corners.Length);

            for (int i = 0; i < path.corners.Length; i++)
            {
                line.SetPosition(i, new Vector3(path.corners[i].x, 0, path.corners[i].z));
            }
        }

    }

    /// <summary>
    /// Rotates the player
    /// </summary>

    private void RotatePlayer()
    {
        if (navagent.destination - this.transform.position != Vector3.zero)
        {
            newRotation = Quaternion.LookRotation(navagent.destination - this.transform.position, Vector3.up);
        }
        newRotation.x = 0.0f;
        newRotation.z = 0.0f;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, 0.05f);

        float degrees = this.transform.rotation.eulerAngles.y - newRotation.eulerAngles.y;

        if (Mathf.Abs(degrees) <= 1)
        {
            rotate = false;
        }

    }



    #endregion

}
