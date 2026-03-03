using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("Ajustes de la Explosión")]
    public float lifeTime = 3f;           // Tiempo antes de explotar
    public float explosionRadius = 2.5f;  // Área de daño
    public float explosionForce = 15f;    // Fuerza con la que empuja a los jugadores

    void Start()
    {
        // En lugar de destruir el objeto directamente, llamamos a la función Explode después de 'lifeTime' segundos
        Invoke("Explode", lifeTime);
    }

void Explode()
    {
        Collider2D[] objectsInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D obj in objectsInRadius)
        {
            // Buscamos si el objeto impactado tiene el script de PlayerMovement
            PlayerMovement player = obj.GetComponent<PlayerMovement>();
            
            if (player != null)
            {
                // Calculamos la dirección del impacto
                Vector2 direction = (obj.transform.position - transform.position).normalized;
                
                // Truco de diseño: Le agregamos un poco de fuerza extra hacia arriba (eje Y) 
                // para que el personaje salte por los aires de forma más dramática
                direction.y += 0.5f; 
                
                // Llamamos a la nueva función mandando la fuerza de la explosión
                player.ApplyKnockback(direction.normalized * explosionForce);
            }
        }

        Destroy(gameObject);
    }

    // Esto dibuja un círculo rojo en el editor de Unity para que puedas ver el tamaño exacto de tu explosión
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}