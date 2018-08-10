using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDoctorControl : MonoBehaviour
{

    private float rotation;
    private float radians;
    private Animation anim;
    private Rigidbody rigidbody;
    private bool free;

    public float rotationalSpeed = 1.5f;
    public float walkingSpeed = 10f;

    // Use this for initialization
    void Start ()
    {
        // Movement intitialising 
        rotation = transform.rotation.eulerAngles.y;
        anim = GetComponent<Animation>();
        rigidbody = GetComponent<Rigidbody>();
        free = true;

    }
	
	// Update is called once per frame
	void Update ()
    {

        free = true;

        if (Input.GetKey(KeyCode.UpArrow))//move forward
        {
            radians = rotation / 360 * 2 * Mathf.PI;
            rigidbody.velocity = new Vector3(Mathf.Sin(radians) * walkingSpeed, 0, Mathf.Cos(radians) * walkingSpeed);
            if (!anim.IsPlaying("walk"))
            {
                //anim["walk"].speed = 1.0f;
                anim.CrossFade("walk");
            }
            free = false;

        }

        if (Input.GetKey(KeyCode.DownArrow))//move backward
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

        if (Input.GetKey(KeyCode.LeftArrow))//rotate left
        {
            rotation -= rotationalSpeed;
            if (!anim.IsPlaying("walk"))
            {
                anim.CrossFade("walk");
            }
            free = false;
        }

        if (Input.GetKey(KeyCode.RightArrow))//rotate right
        {
            rotation += rotationalSpeed;
            if (!anim.IsPlaying("walk"))
            {
                anim.CrossFade("walk");
            }
            free = false;
        }

        if (free)
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
            if (!anim.IsPlaying("free"))
            {
                anim.CrossFade("free");
            }

        }

    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, rotation, 0);

    }
}
