using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody playerRb;
    public AudioSource playerAudio;

    [Header("Movement")]
    public float speed = 10f;
    private Vector2 moveInput;

    [Header("Jump")]
    public float jumpForce = 6f;
    public bool isGrounded = true;

    [Header("Respawn System")]
    public float fallLimit = -10f;
    public Transform respawnPoint;     // ← ESTE ES EL RESPWAN INICIAL Y SE USA SIEMPRE

    [Header("Lives")]
    public int lives = 3;
    public TMP_Text livesText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Sound")]
    public AudioClip[] soundCollection;

    void Awake()
    {
        if (!playerRb) playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = false; // la pelota debe poder rodar
    }

    void Start()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            // obtener componente CHECKPOINT y su transform asociado
            Checkpoint cp = other.GetComponent<Checkpoint>();
            if (cp != null && cp.respawnPoint != null)
            {
                // ASIGNAR el respawnPoint del jugador al transform del checkpoint (antes de desactivar)
                respawnPoint = cp.respawnPoint;
            }
            // desactivar pickup solo *después* de leer el respawnPoint
            other.gameObject.SetActive(false);
            PlaySFX(1);
        }
    }

    #region Movement

    void MovePlayer()
    {
        Vector3 force = new Vector3(moveInput.x, 0, moveInput.y) * speed;
        playerRb.AddForce(force, ForceMode.Force);

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
        // reset físico seguro
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        // usar transform.position para teletransportar y evitar problemas con physics-snap
        if (respawnPoint != null)
            transform.position = respawnPoint.position;
        else
            Debug.LogWarning("Respawn: respawnPoint null!");

        // sincronizar Rigidbody con la nueva posición (por si acaso)
        playerRb.position = transform.position;

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

    #region Lives

    public void LoseLife()
    {
        lives--;
        UpdateLivesUI();
        PlaySFX(2);

        if (lives <= 0)
            GameOver();
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    #endregion

    #region Audio

    public void PlaySFX(int index)
    {
        if (playerAudio != null && soundCollection.Length > index)
            playerAudio.PlayOneShot(soundCollection[index]);
    }

    #endregion
}
