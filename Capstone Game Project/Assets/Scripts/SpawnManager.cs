﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    // for spawning units

    public Transform[] spawnPos;
    public GameObject knight;
    public GameObject[] tower;
    public LayerMask[] unitLayer;
    public LayerMask[] UILayer;
    public int searchRadius;
    public float attacklength;
    public int healthimpact;
    public int attackPerMinion;

    public Camera[] cam;

    public float spawnTime = 5000;
    private float time;


    // Use this for initialization
    void Start () {
        time = spawnTime;
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > spawnTime)
        {
            spawnUnits();
            time = 0;
        }

    }

    void spawnUnits()
    {
        GameObject unit = Instantiate(knight, spawnPos[0].position, spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1], searchRadius, attacklength, healthimpact);
        unit = Instantiate(knight, spawnPos[0].position + new Vector3(1, 0, 0), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1], searchRadius, attacklength, healthimpact);
        unit = Instantiate(knight, spawnPos[0].position + new Vector3(-1, 0, 0), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1], searchRadius, attacklength, healthimpact);
        
        unit = Instantiate(knight, spawnPos[1].position, spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0], searchRadius, attacklength, healthimpact);
        unit = Instantiate(knight, spawnPos[1].position+new Vector3(1,0,0), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0], searchRadius, attacklength, healthimpact);
        unit = Instantiate(knight, spawnPos[1].position + new Vector3(-1, 0, 0), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0], searchRadius, attacklength, healthimpact);
    }
}
