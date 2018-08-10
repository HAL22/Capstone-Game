using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {

    public Camera[] camera;
    public GameObject[] specifics;

	// Use this for initialization
	void Start () {

        camera[0].gameObject.SetActive(true);
        camera[1].gameObject.SetActive(false);
        camera[2].gameObject.SetActive(false);
        camera[3].gameObject.SetActive(false);
        specifics[0].SetActive(true);
        specifics[1].SetActive(true);
        specifics[2].SetActive(false);
        specifics[3].SetActive(false);
        specifics[4].SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Alpha1))//follow camera
        {
            camera[0].gameObject.SetActive(true);
            camera[1].gameObject.SetActive(false);
            camera[2].gameObject.SetActive(false);
            camera[3].gameObject.SetActive(false);
            specifics[0].SetActive(true);
            specifics[1].SetActive(true);
            specifics[2].SetActive(false);
            specifics[3].SetActive(false);
            specifics[4].SetActive(false);

        }

        if (Input.GetKey(KeyCode.Alpha2))//orbit camera
        {
            camera[1].gameObject.SetActive(true);
            camera[0].gameObject.SetActive(false);
            camera[2].gameObject.SetActive(false);
            camera[3].gameObject.SetActive(false);
            specifics[0].SetActive(false);
            specifics[1].SetActive(false);
            specifics[2].SetActive(true);
            specifics[3].SetActive(true);
            specifics[4].SetActive(false);

        }

        if (Input.GetKey(KeyCode.Alpha3))//1st person camera
        {
            camera[2].gameObject.SetActive(true);
            camera[1].gameObject.SetActive(false);
            camera[0].gameObject.SetActive(false);
            camera[3].gameObject.SetActive(false);
            specifics[0].SetActive(false);
            specifics[1].SetActive(false);
            specifics[2].SetActive(false);
            specifics[3].SetActive(false);
            specifics[4].SetActive(true);

        }

        if (Input.GetKey(KeyCode.Alpha4))//fixed camera
        {
            camera[3].gameObject.SetActive(true);
            camera[1].gameObject.SetActive(false);
            camera[2].gameObject.SetActive(false);
            camera[0].gameObject.SetActive(false);
            specifics[0].SetActive(false);
            specifics[1].SetActive(false);
            specifics[2].SetActive(false);
            specifics[3].SetActive(false);
            specifics[4].SetActive(false);

        }

    }
}
