using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {


    private float rotation;
    private float radians;
    private Animation anim;
    private Rigidbody rigidbody;
    private bool free;
    private float actionTimer;
    private float deathTimer;
    private bool dead;
    private Vector3 spawnpoint;
    private AudioSource audio;

    // Gold that the player has
    public int GoldAmount;
    public float actionCooldown = 1f;
    public float rotationalSpeed = 1.5f;
    public float walkingSpeed = 10f;
    public float respawnCooldown = 5f;
    public LayerMask allyLayer;
    public LayerMask enemyLayer;
    public enum Skill { smash, heal };
    public Skill skill;

    //Power-ups
    public int powerupTime;
    public int DamageStrength;
    public int OriginalDamageStrength;

    

    public AudioClip attackSound;
    public AudioClip deathSound;
    //public GameObject magicEffect;
    public GameObject dustEffect;
    public GameObject skillEffect;

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
        spawnpoint = transform.position;
        deathTimer = 0;
        audio = GetComponent<AudioSource>();
        GoldAmount = 0;
        OriginalDamageStrength = DamageStrength;
        
    }
	
	void Update () {

       

        free = true;
        rigidbody.velocity = new Vector3(0, 0, 0);
        actionTimer += Time.deltaTime;
        if (actionTimer > actionCooldown && !dead)//only if not currently dead or attacking
        {
            if (Input.GetKey(attack))//ranged attack
            {
                actionTimer = 0;
                audio.clip = attackSound;
                audio.Play();
                if (!anim.IsPlaying("attack"))
                {
                    anim.CrossFade("attack");
                }

                StartCoroutine(DustEffect(0.2f,1f));//fire effect
                radians = rotation / 360 * 2 * Mathf.PI;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(Mathf.Sin(radians) * 6f, 0, Mathf.Cos(radians) * 6f), 2f, enemyLayer);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    hitColliders[i].gameObject.GetComponent<healthManager>().Damage(DamageStrength);
                    i++;
                }
            }

            if (Input.GetKey(special))//special ability
            {
                actionTimer = 0;
                audio.clip = attackSound;
                audio.Play();
                if (!anim.IsPlaying("skill"))
                {
                    anim.CrossFade("skill");
                }

                StartCoroutine(SpellEffect(0.2f,1f));//fire effect

                radians = rotation / 360 * 2 * Mathf.PI;

                if (skill == Skill.smash)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(Mathf.Sin(radians) * 2.5f, 1, Mathf.Cos(radians) * 2.5f), 2f, enemyLayer);
                    int i = 0;
                    while (i < hitColliders.Length)
                    {
                        hitColliders[i].gameObject.GetComponent<healthManager>().Damage(DamageStrength);
                        i++;
                    }
                }
                else if(skill == Skill.heal)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(Mathf.Sin(radians) * 2.5f, 1, Mathf.Cos(radians) * 2.5f), 2f, allyLayer);
                    int i = 0;
                    while (i < hitColliders.Length)
                    {
                        hitColliders[i].gameObject.GetComponent<healthManager>().Damage(-10);
                        i++;
                    }
                }

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
                rigidbody.velocity = new Vector3(0, 0, 0);
                rigidbody.angularVelocity = new Vector3(0, 0, 0);
                if (!anim.IsPlaying("free"))
                {
                    anim.CrossFade("free");
                }
            }
        }

        if (dead)
        {
            deathTimer += Time.deltaTime;
            if(deathTimer >= respawnCooldown)
            {
                dead = false;
                transform.position = spawnpoint;
                GetComponent<healthManager>().resetHealth();
                anim.CrossFade("free");
                deathTimer = 0;
            }
        }

    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public void addGold(int amount)
    {
        GoldAmount += amount;

    }

    public void useGold(int amount)
    {
        if (GoldAmount >= amount)
        {
            GoldAmount -= amount;
        }
    }

    // Power-ups

    // Strength update

    public void IncreaseDamageStrength(int amount)
    {
        DamageStrength += amount;

        // start the timer;

        StartCoroutine(EndPowerUp(1, amount));

    }

    private IEnumerator EndPowerUp(int type,int amount)
    {
        if (type == 1)
        {
            yield return new WaitForSeconds(powerupTime);

            DamageStrength = Mathf.Max(OriginalDamageStrength, DamageStrength - amount);


        }
    }

    








    public void Die()
    {
        audio.clip = deathSound;
        audio.Play();
        dead = true;
        anim.CrossFade("death");
    }

    
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
         GameObject spell = Instantiate(skillEffect);
         radians = rotation / 360 * 2 * Mathf.PI;
         spell.transform.position = transform.position + new Vector3(Mathf.Sin(radians)*3.5f, -0.1f, Mathf.Cos(radians)*3.5f);
         spell.transform.rotation = transform.rotation;
         yield return new WaitForSeconds(duration);
         Destroy(spell);
     }
     


     private IEnumerator DustEffect(float startDelay, float duration)//create fire animation with delay to line up with animation
       {
           yield return new WaitForSeconds(startDelay);
           GameObject spell = Instantiate(dustEffect);
           radians = rotation / 360 * 2 * Mathf.PI;
           spell.transform.position = transform.position + new Vector3(Mathf.Sin(radians)*3.5f, -0.1f, Mathf.Cos(radians)*3.5f);
           spell.transform.rotation = transform.rotation;
           yield return new WaitForSeconds(duration);
           Destroy(spell);
       }
       
}
