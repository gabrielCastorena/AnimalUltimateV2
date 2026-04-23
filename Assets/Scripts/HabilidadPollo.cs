using UnityEngine;
using System.Collections;

public class HabilidadPollo : AnimalAbility
{
    [Header("Punto de Disparo")]
    public Transform firePoint;

    public override void ActivarHabilidad()
    {
        StartCoroutine(RutinaPollo());
    }

    IEnumerator RutinaPollo()
    {
        abilityInUse = true;
        
        if (stats.abilityPrefab != null && firePoint != null)
        {
            GameObject huevo = Instantiate(stats.abilityPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D eggRb = huevo.GetComponent<Rigidbody2D>();
            if (eggRb != null) eggRb.velocity = Vector2.down * stats.abilityForce;
        }

        yield return new WaitForSeconds(stats.abilityDuration); 
        abilityInUse = false;
    }
}