using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    Collider2D collider;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    public AudioClip[] clips;

    float timer = 2;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (collider.enabled == true)
            {
                spriteRenderer.enabled = true;
                collider.enabled = false;
                
                audioSource.clip = clips[1];
            }
            else
            {
                spriteRenderer.enabled = false;
                collider.enabled = true;

                audioSource.clip = clips[0];
            }
            audioSource.Play();

            timer = 18;
        }
    }
}
