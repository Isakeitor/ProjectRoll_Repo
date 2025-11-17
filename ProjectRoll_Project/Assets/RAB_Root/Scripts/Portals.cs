using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal destino;
    public bool puedeTeletransportar = true;

    private void OnTriggerEnter(Collider other)
    {
        if (puedeTeletransportar && destino != null && other.CompareTag("Player"))
        {
            destino.puedeTeletransportar = false;
            other.transform.position = destino.transform.position + destino.transform.forward * 2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        puedeTeletransportar = true;
    }
}