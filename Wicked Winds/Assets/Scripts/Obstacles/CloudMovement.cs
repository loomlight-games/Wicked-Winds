using System.Collections;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Velocidad de movimiento
    private Vector3 targetPosition; // Posición final fija (X = 105
   
    private void Start()
    {
        SetTargetPosition();
    }

    private void Update()
    {
        // Mover hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Verificar si ha llegado al objetivo
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            ReturnToPool();
        }
    }

    public void SetTargetPosition()
    {
        // Definir el destino en X = 150 manteniendo Y y Z
        targetPosition = new Vector3(105, transform.position.y, transform.position.z);
    }

    private void ReturnToPool()
    {
        // Devolver al pool y desactivar la nube
        CloudPool.Instance.ReturnCloud(gameObject);

    }


}
