using UnityEngine;

public class BirdCollisionController : MonoBehaviour
{

    public float pushBackForce = 5f; // Fuerza de retroceso cuando choca con un p�jaro
    private Rigidbody rb; // Referencia al Rigidbody del personaje

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el Rigidbody del personaje
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto con el que colisiona es un p�jaro
        if (collision.gameObject.CompareTag("Bird"))
        {
            // Obt�n la direcci�n opuesta a la colisi�n para empujar al personaje hacia atr�s
            Vector3 pushBackDirection = transform.position - collision.transform.position;

            // Normaliza la direcci�n y aplica la fuerza de retroceso
            rb.AddForce(pushBackDirection.normalized * pushBackForce, ForceMode.Impulse);

            // Para asegurarnos de que la fuerza no sea cero, puedes verificar la direcci�n antes de aplicar la fuerza:
            if (pushBackDirection.magnitude > 0f)
            {
                rb.AddForce(pushBackDirection.normalized * pushBackForce, ForceMode.Impulse);
            }
        }
    }
}
