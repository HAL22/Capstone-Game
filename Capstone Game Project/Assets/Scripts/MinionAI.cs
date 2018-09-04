using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MinionAI : MonoBehaviour
{

    public GameObject EnemyTower;
    public GameObject AllyTower;
    public GameObject targetObject;
    public int AttackPerMinion; // prevent minions from bunching
    public int howManyMinions; // basically this will count how many minions are attack this minion, prevent bunching
    public LayerMask EnemyLayer;
    public LayerMask AllyLayer;
    public LayerMask UILayer;
    public float searchRadius;
    public float attackRadius;
    public float attackDelay = 500;
    public int healthImpact;
    public List<GameObject> Enemies;
    public Camera cam;
    
    private NavMeshAgent agent;
    private Animator anim;
    private float attackTimer;
    private enum State { Run, Attack, Dead };
    private State state;


    // Use this for initialization
    void Start ()
    {
        //set initial variables
        attackTimer = attackDelay;
        state = State.Run;
        this.targetObject = this.EnemyTower;
        howManyMinions = 0;
        Enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        attackDelay = 1f;

        //set layers of unit and healthbar
        gameObject.layer = (int)Mathf.Log(AllyLayer.value, 2);
        transform.Find("Healthbar Canvas").gameObject.layer = (int)Mathf.Log(UILayer.value, 2);
        gameObject.GetComponentInChildren<healthbarFaceCamera>().cam = this.cam;

        //set starting animation
        anim = GetComponent<Animator>();
        anim.SetBool("Run", true);
        anim.SetBool("AttackToRun", true);
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

    public void setMinionData(GameObject EnemyTower, GameObject AllyTower, LayerMask EnemyLayer, LayerMask AllyLayer, float searchRadius, float attackRadius, int healthImpact, int AttackPerMinion, Camera cam, LayerMask UILayer)
    {
        this.EnemyTower = EnemyTower;
        this.AllyTower = AllyTower;
        this.EnemyLayer = EnemyLayer;
        this.AllyLayer = AllyLayer;
        this.searchRadius = searchRadius;
        this.attackRadius = attackRadius;
        this.healthImpact = healthImpact;
        this.AttackPerMinion = AttackPerMinion;
        this.cam = cam;
        this.UILayer = UILayer;
    }

    void searchForTarget()
    {
        //if (targetObject == null) // I know that the target has died or destroyed
        //{
            targetObject = EnemyTower;
        //}
        //else if (targetObject == EnemyTower) // start searching
        //{
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
                if (Enemies[i] != null) // if the gameobject are not null
                {
                    if (Enemies[i].GetComponent<gameObjectIdentity>().ID == 0) // if its a minion
                    {
                        if (Enemies[i].GetComponent<MinionAI>().howManyMinions < AttackPerMinion)
                        {
                            Enemies[i].GetComponent<MinionAI>().targetThisMinion();
                            targetObject = Enemies[i];
                            break;
                        }
                    }
                    if (Enemies[i].GetComponent<gameObjectIdentity>().ID != 0)
                    {
                        targetObject = Enemies[i];
                        break;
                    }
                }
            }

        //Debug.Log("Current minion " + targetObject.name);
            Enemies.Clear();
        //}
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
                    hitcollider[i].gameObject.GetComponent<healthManager>().Damage(this.healthImpact);
                    break;
                }
            }
        }
    }

    public void targetThisMinion()
    {
        howManyMinions++;
    }

    public void releaseTarget()
    {
        if (this.gameObject != null && howManyMinions > 0)
        {
            howManyMinions--;
        }
    }

    public int targetsOnMinion()
    {
        return howManyMinions;
    }

    static int sortByidentity(GameObject m1,GameObject m2)//sorting priority targets
    {
        int m1ID = m1.GetComponent<gameObjectIdentity>().ID;
        int m2ID = m2.GetComponent<gameObjectIdentity>().ID;

        return m1ID.CompareTo(m2ID);
    }

    public void Die()
    {
        if (targetObject != null && targetObject.GetComponent<MinionAI>() != null)
        {
            targetObject.GetComponent<MinionAI>().releaseTarget();
        }
        agent.isStopped = true;
        anim.CrossFadeInFixedTime("Death", 0.5f);
        state = State.Dead;
        Destroy(gameObject, 1.5f);
    }
}
