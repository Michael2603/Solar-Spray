using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSphere : MonoBehaviour
{
    public AudioSource launchSound;
    public AudioClip[] launchClips;
    
    public AudioSource bounceSound;

    bool bounced = false;

    void Start()
    {
        launchSound.clip = launchClips[Random.Range(0, launchClips.Length)];
        launchSound.Play();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Kill();
            Destroy(this.gameObject);
        }

        if (bounced)
            Destroy(this.gameObject);
        else
            bounceSound.Play();
            
        bounced = true;
    }
}
