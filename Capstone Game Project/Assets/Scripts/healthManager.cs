using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthManager : MonoBehaviour
{


    public int maxHealth = 100;
    public RectTransform []healthBar;
    
    private int currentHealth;

    private float barSize;

	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
        barSize = healthBar[0].sizeDelta.x;

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
            healthBar[0].sizeDelta = new Vector2((float)(currentHealth) / maxHealth * barSize, healthBar[0].sizeDelta.y);
            healthBar[1].sizeDelta = new Vector2((float)(currentHealth)/ maxHealth * barSize, healthBar[1].sizeDelta.y);
            try
            {
                healthBar[2].sizeDelta = new Vector2((float)(currentHealth) / maxHealth * barSize, healthBar[2].sizeDelta.y);
            }
            catch { }
            
        }
        
    }

    

    public void resetHealth()
    {
        currentHealth = maxHealth;
        healthBar[0].sizeDelta = new Vector2((float)(currentHealth) / maxHealth * barSize, healthBar[0].sizeDelta.y);
        healthBar[1].sizeDelta = new Vector2((float)(currentHealth) / maxHealth * barSize, healthBar[1].sizeDelta.y);
    }

    public int getHealth()
    {
        return currentHealth;
    }
}
