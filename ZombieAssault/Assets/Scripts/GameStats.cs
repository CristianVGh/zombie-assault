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

    public HealthBar healthBar;

    public Text ammoText;

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
    }

    public void damagePlayer (int damage)
    {
        playerHealth -= damage;
        updateHealthBar();
    }

    public void addAmmo (int ammo)
    {
        totalAmmo += ammo;
        updateAmmoView();
    }

    public int getAmmo ()
    {
        return totalAmmo;
    }

    public void setAmmo (int ammo) 
    {
        totalAmmo = ammo;
        updateAmmoView();
    }

    public void updateHealthBar ()
    {
        healthBar.SetHealth(playerHealth);
    }

    public void setLoadedAmmo(int ammo)
    {
        loadedAmmo = ammo;
        updateAmmoView();
    }

    public int getLoadedAmmo()
    {
        return loadedAmmo;
    }

    public void updateAmmoView()
    {
        ammoText.text = loadedAmmo + "/" + totalAmmo; 
    }
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHealth -= 10;
            updateHealthBar();
        }

    }
}
