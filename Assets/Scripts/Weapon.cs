using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Estadísticas Base")]
    public WeaponStats stats; // Aquí conectaremos el ScriptableObject

    
    protected int usosRestantes;  
    protected bool haSidoRecogida = false;
    protected bool estaAtacando = false; 
    
    protected Collider2D col;
    protected Rigidbody2D rb;
    protected Transform manoDelJugador;
    protected PlayerCombat dueño;

    protected virtual void Awake() 
    { 
        col = GetComponent<Collider2D>(); 
        rb = GetComponent<Rigidbody2D>();
        
        if (stats != null) usosRestantes = stats.usosMaximos; 
        
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.isTrigger = true; 
    }

    // Todas las armas se recogen igual
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!haSidoRecogida && !estaAtacando)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerCombat combat = collision.GetComponent<PlayerCombat>();
                if (combat != null && combat.armaActual == null)
                {
                    combat.EquiparArma(this);
                }
            }
        }
    }

    public virtual void AlEquipar(Transform mano, PlayerCombat nuevoDueño)
    {
        haSidoRecogida = true;
        estaAtacando = false;
        if (col != null) col.enabled = false; 
        
        if (rb != null) { rb.isKinematic = true; rb.velocity = Vector2.zero; }
        
        manoDelJugador = mano;
        dueño = nuevoDueño;

        transform.SetParent(mano);
        transform.localPosition = Vector3.zero; 
        transform.localRotation = Quaternion.identity;
    }

    public abstract void UsarArma();

    protected void ConsumirUso()
    {
        usosRestantes--; 
        if (usosRestantes <= 0)
        {
            if (dueño != null) dueño.PerderArma(); 
            Destroy(gameObject); 
        }
        else
        {
            estaAtacando = false;
            if (col != null) col.enabled = false;
            
            if (transform.parent == null) 
            {
                transform.SetParent(manoDelJugador);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
    }
}