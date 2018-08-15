﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {


    private float rotation;
    private float radians;
    private Animation anim;
    public Rigidbody rigidbody;
    private bool free;
    private float actionTimer;
    private bool dead;

    public float actionCooldown = 1f;
    public float rotationalSpeed = 1.5f;
    public float walkingSpeed = 10f;
    public LayerMask hitLayer;
    //public GameObject magicEffect;
    //public GameObject dustEffect;

    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode rotLeft = KeyCode.A;
    public KeyCode rotRight = KeyCode.D;
    public KeyCode attack = KeyCode.Q;
    public KeyCode special = KeyCode.E;

    // Use this for initialization
    void Start () {
        rotation = transform.rotation.eulerAngles.y;
        anim = GetComponent<Animation>();
        rigidbody = GetComponent<Rigidbody>();
        free = true;
        actionTimer = actionCooldown;
        dead = false;
    }
	
	void Update () {

        free = true;
        rigidbody.velocity = new Vector3(0, 0, 0);
        actionTimer += Time.deltaTime;
        if (actionTimer > actionCooldown && !dead)//only if not currently 
        {
            if (Input.GetKey(attack))//ranged attack
            {
                actionTimer = 0;
                if (!anim.IsPlaying("attack"))
                {
                    anim.CrossFade("attack");
                }

                //StartCoroutine(SpellEffect(0.2f,1f));//fire effect

                RaycastHit hit;
                if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.forward), out hit, 2.5f, hitLayer))//determine if attack hits something
                {
                    hit.transform.gameObject.GetComponent<healthManager>().Damage(10);
                }
            }

            if (Input.GetKey(special))//ranged attack
            {
                actionTimer = 0;
                if (!anim.IsPlaying("skill"))
                {
                    anim.CrossFade("skill");
                }

                /*StartCoroutine(SpellEffect(0.2f,1f));//fire effect

                RaycastHit hit;
                if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, 1))//determine if attack hits something
                {
                    StartCoroutine(DelayedDestroy(hit.transform.gameObject, 0.3f));
                }
                */
            }

            if (Input.GetKey(forward))//move forward
            {
                radians = rotation / 360 * 2 * Mathf.PI;
                rigidbody.velocity = new Vector3(Mathf.Sin(radians) * walkingSpeed, 0, Mathf.Cos(radians) * walkingSpeed);
                if (!anim.IsPlaying("walk"))
                {
                    anim["walk"].speed = 1.0f;
                    anim.CrossFade("walk");
                }
                free = false;

            }

            if (Input.GetKey(backward))//move backward
            {
                radians = rotation / 360 * 2 * Mathf.PI;
                rigidbody.velocity = new Vector3(Mathf.Sin(radians) * (-walkingSpeed), 0, Mathf.Cos(radians) * (-walkingSpeed));
                if (!anim.IsPlaying("walk"))
                {
                    anim["walk"].speed = -1.0f;
                    anim.CrossFade("walk");
                }
                free = false;
            }

            if (Input.GetKey(rotLeft))//rotate left
            {
                rotation -= rotationalSpeed;
                if (!anim.IsPlaying("walk"))
                {
                    anim.CrossFade("walk");
                }
                free = false;
            }

            if (Input.GetKey(rotRight))//rotate right
            {
                rotation += rotationalSpeed;
                if (!anim.IsPlaying("walk"))
                {
                    anim.CrossFade("walk");
                }
                free = false;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (free)
            {
                if (!anim.IsPlaying("free"))
                {
                    anim.CrossFade("free");
                }
            }
        }

    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public void die()
    {
        dead = true;
        anim.CrossFade("death");
    }

    /*
    private IEnumerator DelayedDestroy(GameObject target, float duration)//destroy hit target after delay
    {     
        yield return new WaitForSeconds(duration);
        GameObject dust = Instantiate(dustEffect);
        dust.transform.position = target.transform.position + new Vector3(0,1,0);
        dust.transform.localScale = target.transform.localScale*100;
        dust.transform.rotation = transform.rotation;
        Destroy(target);
        yield return new WaitForSeconds(1f);
        Destroy(dust);
    }

    private IEnumerator SpellEffect(float startDelay, float duration)//create fire animation with delay to line up with animation
    {
        yield return new WaitForSeconds(startDelay);
        GameObject spell = Instantiate(magicEffect);
        radians = rotation / 360 * 2 * Mathf.PI;
        spell.transform.position = transform.position + new Vector3(Mathf.Sin(radians)*3.5f, -0.1f, Mathf.Cos(radians)*3.5f);
        spell.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(duration);
        Destroy(spell);
    }
    */
}
