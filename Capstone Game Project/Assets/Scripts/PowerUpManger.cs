using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManger : MonoBehaviour
{
    //public variables
    public Transform[] spawnPoints;
    public GameObject[] PowerUps;
    public int Interval;
    public int HealthIncrease;
    public int DamageIncrease;
    
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
            currentPowerUp = Random.Range(0, PowerUps.Length);

            
            Debug.Log("The power up is: "+currentPowerUp);
            Spawn(currentPowerUp, PowerUps[currentPowerUp],spawnPoints[currentPos]);




        }
    }

    private void Spawn(int type,GameObject powerup,Transform pos)
    {
        Debug.Log("The type here: " + type);

        // health
        if (type == 0)
        {
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, HealthIncrease);
        }

        // damage increase

       else if (type == 1)
        {
            
            GameObject Power = Instantiate(powerup, pos.position, pos.rotation);
            Power.GetComponent<PowerUp>().PowerUpSetUp(type, DamageIncrease);
        }



        // type not recognised
        else
        {
            Debug.Log("Type not recognised");
        }

        
    }

   



}
