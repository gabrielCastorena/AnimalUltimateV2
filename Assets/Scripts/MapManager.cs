using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [Header("Configuración del Mapa")]
    public int spawnersMaximos = 3; 

    void Start()
    {
        WeaponSpawner[] todosLosSpawners = FindObjectsOfType<WeaponSpawner>();
        List<WeaponSpawner> listaSpawners = new List<WeaponSpawner>(todosLosSpawners);
        
        for (int i = 0; i < listaSpawners.Count; i++)
        {
            WeaponSpawner temp = listaSpawners[i];
            int randomIndex = Random.Range(i, listaSpawners.Count);
            listaSpawners[i] = listaSpawners[randomIndex];
            listaSpawners[randomIndex] = temp;
        }

        int cantidadAActivar = Mathf.Min(spawnersMaximos, listaSpawners.Count);
        for (int i = 0; i < cantidadAActivar; i++)
        {
            listaSpawners[i].GenerarArmaAleatoria();
        }
    }
}