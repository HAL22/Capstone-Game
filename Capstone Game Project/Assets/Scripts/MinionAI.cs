using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class MinionAI : MonoBehaviour
{
    // For droping gold
    private int DropGoldAmount;
    private GameObject Gold;

    private NavMeshAgent agent;
    private Animator anim;
    private float attackTimer;
    private enum State { Run, Attack, Dead, Fear, Burn };
    private State state;

    private GameObject EnemyTower;
    private GameObject targetObject;
    private LayerMask EnemyLayer;
    private float searchRadius;
    private float attackRadius;
    private float attackDelay;
    private int healthImpact;
    private float speed;
    private List<GameObject> Enemies;

    private GameObject bullet;
    private Transform firePos;


    // Use this for initialization
    void Start ()
    {
        //set initial variables
        attackTimer = attackDelay;
        state = State.Run;
        this.targetObject = this.EnemyTower;
        Enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        attackDelay = 1f;
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

        if (state != State.Dead && attackTimer>attackDelay)//if not currently dead or attacking search and move to target
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

    public void DropGold()
    {
        GameObject gold = Instantiate(Gold, transform.position, transform.rotation);
        gold.GetComponent<Gold>().setAmount(5);
    }

    public void setMinionData(int team, GameObject EnemyTower, LayerMask EnemyLayer, int model, GameObject bullet,GameObject gold)
    {
        this.EnemyTower = EnemyTower;
        this.EnemyLayer = EnemyLayer;
        this.Gold = gold;
        gameObject.layer = 8+team;
        if(model==0)//knight
        {
            searchRadius = 10;
            attackRadius = 2;
            healthImpact = 3;
            this.bullet = null;
        }
        else if (model == 1)//lich
        {
            searchRadius = 10;
            attackRadius = 8;
            healthImpact = 5;
            this.bullet = bullet;
        }
        else if (model == 2)//golem
        {
            searchRadius = 10;
            attackRadius = 3;
            healthImpact = 6;
            this.bullet = null;
        }
        else if (model == 3)//dragon
        {
            searchRadius = 10;
            attackRadius = 6;
            healthImpact = 6;
            this.bullet = bullet;

        }
        else
        {
            Debug.Log("Invalid Unit Number");
            Destroy(this);
        }


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
                if (Enemies[i] != null && Enemies[i].GetComponent<healthManager>().currentHealth>0) // if the gameobject are not null
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
                    if (bullet == null)
                    {
                        hitcollider[i].gameObject.GetComponent<healthManager>().Damage(this.healthImpact);
                    }
                    else
                    {
                        GameObject shootBullet = (GameObject)Instantiate(bullet, firePos.position, firePos.rotation);
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

}
