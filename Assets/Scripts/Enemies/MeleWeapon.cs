using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleWeapon : MonoBehaviour
{

    public bool isActive = false;
    // Start is called before the first frame update
    [SerializeField] private float attackSpeed = 1f; // Tiempo en segundos entre disparos
    private float nextAttackTime = 0f; // Tiempo en el que se puede realizar el siguiente disparo
    private bool canAttack = false;
    private GameObject player;
    private void Update()
    {
        if (Time.time >= nextAttackTime && canAttack)
        {

            //Hacer daño al jugador con funcion OnHit.
            Debug.Log("Atacaaaaaad");
            // Actualiza el tiempo en el que se puede realizar el siguiente disparo
            nextAttackTime = Time.time + 1f / attackSpeed;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isActive && collision.gameObject.tag == "Player")
        {
            canAttack = true;
            player = collision.gameObject;
            

        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canAttack = false;
        }
    }
}
