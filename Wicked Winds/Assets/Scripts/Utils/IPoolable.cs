// Interfaz para los objetos que se reutilizar�n en el pool
public interface IPoolable
{
    // M�todo para inicializar el objeto cuando es tomado del pool
    void OnObjectSpawn();

    // M�todo para desactivar o "limpiar" el objeto cuando es devuelto al pool
    void OnObjectReturn();
}
