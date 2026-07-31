using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Points System")]
    public int points;
    public int winPoints = 10;
    public TMP_Text pointsText;

    [Header("Scene Management")]
    public int sceneToLoad = 2;

    void Start()
    {
        points = 0;
        UpdateObjectiveText();
    }

    void Update()
    {
        if (points >= winPoints)
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        UpdateObjectiveText();
    }

    private void UpdateObjectiveText()
    {
        if (pointsText != null)
        {
            int remaining = Mathf.Max(0, winPoints - points);
            pointsText.text = "Collect " + remaining + " red cards";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            points++;
            other.gameObject.SetActive(false);

            AudioManager.Instance.PlayPickup();

            UpdateObjectiveText();
        }
    }
}