using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//gestioneaza informatii generale despre jucator si jocul in sine
public class GameStats : MonoBehaviour
{
    public static GameStats instance;

    public static int playerHealth = 100;
    public static int totalAmmo = 40;
    public static int loadedAmmo = 7;
    private int score;

    public HealthBar healthBar;

    public Text ammoText;
    public Text scoreText;
    public Text finalScoreText;

    public GameObject endGameUI;
    public GameObject winUI;

    void Awake()
    {
        //se asigura ca obiectul nu va disparea la schimbarea nivelelor
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //initializeaza scorul si punctele de viata la inceperea jocului
        healthBar.SetMaxHealth(playerHealth);
        score = 0;
        scoreText.text = score + "";
    }

    void Update ()
    {
        //verifica in mod constant daca jucatorul este in viata
        if (playerHealth <= 0)
        {
            EndGame();
        }

    }

    //cauzeaza daune jucatorului si actualizeaza bara de hp de pe ecran
    public void DamagePlayer (int damage)
    {
        playerHealth -= damage;
        healthBar.SetHealth(playerHealth);
    }

    //adauga munitie jucatorului si actulizeaza scrisul de pe ecran
    public void AddAmmo (int ammo)
    {
        totalAmmo += ammo;
        ammoText.text = loadedAmmo + "/" + totalAmmo; 
    }

    //returneaza valoarea munitiei pe care o are jucatorul
    public int GetAmmo ()
    {
        return totalAmmo;
    }

    //modifica valoarea munitiei pe care o are jucatorul si actulizeaza scrisul de pe ecran
    public void SetAmmo (int ammo) 
    {
        totalAmmo = ammo;
        ammoText.text = loadedAmmo + "/" + totalAmmo; 
    }

    //modifica valoarea munitiei incarcate in arma si actulizeaza scrisul de pe ecran
    public void SetLoadedAmmo(int ammo)
    {
        loadedAmmo = ammo;
        ammoText.text = loadedAmmo + "/" + totalAmmo; 
    }

    //returneaza valoarea munitiei incarcate in arma 
    public int GetLoadedAmmo()
    {
        return loadedAmmo;
    }

    //adauga puncte jucatorului si actulizeaza scrisul de pe ecran
    public void GivePoints(int points)
    {
        score += points;
        scoreText.text = score + "";
    }

    //substrage puncte jucatorului si actulizeaza scrisul de pe ecran
    public void TakePoints(int points)
    {
        score -= points;
        scoreText.text = score + "";
    }

    //se apeleaza cand jucatorul moare
    void EndGame()
    {
        endGameUI.SetActive(true); //afiseaza ecranul "Game Over"
        Time.timeScale = 0f; //ingheata "timpul" din joc (efect de pauza)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame(); //daca jucatorul apasa "Space" restarteaza jocul
        }
    }


    public void RestartGame()
    {
        endGameUI.SetActive(false); //ascunde ecranul "Game Over"
        SceneManager.LoadScene("Level 1"); //intoarce jucatorul la primu nivel
        AudioManager.instance.Stop("Level_1_Music");
        AudioManager.instance.Stop("Level_2_Music");
        AudioManager.instance.Play("Level_1_Music"); //opreste orice muzica si ruleaza din nou muzica pentru nivelul 1
        //reseteaza punctele de viata si munitia jucatorului la starea initiala
        playerHealth = 100;
        totalAmmo = 40;
        loadedAmmo = 7;
        Time.timeScale = 1f; //porneste din nou timpul jocului
        healthBar.SetHealth(playerHealth);
    }

    //se apeleaza dupa finalizarea celui de-al doilea nivel
    public void WinGame()
    {
        winUI.SetActive(true); //afiseaza ecranul de final de joc
        finalScoreText.text = "Final Score: " + score;
    }
}
