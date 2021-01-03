using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void damagePlayer (int damage)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHealth -= 10;
            UpdateHealthBar();
        }

    }
}
