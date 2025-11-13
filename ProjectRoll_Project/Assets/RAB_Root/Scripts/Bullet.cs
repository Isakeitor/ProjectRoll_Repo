using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 1; // número de vidas a restar si no hay escudo

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
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
                    controller.LoseLife(); // resta vida y actualiza UI
                }
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}