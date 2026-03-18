using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int playerID = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Duck Game Knockback")]
    [Tooltip("Segundos que el personaje permanece aturdido antes de recuperarse.")]
    public float knockbackDuration = 1.2f;
    [Tooltip("Impulso de rotación inicial. El angularDrag lo frenará naturalmente.")]
    public float spinForce = 150f;
    [Tooltip("Cuánto frena el aire el giro durante el knockback. 0 = gira infinito.")]
    public float angularDragOnKnockback = 5f;
    [Tooltip("Escala de gravedad durante el vuelo para dar sensación de peso.")]
    public float gravityScaleOnKnockback = 2f;

    private float knockbackCounter = 0f;
    private float originalAngularDrag;
    private float originalGravityScale;

    private Rigidbody2D rb;
    private PlayerAbility abilities;
    private bool isGrounded;
    
    private Animator anim; // NUEVO: La variable para guardar tu cerebro de animación

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbility>();
        anim = GetComponent<Animator>(); // NUEVO: Conectamos el componente
        
        originalAngularDrag = rb.angularDrag;
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        // Si el contador es mayor a cero, el jugador está aturdido/volando
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;
            
            // NUEVO: Si está volando por un golpe, apagamos la animación de caminar
            if (anim != null) anim.SetBool("isWalking", false); 

            if (knockbackCounter <= 0)
            {
                transform.rotation = Quaternion.identity;
                rb.angularVelocity = 0f;
                rb.velocity = Vector2.zero;
                rb.freezeRotation = true;
                rb.angularDrag = originalAngularDrag;
                rb.gravityScale = originalGravityScale;
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

        // Usamos localScale.y como referencia de tamaño actual.
        float s = Mathf.Abs(transform.localScale.y); 
        if (moveInput > 0)
            transform.localScale = new Vector3(s, s, transform.localScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-s, s, transform.localScale.z);

        // NUEVO: Encender o apagar la animación dependiendo de si hay input
        if (anim != null)
        {
            anim.SetBool("isWalking", moveInput != 0);
        }
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
        rb.velocity = Vector2.zero;
        rb.freezeRotation = false;

        rb.gravityScale = gravityScaleOnKnockback;
        rb.angularDrag = angularDragOnKnockback;

        rb.AddForce(force, ForceMode2D.Impulse);

        float torqueDirection = -Mathf.Sign(force.x);
        rb.AddTorque(torqueDirection * spinForce, ForceMode2D.Impulse);
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