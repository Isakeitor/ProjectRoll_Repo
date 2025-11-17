using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float visionRange = 15f;          // Distancia a la que ve al jugador
    public float fireRate = 1f;             // Disparos por segundo
    public Transform firePoint;             // Punto desde donde sale la bala
    public GameObject bulletPrefab;         // Prefab de la bala

    private Transform player;
    private float fireCooldown = 0f;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con tag 'Player'.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de visión
        if (distance <= visionRange)
        {
            // Mirar hacia el jugador
            Vector3 dir = (player.position - transform.position);
            dir.y = 0f; // Opcional: para que no incline la cabeza hacia arriba/abajo
            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 10f * Time.deltaTime);
            }

            // Disparo con cooldown
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                ShootAtPlayer();
                fireCooldown = 1f / fireRate;
            }
        }
    }

    private void ShootAtPlayer()
    {
        if (firePoint == null || bulletPrefab == null) return;

        // Dirección hacia el jugador desde el firePoint
        Vector3 targetPos = player.position;
        targetPos.y = firePoint.position.y; // iguala altura, dispara recto
        Vector3 dir = (targetPos - firePoint.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        Instantiate(bulletPrefab, firePoint.position, rot);
    }

    private void OnDrawGizmosSelected()
    {
        // Para ver el rango en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}