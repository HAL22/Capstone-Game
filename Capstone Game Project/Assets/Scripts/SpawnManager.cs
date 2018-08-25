using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    // for spawning units

    public Transform[] spawnPos;
    public GameObject unit;
    public GameObject[] tower;
    public LayerMask[] unitLayer;
    public int searchRadius;
    public float attacklength;
    public int healthimpact;
    public int attackPerMinion;

    public Camera[] cam;

    private double time;


    // Use this for initialization
    void Start () {
        time = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime();
        if (time > 500)
        {
            spawnUnits();
            time = 0;
        }
		
	}

    void spawnUnits()
    {

        unit.GetComponent<MinionAI>().setMinionData(tower[0], tower[1], unitLayer[0], unitLayer[1], searchRadius, attacklength, healthimpact, attackPerMinion, cam[0], 1f);
        Instantiate(unit, spawnPos[0].position, spawnPos[0].rotation);

        unit.GetComponent<MinionAI>().setMinionData(tower[1], tower[0], unitLayer[1], unitLayer[0], searchRadius, attacklength, healthimpact, attackPerMinion, cam[1], 1f);
        Instantiate(unit, spawnPos[1].position, spawnPos[1].rotation);
    }
}
