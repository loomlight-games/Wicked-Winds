using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interfaz para los objetos que se reutilizarán en el pool
public interface IPoolable
{
    // Método para inicializar el objeto cuando es tomado del pool
    void OnObjectSpawn();

    // Método para desactivar o "limpiar" el objeto cuando es devuelto al pool
    void OnObjectReturn();
}
