using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject
{
    void setActive(bool active);  // Activar/desactivar el objeto
    bool isActive();  // Comprobar si el objeto está activo
    void Reset();  // Resetear el objeto (opcional, para reiniciar su estado)
}