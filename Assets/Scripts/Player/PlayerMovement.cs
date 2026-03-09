using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int playerID = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Duck Game Knockback")]
    public float knockbackDuration = 1.5f; // Tiempo que pasa tirado en el piso
    private float knockbackCounter = 0f;   
    public float spinForce = 500f; // Fuerza masiva para que gire como loco

    private Rigidbody2D rb;
    private PlayerAbility abilities;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbility>();
    }

    void Update()
    {
        // Si el contador es mayor a cero, el jugador está aturdido/volando
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;

            // Si ya se acabó el tiempo de aturdimiento, se levanta
            if (knockbackCounter <= 0)
            {
                transform.rotation = Quaternion.identity; // Lo pone derecho
                rb.angularVelocity = 0f;                  // Frena el giro en seco
                rb.velocity = Vector2.zero;               // Lo frena para que no resbale
                rb.freezeRotation = true;                 // Vuelve a bloquear la rotación
            }
        }
        else
        {
            // Solo puede moverse, saltar y disparar si NO está volando/aturdido
            Move();
            Jump();
            UseAbility();
        }
    }

    void Move()
    {
        float moveInput = 0;

        if (playerID == 1)
        {
            if (Input.GetKey(KeyCode.A)) moveInput = -1;
            if (Input.GetKey(KeyCode.D)) moveInput = 1;
        }
        else if (playerID == 2)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;
            if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Jump()
    {
        if (playerID == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else if (playerID == 2)
        {
            if (Input.GetKeyDown(KeyCode.RightShift) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    void UseAbility()
    {
        if (abilities == null) return;

        if (playerID == 1 && Input.GetKeyDown(KeyCode.W))
        {
            abilities.ActivateAbility();
        }

        if (playerID == 2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            abilities.ActivateAbility();
        }
    }

    public void ApplyKnockback(Vector2 force)
    {
        knockbackCounter = knockbackDuration; 
        rb.velocity = Vector2.zero; // Frena su movimiento antes de volar
        
        // Magia Duck Game: Quitamos el candado de rotación
        rb.freezeRotation = false;
        
        // Lo mandamos a volar
        rb.AddForce(force, ForceMode2D.Impulse); 
        
        // Lo hacemos girar dándole torque a la izquierda o derecha al azar
        float randomSpin = Random.Range(-spinForce, spinForce);
        rb.AddTorque(randomSpin, ForceMode2D.Impulse);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}