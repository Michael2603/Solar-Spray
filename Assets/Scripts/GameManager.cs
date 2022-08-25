using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject tileMap;
    public List<TilesControll> tiles = new List<TilesControll>();

    public GameObject player;
    public GameObject enemiesManager;
    public GameObject blobPrefab;
    public GameObject globPrefab;

    bool spawn = false;
    float spawnTimer = 15;
    float timer;

    public GameObject collectiblePrefab;
    public bool collectibleOn = true;
    float spawnCollectibleTimer = 40;
    float timer2;

    public GameObject gameOverPanel;
    public GameObject gameVictoryPanel;

    public int tileCount;
    public int darkTileAmount;

    void Start()
    {
        tileMap = GameObject.Find("Tiles");
        enemiesManager = GameObject.Find("Enemies");

        foreach (Transform square in tileMap.transform)
        {
            tiles.Add(square.gameObject.GetComponent<TilesControll>());
        }

        timer = spawnTimer;
        timer2 = spawnCollectibleTimer;

        collectibleOn = true;
    }

    void Update()
    {
        int globCounter = 0;
        int blobCounter = 0;
        foreach (Transform obj in enemiesManager.transform)
        {
            if (obj.gameObject.name.Contains("Blob"))
                blobCounter++;
            else if (obj.gameObject.name.Contains("Glob"))
                globCounter++;
        }

        if (blobCounter < 1 || globCounter < 2)
        {
            spawn = true;
        }

        if (spawn)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                int localCounter = 0;
                foreach (TilesControll obj in tiles)
                {
                    if (obj.state == "Dark")
                        break;
                    else if (localCounter >= tiles.Count)
                    {
                        timer = 10;
                        return;
                    }
                    localCounter++;
                }

                while (true)
                {
                    int index = Random.Range(0, tiles.Count);
                    
                    if (tiles[index].state == "Dark")
                    {
                        Spawn(globCounter, blobCounter, tiles[index].gameObject);
                        timer = spawnTimer;
                        spawn = false;
                        break;
                    }
                }
            }
        }

        if (!collectibleOn)
        {
            timer2 -= Time.deltaTime;

            if (timer2 <= 0)
            {
                int localCounter2 = 1;
                foreach (TilesControll obj in tiles)
                {
                    if (obj.state == "Light")
                        break;
                    else if (localCounter2 >= tiles.Count)
                    {
                        timer2 = 10;
                        return;
                    }
                    localCounter2++;
                }

                localCounter2 = 0;

                while (localCounter2 < tiles.Count)
                {
                    int index2 = Random.Range(0, tiles.Count);
                    
                    if (tiles[index2].state == "Light")
                    {
                        SpawnCollectible(tiles[index2].gameObject);
                        timer2 = spawnCollectibleTimer;
                        collectibleOn = true;
                        break;
                    }
                    localCounter2++;
                }
            }
        }

        if (darkTileAmount >= 10)
        {
            player.GetComponent<PlayerController>().canShoot = true;
            darkTileAmount = 0;
        }

        if (tileCount == tiles.Count)
        {
            GameVictory();
        }
    }

    void Spawn(int globCounter, int blobCounter, GameObject tile)
    {
        if (globCounter < 1 && blobCounter < 2)
        {
            switch (Random.Range(0,2))
            {
                case 0:
                    Instantiate(blobPrefab, tile.transform.position, enemiesManager.transform.rotation, enemiesManager.transform);
                break;
                case 1:
                    Instantiate(globPrefab, tile.transform.position, enemiesManager.transform.rotation, enemiesManager.transform);
                break;
            }
        }
        else if (blobCounter < 1)
            Instantiate(blobPrefab, tile.transform.position, enemiesManager.transform.rotation, enemiesManager.transform);
        else
            Instantiate(globPrefab, tile.transform.position, enemiesManager.transform.rotation, enemiesManager.transform);

    }

    void SpawnCollectible(GameObject tile)
    {
        Instantiate(collectiblePrefab, tile.transform.position, enemiesManager.transform.rotation);
    }


    public void GameOver()
    {
        GameObject.Find("Player").SetActive(false);
        GameObject.Find("Health").SetActive(false);
        GameObject.Find("Enemies").SetActive(false);
        GameObject.Find("Map").SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void GameVictory()
    {
        GameObject.Find("Player").SetActive(false);
        GameObject.Find("Health").SetActive(false);
        GameObject.Find("Enemies").SetActive(false);
        GameObject.Find("Map").SetActive(false);
        gameVictoryPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
