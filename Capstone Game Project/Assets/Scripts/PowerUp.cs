using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class PowerUp : MonoBehaviour//script for power up objects
{
    // private variables
    private int Type;// type of power-up
    private int IntegerIncrease; // will increase any integer data eg health, damage etc
    private float len; // for rotation animation
    private float speed; // speed of rotation animation
    private int spawnpos; // position in the array in powerupmanger;

    // public variables
    private GameObject powerUpManager;
    public float lifetime; // life time of a power up
    public GameObject collectEffect;//particle effect of collection

    public void PowerUpSetUp(int type,int integerincrease, int spawnpos, GameObject powerUpManager, float lifetime)//called when object instatiated
    {
        Type = type;
        IntegerIncrease = integerincrease;
        len = 3;
        speed = 5;
        this.spawnpos = spawnpos;
        this.powerUpManager = powerUpManager;
        this.lifetime = lifetime;
    }

    private void Start()
    {
        StartCoroutine(killPowerUp());//will kill powerup after delay
    }

    private void Update()
    {
        animateObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        //create collection effect, last 1 second
        GameObject effect = Instantiate(collectEffect, transform.position+new Vector3(0,1,0), transform.rotation);
        Destroy(effect, 1f);

        // Health power-up
        if (Type == 0)
        {
            if (other.gameObject.GetComponentInParent<playerControl>() != null)//can nly be picked up by player
            {
                other.gameObject.GetComponentInParent<healthManager>().Damage(-1*IntegerIncrease);//heal target
                powerUpManager.GetComponent<PowerUpManager>().ReleaseSpawnPoint(spawnpos);//release the spawn point for new powerup
                Destroy(gameObject);//destroy the powerup
            }
        }

        // Strength power-up
        else if (Type == 1)
        {
            if (other.gameObject.GetComponentInParent<playerControl>() != null)
            {
                other.gameObject.GetComponentInParent<playerControl>().IncreaseDamageStrength(IntegerIncrease);
                powerUpManager.GetComponent<PowerUpManager>().ReleaseSpawnPoint(spawnpos);
                Destroy(gameObject);
            }           
        }

        // invisiblity 

        else if (Type == 2)
        {
            if (other.gameObject.GetComponentInParent<playerControl>() != null)
            {
                other.gameObject.GetComponentInParent<playerControl>().MakeInvuln();
                powerUpManager.GetComponent<PowerUpManager>().ReleaseSpawnPoint(spawnpos);
                Destroy(gameObject);
            }
        }

        // speed up
        else if (Type == 3)
        {
            if (other.gameObject.GetComponentInParent<playerControl>() != null)
            {
                other.gameObject.GetComponentInParent<playerControl>().IncreaseSpeed(IntegerIncrease);
                powerUpManager.GetComponent<PowerUpManager>().ReleaseSpawnPoint(spawnpos);
                Destroy(gameObject);
            }
        }


        // An error
        else
        {
            Debug.Log("Not a power up: " + Type);
            powerUpManager.GetComponent<PowerUpManager>().ReleaseSpawnPoint(spawnpos);
            Destroy(gameObject);
        }
        
    }

    void animateObject()//animates the rotation
    {

        float y = Mathf.PingPong(speed * Time.time, len);
        Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = pos;

        transform.Rotate(Vector3.up, 100 * Time.deltaTime);

    }

    IEnumerator killPowerUp()//kills after lifetime of powerup
    {
        yield return new WaitForSeconds(lifetime);

        powerUpManager.GetComponent<PowerUpManager>().ReleaseSpawnPoint(spawnpos);

        Destroy(gameObject);


    }

    
}
