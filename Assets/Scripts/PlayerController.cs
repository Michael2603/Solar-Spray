using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D rigidbody2d;
    Collider2D collider2d;
    Animator animator;

    public bool solstice = false;
    public float solsticeTimer;
    float timerCounter;

    public float moveVel;

    [Range(0,5)]public float paintSpeed;

    int health = 3;
    public List<GameObject> HP = new List<GameObject>();
    public Sprite damagedImage;
    public Sprite fineImage;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        timerCounter = solsticeTimer;
    }

    void Update()
    {
        animator.SetFloat("VelocityX", rigidbody2d.velocity.x);
        animator.SetFloat("VelocityY", rigidbody2d.velocity.y);

        if (rigidbody2d.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        }
        else if (rigidbody2d.velocity.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        }

        HealthControll();
    }

    void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector3(Input.GetAxis("Horizontal") * moveVel, Input.GetAxis("Vertical") * moveVel, 0);
        
        if (solstice)
        {
            timerCounter -= Time.deltaTime;

            if (timerCounter > 0)
            {
                moveVel = 3;
                paintSpeed = 0;
            }
            else
            {
                moveVel = 2;
                paintSpeed = 1;
                solstice = false;
                timerCounter = solsticeTimer;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.Space) && other.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            other.gameObject.GetComponent<TilesControll>().Paint(GetComponent<Collider2D>());
            if(!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
        }

        if (Input.GetKeyUp(KeyCode.Space) && other.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            other.gameObject.GetComponent<TilesControll>().StopPainting(GetComponent<Collider2D>());
            GetComponent<AudioSource>().Stop();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (solstice && other.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            Destroy(other.collider.gameObject);
    }
    
    void HealthControll()
    {
        if (health == 3)
        {
            HP[0].GetComponent<Image>().sprite = fineImage;
            HP[1].GetComponent<Image>().sprite = fineImage;
            HP[2].GetComponent<Image>().sprite = fineImage;
        }
        else if (health == 2)
        {
            HP[0].GetComponent<Image>().sprite = fineImage;
            HP[1].GetComponent<Image>().sprite = fineImage;
            HP[2].GetComponent<Image>().sprite = damagedImage;

        }
        else if (health == 1)
        {
            HP[0].GetComponent<Image>().sprite = fineImage;
            HP[1].GetComponent<Image>().sprite = damagedImage;
            HP[2].GetComponent<Image>().sprite = damagedImage;
        }
        else if (health <= 0)
        {
            gameManager.GameOver();
        }
    }

    public void Hit()
    {
        health--;
    }
}