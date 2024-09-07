using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement movement;
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostBehaviour initialBehaviour;
    public Transform target;
    public int points = 200;
    public int stuck = 0;

    private Vector3 prevPos;

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.prevPos = this.transform.position;
    }

    private void Start()
    {
        ResetState();
        InvokeRepeating(nameof(CheckStuck), 3.0f, 0.3f);
    }

    private void CheckStuck()
    {
        if(this.stuck > 3)
        {
            ResetState();
        }
        else if(this.prevPos == this.transform.position)
        {
            this.movement.Reverse();
            this.stuck++;
        }
        else
        {
            this.prevPos = this.transform.position;
        }
    }

    public void ResetState()
    {
        this.movement.speed = 6.0f;
        this.movement.initialSpeed = 6.0f;
        this.stuck = 0;
        this.gameObject.SetActive(true);
        this.movement.ResetState();

        this.chase.Disable();
        this.frightened.Disable();
        this.scatter.Enable();
        if(this.home != this.initialBehaviour)
        {
            this.home.Disable();
        }
        if(this.initialBehaviour != null)
        {
            this.initialBehaviour.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        position.z = this.transform.position.z;
        this.transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PlayerEaten();
            }
        }
    }
}
