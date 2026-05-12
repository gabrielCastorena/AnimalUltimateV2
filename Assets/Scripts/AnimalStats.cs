using UnityEngine;

[CreateAssetMenu(fileName = "NuevaStatAnimal", menuName = "Stats/Animal Individual")]
public class AnimalStats : ScriptableObject 
{
    [Header("Información General")]
    public string animalName;
    
    public GameObject visualPrefab; 
    
    [Header("Configuración de Habilidad")]
    public GameObject abilityPrefab;
    public float abilityForce;
    public float abilityDuration;
}