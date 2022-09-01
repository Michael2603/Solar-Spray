using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float detectionRange;
    public float moveSpeed;
    public float totalMoveSpeed;
    Rigidbody2D rigidbody2d;

    public float paintSpeed;
    TilesControll tileControll;
    public AudioSource[] audioSources;
    public AudioClip[] clips;

    bool killed = false;
    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        moveSpeed = totalMoveSpeed;
    }

    void Update()
    {
        if (killed)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            if (!audioSources[1].isPlaying)
                Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        Collider2D raycastPlayer = Physics2D.OverlapCircle(transform.position, detectionRange, 1 << LayerMask.NameToLayer("Player"));
        if (raycastPlayer != null)
        {
            GoAfter(raycastPlayer.gameObject.GetComponent<Transform>());
        }
        else if (raycastPlayer == null)
        {
            Collider2D[] raycastTile = Physics2D.OverlapCircleAll(transform.position, detectionRange * 1.5f, 1 << LayerMask.NameToLayer("Tile"));
            if (raycastTile != null)
            {
                foreach (var tile in raycastTile)
                {
                    if (tile.gameObject.GetComponent<TilesControll>().state == "Light")
                        tileControll = tile.gameObject.GetComponent<TilesControll>();
                }

                if (tileControll != null && tileControll.state == "Light")
                {
                    GoAfter(tileControll.gameObject.GetComponent<Transform>());
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController pC = collision.collider.gameObject.GetComponent<PlayerController>();
            if (!pC.solstice)
                pC.Hit();
        }
    }

    void GoAfter(Transform player)
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);

        rigidbody2d.AddForce(transform.right * moveSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void Kill()
    {
        foreach (AudioSource sources in audioSources)
        {
            sources.Stop();
        }

        audioSources[1].clip = clips[0];
        audioSources[1].Play();
        killed = true;
    }
}