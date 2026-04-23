using UnityEngine;
using System.Collections;

// Clase abstracta: Obliga a todos los animales a tener la función ActivarHabilidad
public abstract class AnimalAbility : MonoBehaviour
{
    [Header("Estadísticas")]
    public AnimalStats stats;

    protected PlayerMovement movement;
    protected Rigidbody2D rb;
    
    // Variables públicas para que PlayerAbility pueda leerlas
    public bool isShieldActive { get; protected set; }
    public bool abilityInUse { get; protected set; }

    protected virtual void Start()
    {
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // El polimorfismo: Cada animal programará su propia magia aquí
    public abstract void ActivarHabilidad();
}