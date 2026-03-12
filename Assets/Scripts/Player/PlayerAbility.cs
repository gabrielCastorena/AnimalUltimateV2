using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour
{
    [Header("Datos del Animal")]
    public AnimalStats currentAnimal;

    [Header("Puntos de Referencia")]
    public Transform firePoint;   // De donde sale el huevo
    public Transform shieldPoint; // Referencia opcional para el offset Y del escudo

    public bool isShieldActive = false;

    private bool abilityInUse = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    private GameObject activeObject; // Guarda el huevo o el escudo temporalmente

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        UpdateCharacterSprite();
    }

    void UpdateCharacterSprite()
    {
        if (currentAnimal != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = currentAnimal.characterSprite;
        }
    }

    public void ActivateAbility()
    {
        // Si no hay animal asignado o la habilidad ya se está usando, no hace nada
        if (abilityInUse || currentAnimal == null) return;

        // Leemos qué habilidad tiene configurada la tarjeta del animal
        switch (currentAnimal.tipoHabilidad)
        {
            case TipoHabilidad.LanzarHuevo:
                StartCoroutine(ChickenAbility());
                break;
            case TipoHabilidad.EscudoOveja:
                StartCoroutine(SheepAbility());
                break;
            case TipoHabilidad.EncogerRata:
                StartCoroutine(RatAbility());
                break;
            case TipoHabilidad.FlotarPez:
                StartCoroutine(FishAbility());
                break;
        }
    }

    // --- CORRUTINAS DE LAS HABILIDADES ---

    IEnumerator ChickenAbility()
    {
        abilityInUse = true;

        activeObject = Instantiate(currentAnimal.abilityPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D eggRb = activeObject.GetComponent<Rigidbody2D>();

        if (eggRb != null)
        {
            // Usamos la fuerza que configuraste en la tarjeta (ScriptableObject)
            eggRb.velocity = Vector2.down * currentAnimal.abilityForce;
        }

        while (activeObject != null)
            yield return null;

        abilityInUse = false;
    }

    IEnumerator SheepAbility()
    {
        abilityInUse = true;
        isShieldActive = true;

        activeObject = Instantiate(currentAnimal.abilityPrefab, shieldPoint.position, Quaternion.identity, transform);

        yield return new WaitForSeconds(currentAnimal.abilityDuration);

        if (activeObject != null)
            Destroy(activeObject);

        isShieldActive = false;
        abilityInUse = false;
    }

    IEnumerator RatAbility()
    {
        abilityInUse = true;

        // Leemos el signo actual de X para saber hacia dónde mira el personaje (1 = derecha, -1 = izquierda).
        // Multiplicamos por 0.5 para encoger SIN cambiar la dirección del sprite.
        float signX = Mathf.Sign(transform.localScale.x);
        float smallScale = originalScale.y * 0.5f;
        transform.localScale = new Vector3(signX * smallScale, smallScale, originalScale.z);

        yield return new WaitForSeconds(currentAnimal.abilityDuration);

        // Volvemos al tamaño original. Releemos el signo por si el jugador giró mientras era pequeño.
        signX = Mathf.Sign(transform.localScale.x);
        transform.localScale = new Vector3(signX * originalScale.y, originalScale.y, originalScale.z);

        abilityInUse = false;
    }

    IEnumerator FishAbility()
    {
        abilityInUse = true;

        float savedGravityScale = rb.gravityScale;
        rb.gravityScale = 0f; // Desactivamos la gravedad para que la fuerza ascendente sea precisa

        float timer = 0f;
        while (timer < currentAnimal.abilityDuration)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentAnimal.abilityForce);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = savedGravityScale;
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Limpiamos la velocidad vertical para una caída natural

        abilityInUse = false;
    }
}