using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GarenEColliderScript : MonoBehaviour
{

    #region Values

    [SerializeField]
    private MobController garen;

    public bool isactive;

    List<MobController> mobscollided = new List<MobController>(50);

    #endregion

    #region Function

    /// <summary>
    /// Detects the enemies that can be damaged and adds them to the List
    /// </summary>

    private void OnTriggerEnter(Collider col)
    {
            if (!col.gameObject.tag.Contains(garen.GetTeam()) && !col.gameObject.tag.Contains("Turret"))
            {
                if (col.gameObject.tag.Contains("Champion") || col.gameObject.tag.Contains("Minion"))
                {
                    mobscollided.Add(col.gameObject.GetComponent<MobController>());
                }
            }
        
    }

    /// <summary>
    /// Deals Damage to the enemies
    /// </summary>

    public void DoDamage()
    {
        foreach (MobController mob in mobscollided)
        {
            mob.DealDamage(garen.GetAD(), 1);
        }
        mobscollided.Clear();
        this.isactive = false;
    }

    #endregion

}
