using UnityEngine;
using System.Collections;

public class WeaponMazo : Weapon
{
    [Header("Ajustes: Mazo Error")]
    public float radioGolpe = 1.2f;      
    public float duracionError = 3f;     

    public override void UsarArma()
    {
        if (estaAtacando) return; 
        StartCoroutine(RutinaMazo());
    }

    IEnumerator RutinaMazo()
    {
        estaAtacando = true;

        float direccionX = Mathf.Sign(manoDelJugador.lossyScale.x);
        transform.localRotation = Quaternion.Euler(0, 0, -90 * direccionX);

        Collider2D[] golpeados = Physics2D.OverlapCircleAll(transform.position, radioGolpe);
        foreach (Collider2D objetivo in golpeados)
        {
            if (objetivo.CompareTag("Player") && objetivo.gameObject != dueño.gameObject)
            {
                PlayerAbility hab = objetivo.GetComponent<PlayerAbility>();
                if (hab != null && hab.isShieldActive) continue; 

                PlayerMovement victima = objetivo.GetComponent<PlayerMovement>();
                if (victima != null && stats != null)
                {
                    float dirX = Mathf.Sign(victima.transform.position.x - transform.position.x);
                    victima.ApplyKnockback(new Vector2(dirX * 1.5f, 1f).normalized * stats.fuerzaGolpe);
                    victima.InvertirControles(duracionError);
                }
            }
        }

        yield return new WaitForSeconds(0.2f);
        transform.localRotation = Quaternion.identity;
        ConsumirUso();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioGolpe);
    }
}