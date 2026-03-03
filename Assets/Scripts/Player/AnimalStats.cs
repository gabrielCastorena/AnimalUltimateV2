using UnityEngine;

// Lista de las habilidades disponibles en el juego
public enum TipoHabilidad 
{ 
    LanzarHuevo, 
    EscudoOveja, 
    EncogerRata, 
    FlotarPez 
}

[CreateAssetMenu(fileName = "NuevoAnimal", menuName = "Juego/Estadisticas de Animal")]
public class AnimalStats : ScriptableObject
{
    [Header("Información General")]
    public string animalName;
    public Sprite characterSprite;

    [Header("Configuración de Habilidad")]
    public TipoHabilidad tipoHabilidad;  // El menú desplegable mágico
    public GameObject abilityPrefab;     // Úsalo para el Huevo o el Escudo
    public float abilityForce = 10f;     // Fuerza del huevo o del impulso del pez
    public float abilityDuration = 3f;   // Duración de la rata, pez o escudo
}