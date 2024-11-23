using UnityEngine;

public class BirdCollisionController : MonoBehaviour
{

    public float pushBackForce = 5f; // Fuerza de retroceso cuando choca con un pájaro
    private Rigidbody rb; // Referencia al Rigidbody del personaje

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el Rigidbody del personaje
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto con el que colisiona es un pájaro
        if (collision.gameObject.CompareTag("Bird"))
        {
            // Obtén la dirección opuesta a la colisión para empujar al personaje hacia atrás
            Vector3 pushBackDirection = transform.position - collision.transform.position;

            // Normaliza la dirección y aplica la fuerza de retroceso
            rb.AddForce(pushBackDirection.normalized * pushBackForce, ForceMode.Impulse);

            // Para asegurarnos de que la fuerza no sea cero, puedes verificar la dirección antes de aplicar la fuerza:
            if (pushBackDirection.magnitude > 0f)
            {
                rb.AddForce(pushBackDirection.normalized * pushBackForce, ForceMode.Impulse);
            }
        }
    }
}
