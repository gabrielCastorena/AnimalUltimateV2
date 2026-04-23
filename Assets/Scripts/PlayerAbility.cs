using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;
    private AnimalAbility habilidadActual; // El cerebro hijo

    // Truco: Leer el escudo desde la habilidad actual para no romper Weapon.cs
    public bool isShieldActive 
    { 
        get { return habilidadActual != null && habilidadActual.isShieldActive; } 
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Busca automáticamente el script de habilidad que le pusiste al personaje
        habilidadActual = GetComponent<AnimalAbility>();

        // Actualizamos el visual al iniciar
        if (habilidadActual != null && habilidadActual.stats != null)
        {
            // 1. Apagamos el SpriteRenderer original del Player para que no estorbe
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            // 2. Instanciamos el nuevo Prefab animado (La Tortuga Ninja)
            if (habilidadActual.stats.visualPrefab != null)
            {
                // Instantiate crea el objeto, y al pasarle 'transform', lo hace hijo de este Player automáticamente
                GameObject visualInstance = Instantiate(habilidadActual.stats.visualPrefab, transform);
                
                // Nos aseguramos de que quede perfectamente centrado
                visualInstance.transform.localPosition = Vector3.zero;
            }
        }
    }

    void Update()
    {
        if (movement != null && movement.isStunned) return;

        bool useAbilityPressed = (movement.playerID == 1 && Input.GetKeyDown(KeyCode.W)) ||
                                 (movement.playerID == 2 && Input.GetKeyDown(KeyCode.UpArrow));

        // Si presionas el botón, le delegamos el trabajo a la habilidad específica
        if (useAbilityPressed && habilidadActual != null && !habilidadActual.abilityInUse)
        {
            habilidadActual.ActivarHabilidad();
        }
    }
}