using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public float lookRadius = 25f;
    public float health = 100f;
    public float speed = 7f;
    public int damage = 10;

    private bool isAlive = true;
    private bool isAware = false;

    private float nextTimeToAttack = 0f;
    public float attackRate;
    public float attackRange;

    Transform playerLocation;
    NavMeshAgent agent;
    Animator animator;

    public GameObject ammoPrefab;
    void Start()
    {
        playerLocation = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(isAlive)
        {
            animator.SetBool("Aware", false); //ruleaza animatia pentru stat 
            float distance = Vector3.Distance(playerLocation.position, transform.position); //obtine distanta la care se afla jucatorul
            if(distance <= lookRadius) //verifica daca jucatorul este in zona de detectie a inamicului
            {
                if(!isAware) 
                {
                    //daca inamicul statea degeaba atunci cand jucatorul a intrat in raza lui de detectie
                    //atunci audio manager va rula unui din cele 5 efecte sonore prestabilite situatiei acestea
                    //instructiunea if se asigura ca efectul sonor nu va fi rulat in continuu
                    int soundEffect = Random.Range(1, 5);
                    AudioManager.instance.Play("Zombie_Aware_" + soundEffect);
                }
                isAware = true;
                agent.speed = speed;
                animator.SetBool("Aware", true);  //ruleaza animatia pentru alergat
            
                agent.SetDestination(playerLocation.position); //face ca inamicul sa urmareasca jucatorul
                if(distance <= agent.stoppingDistance)
                {
                    FaceTarget();
                }

                if(distance <= attackRange && Time.time > nextTimeToAttack) 
                {
                    nextTimeToAttack = Time.time + attackRate; //ne asiguram ca inamicul poate ataca o data la cateva secunde. Attack Rate da numarul de secunde dintre atacuri
                    animator.SetTrigger("Attack"); //ruleaza animatia pentru atac
                    AudioManager.instance.Play("Player_Hit"); //ruleaza efectul sonor pentru atac
                    GameStats.instance.DamagePlayer(damage); //cauzeaza daune jucatorului in functie de damage-ul inamicului
                    GameStats.instance.TakePoints(100); // scade punctajul jucatorului
                }
            
            } else
            {
                //daca jucatorul iese din raza vizuala inamicul se va opri
                agent.speed = 0;
                isAware = false;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (playerLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); //calculeaza coordonatele rotatie astfel incat inamicul sa fie cu fata la jucator
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //executa rotatia, slerp face rotatia sa fie mai fina
    }

    public void TakeDamage(float amount)
    {
        int soundEffect = Random.Range(1, 4);
        AudioManager.instance.Play("Zombie_Damaged_" + soundEffect); //ruleaza unul din cele 4 efecte sonore pentru situatia in care inamicul este lovit
        health -= amount; //scade punctele de viata in functie puterea armei
        if(health <= 0)
        {
            Die();
            if(this.transform.tag == "Zombie Boss")
            {
                //opreste jocul atunci cand jucatorul a omorat inamicul principal din nivelul 2 si afiseaza mesajul de final
                GameStats.instance.WinGame();
            }
        }
    }

    void Die()
    {
        animator.SetTrigger("Death"); //ruleaza animatia pentru moarte
        int soundEffect = Random.Range(1, 3);
        AudioManager.instance.Play("Zombie_Death_" + soundEffect);
        agent.speed = 0; //opreste inamicul din a se mai misca
        isAlive = false;
        
        // creeaza un pachet de munitie in locul in care a murit
        GameObject ammo = Instantiate(ammoPrefab) as GameObject;
        ammo.transform.position = this.transform.position;


        Destroy(gameObject, 5f); //face ca inamicul sa dispara dupa 5 secunde
    }

    //functie pentru a vedea raza de detectie a inamicului
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
