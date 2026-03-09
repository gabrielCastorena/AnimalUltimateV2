using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("Ajustes de la Explosión")]
    public float lifeTime = 3f;           // Tiempo límite por si no choca con nadie
    public float explosionRadius = 2.5f;  
    public float explosionForce = 25f;    // Fuerza masiva para que salgan volando

    void Start()
    {
        Invoke("Explode", lifeTime);
    }

    // Si tu huevo tiene "Is Trigger" marcado en Unity:
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CancelInvoke("Explode"); // Cancela la cuenta regresiva
            Explode();               // Explota instantáneamente
        }
    }

    // Si tu huevo NO tiene "Is Trigger" y rebota de forma normal:
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CancelInvoke("Explode");
            Explode();
        }
    }

    void Explode()
    {
        Collider2D[] objectsInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D obj in objectsInRadius)
        {
            PlayerMovement player = obj.GetComponent<PlayerMovement>();
            
            if (player != null)
            {
                Vector2 direction = (obj.transform.position - transform.position).normalized;
                
                // Forzamos a que siempre salgan impulsados MUCHO hacia arriba (Arco dramático)
                direction.y = Mathf.Abs(direction.y) + 1.5f; 
                
                player.ApplyKnockback(direction.normalized * explosionForce);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}