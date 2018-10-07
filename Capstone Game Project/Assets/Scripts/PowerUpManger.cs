using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManger : MonoBehaviour
{
    //public variables
    public Transform[] spawnPoints;
    public int[] OccupiedSpawnPoints;
    public GameObject[] PowerUps;
    public int Interval;
    public int HealthIncrease;
    public int DamageIncrease;
    public int SpeedIncrease;
    public int positionPerPowerUp;
    
    //private variable
    private int currentPos;
    private int currentPowerUp;
    

    
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(SpawnPowerUp());
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private IEnumerator SpawnPowerUp()
    {
        

        while (true)
        {
            yield return new WaitForSeconds(Interval);

            // geting the pos
            currentPos = Random.Range(0, spawnPoints.Length);

            // getting the power-up
            //currentPowerUp = Random.Range(0, PowerUps.Length);
            currentPowerUp = 1;
            
            if (OccupiedSpawnPoints[currentPos] < positionPerPowerUp)
            {
                Spawn(currentPowerUp, PowerUps[currentPowerUp], spawnPoints[currentPos]);
                OccupiedSpawnPoints[currentPos]++;
            }
               




        }
    }

    private void Spawn(int type,GameObject powerup,Transform pos)
    {

        // health
        if (type == 0)
        {
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, HealthIncrease, currentPos, gameObject);
        }

        // damage increase

        else if (type == 1)
        {

            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, DamageIncrease, currentPos, gameObject);
        }

        // Invisibility

        else if (type == 2)
        {
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, 0, currentPos, gameObject);

        }

        // Speed up

        else if (type==3)
        {
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, SpeedIncrease, currentPos, gameObject);

        }



        // type not recognised
        else
        {
            Debug.Log("Type not recognised");
        }

        
    }

    public void ReleaseSpawnPoint(int spawnpoint)
    {
        if (OccupiedSpawnPoints[spawnpoint] > 0)
        {
            OccupiedSpawnPoints[spawnpoint]--;
        }

    }

   



}
