using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehaviour
{
    private void OnDisable()
    {
        this.ghost.scatter.Enable();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();
        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistToPacman = float.MaxValue;

            foreach(Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                if(distance < minDistToPacman)
                {
                    direction = availableDirection;
                    minDistToPacman = distance;
                }
            }

            this.ghost.movement.SetDirection(direction);
        }
    }
}
