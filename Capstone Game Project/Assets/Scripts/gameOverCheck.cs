﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOverCheck : MonoBehaviour {

    public GameObject[] tower;
    public Transform[] towerFocus;
    public Text text;
    public Camera gameOverCam;
    public Camera [] playerCamera;

    private bool over;

	// Use this for initialization
	void Start () {
        gameOverCam.enabled = false;
        over = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!over)
        {
            if(tower[0].GetComponent<healthManager>().getHealth() <=0 && tower[1].GetComponent<healthManager>().getHealth() <= 0)
            {
                over = true;
                text.text = "Game Ends in a Tie!";
                gameOverCam.enabled = true;
                playerCamera[0].gameObject.GetComponent<followLook>().target = towerFocus[0];
                playerCamera[1].gameObject.GetComponent<followLook>().target = towerFocus[1];
            }
            else if(tower[1].GetComponent<healthManager>().getHealth() <= 0)
            {
                over = true;
                text.text = "Player 1 Wins!!!";
                gameOverCam.enabled = true;
                playerCamera[1].gameObject.GetComponent<followLook>().target = towerFocus[1];
            }
            else if (tower[0].GetComponent<healthManager>().getHealth() <= 0)
            {
                over = true;
                text.text = "Player 2 Wins!!!";
                gameOverCam.enabled = true;
                playerCamera[0].gameObject.GetComponent<followLook>().target = towerFocus[0];
            }
        }

		
	}

    public void Restart(string name)
    {
        SceneManager.LoadScene(name);
    }
}
