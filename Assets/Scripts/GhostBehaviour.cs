using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehaviour : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    public float duration;

    private void Awake()
    {
        this.ghost = GetComponent<Ghost>();
        InvokeRepeating("ChangeDirection", 3.0f, 0.3f);
    }

    public void Enable()
    {
        Enable(this.duration);
    }

    public virtual void Enable(float duration)
    {
        this.enabled = true;
        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }

    public virtual void Disable()
    {
        this.enabled = false;
        CancelInvoke();
    }
    private void ChangeDirection()
    {
        this.ghost.movement.SetDirection(-this.ghost.movement.direction);
    }
}
