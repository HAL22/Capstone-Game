using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // private variables
    private int Type;// type of power-up
    private int IntegerIncrease; // will increase any integer data eg health, damage etc
    private float len;
    private float speed;

    public void PowerUpSetUp(int type,int integerincrease)
    {
        Type = type;
        IntegerIncrease = integerincrease;
        len = 3;
        speed = 5;

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

                Destroy(gameObject);
            }

            

        }

        if (Type == 1)
        {
            if (other.gameObject.GetComponentInParent<playerControl>() != null)
            {
                other.gameObject.GetComponentInParent<playerControl>().IncreaseDamageStrength(IntegerIncrease);

                Destroy(gameObject);

            }

            
        }



        // An error
        else
        {
            Debug.Log("Not a power up: "+Type);
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

    
}
