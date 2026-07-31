using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody playerRb;

    [Header("Movement")]
    public float speed = 10f;
    private Vector2 moveInput;

    [Header("Jump")]
    public float jumpForce = 6f;
    public bool isGrounded = true;

    [Header("Respawn System")]
    public float fallLimit = -10f;
    public Transform respawnPoint;

    [Header("Lives")]
    public int lives = 3;
    public TMP_Text livesText;

    [Header("Damage")]
    public float damageCooldown = 1f;
    private float lastDamageTime = -1f;

    [Header("Scene Management")]
    public int gameOverScene = 0;

    private bool isDead = false;

    void Awake()
    {
        if (!playerRb) playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = false;
        UpdateLivesUI();
    }

    void Update()
    {
        if (isDead) return;

        if (transform.position.y <= fallLimit)
        {
            LoseLife();
            Respawn();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
            MovePlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

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

            AudioManager.Instance.PlayBonus();
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
        AudioManager.Instance.PlayJump();
    }

    void Respawn()
    {
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        playerRb.position = transform.position;

        isGrounded = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isDead)
            moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isDead && context.performed && isGrounded)
        {
            isGrounded = false;
            Jump();
        }
    }

    #endregion

    #region Lives

    public void LoseLife()
    {
        if (isDead) return;

        if (Time.time - lastDamageTime < damageCooldown)
            return;

        lastDamageTime = Time.time;

        lives--;
        UpdateLivesUI();

        if (lives <= 0)
            GameOver();
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = ": " + lives;
    }

    void GameOver()
    {
        isDead = true;
        SceneManager.LoadScene(gameOverScene);
    }

    #endregion
}