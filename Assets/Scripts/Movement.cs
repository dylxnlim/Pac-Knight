using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
    public new Rigidbody2D rigidbody {get; private set;}
    
    public float speed;
    public float initialSpeed;
    public float speedMult = 1.0f;
    public Vector2 initialDirection;
    public Vector2 direction {get; private set;}
    public Vector2 nextDirection {get; private set;}
    public LayerMask obstacleLayer;
    public Vector3 startingPosition {get; private set;}


    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startingPosition = this.transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if(this.nextDirection != Vector2.zero)
        {
            SetDirection(this.nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = this.rigidbody.position;
        Vector2 translation = this.direction * this.speed * this.speedMult * Time.fixedDeltaTime;
        this.rigidbody.MovePosition(position + translation);
    }

    public void ResetState()
    {
        this.rigidbody.isKinematic = false;
        //this.speed = 8.0f;
        this.speedMult = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        this.enabled = true;
    } 

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if(IsInteger(direction.x) && IsInteger(direction.y))
        {
            if(!Occupied(direction) || forced)
            {
                this.direction = direction;
                this.nextDirection = Vector2.zero;
            }
            else
            {
                this.nextDirection = direction;
            }
        }
    }
    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.0f, this.obstacleLayer);
        return hit.collider != null;
    }
    public void Reverse()
    {
        this.nextDirection = -this.direction;
    }

    private bool IsInteger(float number)
    {
        int convert = (int)number;
        float temp = number - convert;
        if(temp > 0)
        {
            return false;
        }
        return true;
    }
}
