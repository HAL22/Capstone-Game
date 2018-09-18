using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class MinionAI : MonoBehaviour
{
    
    public float searchRadius;
    public float attackRadius;
    public float attackDelay;
    public int healthImpact;
    public GameObject skillEffect;
    public GameObject Gold;

    public enum Type { Footman, Lich, Orc, Golem, Dragon, Grunt };
    public Type type;

    private float attackTimer;

    private Transform firePos;
    private NavMeshAgent agent;
    private Animator anim;

    private enum State { Run, Attack, Dead, Fear, Burn };
    private State state;

    private GameObject EnemyTower;
    private GameObject targetObject;
    private LayerMask EnemyLayer;
    private List<GameObject> Enemies;

    private GameObject fearTarget;
    private float fearDuration;
    private float fearTimer;

    // Use this for initialization
    void Start ()
    {
        //set initial variables
        attackTimer = attackDelay;
        state = State.Run;
        this.targetObject = this.EnemyTower;
        Enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        firePos = transform.Find("BulletPos1");

        //set layers of healthbar
        transform.Find("Healthbar Canvas01").gameObject.layer = LayerMask.NameToLayer("Player 1 UI") ;
        transform.Find("Healthbar Canvas01").gameObject.GetComponent<healthbarFaceCamera>().cam = GameObject.Find("Camera1").GetComponent<Camera>();
        transform.Find("Healthbar Canvas02").gameObject.layer = LayerMask.NameToLayer("Player 2 UI");
        transform.Find("Healthbar Canvas02").gameObject.GetComponent<healthbarFaceCamera>().cam = GameObject.Find("Camera2").GetComponent<Camera>();

        if(gameObject.layer == 9)//if on red team, recolour healthbars
        {
            transform.Find("Healthbar Canvas01/Healthbar Background/Healthbar Foreground").gameObject.GetComponent<Image>().color = new Color32(255, 0,0,255);
            transform.Find("Healthbar Canvas02/Healthbar Background/Healthbar Foreground").gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }


        //set starting animation
        anim = GetComponent<Animator>();
        anim.CrossFadeInFixedTime("Run", 0.5f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        attackTimer += Time.deltaTime;

        if (state != State.Dead)
        {
            if (state == State.Fear)
            {
                fearTimer += Time.deltaTime;
                if(fearTimer >= fearDuration)
                {
                    state = State.Run;
                }
                agent.SetDestination(transform.position - (fearTarget.transform.position - transform.position));
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

    public void DropGold()
    {
        GameObject gold = Instantiate(Gold, transform.position, transform.rotation);
        gold.GetComponent<Gold>().setAmount(5);
    }

    public void setMinionData(int team, GameObject EnemyTower, LayerMask EnemyLayer)
    {
        this.EnemyTower = EnemyTower;
        this.EnemyLayer = EnemyLayer;
        gameObject.layer = 8+team;
    }

    void searchForTarget()
    {
            targetObject = EnemyTower;
            Enemies.Clear();// start with fresh enemies
            // I check the specified radius for enemies
            Collider[] hitcollider = Physics.OverlapSphere(transform.position, searchRadius, EnemyLayer);

            if (hitcollider.Length > 0)
            {
                for (int i = 0; i < hitcollider.Length; i++)
                {
                    Enemies.Add(hitcollider[i].gameObject);
                }

                if (Enemies.Count > 0)
                {
                    Enemies.Sort(sortByidentity);
                }
            }


            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i] != null && Enemies[i].GetComponent<healthManager>().getHealth()>0) // if the gameobject are not null
                {
                    targetObject = Enemies[i];
                    break;
                }
            }
            Enemies.Clear();
    }


    void moveToTarget()
    {
        if (targetObject != null)
        {
            agent.SetDestination(targetObject.transform.position);
            agent.isStopped = false;
        }
    }

    void attackWithinRad()
    {
        if (targetObject != null)
        {
            Collider[] hitcollider = Physics.OverlapSphere(transform.position, attackRadius, EnemyLayer);

            for (int i = 0; i < hitcollider.Length; i++)
            {
                if (hitcollider[i].gameObject == targetObject)
                {
                    attackTimer = 0;
                    agent.isStopped = true;
                    state = State.Attack;
                    anim.CrossFadeInFixedTime("Attack01", 0.5f);
                    GetComponent<AudioSource>().Play();
                    if (skillEffect == null)
                    {
                        hitcollider[i].gameObject.GetComponent<healthManager>().Damage(this.healthImpact);
                    }
                    else
                    {
                        GameObject shootBullet = (GameObject)Instantiate(skillEffect, firePos.position, firePos.rotation);
                        shootBullet.GetComponent<Bullet>().SetData(targetObject, 20.0f);
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

    public void Die()
    {
        agent.isStopped = true;
        anim.CrossFadeInFixedTime("Death", 0.5f);
        state = State.Dead;
        DropGold();
        Destroy(gameObject, 1.5f);
    }

    public void Fear(float fearDuration, GameObject fearTarget)
    {
        this.fearTarget = fearTarget;
        state = State.Fear;
        this.fearDuration = fearDuration;
        fearTimer = 0;
    }

}
