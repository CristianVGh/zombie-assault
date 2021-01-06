using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start ()
    {
        AudioManager.instance.Play("Menu_Music");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
        AudioManager.instance.Stop("Menu_Music");
        AudioManager.instance.Play("Level_1_Music");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
