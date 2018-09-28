using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // private variables
    private int Type;// type of power-up
    private int IntegerIncrease; // will increase any integer data eg health, damage etc
    private float len; // for animation
    private float speed; // for the animation
    private int spawnpos; // position in the array in powerupmanger;
    private int layer; //  for the invisibilty power-up

    // public variables
    private GameObject powerUpManager;
    public int lifetime; // life time of a power up

    public void PowerUpSetUp(int type,int integerincrease, int spawnpos, GameObject powerUpManager)
    {
        Type = type;
        IntegerIncrease = integerincrease;
        len = 3;
        speed = 5;
        this.spawnpos = spawnpos;
        this.powerUpManager = powerUpManager;
        layer = 13;
        lifetime = 10;

    }

    private void Start()
    {
        StartCoroutine(killPowerUp());
    }

    private void Update()
    {

        animateObject();
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("This is the type: " + Type);
        // Health power-up
        if (Type == 0)
        {
            if (other.gameObject.GetComponentInParent<healthManager>() != null)
            {
                other.gameObject.GetComponentInParent<healthManager>().Damage(-1*IntegerIncrease);

                powerUpManager.GetComponent<PowerUpManger>().ReleaseSpawnPoint(spawnpos);

                Destroy(gameObject);
            }

            

        }

        // Strength power-up
        if (Type == 1)
        {
            if (other.gameObject.GetComponentInParent<playerControl>() != null)
            {
                other.gameObject.GetComponentInParent<playerControl>().IncreaseDamageStrength(IntegerIncrease);

                powerUpManager.GetComponent<PowerUpManger>().ReleaseSpawnPoint(spawnpos);

                Destroy(gameObject);

            }

            
        }

        // invisiblity 

        if (Type == 2)
        {
            if (other.gameObject.GetComponentInParent<playerControl>() != null)
            {
                other.gameObject.GetComponentInParent<playerControl>().MakeInVisible(layer);

                powerUpManager.GetComponent<PowerUpManger>().ReleaseSpawnPoint(spawnpos);

                Destroy(gameObject);

            }

        }

        // speed up

        if (Type == 3)
        {

            if (other.gameObject.GetComponentInParent<playerControl>() != null)
            {
                other.gameObject.GetComponentInParent<playerControl>().IncreaseSpeed(IntegerIncrease);

                powerUpManager.GetComponent<PowerUpManger>().ReleaseSpawnPoint(spawnpos);

                Destroy(gameObject);

            }

        }



        // An error
        else
        {
            Debug.Log("Not a power up: " + Type);
            powerUpManager.GetComponent<PowerUpManger>().ReleaseSpawnPoint(spawnpos);
            Destroy(gameObject);
        }
        
    }

    void animateObject()
    {

        float y = Mathf.PingPong(speed * Time.time, len);
        Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = pos;

        transform.Rotate(Vector3.up, 100 * Time.deltaTime);

    }

    IEnumerator killPowerUp()
    {
        yield return new WaitForSeconds(lifetime);

        powerUpManager.GetComponent<PowerUpManger>().ReleaseSpawnPoint(spawnpos);

        Destroy(gameObject);


    }

    
}
