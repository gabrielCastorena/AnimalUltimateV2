using UnityEngine;
using System.Collections;

public class WeaponBumeran : Weapon
{
    [Header("Ajustes: Bumerán")]
    public float velocidadLanzamiento = 15f;
    public float tiempoDeIda = 0.4f;
    public float velocidadGiro = 800f;

    public override void UsarArma()
    {
        if (estaAtacando) return; 
        StartCoroutine(RutinaBumeran());
    }

    // El bumerán necesita extender la función de choque para hacer daño al volar
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Ejecuta la recolección de la clase padre

        if (estaAtacando && collision.CompareTag("Player") && collision.gameObject != dueño.gameObject)
        {
            PlayerAbility hab = collision.GetComponent<PlayerAbility>();
            if (hab != null && hab.isShieldActive) return; 

            PlayerMovement victima = collision.GetComponent<PlayerMovement>();
            if (victima != null && stats != null)
            {
                float dirX = Mathf.Sign(victima.transform.position.x - transform.position.x);
                Vector2 fuerza = new Vector2(dirX * 1.5f, 1f).normalized * stats.fuerzaGolpe;
                victima.ApplyKnockback(fuerza);
            }
        }
    }

    IEnumerator RutinaBumeran()
    {
        estaAtacando = true;
        if (col != null) { col.enabled = true; col.isTrigger = true; }
        transform.SetParent(null);

        float direccionX = Mathf.Sign(manoDelJugador.lossyScale.x);
        Vector3 direccionVuelo = new Vector3(direccionX, 0, 0);

        float tiempo = 0;
        while (tiempo < tiempoDeIda)
        {
            transform.position += direccionVuelo * velocidadLanzamiento * Time.deltaTime;
            transform.Rotate(0, 0, velocidadGiro * Time.deltaTime * -direccionX);
            tiempo += Time.deltaTime;
            yield return null; 
        }

        while (Vector3.Distance(transform.position, manoDelJugador.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, manoDelJugador.position, velocidadLanzamiento * Time.deltaTime);
            transform.Rotate(0, 0, velocidadGiro * Time.deltaTime * -direccionX);
            yield return null;
        }

        ConsumirUso();
    }
}