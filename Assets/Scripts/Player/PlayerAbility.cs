using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour
{
    [Header("Datos del Animal")]
    public AnimalStats currentAnimal;

    [Header("Puntos de Referencia")]
    public Transform firePoint;
    public Transform shieldPoint;

    public bool isShieldActive { get; private set; }
    
    private bool abilityInUse = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement movement;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
        originalScale = transform.localScale;

        UpdateCharacterSprite();
    }

    void Update()
    {
        if (movement != null && movement.isStunned) return;
        HandleAbilityInput();
    }

    void UpdateCharacterSprite()
    {
        if (currentAnimal != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = currentAnimal.characterSprite;
        }
    }

    void HandleAbilityInput()
    {
        bool useAbilityPressed = (movement.playerID == 1 && Input.GetKeyDown(KeyCode.W)) ||
                                 (movement.playerID == 2 && Input.GetKeyDown(KeyCode.UpArrow));

        if (useAbilityPressed) ActivateAbility();
    }

    public void ActivateAbility()
    {
        if (abilityInUse || currentAnimal == null) return;

        switch (currentAnimal.tipoHabilidad)
        {
            case TipoHabilidad.LanzarHuevo: StartCoroutine(ChickenAbility()); break;
            case TipoHabilidad.EscudoOveja: StartCoroutine(SheepAbility()); break;
            case TipoHabilidad.EncogerRata: StartCoroutine(RatAbility()); break;
            case TipoHabilidad.FlotarPez: StartCoroutine(FishAbility()); break;
        }
    }

    IEnumerator ChickenAbility()
    {
        abilityInUse = true;
        GameObject activeObject = Instantiate(currentAnimal.abilityPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D eggRb = activeObject.GetComponent<Rigidbody2D>();

        if (eggRb != null) eggRb.velocity = Vector2.down * currentAnimal.abilityForce;

        yield return new WaitForSeconds(currentAnimal.abilityDuration); 
        abilityInUse = false;
    }

    IEnumerator SheepAbility()
    {
        abilityInUse = true;
        isShieldActive = true;
        GameObject activeObject = Instantiate(currentAnimal.abilityPrefab, shieldPoint.position, Quaternion.identity, transform);

        yield return new WaitForSeconds(currentAnimal.abilityDuration);

        if (activeObject != null) Destroy(activeObject);
        isShieldActive = false;
        abilityInUse = false;
    }

    IEnumerator RatAbility()
    {
        abilityInUse = true;
        float signX = Mathf.Sign(transform.localScale.x);
        float smallScale = originalScale.y * 0.5f;
        transform.localScale = new Vector3(signX * smallScale, smallScale, originalScale.z);

        yield return new WaitForSeconds(currentAnimal.abilityDuration);

        signX = Mathf.Sign(transform.localScale.x);
        transform.localScale = new Vector3(signX * originalScale.y, originalScale.y, originalScale.z);
        abilityInUse = false;
    }

    IEnumerator FishAbility()
    {
        abilityInUse = true;
        float savedGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;

        float timer = 0f;
        while (timer < currentAnimal.abilityDuration)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentAnimal.abilityForce);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = savedGravityScale;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        abilityInUse = false;
    }
}