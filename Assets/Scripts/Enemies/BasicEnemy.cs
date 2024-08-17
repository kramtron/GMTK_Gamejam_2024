using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{

    private Transform target;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] GameObject weapon;
    [SerializeField] bool isMelee = false;    
   


    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);

       UpdateRotation();

        // Comprueba si el agente se ha detenido
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.isStopped)
            {
                StopEnemy(); // Detiene el movimiento del enemigo
            }

            if (!isMelee)
            {
                weapon.GetComponent<Weapon>().isActive = true; // Activa el arma
            }
            else
            {
                weapon.GetComponent<MeleWeapon>().isActive = true;
            }
        }
        else
        {
            if (agent.isStopped)
            {
                MoveEnemy(); // Reactiva el movimiento del enemigo
            }

            if (!isMelee)
            {
                weapon.GetComponent<Weapon>().isActive = false; // Activa el arma
            }
            else
            {
                weapon.GetComponent<MeleWeapon>().isActive = false;
            }
        }

    }

    private void UpdateRotation()
    {
        // Verifica si el agente tiene un camino
        if (agent.pathPending || agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            return;
        }

        // Obtén el siguiente punto en el camino
        if (agent.path.corners.Length < 2)
        {
            return;
        }

        Vector3 nextCorner = agent.path.corners[1];
        Vector3 direction = (nextCorner - transform.position).normalized;

        // Solo rota si la dirección no es cero para evitar errores
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        StopEnemy();

        weapon.GetComponent<Weapon>().isActive = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        weapon.GetComponent<Weapon>().isActive = false;

        MoveEnemy();
    }
    */
    private void StopEnemy()
    {
        agent.isStopped = true;
    }

    private void MoveEnemy()
    {
        agent.isStopped = false;    
    }
}
