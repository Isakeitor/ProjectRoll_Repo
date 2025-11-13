using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    [Header("Escudo")]
    public float shieldDuration = 5f;      // Duración del escudo en segundos
    public GameObject shieldPrefab;        // Prefab visual del escudo

    private void OnTriggerEnter(Collider other)
    {
        // Solo aplica si el jugador recoge el pick-up
        if (other.CompareTag("Player"))
        {
            PlayerInteractor interactor = other.GetComponent<PlayerInteractor>();
            if (interactor != null)
            {
                interactor.ActivateShield(); // Llama al método que ya tienes en PlayerInteractor
            }

            // Destruye el pick-up
            Destroy(gameObject);
        }
    }
}
