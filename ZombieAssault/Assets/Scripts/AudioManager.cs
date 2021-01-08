using UnityEngine;
using UnityEngine.Audio;
using System;

//gestioneaza fisierele audio
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    void Awake()
    {
        //ne asiguram ca obiectul ramane cand schimbam niveiul
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
         Debug.Log("AUdI START");
        //atribuie proprietati fisierului audio (volum, repetabil sau nu, locul de unde sa se auda sonorul >> 3D sau 2D)
        //proprietatile sunt luate din obiectul AudioManager din fisierul prefabs, fiecare efect sonor poate fi ajustat de acolo
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
           
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if(s == null)
        {
            Debug.Log("The sound effect " + name + " could not be found");
            return;
        }
            
        s.source.Play();
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
         if(s == null)
        {
            Debug.Log("The sound effect " + name + " could not be found");
            return;
        }
            
        s.source.Stop();
    }
}
