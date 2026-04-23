using UnityEngine;
using System.Collections;

public class HabilidadRata : AnimalAbility
{
    private Vector3 originalScale;

    protected override void Start()
    {
        base.Start(); // Ejecuta el Start de la clase padre primero
        originalScale = transform.localScale;
    }

    public override void ActivarHabilidad()
    {
        StartCoroutine(RutinaRata());
    }

    IEnumerator RutinaRata()
    {
        abilityInUse = true;
        
        float signX = Mathf.Sign(transform.localScale.x);
        float smallScale = originalScale.y * 0.5f;
        transform.localScale = new Vector3(signX * smallScale, smallScale, originalScale.z);

        yield return new WaitForSeconds(stats.abilityDuration);

        signX = Mathf.Sign(transform.localScale.x);
        transform.localScale = new Vector3(signX * originalScale.y, originalScale.y, originalScale.z);
        
        abilityInUse = false;
    }
}