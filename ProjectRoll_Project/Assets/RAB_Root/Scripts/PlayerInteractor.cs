using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Points System")]
    public int points; //Puntuación actual del player (en juego)
    public int winPoints = 1; //Puntuación a alcanzar para completar el nivel
    public TMP_Text pointsText; //Ref al texto de puntos para que cambie dinámicamente

    [Header("Scene Management")]
    public int sceneToLoad = 2;

    [Header("Sound References")]
    public PlayerController playerCont; //Ref al script que contiene las llamadas a sonidos

    [Header("Shield Power-Up")]
    public bool hasShield = false;              // ¿Tiene escudo activo?
    public GameObject shieldVisual;             // Opcional: objeto visual del escudo (una esfera, aura, etc.)

    void Start()
    {
        points = 0;
        UpdatePointsText();

        // Aseguramos que el visual del escudo está coherente con el estado
        if (shieldVisual != null)
            shieldVisual.SetActive(hasShield);
    }

    void Update()
    {
        if (points >= winPoints)
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        UpdatePointsText();
    }

    private void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = "Points: " + points.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // PICKUP DE PUNTOS
        if (other.gameObject.CompareTag("PickUp"))
        {
            points += 1;
            other.gameObject.SetActive(false);
            if (playerCont != null)
                playerCont.PlaySFX(1); //sonido de recoger puntos
        }

        // PICKUP DE ESCUDO
        if (other.gameObject.CompareTag("ShieldPickUp"))
        {
            TryPickupShield(other.gameObject);
        }
    }

    private void TryPickupShield(GameObject shieldItem)
    {
        if (hasShield)
        {
            // Ya tenemos escudo, ignoramos el pickup
            // Aquí podrías poner un sonido de "error" si quieres
            Debug.Log("Ya tienes un escudo activo, no puedes coger otro.");
            return;
        }

        // Activamos escudo
        ActivateShield();

        // Desactivamos el objeto del mundo
        shieldItem.SetActive(false);

        if (playerCont != null)
            playerCont.PlaySFX(2); //sonido de pickup de escudo (elige otro índice)
    }

    public void ActivateShield()
    {
        hasShield = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        Debug.Log("Escudo activado.");
    }
}

    /// <summary>
    /// Intenta usar el escudo para bloquear un golpe.
    /// Devuelve true si el golpe ha sido bloqueado y el escudo se ha gastado. (26 líneas restantes)