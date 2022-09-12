using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920, 1080, true, 60);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HudSound()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
    }
}