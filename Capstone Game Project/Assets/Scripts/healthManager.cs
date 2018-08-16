using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthManager : MonoBehaviour {

    public int maxHealth = 100;
    public RectTransform healthBar;
    
    private int currentHealth;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage(int amt)
    {
        if(currentHealth > 0)
        {
            currentHealth -= amt;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                GetComponent<playerControl>().die();
            }

            healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        }
        
    }
}
