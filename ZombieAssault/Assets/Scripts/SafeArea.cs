using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeArea : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) 
    {
        if(collider.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Level 2");
            AudioManager.instance.Stop("Level_1_Music");
            AudioManager.instance.Play("Level_2_Music");
        }
    }   
}
