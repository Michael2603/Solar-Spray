using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    BackgrondBehaviour background;
    PlayerController player;
    public GameManager manager;

    [SerializeField] AudioClip[] lightSounds;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        background = GameObject.Find("Background").GetComponent<BackgrondBehaviour>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = lightSounds[Random.Range(0,lightSounds.Length)];
            audioSource.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D playerCollider)
    {
        if (playerCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            background.solstice = true;
            background.timerCounter = background.solsticeTimer;
            background.startTimer = true;
            player.solstice = true;
            manager.collectibleOn = false;
            Destroy(this.gameObject);
        }
    }
}
