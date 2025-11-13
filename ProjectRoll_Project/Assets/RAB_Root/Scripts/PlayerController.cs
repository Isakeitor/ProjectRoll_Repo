using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Editor References")]
    public Rigidbody playerRb;
    public AudioSource playerAudio;

    [Header("Movement Parameters")]
    public float speed = 10f;
    public Vector2 moveInput;

    [Header("Jump Parameters")]
    public float jumpForce = 6f;
    public bool isGrounded = true;

    [Header("Respawn System")]
    public float fallLimit = -10f;
    public Transform respawnPoint;

    [Header("Sound Configuration")]
    public AudioClip[] soundCollection;

    [Header("Lives System")]
    public int lives = 3;
    public TMP_Text livesText;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    void Start()
    {
        lives = 3;
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // Respawn por caída
        if (transform.position.y <= fallLimit)
        {
            LoseLife();
            Respawn();
        }

        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    private void FixedUpdate()
    {
        PhysicalMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            LoseLife();
            Respawn();
        }

        if (collision.gameObject.CompareTag("Health"))
        {
            lives++;
            collision.gameObject.SetActive(false);
        }
    }

    #region Movement

    void PhysicalMovement()
    {
        playerRb.AddForce(Vector3.right * speed * moveInput.x, ForceMode.VelocityChange);
        playerRb.AddForce(Vector3.forward * speed * moveInput.y, ForceMode.VelocityChange);
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySFX(0);
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        playerRb.linearVelocity = Vector3.zero;
        PlaySFX(2);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            isGrounded = false;
            Jump();
        }
    }

    #endregion

    #region Lives and Game Over

    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;
        PlaySFX(2);

        if (lives <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    #endregion

    #region Audio

    public void PlaySFX(int soundToPlay)
    {
        playerAudio.PlayOneShot(soundCollection[soundToPlay]);
    }

    #endregion
}