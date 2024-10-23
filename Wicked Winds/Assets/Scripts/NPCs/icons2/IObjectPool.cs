using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    IPoolableObject get();  // Obtener un objeto del pool
    void release(IPoolableObject obj);  // Devolver un objeto al pool
}