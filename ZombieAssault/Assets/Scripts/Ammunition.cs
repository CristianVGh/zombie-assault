﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * 50, 0)); 
    }
    void OnTriggerEnter(Collider collider) 
    {
        if(collider.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            int random = Random.Range(3, 6);
            GameStats.instance.AddAmmo(random);
        }
    }   
}
