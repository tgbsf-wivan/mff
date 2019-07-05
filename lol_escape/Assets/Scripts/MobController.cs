using UnityEngine;
using System.Collections;

public class MobController : MonoBehaviour
{

    #region Values

    protected struct stats
    {
        public float life;

        public float totallife;

        public float regen;

        public float ad;

        public float armor;

        public float mr;

        public float speed;

        public float attackspeed;

        public float shield;

        public float magicshield;

        public float gold;

        public float exp;

        public float attackrange;

        public float reduceddamage;

        public string team;
    }

    private float lifetimer;

    protected stats statsvalues;

    #endregion

    #region Start

    private void Start()
    {
        SetStats(100,100,0,10,0,0,1.25F,0,0,0,50,0,1,0,"Undefined");
    }

    #endregion

    #region Functions

    /// <summary>
    ///Deals Damage to the character depending on the damage type 0.True 1.AD 2.MD
    /// </summary>

    public void DealDamage(float damage, int damagetype)
    {
        if (damagetype == 0) //TrueDamage
        {
            DamageTextScript go = Instantiate(Resources.Load("Prefabs/Damage"), this.transform.position, this.transform.rotation) as DamageTextScript;
            go.Settext(""+damage);
            if (damage >= this.statsvalues.shield)
            {
                damage = Mathf.Clamp(damage - this.statsvalues.shield, 0, damage);
                this.statsvalues.shield = 0;
                this.statsvalues.life = Mathf.Clamp(this.statsvalues.life - damage, 0, this.statsvalues.totallife);
            }
            else 
            {
                this.statsvalues.shield = Mathf.Clamp(this.statsvalues.shield - damage, 0, this.statsvalues.shield);
            }
            

        }
        else if (damagetype == 1) //AD - armor
        {
            if (this.statsvalues.armor >= 0)
            {
                if (((100 - this.statsvalues.reduceddamage) / 100) * damage >= this.statsvalues.shield)
                {
                    damage = Mathf.Clamp(((100 - this.statsvalues.reduceddamage)/100) * damage - this.statsvalues.shield, 0, ((100 - this.statsvalues.reduceddamage) / 100) * damage);
                    this.statsvalues.shield = 0;
                    this.statsvalues.life = Mathf.Clamp(this.statsvalues.life - (damage * (100 / (100 + this.statsvalues.armor))), 0, this.statsvalues.totallife);
                    GameObject go = Instantiate(Resources.Load("Prefabs/Damage"), this.transform.position, Quaternion.Euler(new Vector3(60,180,0))) as GameObject;
                    go.GetComponent<DamageTextScript>().Settext("" + (int)(damage * (100 / (100 + this.statsvalues.armor))));
                    
                }
                else
                {
                    this.statsvalues.shield = Mathf.Clamp(this.statsvalues.shield - ((100 - this.statsvalues.reduceddamage) / 100) * damage, 0, this.statsvalues.shield);
                }
            }
            else
            {
                if (((100 - this.statsvalues.reduceddamage) / 100) * damage >= this.statsvalues.shield)
                {
                    damage = Mathf.Clamp(((100 - this.statsvalues.reduceddamage) / 100) * damage - this.statsvalues.shield, 0, ((100 - this.statsvalues.reduceddamage) / 100) * damage);
                    this.statsvalues.shield = 0;
                    this.statsvalues.life = Mathf.Clamp(this.statsvalues.life - (damage * (2 - (100 / (100 - this.statsvalues.armor)))), 0, this.statsvalues.totallife);
                    DamageTextScript go = Instantiate(Resources.Load("Prefabs/Damage"), this.transform.position, Quaternion.Euler(new Vector3(60, 180, 0))) as DamageTextScript;
                    go.Settext("" + (int)(damage * (2 - (100 / (100 - this.statsvalues.armor)))));
                }
                else
                {
                    this.statsvalues.shield = Mathf.Clamp(this.statsvalues.shield - ((100 - this.statsvalues.reduceddamage) / 100) * damage, 0, this.statsvalues.shield);
                }
            }
        }
        else if (damagetype == 2) //MD- mr
        {
            if (this.statsvalues.armor >= 0)
            {
                if (((100 - this.statsvalues.reduceddamage) / 100) * damage >= this.statsvalues.magicshield)
                {
                    damage = Mathf.Clamp(((100 - this.statsvalues.reduceddamage) / 100) * damage - this.statsvalues.magicshield, 0, ((100 - this.statsvalues.reduceddamage) / 100) * damage);
                    this.statsvalues.magicshield = 0;
                    if (damage >= this.statsvalues.shield)
                    {
                        damage = Mathf.Clamp(damage - this.statsvalues.shield, 0, damage);
                        this.statsvalues.shield = 0;
                        this.statsvalues.life = Mathf.Clamp(this.statsvalues.life - (damage * (100 / (100 + this.statsvalues.mr))), 0, this.statsvalues.totallife);
                    }
                    else
                    {
                        this.statsvalues.shield = Mathf.Clamp(this.statsvalues.shield - damage, 0, this.statsvalues.shield);
                    }
                }
                else
                {
                    this.statsvalues.magicshield = Mathf.Clamp(this.statsvalues.magicshield - ((100 - this.statsvalues.reduceddamage) / 100) * damage, 0, this.statsvalues.shield);
                }
            }
            else
            {

                if (((100 - this.statsvalues.reduceddamage) / 100) * damage >= this.statsvalues.magicshield)
                {
                    damage = Mathf.Clamp(((100 - this.statsvalues.reduceddamage) / 100) * damage - this.statsvalues.magicshield, 0, ((100 - this.statsvalues.reduceddamage) / 100) * damage);
                    this.statsvalues.magicshield = 0;
                    if (damage >= this.statsvalues.shield)
                    {
                        damage = Mathf.Clamp(damage - this.statsvalues.shield, 0, damage);
                        this.statsvalues.shield = 0;
                        this.statsvalues.life = Mathf.Clamp(this.statsvalues.life - (damage * (2 - (100 / (100 - this.statsvalues.mr)))), 0, this.statsvalues.totallife);
                    }
                    else
                    {
                        this.statsvalues.shield = Mathf.Clamp(this.statsvalues.shield - damage, 0, this.statsvalues.shield);
                    }
                }
                else
                {
                    this.statsvalues.magicshield = Mathf.Clamp(this.statsvalues.magicshield - ((100 - this.statsvalues.reduceddamage) / 100) * damage, 0, this.statsvalues.shield);
                }
                
            }

        }
    }

    /// <summary>
    ///Manages the Liferegeneration per second
    /// </summary>
    
    public void UpdateLife()
    {
        if (this.lifetimer >= 1)
        {
            if (this.statsvalues.life < this.statsvalues.totallife)
            {
                this.statsvalues.life = Mathf.Clamp(this.statsvalues.life + this.statsvalues.regen, 0, this.statsvalues.totallife);
            }
            this.lifetimer = 0;
        }
        else
        {
            this.lifetimer += Time.deltaTime;
        }
    }

    /// <summary>
    ///Heals the character an amount
    /// </summary>

    public void Heal(float heal)
    {
        this.statsvalues.life = Mathf.Clamp(this.statsvalues.life + heal, 0, this.statsvalues.totallife);
    }

    /// <summary>
    ///Modifies the speed based on a percentage of the base speed 
    /// </summary>
    
    public void ModifySpeed(float percentspeed)
    {
        this.statsvalues.speed = this.statsvalues.speed + this.statsvalues.speed * (percentspeed / 100);
    }

    /// <summary>
    ///Modifies the speed based on a percentage of the actual speed
    /// </summary>
    
    public void ModifySpeed(float percentspeed, float basespeed)
    {
        this.statsvalues.speed = this.statsvalues.speed + basespeed * (percentspeed / 100);
    }

    public float GetAD()
    {
        return this.statsvalues.ad;
    }

    public float GetLife()
    {
        return this.statsvalues.life;
    }

    public string GetTeam()
    {
        return this.statsvalues.team;
    }

    public void SetStats(float life, float totallife, float regen, float ad, float mr, float speed, float attackspeed, float armor, float shield, float magicshield, float gold, float exp, float attackrange, float reduceddamage, string team)
    {
        this.statsvalues.life = life;

        this.statsvalues.totallife = totallife;

        this.statsvalues.regen = regen;

        this.statsvalues.ad = ad;

        this.statsvalues.mr = mr;

        this.statsvalues.speed = speed;

        this.statsvalues.attackspeed = attackspeed;

        this.statsvalues.armor = armor;

        this.statsvalues.shield = shield;

        this.statsvalues.magicshield = magicshield;

        this.statsvalues.gold = gold;

        this.statsvalues.exp = exp;

        this.statsvalues.attackrange = attackrange;

        this.statsvalues.reduceddamage = reduceddamage;

        this.statsvalues.team = team;
}



    #endregion


}

