using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración del Jugador")]
    public int playerID = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Físicas y Knockback")]
    public float knockbackDuration = 1.2f;
    public float spinForce = 150f;
    public float angularDragOnKnockback = 5f;
    public float gravityScaleOnKnockback = 2f;

    [Header("Efectos de Estado")]
    public float tiempoError = 0f;

    public bool isStunned { get; private set; } 
    private bool isGrounded;

    private Rigidbody2D rb;
    private Animator anim;

    private float knockbackCounter;
    private float originalAngularDrag;
    private float originalGravityScale;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalAngularDrag = rb.angularDrag;
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        HandleStunState();

        if (tiempoError > 0) tiempoError -= Time.deltaTime;

        if (!isStunned)
        {
            ReadInput();
            Jump();
        }
    }

    void FixedUpdate() 
    {
        if (!isStunned) ApplyMovement();
    }

    private void HandleStunState()
    {
        if (knockbackCounter > 0)
        {
            isStunned = true;
            knockbackCounter -= Time.deltaTime;
            if (anim != null) anim.SetBool("isWalking", false);

            if (knockbackCounter <= 0) RecoverFromStun();
        }
    }

    private void RecoverFromStun()
    {
        isStunned = false;
        transform.rotation = Quaternion.identity;
        rb.angularVelocity = 0f;
        rb.velocity = Vector2.zero;
        rb.freezeRotation = true;
        rb.angularDrag = originalAngularDrag;
        rb.gravityScale = originalGravityScale;
    }

    private void ReadInput()
    {
        moveInput = 0;
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

        // Si tiene el error del mazo, se invierten los controles
        if (tiempoError > 0) moveInput *= -1; 
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput != 0)
        {
            float s = Mathf.Abs(transform.localScale.y);
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * s, s, transform.localScale.z);
        }

        if (anim != null) anim.SetBool("isWalking", moveInput != 0);
    }

    private void Jump()
    {
        bool jumpPressed = (playerID == 1 && Input.GetKeyDown(KeyCode.Space)) ||
                           (playerID == 2 && Input.GetKeyDown(KeyCode.RightShift));

        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

    public void InvertirControles(float duracion)
    {
        tiempoError = duracion;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) isGrounded = false;
    }
}