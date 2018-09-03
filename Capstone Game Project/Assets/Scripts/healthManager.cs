using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthManager : MonoBehaviour
{


    public int maxHealth = 100;
    public RectTransform healthBar;
    
    public int currentHealth;

    public void MinionDeath()
    {
        if (currentHealth <= 0 && gameObject.GetComponent<MinionAI>() != null)
        {
            gameObject.GetComponent<MinionAI>().Die();

        }
    }

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void Damage(int amt)
    {
        if(currentHealth > 0)
        {
            currentHealth -= amt;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            if(currentHealth <= 0)
            {
                if (GetComponent<playerControl>() != null)
                {
                    GetComponent<playerControl>().Die();
                }
                if (GetComponent<MinionAI>() != null)
                {
                    GetComponent<MinionAI>().Die();

                }
            }
            healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
            Debug.Log("Damage " + amt + " Current " + currentHealth);
        }
        
    }
}
