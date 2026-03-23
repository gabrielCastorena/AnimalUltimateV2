using UnityEngine;

public class CursorHazard : MonoBehaviour
{
    [Header("Movimiento Aleatorio")]
    public float velocidad = 2f;
    public Vector2 limitesMin = new Vector2(-8f, -4f); 
    public Vector2 limitesMax = new Vector2(8f, 4f);   

    private Vector3 objetivo;

    void Start() { ElegirNuevoObjetivo(); }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivo) < 0.1f)
        {
            ElegirNuevoObjetivo();
        }
    }

    void ElegirNuevoObjetivo()
    {
        float randomX = Random.Range(limitesMin.x, limitesMax.x);
        float randomY = Random.Range(limitesMin.y, limitesMax.y);
        objetivo = new Vector3(randomX, randomY, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat combat = collision.GetComponent<PlayerCombat>();
            
            if (combat != null && combat.armaActual != null)
            {
                GameObject armaADestruir = combat.armaActual.gameObject;
                combat.PerderArma(); 
                Destroy(armaADestruir);
            }
        }
    }
}