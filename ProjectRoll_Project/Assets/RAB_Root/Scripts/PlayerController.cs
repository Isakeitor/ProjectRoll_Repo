using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Editor References")]
    public Rigidbody playerRb;
    public AudioSource playerAudio;

    [Header("Movement Parameters")]
    public float speed = 10f;
    private Vector2 moveInput;

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

    void Awake()
    {
        if (!playerRb) playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = false;   // la pelota debe poder rodar
    }

    void Start()
    {
        lives = 3;
        gameOverPanel.SetActive(false);
        UpdateLivesUI();
    }

    void Update()
    {
        if (transform.position.y <= fallLimit)
        {
            LoseLife();
            Respawn();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
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
            UpdateLivesUI();
        }
    }

    #region Movement

    void MovePlayer()
    {
        // FUERZA DE MOVIMIENTO TIPO PELOTA REAL
        Vector3 force = new Vector3(moveInput.x, 0, moveInput.y) * speed;
        playerRb.AddForce(force, ForceMode.Force);

        // LIMITADOR DE VELOCIDAD HORIZONTAL
        Vector3 horizontalVel = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
        float maxSpeed = 45f;
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            playerRb.linearVelocity = new Vector3(horizontalVel.x, playerRb.linearVelocity.y, horizontalVel.z);
        }
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySFX(0);
    }

    void Respawn()
    {
        // RESETEOS CRÍTICOS PARA EVITAR BUGS
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        playerRb.position = respawnPoint.position;
        isGrounded = true;

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
        UpdateLivesUI();
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

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    #endregion

    #region Audio

    public void PlaySFX(int soundToPlay)
    {
        if (playerAudio != null && soundCollection.Length > soundToPlay)
            playerAudio.PlayOneShot(soundCollection[soundToPlay]);
    }

    #endregion
}