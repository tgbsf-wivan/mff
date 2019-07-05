using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurretController : MobController {


    #region Values

    public Image LifeBar;
    public Image DamageBar;
    public Canvas LifeBarCanvas;
    private bool aggro = false;
    public GameObject objective = null;
    public bool death = false;
    private bool shoot = false;
    public GameObject gun;
    public GameObject proyectile;
    private float proyectilespeed;
    private Transform myTransform;
    public GameObject child;
    private LineRenderer Line;
    private float distance;
    public string team;
    public string enemyteam;
    public bool active;

    #endregion


    #region Start

    // Use this for initialization
    void Start () {
        this.myTransform = this.transform;
        this.Line = this.GetComponent<LineRenderer>();
        Line.enabled = false;
        Line.SetPosition(0, new Vector3(this.transform.position.x - 0.17f, this.transform.position.y + 5.65f, this.transform.position.z + 0.6f));
        this.statsvalues.life = 3500;
        this.statsvalues.totallife = 3500;
        this.statsvalues.ad = 152;
        this.statsvalues.armor = 40;
        this.statsvalues.mr = 40;
        this.statsvalues.attackspeed = 0.83f;
        this.statsvalues.attackrange = 7.4f;
        this.proyectilespeed = 15;
    }

    #endregion


    #region Update

    // Update is called once per frame
    void Update () {
        if(active == true) { 
        Collider[] hitColliders = Physics.OverlapSphere(child.transform.position, this.statsvalues.attackrange);

            if (aggro == false)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].tag.Contains(enemyteam))
                    {
                        if (hitColliders[i].tag.Contains("Champion"))
                        {
                            if (MinionCheck(hitColliders) == false)
                            {
                                objective = hitColliders[i].gameObject;
                                aggro = true;
                            }
                        }
                        else
                        {
                            objective = hitColliders[i].gameObject;
                            aggro = true;
                        }

                    }
                }
            }
            else if (aggro == true)
            {
                if (this.statsvalues.life > 0)
                {
                    if (objective)
                    {
                        if (objective.GetComponent<MobController>().GetLife() > 0)
                        {
                            distance = Vector3.Distance(this.transform.position, objective.transform.position);
                            if (distance < this.statsvalues.attackrange)
                            {
                                Line.enabled = true;
                                Line.SetPosition(1, new Vector3(objective.transform.position.x, objective.transform.position.y + objective.transform.lossyScale.y / 1.5f, objective.transform.position.z));
                                if (shoot == false)
                                {

                                    StartCoroutine(Shoot());

                                }
                            }
                            else
                            {
                                aggro = false;
                                Line.enabled = false;
                            }
                        }
                        else
                        {
                            aggro = false;
                            Line.enabled = false;
                        }
                    }
                    else
                    {
                        aggro = false;
                        Line.enabled = false;
                    }
                }
            }
        }

        LifeBar.fillAmount = this.statsvalues.life / this.statsvalues.totallife;
        LifeBarCanvas.transform.rotation = FindObjectOfType<CamaraScript>().transform.rotation;

        if (LifeBar.fillAmount != DamageBar.fillAmount)
        {
            if (LifeBar.fillAmount > DamageBar.fillAmount)
            {
                DamageBar.fillAmount = DamageBar.fillAmount + 0.01f;

                if (LifeBar.fillAmount < DamageBar.fillAmount)
                {
                    DamageBar.fillAmount = LifeBar.fillAmount;
                }
            }
            else if (LifeBar.fillAmount < DamageBar.fillAmount)
            {
                DamageBar.fillAmount = DamageBar.fillAmount - 0.01f;

                if (LifeBar.fillAmount > DamageBar.fillAmount)
                {
                    DamageBar.fillAmount = LifeBar.fillAmount;
                }
            }
        }

        if (this.statsvalues.life < 0)
        {
            this.statsvalues.life = 0;
            this.tag = "Death";
            Line.enabled = false;
            LifeBarCanvas.enabled = false;
        }

	}

    #endregion


    #region Function

    /// <summary>
    /// Checks if there are minions
    /// </summary>

    private bool MinionCheck(Collider[] colliders)
    {
        bool aux = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag.Contains(("Minion")+enemyteam))
            {
                aux = true;
            }
        }
        return aux;
    }

    /// <summary>
    /// Instantiates a proyectile
    /// </summary>

    private IEnumerator Shoot()
    {
        if (objective.GetComponent<MobController>().GetLife() > 0)
        {
            shoot = true;
            if (distance < this.statsvalues.attackrange)
            {
                proyectile = Instantiate(Resources.Load("Prefabs/Proyectiles/Proyectile1"), gun.transform.position, gun.transform.rotation) as GameObject;
                proyectile.GetComponent<ProyectileController>().SetObjective(this.objective);
                proyectile.GetComponent<ProyectileController>().Setdamage(this.statsvalues.ad);
                proyectile.GetComponent<ProyectileController>().Setspeed(this.proyectilespeed);
            }
            yield return new WaitForSeconds(1 / this.statsvalues.attackspeed);
            shoot = false;
        }
    }


    /// <summary>
    /// Draws a Gizmo when you select it
    /// </summary>

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(child.transform.position, this.statsvalues.attackrange);

    }

    #endregion

}
