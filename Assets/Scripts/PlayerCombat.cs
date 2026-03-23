using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Referencias")]
    public Transform weaponPoint; 
    public Weapon armaActual;
    
    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (armaActual != null && movement != null && !movement.isStunned)
        {
            if ((movement.playerID == 1 && Input.GetKeyDown(KeyCode.F)) ||
                (movement.playerID == 2 && Input.GetKeyDown(KeyCode.RightControl)))
            {
                armaActual.UsarArma();
            }
        }
    }

    public void EquiparArma(Weapon nuevaArma)
    {
        if (armaActual != null) return; 
        
        armaActual = nuevaArma;
        armaActual.AlEquipar(weaponPoint, this); 
    }

    public void PerderArma()
    {
        armaActual = null;
    }
}