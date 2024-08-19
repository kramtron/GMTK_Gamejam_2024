using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetDmg : MonoBehaviour
{
    [SerializeField] GameObject blood;

    public void Hit()
    {
        Instantiate(blood, transform.position, Quaternion.identity);
        FindObjectOfType<PlayerDeath>().PlayerDie(gameObject);
    }

}
