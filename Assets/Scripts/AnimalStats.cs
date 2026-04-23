using UnityEngine;

[CreateAssetMenu(fileName = "NuevaStatAnimal", menuName = "Stats/Animal Individual")]
// El nombre de la clase ahora coincide con el archivo AnimalStats.cs
public class AnimalStats : ScriptableObject 
{
    [Header("Información General")]
    public string animalName;
    
    // ANTES: public Sprite characterSprite;
    // AHORA: Guardamos el PREFAB completo (la TortugaNinja con su Animator)
    public GameObject visualPrefab; 
    
    [Header("Configuración de Habilidad")]
    public GameObject abilityPrefab;
    public float abilityForce;
    public float abilityDuration;
}