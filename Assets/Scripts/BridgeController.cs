using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    Collider2D collider;
    SpriteRenderer spriteRenderer;

    float timer = 2;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (collider.enabled == true)
            {
                collider.enabled = false;
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
                collider.enabled = true;
            }

            timer = 18;
        }
    }
}
