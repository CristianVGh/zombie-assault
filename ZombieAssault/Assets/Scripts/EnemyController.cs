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

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            animator.SetBool("Aware", false);
            float distance = Vector3.Distance(playerLocation.position, transform.position);
            if(distance <= lookRadius)
            {
                if(!isAware) 
                {
                    int soundEffect = Random.Range(1, 5);
                    AudioManager.instance.Play("Zombie_Aware_" + soundEffect);
                }
                isAware = true;
                agent.speed = speed;
                animator.SetBool("Aware", true);
            
                agent.SetDestination(playerLocation.position);
                if(distance <= agent.stoppingDistance)
                {
                    FaceTarget();
                }

                if(distance <= attackRange && Time.time > nextTimeToAttack) 
                {
                    nextTimeToAttack = Time.time + attackRate;
                    animator.SetTrigger("Attack");
                    AudioManager.instance.Play("Player_Hit");
                    GameStats.instance.DamagePlayer(damage);
                    GameStats.instance.TakePoints(100);
                }
            
            } else
            {
                agent.speed = 0;
                isAware = false;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (playerLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); //rotation will point to player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); //update enemy rotation to point in this direction (slerp smooths the animation)
    }

    public void TakeDamage(float amount)
    {
        int soundEffect = Random.Range(1, 4);
        AudioManager.instance.Play("Zombie_Damaged_" + soundEffect);
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");
        int soundEffect = Random.Range(1, 3);
        AudioManager.instance.Play("Zombie_Death_" + soundEffect);
        agent.speed = 0;
        isAlive = false;
        
        GameObject ammo = Instantiate(ammoPrefab) as GameObject;
        ammo.transform.position = this.transform.position;


        Destroy(gameObject, 5f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
