using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Editor References")]
    public Rigidbody playerRb; //Referencia al Rigidbody del player
    public AudioSource playerAudio; //Ref al emisor de sonidos del player

    [Header("Movement Parameters")]
    public float speed = 10;
    public Vector2 moveInput; //Almacén del input de movimiento de los periféricos que usamos para jugar

    [Header("Jump Parameters")]
    public float jumpForce = 6;
    public bool isGrounded = true;

    [Header("Respawn System")]
    public float fallLimit = -10;
    public Transform respawnPoint;

    [Header("Sound Configuration")]
    public AudioClip[] soundCollection;

    [Header("Lives System")]
    public int lives = 3;
    public TMP_Text livesText; //Texto para mostrar vidas

    [Header("Game Over")]
    public GameObject gameOverPanel; //Texto de "Has perdido"


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lives = 3;
        lives = 3;
        gameOverPanel.SetActive(false); //Oculta el panel al inicio
    }

    // Update is called once per frame
    void Update()
    {
        //CinematicMovement();
        //Respawn por altura
        if (transform.position.y <= fallLimit)
        {
            lives -= 1; ;
            Respawn();
        }
        livesText.text = "Lives: " + lives;
    }

    private void FixedUpdate()
    {
        //Update para calcular movimientos físicos
        PhysicalMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; //Devuelve la capacidad de saltar
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            lives -= 1;
            Respawn();
        }
        if (collision.gameObject.CompareTag("Health"))
        {
            lives += 1;
            collision.gameObject.SetActive(false);
        }
    }


    void CinematicMovement()
    {
        //Movimiento = (Dirección * velocidad * input)
        //Necesitais multiplicar el movimiento por Time.deltaTime
        transform.Translate(Vector3.right * speed * moveInput.x * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * moveInput.y * Time.deltaTime);
    }

    void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;
        PlaySFX(2);

        if (lives <= 0)
        {
            GameOver(); //Llama a GameOver() cuando las vidas son 0
        }
    }

    void GameOver() //Muestra el panel y pausa el juego
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void PhysicalMovement()
    {
        //Añadir una fuerza al rigidbody = (Dirección * velocidad * input)
        //playerRb.linearVelocity = new Vector3 (moveInput.x * speed, playerRb.linearVelocity.y, moveInput.y * speed);
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
        //Sustituir el transform.position del player por el del punto de respawn
        transform.position = respawnPoint.position;
        //Resetear el valor de aceleración del rigidbody
        playerRb.linearVelocity = new Vector3(0,0,0);
        PlaySFX(2);
    }

    public void PlaySFX(int soundToPlay)
    {
        playerAudio.PlayOneShot(soundCollection[soundToPlay]);
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) 
    {

        if (context.performed && isGrounded == true)
        {
            isGrounded = false;
            Jump();
        }
    }




    #endregion
}
