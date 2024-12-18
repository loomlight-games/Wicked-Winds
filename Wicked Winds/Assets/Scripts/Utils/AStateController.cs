using UnityEngine;

/// <summary>
/// Defines context methods. Implements MonoBehaviour.
/// Controls states' actions (switching states, detect collisions...)
/// </summary>
public abstract class AStateController : MonoBehaviour
{
    protected AState currentState;
    protected AState previousState;

    public virtual void Awake() { }

    public abstract void Start();

    public virtual void Update()
    {
        currentState?.Update();

        UpdateFrame();
    }

    public virtual void UpdateFrame() { }

    public virtual void OnTriggerEnter(Collider other)
    {
        currentState?.OnTriggerEnter(other);
    }

    public virtual void OnTriggerStay(Collider other)
    {
        currentState?.OnTriggerStay(other);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        currentState?.OnCollisionEnter(collision);
    }

    /// <summary>
    /// Gets the current state of the controller.
    /// </summary>
    /// <returns>Current state</returns>
    public virtual AState GetState()
    {
        return currentState;
    }

    /// <summary>
    /// Sets the current state of the controller.
    /// </summary>
    public virtual void SetState(AState state)
    {
        currentState = state;
        currentState.Enter();

        //Debug.LogWarning(currentState.ToString());
    }

    /// <summary>
    /// Sets the current state of the controller with additional info.
    /// </summary>
    public virtual void SetState(AState state, string info)
    {
        currentState = state;
        currentState.Enter(info);

        //Debug.LogWarning(currentState.ToString());
    }

    /// <summary>
    /// Switchs to another state after exiting the current.
    /// </summary>
    public virtual void SwitchState(AState state)
    {
        previousState = currentState;
        currentState.Exit();
        currentState = state;
        currentState.Enter();

        //Debug.LogWarning(currentState.ToString());
    }

    /// <summary>
    /// Switchs to another state, sending additional info, after leaving the current.
    /// </summary>
    public virtual void SwitchState(AState state, string info)
    {
        previousState = currentState;
        currentState.Exit();
        currentState = state;
        currentState.Enter(info);

        //Debug.LogWarning(currentState.ToString());
    }

    /// <summary>
    /// Switchs to previous state after exiting the current.
    /// </summary>
    public virtual void ReturnToPreviousState()
    {
        currentState.Exit();
        currentState = previousState;
        currentState.Enter();

        //Debug.LogWarning(currentState.ToString());
    }
}
