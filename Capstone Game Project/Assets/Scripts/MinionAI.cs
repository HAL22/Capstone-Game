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
    public string EnemyLayer;
    public string AllyLayer;
    public float searchRadius;
    public float attackLength;
    public int healthImpact;
    public List<GameObject> Enemies;
    public Camera cam;
    public float attackrad;
    

    private LayerMask EnemyraycastLayer;
    private LayerMask AllyraycastLayer;
    private NavMeshAgent agent;


    // Use this for initialization
    void Start ()
    {


        localSetMinionData();
        
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        KillGame();

        SearchFortarget();
        moveToTarget();
		
	}

    void Attack(GameObject target)
    {
        if (target != null)
        {
            Debug.Log("attack");
            target.GetComponent<healthManager>().Damage(this.healthImpact);
        }

    }

    void SearchFortarget()
    {
        // will be improved when death is impleneted in all game object
        if (targetObect != null) 
        {

            if (targetObect.GetComponent<healthManager>() != null)
            {
                if (targetObect.GetComponent<healthManager>().currentHealth <= 0)
                {
                    targetObect = null;

                }
            }


        }


        if (targetObect == null) // I know that the target has died or destroyed
        {
            targetObect = EnemyTower;

        }

        if (targetObect == null || (WithInAttackDistance(EnemyTower) == false && targetObect == EnemyTower)) // start searching
        {

            Enemies.Clear();// start with fresh enemies

            // I check the specified radius for enemies //Collider[] hitCollider = Physics.OverlapSphere(myTransform.position, rad, raycastLayer);
            Collider[] hitcollider = Physics.OverlapSphere(transform.position, searchRadius, EnemyraycastLayer);

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

                     

                        targetObect = Enemies[i];
                        break;

                    }

                }
            }


            // do the attack

            attackwithinRad();



        }









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

    /// <summary>
    /// When the one of the towers gets destroyed, for testing
    /// </summary>

    void KillGame()
    {

        if (AllyTower == null || EnemyTower == null)
        {
            Application.Quit();
        }

    }



    public void setMinionData(GameObject EnemyTower, GameObject AllyTower, string EnemyLayer, string AllyLayer, float searchRadius, float attackLength, int healthImpact,int AttackPerMinion, Camera cam)
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

    }

    public void localSetMinionData()
    {
        this.targetObect = this.EnemyTower;
        howManyMinions = 0;
        this.EnemyraycastLayer = 1 << LayerMask.NameToLayer(this.EnemyLayer);
        this.AllyraycastLayer = 1 << LayerMask.NameToLayer(this.AllyLayer);
        Enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();

        gameObject.layer = LayerMask.NameToLayer(AllyLayer);

        gameObject.GetComponentInChildren<healthbarFaceCamera>().cam = this.cam;
        attackrad = 5;
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
        float distance = Vector3.Distance(transform.position, targetObect.transform.position);
      //  Debug.Log(distance);
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

        if (targetObect != null)
        {
            Collider[] hitcollider = Physics.OverlapSphere(transform.position, attackrad, EnemyraycastLayer);

            for (int i = 0; i < hitcollider.Length; i++)
            {
                
                    hitcollider[i].gameObject.GetComponent<healthManager>().Damage(10);
                    //break;

                
            }


        }

    }

    public void Die()
    {
        if (targetObect != null && targetObect.GetComponent<MinionAI>() != null)
        {
            targetObect.GetComponent<MinionAI>().releaseTarget();
            Destroy(gameObject, 1);

        }
    }
}
