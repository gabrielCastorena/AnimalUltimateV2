using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;
    private AnimalAbility habilidadActual; 

    public bool isShieldActive 
    { 
        get { return habilidadActual != null && habilidadActual.isShieldActive; } 
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        habilidadActual = GetComponent<AnimalAbility>();

        if (habilidadActual != null && habilidadActual.stats != null)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            if (habilidadActual.stats.visualPrefab != null)
            {
                // Instantiate crea el objeto, y al pasarle 'transform', lo hace hijo de este Player automáticamente
                GameObject visualInstance = Instantiate(habilidadActual.stats.visualPrefab, transform);
                
                visualInstance.transform.localPosition = Vector3.zero;
            }
        }
    }

    void Update()
    {
        if (movement != null && movement.isStunned) return;

        bool useAbilityPressed = (movement.playerID == 1 && Input.GetKeyDown(KeyCode.W)) ||
                                 (movement.playerID == 2 && Input.GetKeyDown(KeyCode.UpArrow));

        if (useAbilityPressed && habilidadActual != null && !habilidadActual.abilityInUse)
        {
            habilidadActual.ActivarHabilidad();
        }
    }
}