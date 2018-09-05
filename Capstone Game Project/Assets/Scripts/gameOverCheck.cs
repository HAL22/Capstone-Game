using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOverCheck : MonoBehaviour {

    public GameObject[] tower;
    public Text text;
    public Camera cam;

    private bool over;

	// Use this for initialization
	void Start () {
        cam.enabled = false;
        over = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!over)
        {
            if(tower[0].GetComponent<healthManager>().currentHealth <=0 && tower[1].GetComponent<healthManager>().currentHealth <= 0)
            {
                over = true;
                text.text = "Game Ends in a Tie!";
                cam.enabled = true; 
            }
            else if(tower[0].GetComponent<healthManager>().currentHealth <= 0)
            {
                over = true;
                text.text = "Player 1 Wins!!!";
                cam.enabled = true;
            }
            else if (tower[1].GetComponent<healthManager>().currentHealth <= 0)
            {
                over = true;
                text.text = "Player 2 Wins!!!";
                cam.enabled = true;
            }
        }

		
	}

    public void Restart(string name)
    {
        SceneManager.LoadScene(name);
    }
}
