using UnityEngine;

public enum TipoHabilidad { LanzarHuevo, EscudoOveja, EncogerRata, FlotarPez }

[CreateAssetMenu(fileName = "NuevoAnimal", menuName = "Juego/Estadisticas de Animal")]
public class AnimalStats : ScriptableObject
{
    [Header("Información General")]
    public string animalName;
    public Sprite characterSprite;

    [Header("Configuración de Habilidad")]
    public TipoHabilidad tipoHabilidad;  
    public GameObject abilityPrefab;     
    public float abilityForce = 10f;     
    public float abilityDuration = 3f;   
}