using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MinionAI : MonoBehaviour
{

    public GameObject EnemyTower;
    public GameObject AllyTower;
    public GameObject targetObect;
    public int AttackPerMinion; // prevent minions from bunching
    public int howManyMinions; // basically this will count how many minions are attack this minion, prevent bunching
    public LayerMask EnemyLayer;
    public LayerMask AllyLayer;
    public LayerMask UILayer;
    public float searchRadius;
    public float attackLength;
    public float attackDelay = 500;
    public int healthImpact;
    public List<GameObject> Enemies;
    public Camera cam;
    public float AttackRadius; // will attack in this radius
    
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
        this.targetObect = this.EnemyTower;
        howManyMinions = 0;
        Enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        attackDelay = 1f;

        //set layers of unit and healthbar
        gameObject.layer = (int)Mathf.Log(AllyLayer.value, 2);
        transform.Find("Healthbar Canvas").gameObject.layer = (int)Mathf.Log(UILayer.value, 2);
        gameObject.GetComponentInChildren<healthbarFaceCamera>().cam = this.cam;

        GetComponent<healthManager>().healthBar = GetComponentInChildren<healthBarForrground>()

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

        if (state != State.Dead && attackTimer>attackDelay)//if not currently attacking search and move to target
        {
            if(state != State.Run)
                anim.CrossFadeInFixedTime("Run", 0.5f);
                
            searchForTarget();
            moveToTarget();
            attackWithinRad();
            
        }
    }

    public void setMinionData(GameObject EnemyTower, GameObject AllyTower, LayerMask EnemyLayer, LayerMask AllyLayer, float searchRadius, float attackLength, int healthImpact, int AttackPerMinion, Camera cam, LayerMask UILayer, float rad)
    {
        this.EnemyTower = EnemyTower;
        this.AllyTower = AllyTower;
        this.EnemyLayer = EnemyLayer;
        this.AllyLayer = AllyLayer;
        this.searchRadius = searchRadius;
        this.attackLength = attackLength;
        this.healthImpact = healthImpact;
        this.AttackPerMinion = AttackPerMinion;
        this.cam = cam;
        this.UILayer = UILayer;
        this.AttackRadius = rad;
    }

    void searchForTarget()
    {
        if (targetObect == null) // I know that the target has died or destroyed
        {
            targetObect = EnemyTower;
        }
        else if (targetObect == EnemyTower) // start searching
        {
            Enemies.Clear();// start with fresh enemies
            // I check the specified radius for enemies //Collider[] hitCollider = Physics.OverlapSphere(myTransform.position, rad, raycastLayer);
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

            if (Enemies.Count > 0)
            {
                for (int i = 0; i < Enemies.Count; i++)
                {
                    if (Enemies[i] != null) // if the gameobject are not null
                    {
                        if (Enemies[i].GetComponent<gameObjectIdentity>().ID == 0) // if its a minion
                        {
                            if (Enemies[i].GetComponent<MinionAI>().howManyMinions < AttackPerMinion)
                            {
                                Enemies[i].GetComponent<MinionAI>().targetThisMinion();
                                targetObect = Enemies[i];
                                break;
                            }
                        }
                        if (Enemies[i].GetComponent<gameObjectIdentity>().ID != 0)
                        {
                            targetObect = Enemies[i];
                            break;
                        }
                    }
                }
            }
        }
    }


    void moveToTarget()
    {
        if (targetObect != null)
        {
            agent.SetDestination(targetObect.transform.position);
        }
    }

    void attackWithinRad()
    {
        if (targetObect != null)
        {
            Collider[] hitcollider = Physics.OverlapSphere(transform.position, AttackRadius, EnemyLayer);

            for (int i = 0; i < hitcollider.Length; i++)
            {
                if (hitcollider[i].gameObject == targetObect)
                {
                    attackTimer = 0;
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
        if (targetObect != null && targetObect.GetComponent<MinionAI>() != null)
        {
            targetObect.GetComponent<MinionAI>().releaseTarget();
        }
        anim.CrossFadeInFixedTime("Death", 0.5f);
        state = State.Dead;
        Destroy(this, 2);
    }
}
