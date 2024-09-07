using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Vector2> availableDirections = new();
    public LayerMask obstacleLayer;
    public LayerMask nodeCastLayer;

    private void Start ()
    {
        this.availableDirections.Clear();

        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    private void Update()
    {
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    public void CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D nodeHit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 100.0f, this.nodeCastLayer);

        RaycastHit2D wallHit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.0f, this.obstacleLayer);

        if(wallHit.collider == null && nodeHit.collider != null)
        {
            this.availableDirections.Add(direction);
        }
    }
}
