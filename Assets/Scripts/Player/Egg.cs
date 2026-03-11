using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("Ajustes de la Explosión")]
    public float lifeTime = 3f;
    public float explosionRadius = 2.5f;
    public float explosionForce = 25f;

    [Header("Duck Game Dirección")]
    [Tooltip("Cuánto se amplifica el componente horizontal. Más alto = más parecido a Duck Game.")]
    public float horizontalBias = 3f;
    [Tooltip("Bump vertical fijo para despegar del suelo. Pequeño pero siempre presente.")]
    public float verticalBump = 0.5f;

    void Start()
    {
        Invoke("Explode", lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Shield"))
        {
            CancelInvoke("Explode");
            Explode();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shield"))
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
            if (player == null) continue;

            PlayerAbility ability = obj.GetComponent<PlayerAbility>();
            if (ability != null && ability.isShieldActive)
            {
                Debug.Log(obj.name + ": ¡La burbuja absorbió el impacto!");
                continue;
            }

            float dirX = obj.transform.position.x - transform.position.x;
            float horizontalComponent = Mathf.Abs(dirX) < 0.1f
                ? (Random.value > 0.5f ? 1f : -1f)
                : Mathf.Sign(dirX);

            Vector2 duckDir = new Vector2(horizontalComponent * horizontalBias, verticalBump);
            player.ApplyKnockback(duckDir.normalized * explosionForce);
        }

        Destroy(gameObject);
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}