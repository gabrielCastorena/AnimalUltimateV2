using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int playerID = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    // --- VARIABLES PARA EL EMPUJE Y GIRO ---
    public float knockbackDuration = 0.3f; 
    private float knockbackCounter = 0f;   
    public float spinForce = 15f; // NUEVO: Qué tan rápido va a girar en el aire

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
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;

            // NUEVO: Cuando se le pasa el aturdimiento, lo enderezamos
            if (knockbackCounter <= 0)
            {
                transform.rotation = Quaternion.identity; // Lo pone derechito al instante
                rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Le vuelve a poner el candado para que camine normal
            }
        }
        else
        {
            Move(); // Solo se mueve si no está aturdido
        }

        Jump();
        UseAbility();
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

    // --- FUNCIÓN ACTUALIZADA PARA RECIBIR EL IMPACTO Y GIRAR ---
    public void ApplyKnockback(Vector2 force)
    {
        knockbackCounter = knockbackDuration; 
        rb.velocity = Vector2.zero;           
        
        // 1. Le quitamos el candado de rotación Z para que las físicas lo dejen girar
        rb.constraints = RigidbodyConstraints2D.None;
        
        // 2. Lo mandamos a volar
        rb.AddForce(force, ForceMode2D.Impulse); 
        
        // 3. Le aplicamos una fuerza de rotación (Torque) aleatoria (a veces gira a la derecha, a veces a la izquierda)
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