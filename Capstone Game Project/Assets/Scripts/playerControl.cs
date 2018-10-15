using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BATTLE LANE
 * for CSC3020H Capestone Game
 * Steven Mare - MRXSTE008
 * Thethela Faltien - FLTTHE004
 */

public class playerControl : MonoBehaviour {

    //model details
    private float rotation;
    private float radians;
    private Animation anim;
    private Rigidbody rigidbody;
    private bool free;
    private float actionTimer;
    private float skillTimer;
    private float deathTimer;
    private bool dead;
    private Vector3 spawnpoint;
    private AudioSource audio;
    private GoldManager goldManager;
    private healthManager health;

    //hero player details
    public float actionCooldown = 1f;
    public float skillCooldown = 5f;
    public float rotationalSpeed = 1.5f;
    public float walkingSpeed = 10f;
    public float respawnCooldown = 5f;
    public LayerMask allyLayer;
    public LayerMask enemyLayer;
    public enum Skill { smash, heal };
    public Skill skill;

    //minion spawner details and HUD indicators
    public SpawnManager spawnManager;
    public RectTransform[] cooldownIndicator;
    public float maxSpawnTimer1 = 3;
    public float maxSpawnTimer2 = 5;
    public float maxSpawnTimer3 = 8;
    private float spawnTimer1;
    private float spawnTimer2;
    private float spawnTimer3;
    private float minionBarSize;
    private float attackBarSize;
    private float skillBarSize;

    //Used for Power-ups
    public int powerupTime;
    public int DamageStrength;
    public int OriginalDamageStrength;
    public float OriginalWalkingspeed;


    //Sound Effects
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip respawnSound;
    public AudioClip skillSound;
    public AudioClip minionSpawn;

    //game objects like gold and effects
    public GameObject gold;
    public GameObject attackEffect;
    public GameObject skillEffect;
    public GameObject strengthAura;
    public GameObject invulnAura;
    public GameObject speedAura;


    //keys for player input
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
        skillTimer = skillCooldown;
        dead = false;
        spawnpoint = transform.position;
        deathTimer = 0;
        audio = GetComponent<AudioSource>();
        goldManager = GetComponent<GoldManager>();
        health = GetComponent<healthManager>();

        spawnTimer1 = maxSpawnTimer1;
        spawnTimer2 = maxSpawnTimer2;
        spawnTimer3 = maxSpawnTimer3;
        minionBarSize = cooldownIndicator[0].sizeDelta.y;
        attackBarSize = cooldownIndicator[3].sizeDelta.y;
        skillBarSize = cooldownIndicator[4].sizeDelta.y;
        OriginalDamageStrength = DamageStrength;
        OriginalWalkingspeed = walkingSpeed;
       
    }
	
	void Update () {

        free = true;//set state to free before checking for inputs

        //resolve rigidbody inconsistencies
        rigidbody.velocity = new Vector3(0, 0, 0);
        rigidbody.angularVelocity = new Vector3(0, 0, 0);

        //incrmeent cooldown timers
        actionTimer += Time.deltaTime;
        skillTimer += Time.deltaTime;
        spawnTimer1 = Mathf.Min(maxSpawnTimer1, spawnTimer1 + Time.deltaTime);
        spawnTimer2 = Mathf.Min(maxSpawnTimer2, spawnTimer2 + Time.deltaTime);
        spawnTimer3 = Mathf.Min(maxSpawnTimer3, spawnTimer3 + Time.deltaTime);
        cooldownUpdate();//update cooldown ui

        if (actionTimer > actionCooldown && !dead)//only if not currently dead or attacking
        {
            if (Input.GetKey(attack))//basic attack
            {
                actionTimer = 0;
                audio.clip = attackSound;
                audio.Play();
                if (!anim.IsPlaying("attack"))
                {
                    anim.CrossFade("attack");
                }

                //create hit effect
                GameObject hitEffect = Instantiate(attackEffect, transform.position, transform.rotation);
                Destroy(hitEffect, 0.7f);

                //hits in small radius in front of hero
                radians = rotation / 360 * 2 * Mathf.PI;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(Mathf.Sin(radians) * 6f, 0, Mathf.Cos(radians) * 6f), 2f, enemyLayer);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    hitColliders[i].gameObject.GetComponent<healthManager>().Damage(DamageStrength);
                    i++;
                }
            }

            if (Input.GetKey(special) && skillTimer>skillCooldown)//special ability
            {
                actionTimer = 0;
                skillTimer = 0;
                audio.clip = attackSound;
                audio.Play();
                if (!anim.IsPlaying("skill"))
                {
                    anim.CrossFade("skill");
                }
                

                radians = rotation / 360 * 2 * Mathf.PI;


                if (skill == Skill.smash)//if skill is smash attack
                {
                    //sound
                    audio.clip = skillSound;
                    audio.Play();
                    //particle effect
                    GameObject spellEffect = Instantiate(skillEffect, transform.position + new Vector3(Mathf.Sin(radians) * 3.5f, 0, Mathf.Cos(radians) * 3.5f), transform.rotation);
                    Destroy(spellEffect, 1f);

                    //hits in larger radius in front of hero
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(Mathf.Sin(radians) * 3.5f, 1, Mathf.Cos(radians) * 3.5f), 3f, enemyLayer);
                    int i = 0;
                    while (i < hitColliders.Length)
                    {
                        hitColliders[i].gameObject.GetComponent<healthManager>().Damage(DamageStrength*2);
                        i++;
                    }
                }
                else if(skill == Skill.heal)//if skill is heal effect
                {
                    //sound
                    audio.clip = skillSound;
                    audio.Play();
                    //particle effect
                    GameObject spellEffect = Instantiate(skillEffect, transform.position + new Vector3(Mathf.Sin(radians) * 3.5f, 3, Mathf.Cos(radians) * 3.5f), transform.rotation);
                    Destroy(spellEffect, 1.2f);

                    //heal in radius in front of player
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(Mathf.Sin(radians) * 3.5f, 1, Mathf.Cos(radians) * 3.5f), 3.5f, allyLayer);
                    int i = 0;
                    while (i < hitColliders.Length)
                    {
                        hitColliders[i].gameObject.GetComponent<healthManager>().Damage(-15);
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

            if (Input.GetKey(KeyCode.Escape))//kills game
            {
                Application.Quit();
            }

            if (free)//if no other control was selected, do free animation
            {
                if (!anim.IsPlaying("free"))
                {
                    anim.CrossFade("free");
                }
            }
        }

        if (Input.GetKey(spawnOne) && spawnTimer1==maxSpawnTimer1)//spawn grunt at heor location
        {
            if (goldManager.spendGold(20))//if has enough gold to spend
            {
                spawnManager.spawnUnit(transform, 2, gameObject.layer - 8);
                spawnTimer1 = 0;
            }

        }

        if (Input.GetKey(spawnTwo) && spawnTimer2 == maxSpawnTimer2)//spawn golem
        {
            if (goldManager.spendGold(40))
            {
                spawnManager.spawnUnit(transform, 3, gameObject.layer - 8);
                spawnTimer2 = 0;
            }

        }

        if (Input.GetKey(spawnThree) && spawnTimer3 == maxSpawnTimer3)//spawn dragon
        {
            if (goldManager.spendGold(60))
            {
                spawnManager.spawnUnit(transform, 4, gameObject.layer - 8);
                spawnTimer3 = 0;
            }

        }

        if (dead)//if player is dead
        {
            deathTimer += Time.deltaTime;
            if(deathTimer >= respawnCooldown)//respawn them at spawn point after delay
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

    void FixedUpdate()//update hero rotation
    {
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    // Power-ups

    // Strength powerup
    public void IncreaseDamageStrength(int amount)
    {
        DamageStrength += amount;

        // start the timer;

        StartCoroutine(EndPowerUp(1, amount));

    }

    //invulnerability powerup
    public void MakeInvuln()
    {
        health.makeInvulnerable(true);
        StartCoroutine(EndPowerUp(2,0));


    }

    //speed powerup
    public void IncreaseSpeed(float speed)
    {
        walkingSpeed += speed;

        StartCoroutine(EndPowerUp(3,(int)speed));
    }

    private IEnumerator EndPowerUp(int type,int amount)//after delay, reset effect of power up
    {
        if (type == 1)//if strength
        {
            GameObject aura = Instantiate(strengthAura, transform.position+new Vector3(0,2.74f,0), transform.rotation, transform);
            yield return new WaitForSeconds(powerupTime);
            Destroy(aura);
            DamageStrength = Mathf.Max(OriginalDamageStrength, DamageStrength - amount);

        }

        if (type == 2)//if invulnerability
        {
            GameObject aura = Instantiate(invulnAura, transform.position + new Vector3(0, 2.74f, 0), transform.rotation, transform);
            yield return new WaitForSeconds(powerupTime);
            Destroy(aura);
            health.makeInvulnerable(false);

        }

        if (type == 3)//if speed
        {
            GameObject aura = Instantiate(speedAura, transform.position + new Vector3(0, 2.74f, 0), transform.rotation, transform);
            yield return new WaitForSeconds(powerupTime);
            Destroy(aura);
            walkingSpeed = Mathf.Max(OriginalWalkingspeed, walkingSpeed - amount);

        }
    }

    public void cooldownUpdate()//update cooldown indicators on HUD
    {
        cooldownIndicator[0].sizeDelta = new Vector2(cooldownIndicator[0].sizeDelta.x, (float)(maxSpawnTimer1 - spawnTimer1) / maxSpawnTimer1 * minionBarSize);
        cooldownIndicator[1].sizeDelta = new Vector2(cooldownIndicator[1].sizeDelta.x, (float)(maxSpawnTimer2 - spawnTimer2) / maxSpawnTimer2 * minionBarSize);
        cooldownIndicator[2].sizeDelta = new Vector2(cooldownIndicator[2].sizeDelta.x, (float)(maxSpawnTimer3 - spawnTimer3) / maxSpawnTimer3 * minionBarSize);
        cooldownIndicator[3].sizeDelta = new Vector2(cooldownIndicator[3].sizeDelta.x, (float)(actionCooldown - actionTimer) / actionCooldown * attackBarSize);
        cooldownIndicator[4].sizeDelta = new Vector2(cooldownIndicator[4].sizeDelta.x, (float)(skillCooldown - skillTimer) / skillCooldown * skillBarSize);

    }

    public void Die()//drop gold and play sounds when dead
    {
        audio.clip = deathSound;
        audio.Play();
        dead = true;
        anim.CrossFade("death");
        GameObject cash = Instantiate(gold, transform.position + new Vector3(1,0,0), transform.rotation);
        setGold(cash);
        cash = Instantiate(gold, transform.position + new Vector3(-1, 0, 0), transform.rotation);
        setGold(cash);
        cash = Instantiate(gold, transform.position + new Vector3(0, 0, 1), transform.rotation);
        setGold(cash);
        cash =Instantiate(gold, transform.position + new Vector3(0, 0, -1), transform.rotation);
        setGold(cash);
    }

    public void setGold(GameObject gold)//sets the gold layer and amount
    {
        if (gameObject.layer == 9)
        {
            gold.layer = LayerMask.NameToLayer("Currency 1");
            gold.transform.Find("gold_bar").gameObject.layer = LayerMask.NameToLayer("Currency 1");
        }
        else
        {
            gold.layer = LayerMask.NameToLayer("Currency 2");
            gold.transform.Find("gold_bar").gameObject.layer = LayerMask.NameToLayer("Currency 2");
        }
        gold.GetComponent<Gold>().setAmount(5, enemyLayer);
    }
       
}
