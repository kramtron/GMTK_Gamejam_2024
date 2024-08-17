using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // Prefab del proyectil
    [SerializeField] private Transform shootPoint; // Punto desde donde se disparará el proyectil
    [SerializeField] private float projectileSpeed = 10f; // Velocidad del proyectil
    [SerializeField] private float attackSpeed = 1f; // Tiempo en segundos entre disparos
    private Transform target; // El objetivo al que el arma debe apuntar

    public bool isActive = false; // Estado del arma
    private float nextAttackTime = 0f; // Tiempo en el que se puede realizar el siguiente disparo

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        // Comprueba si el arma está activa y si el jugador presiona el botón de disparo
        if (isActive)
        {
            // Dispara solo si el tiempo actual es mayor o igual al siguiente tiempo de ataque
            if (Time.time >= nextAttackTime)
            {
                Shoot();
                // Actualiza el tiempo en el que se puede realizar el siguiente disparo
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }

    private void Shoot()
    {
        if (projectilePrefab == null || shootPoint == null || target == null)
        {
            Debug.LogWarning("ProjectilePrefab, ShootPoint, or Target is not assigned.");
            return;
        }

        // Apunta el shootPoint hacia el target
        Vector3 direction = (target.position - shootPoint.position).normalized;
        shootPoint.up = direction;

        // Instancia el proyectil en la posición del punto de disparo
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        // Añade una fuerza al proyectil para que se mueva
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootPoint.up * projectileSpeed; // Mueve el proyectil en la dirección hacia adelante
        }
        else
        {
            Debug.LogWarning("Projectile does not have a Rigidbody2D component.");
        }
    }
}


