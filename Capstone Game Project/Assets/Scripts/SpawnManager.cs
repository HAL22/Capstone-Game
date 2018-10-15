using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class SpawnManager : MonoBehaviour {//used to spawn minions regularly and when manual requested by player

    // for spawning units

    public Transform[] spawnPos;//list of spawn positions (2 - one for each side)
    public GameObject[] model;//list of minions that can be spawned 0 = footman 1 = lich 2 = grun 3 = golem 4 = dragon
    public GameObject[] tower;// list of 2 towers
    public LayerMask[] unitLayer;//list of 2 unit layers
    public GameObject spawnEffect;//creates a spawn effect when manually spawned

    public float spawnTime = 5000;
    private float time;


    // Use this for initialization
    void Start () {
        time = spawnTime;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > spawnTime)//if spawn cooldown has passed, spawn units
        {
            spawnUnits();
            time = 0;
        }

    }

    void spawnUnits()//spwns 1 lich and 3 knights for each player
    {
        GameObject unit = Instantiate(model[0], spawnPos[0].position, spawnPos[0].rotation);//generate a knight
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);
        unit = Instantiate(model[0], spawnPos[0].position + new Vector3(1, 0, 0), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);
        unit = Instantiate(model[0], spawnPos[0].position + new Vector3(-1, 0, 0), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);
        unit = Instantiate(model[1], spawnPos[0].position + new Vector3(0, 0, -1), spawnPos[0].rotation);//generate lich
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);

        unit = Instantiate(model[0], spawnPos[1].position, spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);
        unit = Instantiate(model[0], spawnPos[1].position + new Vector3(1, 0, 0), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);
        unit = Instantiate(model[0], spawnPos[1].position + new Vector3(-1, 0, 0), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);
        unit = Instantiate(model[1], spawnPos[1].position + new Vector3(0, 0, -1), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);


    }

    public void spawnUnit(Transform target, int modelNo, int team)//used to spawn requested unit for player at requested position
    {
        GameObject unit = Instantiate(model[modelNo], target.position, target.rotation);
        unit.GetComponent<MinionAI>().setMinionData(team, tower[Mathf.Abs(1-team)], unitLayer[Mathf.Abs(1-team)]);
        GameObject effect = Instantiate(spawnEffect, target.position + new Vector3(0,1,0), Quaternion.Euler(-90,0,0),unit.transform);//creates summoning effect
        Destroy(effect, 5);

    }
}
