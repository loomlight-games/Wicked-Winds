using UnityEngine;

/// <summary>
/// Defines common functionalities in all states.
/// </summary>
public abstract class AState
{
    public virtual void Enter() { }
    public virtual void Enter(string info) { }
    public virtual void Update() { }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
    public virtual void OnTriggerStay(Collider other) { }
    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void Exit() { }
}