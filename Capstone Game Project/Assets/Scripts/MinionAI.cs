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
    public float healthImpact;
    public List<GameObject> Enemies;
    

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
		
	}



    public void setMinionData(GameObject EnemyTower, GameObject AllyTower, string EnemyLayer, string AllyLayer, float searchRadius, float attackLength, float healthImpact,int AttackPerMinion)
    {
        this.EnemyTower = EnemyTower;
        this.AllyTower = AllyTower;
        this.EnemyLayer = EnemyLayer;
        this.AllyLayer = AllyLayer;
        this.searchRadius = searchRadius;
        this.attackLength = attackLength;
        this.healthImpact = healthImpact;
        this.AttackPerMinion = AttackPerMinion;

    }

    public void localSetMinionData()
    {
        this.targetObect = this.EnemyTower;
        howManyMinions = 0;
        this.EnemyraycastLayer = 1 << LayerMask.NameToLayer(this.EnemyLayer);
        this.AllyraycastLayer = 1 << LayerMask.NameToLayer(this.AllyLayer);
        Enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();

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

    public bool WithInAttackDistance()
    {
        float distance = (float)Math.Sqrt((transform.position.x - targetObect.transform.position.x) * (transform.position.x - targetObect.transform.position.x) + (transform.position.y-targetObect.transform.position.y) * (transform.position.y - targetObect.transform.position.y) + (transform.position.z-targetObect.transform.position.z) * (transform.position.z - targetObect.transform.position.z));

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

    public void Die()
    {
        if (targetObect != null && targetObect.GetComponent<MinionAI>() != null)
        {
            targetObect.GetComponent<MinionAI>().releaseTarget();
            Destroy(gameObject, 1);

        }
    }
}
