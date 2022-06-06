using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    BackgrondBehaviour background;
    PlayerController player;
    public GameManager manager;

    void Start()
    {
        background = GameObject.Find("Background").GetComponent<BackgrondBehaviour>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
