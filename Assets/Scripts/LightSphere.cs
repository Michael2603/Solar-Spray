using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightSphere : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            Destroy(other.gameObject);
    }
}
