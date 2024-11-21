using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    public Color fogColor = Color.gray;
    public float fogDensity = 0.1f;

    void Start()
    {
        // Obtén el BoxCollider del objeto
        BoxCollider collider = GetComponent<BoxCollider>();

        if (collider != null)
        {
            // Ajusta el tamaño del BoxCollider según el tamaño del tile
            collider.size = new Vector3(50f, 50f, 50f);
            collider.providesContacts = true;
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerManager.Instance.potionFog == false) // Si el jugador entra en la zona especificada
        {
            Debug.Log("Player entered the fog area.");  // Debug para ver cuando el jugador entra
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor; // Cambia el color de la niebla
            RenderSettings.fogDensity = fogDensity; // Ajusta la densidad
            Debug.Log($"Fog enabled. Color: {fogColor}, Density: {fogDensity}");  // Muestra los valores de la niebla
            PlayerManager.Instance.playerIsInsideFog = true;    
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the fog area.");  // Debug para ver cuando el jugador sale
            RenderSettings.fog = false; // Desactiva la niebla al salir
            Debug.Log("Fog disabled.");  // Muestra que la niebla se ha desactivado
            PlayerManager.Instance.playerIsInsideFog = false;
        }
    }
}
