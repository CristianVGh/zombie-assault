using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage;
    public float range;
    public float fireRate;

    public int magazineSize;
    public float reloadTime;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 0f;

    public Animator animator;

    void Start()
    {
        GameStats.instance.SetLoadedAmmo(magazineSize);
    }

    void Update()
    {
        if(isReloading)
            return;

        if(UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (UnityEngine.Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        int ammunition = GameStats.instance.GetAmmo();
        int loadedAmmo = GameStats.instance.GetLoadedAmmo();
        if( ammunition > 0  && loadedAmmo < magazineSize)
        {
            isReloading = true;
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(reloadTime - .25f);
            animator.SetBool("Reloading", false);
            AudioManager.instance.Play("Reload");
            yield return new WaitForSeconds(.25f);
            if(ammunition >= magazineSize - loadedAmmo) 
            {
                ammunition -= magazineSize - loadedAmmo;
                loadedAmmo = magazineSize;   
                
            } else 
            {
                loadedAmmo += ammunition;
                ammunition = 0;
            }
            isReloading = false;
            GameStats.instance.SetAmmo(ammunition);
            GameStats.instance.SetLoadedAmmo(loadedAmmo);

        } else 
        {
            //play sad song
        }
        
    }
    
    public void Shoot()
    {
        int loadedAmmo = GameStats.instance.GetLoadedAmmo();
        if(loadedAmmo <= 0) 
        {
            AudioManager.instance.Play("Empty_Clip");
            return;
        }
        loadedAmmo--;
        GameStats.instance.SetLoadedAmmo(loadedAmmo);
        muzzleFlash.Play();
        RaycastHit hit;

        AudioManager.instance.Play("Gunshot");
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            EnemyController zombieController = hit.transform.GetComponent<EnemyController>();
            
            if(zombieController != null)
            {
                zombieController.TakeDamage(damage);
                Debug.Log("We hit " + hit.collider.name + " for " + damage + " damage") ;
                GameStats.instance.GivePoints(100);
            } else
            {
                GameStats.instance.TakePoints(50);
            }
            
        }

    }
}
