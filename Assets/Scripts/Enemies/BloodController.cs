using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float timeToDestroy = 0.5f;
    
    [SerializeField] List<AudioSource> sounds;
    void Start()
    {
        if (sounds != null) 
        { 

        sounds[Random.Range(0, sounds.Count)].Play();

        }

        else
        {

            Debug.Log("Enemy sound error");

        }


        Destroy(gameObject,timeToDestroy);

    }

    
}
