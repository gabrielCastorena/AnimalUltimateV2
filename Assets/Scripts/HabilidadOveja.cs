using UnityEngine;
using System.Collections;

public class HabilidadOveja : AnimalAbility
{
    [Header("Punto de Escudo")]
    public Transform shieldPoint;

    public override void ActivarHabilidad()
    {
        StartCoroutine(RutinaOveja());
    }

    IEnumerator RutinaOveja()
    {
        abilityInUse = true;
        isShieldActive = true;
        
        GameObject escudo = null;
        if (stats.abilityPrefab != null && shieldPoint != null)
        {
            escudo = Instantiate(stats.abilityPrefab, shieldPoint.position, Quaternion.identity, transform);
        }

        yield return new WaitForSeconds(stats.abilityDuration);

        if (escudo != null) Destroy(escudo);
        isShieldActive = false;
        abilityInUse = false;
    }
}