using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.IO;

public class GarenController : ChampionController
{

    #region Values

    /// <summary>
    /// E Cooldown
    /// </summary>
    [SerializeField]
    [Header("Abilities Cooldowns")]
    private float Etime = 3;
    /// <summary>
    /// Q Cooldown
    /// </summary>
    [SerializeField]
    private float Qtime = 6.5f;
    /// <summary>
    /// W Cooldown
    /// </summary>
    [SerializeField]
    private float Wtime = 2;

    /// <summary>
    /// E Cooldown time
    /// </summary>
    private float Ecooldowntime = 0;
    /// <summary>
    /// Q Cooldown time
    /// </summary>
    private float Qcooldowntime = 0;
    /// <summary>
    /// W Cooldown time
    /// </summary>
    private float Wcooldowntime = 0;

    /// <summary>
    /// Auxiliar speed
    /// </summary>
    private float auxspeed;

    /// <summary>
    /// Garen LifeBar
    /// </summary> 
    private Image LifeBarS;

    /// <summary>
    /// Garen LifeBar Canvas
    /// </summary> 
    private Canvas LifeBarSCanvas;

    /// <summary>
    /// Garen DamageBar
    /// </summary> 
    public Image DamageBar;

    /// <summary>
    /// Garen Animator
    /// </summary> 
    private Animator Garenanim;

    /// <summary>
    /// Is Q on cooldown?
    /// </summary
    private bool cooldownQ;
    /// <summary>
    /// Is W on cooldown?
    /// </summary>    
    private bool cooldownW;
    /// <summary>
    /// Is E on cooldown?
    /// </summary>     
    private bool cooldownE;

    /// <summary>
    /// E duration
    /// </summary> 
    [SerializeField]
    private int timerE = 5;

    [Header("Abilities Images")]
    private GameObject[] Images = new GameObject[10];

    [Header("Abilities Texts")]
    private GameObject[] Texts = new GameObject[5];

    /// <summary>
    /// E Collider Script
    /// </summary>
    [Header("Garen E Collider Script")]
    private GarenEColliderScript ECollider;

    #endregion


    #region Start

    // Use this for initialization
    new void Start()
    {
        Garenanim = this.GetComponent<Animator>();
        this.ResetCooldowns();
        this.SetStats();
        this.SetImages();
        this.SetTexts();
        this.SetLifeBar();
        this.ECollider = GameObject.FindGameObjectWithTag("GarenECollider").GetComponent<GarenEColliderScript>();
        base.Start();

    }

    #endregion


    #region Update

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Garenanim.SetBool("move", isMoving);
        Garenanim.SetBool("attack", isAttacking);

        if (Input.GetKey(KeyCode.B))
        {
            Garenanim.SetBool("back", true);
            targetPosition = this.transform.position;
            MovePlayer();
        }

        if (Garenanim.GetBool("back"))
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.R))
            {
                Garenanim.SetBool("back", false);
            }
        }

        if (Input.GetKey(KeyCode.LeftApple))
        {
            Garenanim.SetBool("dance", true);
        }

        if (Garenanim.GetBool("dance"))
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.R))
            {
                Garenanim.SetBool("dance", false);
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            if (cooldownQ == true)
            {
                if (Qcooldowntime <= 0)
                {
                    StartCoroutine(CorrutinaQ());
                }
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (cooldownW == true)
            {
                if (Wcooldowntime <= 0)
                {
                    StartCoroutine(CorrutinaW());
                }
            }
        }


        if (Input.GetKey(KeyCode.E))
        {
            if (cooldownE == true)
            {

                if (Ecooldowntime <= 0)
                {

                    StartCoroutine(CorrutinaE());
                }

            }
        }

        if (cooldownQ == true)
        {
            Qcooldowntime -= Time.deltaTime;
            Images[0].GetComponent<Image>().fillAmount = Qcooldowntime / 8;
            Texts[1].GetComponent<Text>().text = "" + (int)(Qcooldowntime + 1);
            if (Qcooldowntime <= 0)
            {
                if (Images[0].activeSelf == true && Images[1].activeSelf == true)
                {
                    Images[0].GetComponent<Image>().enabled = false;
                    Images[1].GetComponent<Image>().enabled = false;
                }
                Texts[1].GetComponent<Text>().enabled = false;
            }
        }

        if (cooldownW == true)
        {
            Wcooldowntime -= Time.deltaTime;
            Images[2].GetComponent<Image>().fillAmount = Wcooldowntime / 24;
            Texts[2].GetComponent<Text>().text = "" + (int)(Wcooldowntime + 1);
            if (Wcooldowntime <= 0)
            {
                if (Images[2].activeSelf == true && Images[3].activeSelf == true)
                {
                    Images[2].GetComponent<Image>().enabled = false;
                    Images[3].GetComponent<Image>().enabled = false;
                }
                Texts[2].GetComponent<Text>().enabled = false;
            }
        }

        if (cooldownE == true)
        {

            Ecooldowntime -= Time.deltaTime;
            Images[4].GetComponent<Image>().fillAmount = Ecooldowntime / 15;
            Texts[3].GetComponent<Text>().text = "" + (int)(Ecooldowntime + 1);
            if (Ecooldowntime <= 0)
            {
                if (Images[4].activeSelf == true && Images[5].activeSelf == true)
                {
                    Images[4].GetComponent<Image>().enabled = false;
                    Images[5].GetComponent<Image>().enabled = false;
                }
                Texts[3].GetComponent<Text>().enabled = false;
            }
        }

        if (base.death)
        {
            Garenanim.SetBool("death", true);
            LifeBarSCanvas.enabled = false;
        }

        
        if (!LifeBarS || !LifeBarSCanvas)
        {
            this.SetLifeBar();
        }

            LifeBarS.fillAmount = this.statsvalues.life / this.statsvalues.totallife;
            LifeBarSCanvas.transform.rotation = FindObjectOfType<CamaraScript>().transform.rotation;


            if (LifeBarS.fillAmount != DamageBar.fillAmount)
            {
                if (LifeBarS.fillAmount > DamageBar.fillAmount)
                {
                    DamageBar.fillAmount = DamageBar.fillAmount + 0.01f;

                    if (LifeBarS.fillAmount < DamageBar.fillAmount)
                    {
                        DamageBar.fillAmount = LifeBarS.fillAmount;
                    }
                }
                else if (LifeBarS.fillAmount < DamageBar.fillAmount)
                {
                    DamageBar.fillAmount = DamageBar.fillAmount - 0.01f;

                    if (LifeBarS.fillAmount > DamageBar.fillAmount)
                    {
                        DamageBar.fillAmount = LifeBarS.fillAmount;
                    }
                }
            }
            
        
        
    }

    #endregion


    #region Function

    public void SetStats()
    {
        this.statsvalues.totallife = 616.28f;
        this.statsvalues.life = this.statsvalues.totallife;
        this.statsvalues.regen = 7.84f;
        this.statsvalues.ad = 57.88f;
        this.statsvalues.attackspeed = 0.625f;
        this.statsvalues.speed = 340;
        this.statsvalues.armor = 27.536f;
        this.statsvalues.mr = 32.1f;
        this.statsvalues.shield = 0;
        this.statsvalues.magicshield = 0;
        this.statsvalues.attackrange = 175/122.22f;
    }

    public void SetImages()
    {
        this.Images[0] = GameObject.FindGameObjectWithTag("QCooldown1");
        this.Images[1] = GameObject.FindGameObjectWithTag("QCooldown2");
        this.Images[2] = GameObject.FindGameObjectWithTag("WCooldown1");
        this.Images[3] = GameObject.FindGameObjectWithTag("WCooldown2");
        this.Images[4] = GameObject.FindGameObjectWithTag("ECooldown1");
        this.Images[5] = GameObject.FindGameObjectWithTag("ECooldown2");
    }

    public void SetTexts()
    {
        this.Texts[1] = GameObject.FindGameObjectWithTag("QTime");
        this.Texts[2] = GameObject.FindGameObjectWithTag("WTime");
        this.Texts[3] = GameObject.FindGameObjectWithTag("ETime");
    }

    public void SetLifeBar()
    {
        this.LifeBarS = GameObject.FindGameObjectWithTag("LifeBarS").GetComponent<Image>();
        this.LifeBarSCanvas = GameObject.FindGameObjectWithTag("LifeBarSCanvas").GetComponent<Canvas>();
        this.DamageBar = GameObject.FindGameObjectWithTag("DamageBar").GetComponent<Image>();
    }

    public void ResetCooldowns()
    {
        cooldownE = true;
        cooldownW = true;
        cooldownQ = true;
    }

    /// <summary>
    /// Manages E's cooldown
    /// </summary>

    public IEnumerator CorrutinaE()
    {
        if (!ECollider)
        {
            this.ECollider = GameObject.FindGameObjectWithTag("GarenECollider").GetComponent<GarenEColliderScript>();
        }

        Garenanim.SetBool("Etime", true);
        cooldownE = false;
        for (int i = 0; i < timerE; i++)
        {
            ECollider.isactive = true;
            ECollider.DoDamage();
            yield return new WaitForSeconds(Etime / 5);

        }
        ECollider.enabled = false;
        Ecooldowntime = 15;
        Images[4].GetComponent<Image>().enabled = true;
        Images[5].GetComponent<Image>().enabled = true;
        Texts[3].GetComponent<Text>().enabled = true;
        cooldownE = true;
        Garenanim.SetBool("Etime", false);
    }

    /// <summary>
    /// Manages Q's cooldown
    /// </summary>

    public IEnumerator CorrutinaQ()
    {
        Garenanim.SetBool("QTime", true);
        cooldownQ = false;
        auxspeed = this.statsvalues.speed;
        this.ModifySpeed(30);
        //print(this.statsvalues.speed);
        yield return new WaitForSeconds(2);
        this.ModifySpeed(-30, auxspeed);
        //print(this.statsvalues.speed);
        yield return new WaitForSeconds(Qtime-2);
        Qcooldowntime = 8;        
        Images[0].GetComponent<Image>().enabled = true;
        Images[1].GetComponent<Image>().enabled = true;
        Texts[1].GetComponent<Text>().enabled = true;
        cooldownQ = true;
        Garenanim.SetBool("QTime", false);

    }

    /// <summary>
    /// Manages W's cooldown
    /// </summary>

    public IEnumerator CorrutinaW()
    {
        cooldownW = false;
        this.statsvalues.reduceddamage = 30;
        yield return new WaitForSeconds(Wtime);
        this.statsvalues.reduceddamage = 0;
        //print(this.statsvalues.reduceddamage);
        Wcooldowntime = 24;
        Images[2].GetComponent<Image>().enabled = true;
        Images[3].GetComponent<Image>().enabled = true;
        Texts[2].GetComponent<Text>().enabled = true;
        cooldownW = true;

    }



    #endregion


}
