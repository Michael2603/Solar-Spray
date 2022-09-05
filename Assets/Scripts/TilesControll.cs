using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TilesControll : MonoBehaviour
{
    public GameManager gameManager;
    Animator animator;
    float paintTimer;

    bool ended = false;
    public string state = "Dark";
    float enemySpeed;

    public bool lightSphereHit = false;

    public AudioClip[] graffitiSounds;
    public AudioClip[] paintedSounds;
    public AudioSource[] audioSources;
    int paintingStage = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (!ended)
        {
            paintTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player") && !ended)
            paintTimer = collider.gameObject.GetComponent<PlayerController>().paintSpeed;
        else if (collider.gameObject.layer == LayerMask.NameToLayer("LightSphere") && !ended)
        {
            paintTimer = 1;
            lightSphereHit = true;
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && !ended)
        {
            try
            {
                paintTimer = collider.gameObject.GetComponent<Enemy>().paintSpeed;
            }
            catch(Exception e)
            {}
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Paint(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        StopPainting(other);
    }

    public void StopPainting(Collider2D collider)
    {
        if ((state == "Light" && collider.name.Contains("Glob")) || (collider.gameObject.layer == LayerMask.NameToLayer("LightSphere") && state == "Dark"))
        {
            animator.speed = 1;
        }
        else
        {
            if (paintTimer > 0)
                animator.Play("LightCube", -1, 0);
            
            animator.speed = 0;
        }

        paintingStage = 0;
        ended = true;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            collider.gameObject.GetComponent<Enemy>().moveSpeed = collider.GetComponent<Enemy>().totalMoveSpeed;
    }

    public void Paint(Collider2D collider)
    {
        ended = false;

        if (state == "Dark")
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (paintTimer <= 0)
                {
                    animator.speed = 1;
                    paintTimer = collider.gameObject.GetComponent<PlayerController>().paintSpeed;
                    ended = true;

                    if(collider.GetComponent<PlayerController>().solstice == false)
                    {
                        audioSources[0].clip = graffitiSounds[paintingStage];
                        audioSources[0].Play();
                    }
                    else
                    {
                        audioSources[0].clip = graffitiSounds[2];
                        if (!audioSources[0].isPlaying)
                            audioSources[0].Play();
                    }

                    if (paintingStage < 2)
                        paintingStage++;
                }
            }
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collider.GetComponent<Enemy>().moveSpeed = collider.GetComponent<Enemy>().totalMoveSpeed;
                lightSphereHit = false;
            }
            if (collider.gameObject.layer == LayerMask.NameToLayer("LightSphere"))
            {
                animator.speed = 1;
                lightSphereHit = true;
            }
        }
        else if (state == "Light")
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                animator.speed = 1;
                paintTimer = collider.gameObject.GetComponent<Enemy>().paintSpeed;
                ended = true;

                if (!audioSources[0].isPlaying)
                {
                    audioSources[0].clip = graffitiSounds[3];
                    audioSources[0].Play();
                }

                collider.GetComponent<Enemy>().moveSpeed = collider.GetComponent<Enemy>().totalMoveSpeed / 2;

            }
        }
    } 


    public void FreezeAnimation()
    {
        if (!lightSphereHit)
            animator.speed = 0;
    }

    public void EndedAnimation()
    {
        ended = true;
        lightSphereHit = false;
        animator.speed = 0;
        animator.SetBool("Dark", !animator.GetBool("Dark"));

        GameObject lightParticles = this.transform.GetChild(0).gameObject;
        GameObject darkParticles = this.transform.GetChild(1).gameObject;
        paintingStage = 0;

        if (state == "Dark")
        {
            state = "Light";
            audioSources[1].clip = paintedSounds[0];
            audioSources[1].Play();
            gameManager.tileCount++;

            lightParticles.SetActive(true);
            lightParticles.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            state = "Dark";
            audioSources[1].clip = paintedSounds[1];
            audioSources[1].Play();
            gameManager.tileCount--;
            gameManager.PaintedDark();
            
            darkParticles.SetActive(true);
            darkParticles.GetComponent<ParticleSystem>().Play();
        }
    }
}