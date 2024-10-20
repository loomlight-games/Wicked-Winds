using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Serializable<T>
{
    T value;

    /// <summary>
    /// Saves in memory.
    /// </summary>
    public virtual void Serialize(T value, string fileName)
    {
        this.value = value;
        BinaryFormatter formatter = new();
        FileStream jsonFile = File.Create(fileName); //Creates file with that name
        formatter.Serialize(jsonFile, this); //Writes information in file
        jsonFile.Close(); //Close file
    }

    /// <summary>
    /// Reads from memory.
    /// </summary>
    public virtual T Deserialize(string fileName)
    {
        BinaryFormatter formatter = new();
        FileStream jsonFile = File.OpenRead(fileName); //Open and read file with that name
        Serializable<T> data = (Serializable<T>)formatter.Deserialize(jsonFile); //Extracts information from file
        jsonFile.Close(); //Close file
        this.value = data.value;
        return value;
    }
}