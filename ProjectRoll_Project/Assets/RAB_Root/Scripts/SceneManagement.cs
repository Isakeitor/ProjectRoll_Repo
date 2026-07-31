using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void PlayButtonClick()
    {
        AudioManager.Instance.PlayButtonClick();
    }

    public void ExitGame()
    {
        Debug.Log("Has cerrado el juego.");
        Application.Quit();
    }
}