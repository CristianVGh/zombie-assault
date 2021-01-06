using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public GameObject endGameUI;
    public GameObject winUI;

    void Awake()
    {
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
        healthBar.SetMaxHealth(playerHealth);
        score = 0;
        scoreText.text = score + "";
    }

    public void DamagePlayer (int damage)
    {
        playerHealth -= damage;
        UpdateHealthBar();
    }

    public void AddAmmo (int ammo)
    {
        totalAmmo += ammo;
        UpdateAmmoView();
    }

    public int GetAmmo ()
    {
        return totalAmmo;
    }

    public void SetAmmo (int ammo) 
    {
        totalAmmo = ammo;
        UpdateAmmoView();
    }

    public void UpdateHealthBar ()
    {
        healthBar.SetHealth(playerHealth);
    }

    public void SetLoadedAmmo(int ammo)
    {
        loadedAmmo = ammo;
        UpdateAmmoView();
    }

    public int GetLoadedAmmo()
    {
        return loadedAmmo;
    }

    public void UpdateAmmoView()
    {
        ammoText.text = loadedAmmo + "/" + totalAmmo; 
    }

    public void GivePoints(int points)
    {
        score += points;
        scoreText.text = score + "";
    }

    public void TakePoints(int points)
    {
        score -= points;
        scoreText.text = score + "";
    }
    void Update ()
    {
        if (playerHealth <= 0)
        {
            EndGame();
        }

    }

    void EndGame()
    {
        endGameUI.SetActive(true);
        Time.timeScale = 0f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        endGameUI.SetActive(false);
        SceneManager.LoadScene("Level 1");
        AudioManager.instance.Stop("Level_1_Music");
        AudioManager.instance.Stop("Level_2_Music");
        AudioManager.instance.Play("Level_1_Music");
        playerHealth = 100;
        totalAmmo = 40;
        loadedAmmo = 7;
        Time.timeScale = 1f;
        UpdateHealthBar();
    }

    public void WinGame()
    {
        winUI.SetActive(true);
    }
}
