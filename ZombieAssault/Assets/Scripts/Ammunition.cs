using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pentru pachetele de munitie
public class Ammunition : MonoBehaviour
{
    void Update()
    {
        //face obiectul sa se roteasca
        transform.Rotate(new Vector3(0, Time.deltaTime * 50, 0)); 
    }
    void OnTriggerEnter(Collider collider) 
    {   
        //cand playerul intra in contact cu obiectul acesta va fi distrus iar playerul va primi munitie (un numar aleator intre 3 si 6)
        if(collider.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            int random = Random.Range(3, 6);
            GameStats.instance.AddAmmo(random);
        }
    }   
}
