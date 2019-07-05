using UnityEngine;
using System.Collections;

public class ProyectileController : MonoBehaviour {

    //clase que hace que se pegue algo al player de forma muy gitanesca
    
    #region Values

    private Vector3 objectivePosition;

    private GameObject objective = null;

    private Transform tr;

    private Rigidbody rb;

    private float damage;

    private float speed = 14;

    #endregion


    #region Start

    // Use this for initialization

    void Start()
    {
        this.tr = this.transform;
        this.rb = this.GetComponent<Rigidbody>();
    }

    #endregion


    #region Update

    // Update is called once per frame
    void Update()
    {
        if (this.objective != null)
        {
            this.objectivePosition = new Vector3(objective.transform.position.x, objective.transform.position.y + objective.transform.lossyScale.y/2, objective.transform.position.z);
            this.tr.LookAt(objectivePosition);
            this.rb.velocity = speed * 100 * this.tr.forward * Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion


    #region Function

    /// <summary>
    /// Sets the objective the proyectile
    /// </summary>
    
    public void SetObjective(GameObject go)
    {
        this.objective = go;
        this.objectivePosition = objective.transform.position;
    }

    /// <summary>
    /// Deals damage on collision with the objective
    /// </summary>

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == objective)
        {
            objective.GetComponent<MobController>().DealDamage(this.damage, 1);

            Destroy(this.gameObject);
        }
    }

    public void Setdamage(float damage)
    {
        this.damage = damage;
    }

    public void Setspeed(float speed)
    {
        this.speed = speed;
    }

    #endregion


}
