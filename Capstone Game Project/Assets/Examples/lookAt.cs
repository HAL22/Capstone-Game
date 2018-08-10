using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAt : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.LookAt(target.position+new Vector3(0,2.5f,0));
    }
}
