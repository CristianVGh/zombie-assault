using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start ()
    {
        AudioManager.instance.Play("Menu_Music");
        Cursor.lockState = CursorLockMode.None; //face cursorul sa apara pe ecran
    }

    //butonul "PLAY"
    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1"); //face tranzitia catre scena cu numele "Level 1"
        AudioManager.instance.Stop("Menu_Music");
        AudioManager.instance.Play("Level_1_Music");
    }

    //butonul "QUIT"
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit(); //metoda are efect doar in cazul in care jocul este rulat din fisierul exe, nu din unity
    }
}
