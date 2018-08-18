using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    

    private LayerMask EnemyraycastLayer;
    private LayerMask AllyraycastLayer;


    // Use this for initialization
    void Start ()
    {
        
		
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

    }
}
