using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesControll : MonoBehaviour
{
    public GameManager gameManager;
    Animator animator;
    float paintTimer;

    bool ended = false;
    public string state = "Dark";
    float enemySpeed;

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
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && !ended)
        {
            paintTimer = collider.gameObject.GetComponent<Enemy>().paintSpeed;
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
        if (state == "Light" && collider.name.Contains("Glob"))
        {
            animator.speed = 1;
        }
        else
        {
            if (paintTimer > 0)
                animator.Play("LightCube", -1, 0);
            
            animator.speed = 0;
        }

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
                }
            }
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collider.GetComponent<Enemy>().moveSpeed = collider.GetComponent<Enemy>().totalMoveSpeed;
            }
        }
        else if (state == "Light")
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                animator.speed = 1;
                paintTimer = collider.gameObject.GetComponent<Enemy>().paintSpeed;
                ended = true;

                collider.GetComponent<Enemy>().moveSpeed = collider.GetComponent<Enemy>().totalMoveSpeed / 2;

            }
        }
    } 


    public void FreezeAnimation()
    {
        animator.speed = 0;
    }

    public void EndedAnimation()
    {
        ended = true;
        animator.speed = 0;
        animator.SetBool("Dark", !animator.GetBool("Dark"));
        if (state == "Dark")
        {
            state = "Light";
            gameManager.tileCount++;
        }
        else
        {
            state = "Dark";
            gameManager.tileCount--;
        }

        GetComponent<ParticleSystem>().Play();
    }
}