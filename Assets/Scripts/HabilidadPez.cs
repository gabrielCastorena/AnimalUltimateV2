using UnityEngine;
using System.Collections;

public class HabilidadPez : AnimalAbility
{
    public override void ActivarHabilidad()
    {
        StartCoroutine(RutinaPez());
    }

    IEnumerator RutinaPez()
    {
        abilityInUse = true;
        
        float savedGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;

        float timer = 0f;
        while (timer < stats.abilityDuration)
        {
            rb.velocity = new Vector2(rb.velocity.x, stats.abilityForce);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = savedGravityScale;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        
        abilityInUse = false;
    }
}