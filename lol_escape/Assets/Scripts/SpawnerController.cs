using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour {

    #region Values

    public float wavetime = 75;

    private float timer = 0;

    public float wave = 0;

    public string team;

    public string enemyteam;

    public int lane;

    private List<GameObject> minions;

    #endregion


    #region Start
    
    // Use this for initialization
    void Start ()
    {
        Vector3 pos = this.transform.position;
        this.minions = new List<GameObject>(50);

    }

    #endregion


    #region Update

    // Update is called once per frame
    void Update () {
        
        timer += Time.deltaTime;

        if (timer > wavetime)
        {
            wavetime = 30;
            wave++;
            StartCoroutine(MinionSpawn());                        
            timer = 0;
        }
        
       
    }

    #endregion


    #region Functions

    /// <summary>
    /// Spawns a number of minions depending on the wave
    /// </summary>

    public IEnumerator MinionSpawn()
    {       

        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                this.minions.Add(Instantiate(Resources.Load("Prefabs/Minion/Minion" + team), this.transform.position, this.transform.rotation) as GameObject);
                minions[minions.Count - 1].transform.SetParent(this.transform);
                yield return new WaitForSeconds(1);
            }else 
            {

                this.minions.Add(Instantiate(Resources.Load("Prefabs/Minion/MinionCaster" + team), this.transform.position, this.transform.rotation) as GameObject);
                minions[minions.Count - 1].transform.SetParent(this.transform);
                yield return new WaitForSeconds(1);

            }
        }

        if (wave % 3 == 0)
        {
            this.minions.Add(Instantiate(Resources.Load("Prefabs/Minion/Minion"+team), this.transform.position, this.transform.rotation) as GameObject);
            minions[minions.Count-1].transform.SetParent(this.transform);
        }   
              
    }

    /// <summary>
    /// Sets a turret objective for each minion
    /// </summary>

    public void SetTurretObjective(GameObject newobjective)
    {
        foreach(GameObject minion in minions)
        {
            minion.GetComponent<EnemyController>().Turretobjective = newobjective;
            minion.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(newobjective.transform.position);
        }
    }

    #endregion


}
