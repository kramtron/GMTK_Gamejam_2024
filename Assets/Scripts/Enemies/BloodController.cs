using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float timeToDestroy = 0.5f;
    [SerializeField] AudioSource sound;
    void Start()
    {
        sound.Play();
        Destroy(gameObject,timeToDestroy);
    }

    
}
