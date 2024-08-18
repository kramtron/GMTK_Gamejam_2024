using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField] GameObject blood;

    public void Kill()
    {
        Instantiate(blood, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
