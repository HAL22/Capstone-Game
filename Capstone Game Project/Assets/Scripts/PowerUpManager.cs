using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class PowerUpManager : MonoBehaviour//manages the spawning of powerups and initial powerup states
{
    //public variables
    public Transform[] spawnPoints;
    private bool[] OccupiedSpawnPoints;//used to check if spawn points are open or not
    public GameObject[] PowerUps;
    public int Interval;
    public int HealthIncrease;
    public int DamageIncrease;
    public int SpeedIncrease;
    public float lifetime;
    
    //private variable
    private int currentPos;
    private int currentPowerUp;
    

    
	// Use this for initialization
	void Start ()
    {
        OccupiedSpawnPoints = new bool[spawnPoints.Length];
        StartCoroutine(SpawnPowerUp());
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private IEnumerator SpawnPowerUp()//after regular intervals, spawn a powerup
    {
        

        while (true)
        {
            // geting the pos
            currentPos = Random.Range(0, spawnPoints.Length);

            // getting the power-up
            currentPowerUp = Random.Range(0, PowerUps.Length);
            
            if (!OccupiedSpawnPoints[currentPos])//if an empty powerup spawnpoint
            {
                yield return new WaitForSeconds(Interval);//wait so many seonds so power up doesn't spawn as soon as previous one was collected
                Spawn(currentPowerUp, PowerUps[currentPowerUp], spawnPoints[currentPos]);
                OccupiedSpawnPoints[currentPos] = true;
            }
               




        }
    }

    private void Spawn(int type,GameObject powerup,Transform pos)//spawn powerup of specified type and intitial data
    {
        // health
        if (type == 0)
        {
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, HealthIncrease, currentPos, gameObject, lifetime);
        }

        // damage increase

        else if (type == 1)
        {
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, DamageIncrease, currentPos, gameObject, lifetime);
        }

        // Invisibility

        else if (type == 2)
        {
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, 0, currentPos, gameObject, lifetime);

        }

        // Speed up

        else if (type==3)
        {
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, SpeedIncrease, currentPos, gameObject, lifetime);

        }



        // type not recognised
        else
        {
            Debug.Log("Type not recognised");
        }

        
    }

    public void ReleaseSpawnPoint(int spawnpoint)//used when a power up is destroyed or collected to free the spawnpoint up
    {
        if (OccupiedSpawnPoints[spawnpoint])
        {
            OccupiedSpawnPoints[spawnpoint] = false;
        }

    }

   



}
