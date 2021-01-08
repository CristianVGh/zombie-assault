using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//face tranzitia catre nivelul 2 cand jucatorul intra in zona de siguranta
public class SafeArea : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) 
    {
        if(collider.gameObject.tag == "Player") //verifica daca jucatorul este cel care a intrat in zona
        {
            SceneManager.LoadScene("Level 2"); //face tranzitia catre urmatorul nivel
            AudioManager.instance.Stop("Level_1_Music");
            AudioManager.instance.Play("Level_2_Music");
        }
    }   
}
