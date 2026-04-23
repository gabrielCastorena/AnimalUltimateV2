using UnityEngine;

[CreateAssetMenu(fileName = "NuevaArma", menuName = "Juego/Estadisticas de Arma")]
public class WeaponStats : ScriptableObject
{
    [Header("Datos Generales")]
    public string nombreArma = "Arma Nueva";
    public int usosMaximos = 3;
    public float fuerzaGolpe = 20f;
    // Si mañana quieres agregar un ícono para la UI del inventario, lo pondrías aquí
}