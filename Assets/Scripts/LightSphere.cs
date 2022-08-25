using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightSphere : MonoBehaviour
{
    bool bounced = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        if (bounced)
            Destroy(this.gameObject);
            
        bounced = true;
    }
}
