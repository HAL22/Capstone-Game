using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class healthManager : MonoBehaviour //used my all units to monitor and cntrol health
    //also instructs appripriate scripts in event of unit death
{


    public int maxHealth = 100;
    public RectTransform []healthBar;
    
    private int currentHealth;
    private float barSize;//used to keep health bars in proportion on ui
    private float thirdBarSize;//use for tower healthbar on HUD
    private bool invulnerable;//used when hero become immune

	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
        barSize = healthBar[0].sizeDelta.x;
        invulnerable = false;
        try
        {
            thirdBarSize = healthBar[2].sizeDelta.x;
        }
        catch { }

    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void Damage(int amt)//damage - can be negetive(ie healing)
    {
        if(currentHealth > 0 && (amt<0 || !invulnerable))//only deal damage if currenthealth >0 (ie not dead) and it is either healing or target is not invulnerable
        {
            currentHealth -= amt;//deal damage
            currentHealth = Mathf.Min(currentHealth, maxHealth);//check health has not exceeded max
            if(currentHealth <= 0)//check if dead
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

            //update healthbars
            healthBar[0].sizeDelta = new Vector2((float)(currentHealth) / maxHealth * barSize, healthBar[0].sizeDelta.y);
            healthBar[1].sizeDelta = new Vector2((float)(currentHealth)/ maxHealth * barSize, healthBar[1].sizeDelta.y);
            try
            {
                healthBar[2].sizeDelta = new Vector2((float)(currentHealth) / maxHealth * thirdBarSize, healthBar[2].sizeDelta.y);
            }
            catch { }
            
        }
        
    }

    public void makeInvulnerable(bool b)//sets invulnerability status
    {
        invulnerable = b;
    }

    public void resetHealth()//reset health on respawn
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
