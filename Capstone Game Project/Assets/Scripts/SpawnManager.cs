using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    // for spawning units

    public Transform[] spawnPos;
    public GameObject[] model;
    public GameObject[] tower;
    public LayerMask[] unitLayer;
    public LayerMask[] UILayer;
    public GameObject[] projectile;
    public GameObject[] Props;

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
        GameObject unit = Instantiate(model[0], spawnPos[0].position, spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);//generate a knight
        unit = Instantiate(model[0], spawnPos[0].position + new Vector3(1, 0, 0), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);
        unit = Instantiate(model[0], spawnPos[0].position + new Vector3(-1, 0, 0), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);
        unit = Instantiate(model[1], spawnPos[0].position + new Vector3(0, 0, -1), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);
        unit = Instantiate(model[2], spawnPos[0].position + new Vector3(0, 0, 2), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);
        unit = Instantiate(model[3], spawnPos[0].position + new Vector3(0, 0, -2), spawnPos[0].rotation);
        unit.GetComponent<MinionAI>().setMinionData(0, tower[1], unitLayer[1]);

       /* unit = Instantiate(model[0], spawnPos[1].position, spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);//generate a knight
        unit = Instantiate(model[0], spawnPos[1].position + new Vector3(1, 0, 0), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]]);
        unit = Instantiate(model[0], spawnPos[1].position + new Vector3(-1, 0, 0), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);
        unit = Instantiate(model[1], spawnPos[1].position + new Vector3(0, 0, -1), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);*/
        unit = Instantiate(model[2], spawnPos[1].position + new Vector3(0, 0, 2), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);
        unit = Instantiate(model[3], spawnPos[1].position + new Vector3(0, 0, -2), spawnPos[1].rotation);
        unit.GetComponent<MinionAI>().setMinionData(1, tower[0], unitLayer[0]);

    }
}
