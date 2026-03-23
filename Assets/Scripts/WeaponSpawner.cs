using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("Armas Posibles")]
    public GameObject[] armasPrefabs; 

    [Header("Ajustes de Flotación")]
    public float alturaSpawn = 0.5f;      
    public float amplitudFlotacion = 0.2f; 
    public float velocidadFlotacion = 2f;  

    private GameObject armaGenerada;
    private Vector3 posicionBase;

    public void GenerarArmaAleatoria()
    {
        if (armasPrefabs.Length == 0) return;

        int index = Random.Range(0, armasPrefabs.Length);
        posicionBase = transform.position + Vector3.up * alturaSpawn;
        
        armaGenerada = Instantiate(armasPrefabs[index], posicionBase, Quaternion.identity);
        armaGenerada.transform.SetParent(transform);
    }

    void Update()
    {
        if (armaGenerada != null && armaGenerada.transform.parent == transform)
        {
            float nuevoY = posicionBase.y + Mathf.Sin(Time.time * velocidadFlotacion) * amplitudFlotacion;
            armaGenerada.transform.position = new Vector3(posicionBase.x, nuevoY, posicionBase.z);
        }
    }
}