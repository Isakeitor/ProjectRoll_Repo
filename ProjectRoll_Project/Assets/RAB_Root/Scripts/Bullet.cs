using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 1; // número de vidas a restar si no hay escudo

    private bool hasHit = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        GameObject other = collision.gameObject;

        if (other.CompareTag("Player"))
        {
            // Primero consultamos PlayerInteractor
            PlayerInteractor interactor = other.GetComponent<PlayerInteractor>();
            bool blocked = false;

            if (interactor != null)
            {
                //blocked = interactor.TryUseShield();
            }

            // Si no estaba bloqueado por el escudo, aplicamos daño
            if (!blocked)
            {
                PlayerController controller = other.GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.LoseLife();
                }
            }

            Destroy(gameObject);
        }
        else if (!other.CompareTag("Ground") && !other.CompareTag("Obstacle"))
        {
            // Destruye la bala al tocar cualquier cosa excepto el suelo y los obstáculos
            Destroy(gameObject);
        }
        else
        {
            // Permite que la bala siga atravesando suelo y obstáculos
            hasHit = false;
        }
    }
}