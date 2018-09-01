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


    // Use this for initialization
    void Start ()
    {
        attackTimer = attackDelay;
        localSetMinionData();
	}
	
	// Update is called once per frame
	void Update ()
    {
        attackTimer += Time.deltaTime;

        if (gameObject.GetComponent<healthManager>().currentHealth > 0 && attackTimer>attackDelay)
        {
            anim.CrossFadeInFixedTime("Run", 0.5f);
            SearchFortarget();
            moveToTarget();
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

    public void localSetMinionData()
    {
        this.targetObect = this.EnemyTower;
        howManyMinions = 0;
        Enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        gameObject.layer = (int)Mathf.Log(AllyLayer.value, 2);
        transform.Find("Healthbar Canvas").gameObject.layer = (int)Mathf.Log(UILayer.value, 2);
        gameObject.GetComponentInChildren<healthbarFaceCamera>().cam = this.cam;
        anim = GetComponent<Animator>();
        anim.CrossFadeInFixedTime("Run", 0.5f);
    }

    void Attack(GameObject target)
    {
        if (target != null && attackTimer>attackDelay)
        {
            attackTimer = 0;
            anim.CrossFadeInFixedTime("Attack01",0.5f);
            target.GetComponent<healthManager>().Damage(this.healthImpact);
        }

    }

    void SearchFortarget()
    {
        // will be improved when death is impleneted in all game object
        if (targetObect.GetComponent<healthManager>().currentHealth == 0)
        {
            targetObect = null;
        }

        if (targetObect == null) // I know that the target has died or destroyed
        {
            targetObect = EnemyTower;
        }

        if (targetObect == null || ( targetObect == EnemyTower)) // start searching
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
            else
            {
                targetObect = EnemyTower;
            }

            if (Enemies.Count > 0)
            {
                for (int i = 0; i < Enemies.Count; i++)
                {
                    if (Enemies[i]!=null) // if the gameobject are not null
                    {
                        if (Enemies[i].GetComponent<gameObjectIdentity>().ID == 0) // if its a minion
                        {
                            if (Enemies[i].GetComponent<MinionAI>().howManyMinions < AttackPerMinion)
                            {
                                Enemies[i].GetComponent<MinionAI>().targetThisMinion();
                                targetObect = Enemies[i];
                                break;
                            }
                            else if (Enemies.Count == 1)
                            {
                                Enemies[i].GetComponent<MinionAI>().targetThisMinion();
                                targetObect = Enemies[i];
                                break;
                            }
                        }
                        if (Enemies[i].GetComponent<gameObjectIdentity>().ID != 0 && Enemies[i].GetComponent<healthManager>().currentHealth>0)
                        {
                            targetObect = Enemies[i];
                            break;

                        }
                    }
                }
            }
        }
        /* if (targetObect != null)
         {
             if (WithInAttackDistance(targetObect))
             {
                 targetObect.GetComponent<healthManager>().Damage(10);
             }
         }*/
        attackwithinRad();
    }

    void moveToTarget()
    {
        if (targetObect != null)
        {
            SetNav();
        }
    }

    void SetNav()
    {
        agent.SetDestination(targetObect.transform.position);
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

    /// <summary>
    /// This will check if the target object is with attack distance
    /// </summary>
    /// <returns></returns>

    public bool WithInAttackDistance(GameObject targetObect)
    {
        // float distance = (float)Math.Sqrt((transform.position.x - targetObect.transform.position.x) * (transform.position.x - targetObect.transform.position.x)) ;
        float distance = (float)Math.Sqrt(Vector3.Distance(transform.position, targetObect.transform.position));
        if(targetObect!=EnemyTower)
            Debug.Log(distance);
        if (distance <= attackLength)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    /// <summary>
    /// for sorting the list of enemies
    /// </summary>
    /// <returns></returns>

    static int sortByidentity(GameObject m1,GameObject m2)
    {

        int m1ID = m1.GetComponent<gameObjectIdentity>().ID;
        int m2ID = m2.GetComponent<gameObjectIdentity>().ID;

        return m1ID.CompareTo(m2ID);


    }

    public void attackwithinRad()
    {
    
        if (targetObect != null && gameObject.GetComponent<healthManager>().currentHealth>0)
        {
            Collider[] hitcollider = Physics.OverlapSphere(transform.position, AttackRadius, EnemyLayer);

            for (int i = 0; i < hitcollider.Length; i++)
            {


                if (hitcollider[i].gameObject == targetObect && attackTimer > attackDelay)
                {
                    attackTimer = 0;
                    Debug.Log("Attack!");
                    anim.CrossFadeInFixedTime("Attack01", 0.5f);
                    hitcollider[i].gameObject.GetComponent<healthManager>().Damage(this.healthImpact);
                    break;
                }

                
            }


        }

    }

    public void Die()
    {
        if (targetObect != null && targetObect.GetComponent<MinionAI>() != null)
        {
            if(targetObect.GetComponent<healthManager>().currentHealth<=0)
            targetObect.GetComponent<MinionAI>().releaseTarget();
           // Destroy(gameObject, 1);

        }
    }
}
