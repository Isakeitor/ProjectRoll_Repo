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

    void Start()
    {
        points = 0;
        UpdatePointsText();
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
    }
}