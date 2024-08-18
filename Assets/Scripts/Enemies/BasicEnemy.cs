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
    [SerializeField] float attackStopDistance;
    [SerializeField] float attackMovementSpeed;

    [SerializeField] float wanderingStopDistance;
    [SerializeField] float wanderingMovementSpeed;



    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] float patrolPauseDuration = 2.0f;
    private int currentPatrolIndex = 0;

    EnemyState enemyState;


    NavMeshAgent agent;
    private bool isMovingToNextPoint;

    [SerializeField] GameObject blood;
    
    [SerializeField] GameObject wanderingCone;

    enum EnemyState
    {
        wandering,
        attacking

    };
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        target = GameObject.FindGameObjectWithTag("Player").transform;

        enemyState = EnemyState.wandering;

        agent.stoppingDistance = wanderingStopDistance;
        agent.speed = wanderingMovementSpeed;

    }

    // Update is called once per frame
    void Update()
    {

        UpdateRotation();

        UpdateState();

        //Quitar. Esto es solo para testing
        
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

    private void UpdateState()
    {
        switch (enemyState)
        {
            case EnemyState.wandering:
                Wander();

                break;
            case EnemyState.attacking:
                agent.SetDestination(target.position);
                wanderingCone.SetActive(false);

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
                        weapon.GetComponent<Weapon>().isActive = false; 
                    }
                    else
                    {
                        weapon.GetComponent<MeleWeapon>().isActive = false;
                    }
                }
                break;
            default:
                Debug.Log("Enemy State error!!");
                break;
        }

    }

    public void Kill()
    {
        Instantiate(blood, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            agent.stoppingDistance = attackStopDistance;
            agent.speed = attackMovementSpeed;

            enemyState = EnemyState.attacking;
        }
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if (collision.gameObject.tag == "Player")
        {
            agent.stoppingDistance = wanderingStopDistance;
            agent.speed = wanderingMovementSpeed;

            enemyState = EnemyState.wandering;
        }*/
    }
    
    private void StopEnemy()
    {
        agent.isStopped = true;
    }

    private void MoveEnemy()
    {
        agent.isStopped = false;    
    }

    private void Wander()
    {
        if (!isMovingToNextPoint && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            StartCoroutine(PatrolPause());
        }
    }

    private IEnumerator PatrolPause()
    {
        isMovingToNextPoint = true; // Evita que el enemigo intente moverse mientras está en pausa
        yield return new WaitForSeconds(patrolPauseDuration);
        SetNextPatrolPoint();
        isMovingToNextPoint = false; // Permite al enemigo moverse de nuevo
    }

    private void SetNextPatrolPoint()
    {
        if (patrolPoints.Count == 0) return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
}
