using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class MinionAI : MonoBehaviour
{
    //minion details
    public float searchRadius;
    public float attackRadius;
    private float attackTimer;
    public float attackDelay;
    public int healthImpact;
    private float skillTimer;
    public float skillDelay;
    public enum Type { Footman, Lich, Orc, Golem, Dragon, Grunt };
    public Type type;
    //particle effect
    public GameObject attackEffect;
    public GameObject skillEffect;
    public GameObject burnEffect;
    
    //gold dropped on death
    public GameObject Gold;

    //sound
    public AudioClip attackSound;
    public AudioClip skillSound;
    private AudioSource audio;

    //model details
    private Transform firePos;
    private NavMeshAgent agent;
    private Animator anim;

    private enum State { Run, Attack, Dead, Fear, Burn };
    private State state;

    //details for targetting
    private GameObject EnemyTower;
    private GameObject targetObject;
    private LayerMask EnemyLayer;
    private List<GameObject> Enemies;

    //fear details
    private GameObject fearTarget;
    private float fearDuration;
    private float fearTimer;

    //burn details
    private float burnDuration;
    private float burnTimer;
    private int burnCounter;

    // Use this for initialization
    void Start ()
    {
        //set initial variables
        attackTimer = attackDelay;
        skillTimer = skillDelay;
        state = State.Run;
        this.targetObject = this.EnemyTower;
        Enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        firePos = transform.Find("BulletPos1");

        //set layers of healthbar and map ui indicator
        transform.Find("Healthbar Canvas01").gameObject.layer = LayerMask.NameToLayer("Player 1 UI") ;
        transform.Find("Healthbar Canvas01").gameObject.GetComponent<healthbarFaceCamera>().cam = GameObject.Find("Camera1").GetComponent<Camera>();
        transform.Find("Healthbar Canvas02").gameObject.layer = LayerMask.NameToLayer("Player 2 UI");
        transform.Find("Healthbar Canvas02").gameObject.GetComponent<healthbarFaceCamera>().cam = GameObject.Find("Camera2").GetComponent<Camera>();

        if(gameObject.layer == 9)//if on red team, recolour healthbars and indicator
        {
            transform.Find("Healthbar Canvas01/Healthbar Background/Healthbar Foreground").gameObject.GetComponent<Image>().color = new Color32(255, 0,0,255);
            transform.Find("Healthbar Canvas02/Healthbar Background/Healthbar Foreground").gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            transform.Find("Minion Indicator/Indicator").gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }


        //set starting animation
        anim = GetComponent<Animator>();
        anim.CrossFadeInFixedTime("Run", 0.5f);

        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        attackTimer += Time.deltaTime;
        skillTimer += Time.deltaTime;

        if (state != State.Dead)//if not dead
        {
            if (state == State.Fear)//if feared
            {
                fearTimer += Time.deltaTime;
                if (fearTimer >= fearDuration)//if fear ended
                {
                    state = State.Run;
                }
                agent.SetDestination(transform.position - (fearTarget.transform.position - transform.position));//run away from fear target
                agent.isStopped = false;
            }
            else if (state == State.Burn)//if burning
            {
                burnTimer += Time.deltaTime;
                if(burnTimer > burnCounter)//if burn still active and a second has passed take damage
                {
                    this.GetComponent<healthManager>().Damage(3);
                    burnCounter++;
                }
                if (burnTimer >= burnDuration)//if burn over
                {
                    state = State.Run;
                }
                agent.SetDestination(transform.position + new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), 0, UnityEngine.Random.Range(-10.0f, 10.0f)));//if burning run in random directions
                agent.isStopped = false;
            }
            else if (attackTimer > attackDelay)//if not currently dead or attacking search and move to target
            {
                if (state != State.Run)
                {
                    state = State.Run;
                    anim.CrossFadeInFixedTime("Run", 0.5f);
                }

                searchForTarget();
                moveToTarget();
                attackWithinRad();

            }
        }
    }

    public void setMinionData(int team, GameObject EnemyTower, LayerMask EnemyLayer)//used to set minion team and targets on creation
    {
        this.EnemyTower = EnemyTower;
        this.EnemyLayer = EnemyLayer;
        gameObject.layer = 8+team;
    }

    void searchForTarget()//search for enemy
    {
            targetObject = EnemyTower;//initially sets enemy tower
            Enemies.Clear();// start with fresh enemies
            // check specified radius for enemies
            Collider[] hitcollider = Physics.OverlapSphere(transform.position, searchRadius, EnemyLayer);

            if (hitcollider.Length > 0)
            {
                for (int i = 0; i < hitcollider.Length; i++)
                {
                    Enemies.Add(hitcollider[i].gameObject);
                }

                if (Enemies.Count > 0)//sorts enemies by priority order
                {
                    Enemies.Sort(sortByidentity);
                }
            }


            for (int i = 0; i < Enemies.Count; i++)//targets first valid target in list
            {
                if (Enemies[i] != null && Enemies[i].GetComponent<healthManager>().getHealth()>0) // if the gameobject are not null
                {
                    targetObject = Enemies[i];
                    break;
                }
            }
            Enemies.Clear();
    }


    void moveToTarget()//if target out of range, move towards them
    {
        if (targetObject != null)
        {
            agent.SetDestination(targetObject.transform.position);
            agent.isStopped = false;
        }
    }

    void attackWithinRad()//checks if target withi range, attack it if it is
    {
        if (targetObject != null)
        {
            Collider[] hitcollider = Physics.OverlapSphere(transform.position, attackRadius, EnemyLayer);

            for (int i = 0; i < hitcollider.Length; i++)//checks if target is in attack range
            {
                if (hitcollider[i].gameObject == targetObject)//if it is
                {
                    transform.LookAt(targetObject.transform);//look at it
                    attackTimer = 0;
                    agent.isStopped = true;
                    state = State.Attack;

                    Collider[] flamecollider = new Collider[0];

                    if (type == Type.Dragon && skillTimer > skillDelay)//if dragon and fire breath off cooldown find how many targets can be breathed upon
                    {
                        float radians = transform.rotation.eulerAngles.y / 360 * 2 * Mathf.PI;
                        flamecollider = Physics.OverlapSphere(transform.position + new Vector3(Mathf.Sin(radians) * attackRadius, 0, Mathf.Cos(radians) * attackRadius), 3f, EnemyLayer);
                    }


                    if ((type == Type.Golem) && skillTimer > skillDelay && hitcollider.Length >2)//golem does fear instead of attack, but only if 3 or more targets in range
                    {
                        anim.CrossFadeInFixedTime("Skill", 0.5f);
                        audio.clip = skillSound;
                        audio.Play();
                        GameObject effect = Instantiate(skillEffect, transform.position+ new Vector3(0,2.5f,0), transform.rotation);
                        Destroy(effect, 2f);
                        skillTimer = 0;
                        foreach (Collider scareTarget in hitcollider)
                        {
                            if (scareTarget != null)
                                scareTarget.gameObject.GetComponent<MinionAI>().Fear(5, gameObject);
                        }
                    }
                    else if ((type == Type.Dragon) && skillTimer > skillDelay && flamecollider.Length > 2)//dragon does burn instead of attack, but only if 3 or more tarets in range
                    {
                        anim.CrossFadeInFixedTime("Skill", 0.5f);
                        audio.clip = skillSound;
                        audio.Play();
                        GameObject effect = Instantiate(skillEffect, transform.position+new Vector3(0,2.0f,0), transform.rotation);
                        Destroy(effect, 2f);
                        skillTimer = 0;
                        foreach (Collider burnTarget in flamecollider)
                        {
                            if (burnTarget != null)
                            {
                                GameObject burn = Instantiate(burnEffect, burnTarget.transform.position+new Vector3(0,2.5f,0), burnTarget.transform.rotation, burnTarget.transform);
                                Destroy(burn, 5f);
                                burnTarget.gameObject.GetComponent<MinionAI>().Burn(5);
                            }
                                
                        }
                    }
                    else//minion does regular attack
                    {
                        anim.CrossFadeInFixedTime("Attack01", 0.5f);
                        audio.clip = attackSound;
                        audio.Play();
                        GetComponent<AudioSource>().Play();
                        if (attackEffect == null)//if not attack effect (ie melee attack)
                        {
                            hitcollider[i].gameObject.GetComponent<healthManager>().Damage(this.healthImpact);
                        }
                        else//if it has a ranged effect (ie the lich) shoot bullet
                        {
                            GameObject shootBullet = (GameObject)Instantiate(attackEffect, firePos.position, firePos.rotation);
                            shootBullet.GetComponent<Bullet>().SetData(targetObject, 15.0f);
                        }
                    }
                    
                    
                    break;
                }
            }
        }
    }

    static int sortByidentity(GameObject m1,GameObject m2)//sorting priority targets
    {
        int m1ID = m1.GetComponent<gameObjectIdentity>().ID;
        int m2ID = m2.GetComponent<gameObjectIdentity>().ID;

        return m1ID.CompareTo(m2ID);
    }

    public void Die()//when health reaches 0 death animation, drop gold, and destroy after delay
    {
        agent.isStopped = true;
        anim.CrossFadeInFixedTime("Death", 0.5f);
        state = State.Dead;
        DropGold();
        Destroy(gameObject, 1.5f);
    }

    public void Fear(float fearDuration, GameObject fearTarget)//sets initial fear variables
    {
        if(type != Type.Golem)//connot fear other golems
        {
            this.fearTarget = fearTarget;
            state = State.Fear;
            anim.CrossFadeInFixedTime("Run", 0.5f);
            this.fearDuration = fearDuration;
            fearTimer = 0;
        }
        
    }

    public void Burn(float burnDuration)//set initial burn variables
    {
        if (type != Type.Golem && type != Type.Dragon)//connot burn golem or dragons
        {
            state = State.Burn;
            anim.CrossFadeInFixedTime("Run", 0.5f);
            this.burnDuration = burnDuration;
            burnTimer = 0;
            burnCounter = 0;
        }

    }

    public void DropGold()//set gold dropped layer for corresponding player
    {
        GameObject gold = Instantiate(Gold, transform.position, transform.rotation);
        if(gameObject.layer == 9)
        {
            gold.layer = LayerMask.NameToLayer("Currency 1");
            gold.transform.Find("gold_bar").gameObject.layer = LayerMask.NameToLayer("Currency 1");
        }
        else
        {
            gold.layer = LayerMask.NameToLayer("Currency 2");
            gold.transform.Find("gold_bar").gameObject.layer = LayerMask.NameToLayer("Currency 2");
        }
        gold.GetComponent<Gold>().setAmount(5, EnemyLayer);
    }

}
