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
    private GoldManager goldManager;

    public float actionCooldown = 1f;
    public float rotationalSpeed = 1.5f;
    public float walkingSpeed = 10f;
    public float respawnCooldown = 5f;
    public LayerMask allyLayer;
    public LayerMask enemyLayer;
    public enum Skill { smash, heal };
    public Skill skill;

    //unit spawners
    public SpawnManager spawnManager;
    public RectTransform[] cooldownIndicator;
    public float maxSpawnTimer1 = 3;
    public float maxSpawnTimer2 = 5;
    public float maxSpawnTimer3 = 8;
    private float spawnTimer1;
    private float spawnTimer2;
    private float spawnTimer3;
    private float barSize;

    //Power-ups
    public int powerupTime;
    public int DamageStrength;
    public int OriginalDamageStrength;
    public int OriginalLayer; // for invisible 
    public int invisiblityTime;
    public float OriginalWalkingspeed;


    //sound effects
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip respawnSound;
    public AudioClip skillSound;

    //public GameObject magicEffect;
    public GameObject attackEffect;
    public GameObject skillEffect;
    public GameObject strengthAura;
    public GameObject invisibleAura;
    public GameObject speedAura;

    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode rotLeft = KeyCode.A;
    public KeyCode rotRight = KeyCode.D;
    public KeyCode attack = KeyCode.Q;
    public KeyCode special = KeyCode.E;
    public KeyCode spawnOne = KeyCode.Alpha1;
    public KeyCode spawnTwo = KeyCode.Alpha2;
    public KeyCode spawnThree = KeyCode.Alpha3;

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
        goldManager = GetComponent<GoldManager>();

        spawnTimer1 = maxSpawnTimer1;
        spawnTimer2 = maxSpawnTimer2;
        spawnTimer3 = maxSpawnTimer3;
        barSize = cooldownIndicator[0].sizeDelta.y;
        OriginalDamageStrength = DamageStrength;
        invisiblityTime = 0;
        OriginalWalkingspeed = walkingSpeed;
       
    }
	
	void Update () {

        free = true;
        rigidbody.velocity = new Vector3(0, 0, 0);
        actionTimer += Time.deltaTime;
        spawnTimer1 = Mathf.Min(maxSpawnTimer1, spawnTimer1 + Time.deltaTime);
        spawnTimer2 = Mathf.Min(maxSpawnTimer2, spawnTimer2 + Time.deltaTime);
        spawnTimer3 = Mathf.Min(maxSpawnTimer3, spawnTimer3 + Time.deltaTime);
        cooldownUpdate();

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

                //StartCoroutine(DustEffect(0.2f,1f));//fire effect
                GameObject hitEffect = Instantiate(attackEffect, transform.position, transform.rotation);
                Destroy(hitEffect, 0.7f);
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

                //StartCoroutine(SpellEffect(0.2f,1f));//fire effect
                

                radians = rotation / 360 * 2 * Mathf.PI;

                if (skill == Skill.smash)
                {
                    audio.clip = skillSound;
                    audio.Play();

                    GameObject spellEffect = Instantiate(skillEffect, transform.position + new Vector3(Mathf.Sin(radians) * 3.5f, 0, Mathf.Cos(radians) * 3.5f), transform.rotation);
                    Destroy(spellEffect, 1f);

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
                    audio.clip = skillSound;
                    audio.Play();

                    GameObject spellEffect = Instantiate(skillEffect, transform.position + new Vector3(Mathf.Sin(radians) * 3.5f, 3, Mathf.Cos(radians) * 3.5f), transform.rotation);
                    Destroy(spellEffect, 1.2f);

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

        if (Input.GetKey(spawnOne) && spawnTimer1==maxSpawnTimer1)
        {
            if (goldManager.spendGold(20))
            {
                spawnManager.spawnUnit(transform, 2, gameObject.layer - 8);
                spawnTimer1 = 0;
            }

        }

        if (Input.GetKey(spawnTwo) && spawnTimer2 == maxSpawnTimer2)
        {
            if (goldManager.spendGold(40))
            {
                spawnManager.spawnUnit(transform, 3, gameObject.layer - 8);
                spawnTimer2 = 0;
            }

        }

        if (Input.GetKey(spawnThree) && spawnTimer3 == maxSpawnTimer3)
        {
            if (goldManager.spendGold(60))
            {
                spawnManager.spawnUnit(transform, 4, gameObject.layer - 8);
                spawnTimer3 = 0;
            }

        }

        if (dead)
        {
            deathTimer += Time.deltaTime;
            if(deathTimer >= respawnCooldown)
            {
                dead = false;
                transform.position = spawnpoint;
                audio.clip = respawnSound;
                audio.Play();
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

    // Power-ups

    // Strength update

    public void IncreaseDamageStrength(int amount)
    {
        DamageStrength += amount;

        // start the timer;

        StartCoroutine(EndPowerUp(1, amount));

    }

    public void MakeInVisible(int Newlayer)
    {
        gameObject.layer = Newlayer;
        invisiblityTime += powerupTime;

        StartCoroutine(EndPowerUp(2,0));


    }

    public void IncreaseSpeed(float speed)
    {
        walkingSpeed += speed;

        StartCoroutine(EndPowerUp(3,(int)speed));
    }

    private IEnumerator EndPowerUp(int type,int amount)
    {
        if (type == 1)
        {
            Debug.Log("start Aura");
            GameObject aura = Instantiate(strengthAura, transform.position+new Vector3(0,1,0), transform.rotation, transform);
            yield return new WaitForSeconds(powerupTime);
            Destroy(aura);
            DamageStrength = Mathf.Max(OriginalDamageStrength, DamageStrength - amount);


        }

        if (type == 2)
        {
            yield return new WaitForSeconds(powerupTime);

            invisiblityTime -= powerupTime;

            if (invisiblityTime <= 0)
            {
                gameObject.layer = OriginalLayer;
                invisiblityTime = 0;
            }

        }

        if (type == 3)
        {
            yield return new WaitForSeconds(powerupTime);

            walkingSpeed = Mathf.Max(OriginalWalkingspeed, walkingSpeed - amount);

        }
    }

    public void cooldownUpdate()
    {
        cooldownIndicator[0].sizeDelta = new Vector2(cooldownIndicator[0].sizeDelta.x, (float)(maxSpawnTimer1 - spawnTimer1) / maxSpawnTimer1 * barSize);
        cooldownIndicator[1].sizeDelta = new Vector2(cooldownIndicator[1].sizeDelta.x, (float)(maxSpawnTimer2 - spawnTimer2) / maxSpawnTimer2 * barSize);
        cooldownIndicator[2].sizeDelta = new Vector2(cooldownIndicator[2].sizeDelta.x, (float)(maxSpawnTimer3 - spawnTimer3) / maxSpawnTimer3 * barSize);
    }

    public void Die()
    {
        audio.clip = deathSound;
        audio.Play();
        dead = true;
        anim.CrossFade("death");
    }

    
    /*private IEnumerator DelayedDestroy(GameObject target, float duration)//destroy hit target after delay
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
           GameObject spell = Instantiate(attackEffect);
           radians = rotation / 360 * 2 * Mathf.PI;
           spell.transform.position = transform.position + new Vector3(Mathf.Sin(radians)*3.5f, -0.1f, Mathf.Cos(radians)*3.5f);
           spell.transform.rotation = transform.rotation;
           yield return new WaitForSeconds(duration);
           Destroy(spell);
       }*/
       
}
