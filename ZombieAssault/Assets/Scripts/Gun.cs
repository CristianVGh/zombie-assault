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
        GameStats.instance.SetLoadedAmmo(magazineSize); //incarca gloante in arma in fuctie de cat de mare este capacitatea incarcatorului
    }

    void Update()
    {
        //daca arma este in proces de a fi reincarcata blocheaza posibilitatea de a impusca
        if(isReloading)
            return;

        //atribuie butonul R operatiunii de incarcare a armei
        if(UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            //coroutine se asigura ca o actiune se executa de-a lungul unei perioade de timp si nu instant
            //in cazul incarcarii armei nu vrem ca acest lucru sa se intample instant
            StartCoroutine(Reload()); 
            return;
        }

        //atirbuie butonul click stanga operatiunoii de a impusca
        //si in acelasi timp se asigura ca arma respecta o rata de foc
        if (UnityEngine.Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        //Numarul de gloante al jucatorului este stocat in clasa GameStats
        int ammunition = GameStats.instance.GetAmmo();
        int loadedAmmo = GameStats.instance.GetLoadedAmmo();
        if( ammunition > 0  && loadedAmmo < magazineSize) // conditie pentru a putea incarca arma, trebuie sa ai gloante disponibile si sa nu ai 7 gloante incarcate deja
        {
            isReloading = true;
            animator.SetBool("Reloading", true); //ruleaza animatia de incarcare a armei
            yield return new WaitForSeconds(reloadTime - .25f); //blocheaza orice alta functie a armei cat timp arma se reincarca
            //animatia de incarcat arma este separata in 2 faze, cea cand lasa arma in jos si cea cand ridica arma inapoi
            //in prima faza lasam arma jos si blocam orice alta functie cat timp asteptam sa se incarce, acest lucru va bloca si posibilitatea de a rula ce-a de-a 2 parte a animatiei
            //de aceea oferim un timp de 0.25 secunde (.25f) ca arma sa revina la pozitia initiala inainte de a permite jucatorului sa impuste din nou
            animator.SetBool("Reloading", false); //ruleaza cea de-a 2 parte a animatiei
            AudioManager.instance.Play("Reload"); //ruleaza sunetul pentru incarcarea armei
            yield return new WaitForSeconds(.25f);
            
            //incarca arma in functie de cate gloante sunt disponibile
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

        } 
        
    }
    
    public void Shoot()
    {
        int loadedAmmo = GameStats.instance.GetLoadedAmmo();
        if(loadedAmmo <= 0) 
        {
            AudioManager.instance.Play("Empty_Clip"); //daca incarcatorul este gol ruleaza efectul sonor aferent si opreste functia shoot
            return;
        }

        loadedAmmo--;
        GameStats.instance.SetLoadedAmmo(loadedAmmo);
        muzzleFlash.Play(); //activeaza efectul vizual cand impusti (focul de arma)
        AudioManager.instance.Play("Gunshot");
        
        RaycastHit hit; //obiectul lovit
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) //verifica daca obiectul lovit se afla in raza prestabilita
        {
            EnemyController zombieController = hit.transform.GetComponent<EnemyController>();
            
            if(zombieController != null) //verifica daca obiectul lovit este un inamic
            {
                zombieController.TakeDamage(damage); //apeleaza functia TakeDamage ce apartine inamicului
                GameStats.instance.GivePoints(100); //ofera 100 puncte jucatorului
            } else
            {
                GameStats.instance.TakePoints(50); //daca jucatorul nu a lovit nimic ii vor fi substrase 50 de puncte
            }
            
        }

    }
}
